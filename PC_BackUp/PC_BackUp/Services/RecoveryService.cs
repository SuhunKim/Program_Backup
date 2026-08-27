using System.Diagnostics;

namespace PC_BackUp.Services;

public sealed class RecoveryService
{
    /// <summary>
    /// 이름만으로 매칭하면 우연히 이름이 같은 무관한 프로세스를 강제 종료할 수 있으므로,
    /// 실제 실행 파일 경로(MainModule.FileName)가 설정된 ExecutablePath와 일치하는 것만 대상으로 삼는다.
    /// 접근 거부 등으로 경로를 확인할 수 없는 프로세스는 안전하게 제외한다.
    /// </summary>
    public IReadOnlyList<Process> GetRunningTargetProcesses(AppSettings settings)
    {
        var processName = Path.GetFileNameWithoutExtension(settings.ExecutablePath);
        if (string.IsNullOrWhiteSpace(processName))
            return Array.Empty<Process>();

        var targetPath = Path.GetFullPath(settings.ExecutablePath);
        var matched = new List<Process>();
        foreach (var process in Process.GetProcessesByName(processName))
        {
            if (process.Id == Environment.ProcessId)
            {
                process.Dispose();
                continue;
            }

            bool isTarget;
            try
            {
                isTarget = string.Equals(process.MainModule?.FileName, targetPath, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                isTarget = false;
            }

            if (isTarget)
                matched.Add(process);
            else
                process.Dispose();
        }
        return matched;
    }

    public async Task<OperationResult> StopProcessesAsync(IEnumerable<Process> processes)
    {
        try
        {
            foreach (var process in processes)
            {
                using (process)
                {
                    if (process.HasExited)
                        continue;

                    process.CloseMainWindow();
                    using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                    try
                    {
                        await process.WaitForExitAsync(timeout.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        process.Kill(true);
                        await process.WaitForExitAsync();
                    }
                }
            }

            return OperationResult.Success("실행 중인 대상 프로그램을 종료했습니다.");
        }
        catch (Exception exception)
        {
            return OperationResult.Failure(string.Format("대상 프로그램 종료에 실패했습니다.\n{0}", exception.Message));
        }
    }

    public async Task<OperationResult> RestoreAsync(
        BackupRecord record,
        AppSettings settings,
        IProgress<int>? progress = null,
        CancellationToken cancellationToken = default)
    {
        // 복원은 대상 파일(실행 파일 포함)이 사라진 상황을 되살리는 기능이므로,
        // 실행 파일이 지금 실제로 존재하는지는 검사하지 않는다 — 경로 설정 자체만 확인한다.
        var errors = settings.Validate(requireExecutableExists: false);
        if (errors.Count > 0)
            return OperationResult.Failure(string.Join(Environment.NewLine, errors));
        if (!File.Exists(record.FullPath) && !Directory.Exists(record.FullPath))
            return OperationResult.Failure("선택한 백업 파일을 찾을 수 없습니다.");

        var temporaryRoot = Path.Combine(Path.GetTempPath(), "PC_BackUp", Guid.NewGuid().ToString("N"));
        try
        {
            return await Task.Run(() =>
            {
                var restoreSource = record.FullPath;
                if (File.Exists(record.FullPath))
                {
                    // zip 파일(전체 백업 또는 압축된 선별 백업)이면 해제하고, 폴더면(구버전 선별 백업) 그대로 사용한다.
                    Directory.CreateDirectory(temporaryRoot);
                    ZipExtraction.SafeExtractToDirectory(record.FullPath, temporaryRoot);
                    restoreSource = temporaryRoot;
                }

                OverlayDirectory(restoreSource, settings.GetSourceRoot(), progress, cancellationToken);
                return OperationResult.Success(string.Format("복원이 완료되었습니다.\n{0}", record.FileName));
            }, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return OperationResult.Failure("복원이 취소되었습니다.");
        }
        catch (Exception exception)
        {
            return OperationResult.Failure(string.Format("복원 중 오류가 발생했습니다.\n{0}", exception.Message));
        }
        finally
        {
            if (Directory.Exists(temporaryRoot))
            {
                try { Directory.Delete(temporaryRoot, true); }
                catch { /* OS가 잠근 임시 파일은 다음 정리 시 제거한다. */ }
            }
        }
    }

    private static void OverlayDirectory(
        string sourceRoot,
        string destinationRoot,
        IProgress<int>? progress,
        CancellationToken cancellationToken)
    {
        // IgnoreInaccessible을 의도적으로 false로 둔다 — BackupService/XmlComparisonService/
        // BackupComparisonService는 "지금 실행 중인 소스 트리"를 읽어서 일시적 잠금/권한 문제를
        // 건너뛰고 계속 진행해도 되지만, 여기 sourceRoot는 이미 해제된 백업(임시 폴더 또는
        // 백업이 폴더 형태인 경우 그 폴더)이라 접근 불가 항목이 있다면 복원이 일부만 적용된
        // 상태로 조용히 끝나는 쪽이 더 위험하다. 실패를 그대로 드러내 호출자가 알게 한다.
        var files = Directory.EnumerateFiles(sourceRoot, "*", new EnumerationOptions
        {
            RecurseSubdirectories = true,
            IgnoreInaccessible = false,
            AttributesToSkip = FileAttributes.ReparsePoint
        }).ToList();

        for (var index = 0; index < files.Count; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var relativePath = Path.GetRelativePath(sourceRoot, files[index]);
            var targetPath = Path.GetFullPath(Path.Combine(destinationRoot, relativePath));
            EnsureChildPath(targetPath, destinationRoot);
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
            File.Copy(files[index], targetPath, true);
            progress?.Report((index + 1) * 100 / files.Count);
        }
    }

    private static void EnsureChildPath(string candidate, string parent)
    {
        var normalizedParent = Path.TrimEndingDirectorySeparator(Path.GetFullPath(parent));
        if (!candidate.StartsWith(string.Format("{0}{1}", normalizedParent, Path.DirectorySeparatorChar),
                StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("백업에 대상 경로를 벗어나는 파일이 포함되어 있습니다.");
    }
}
