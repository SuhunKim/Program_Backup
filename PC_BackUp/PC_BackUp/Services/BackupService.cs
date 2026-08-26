using System.IO.Compression;

namespace PC_BackUp.Services;

public sealed class BackupService
{
    private const int MaxNameCollisionRetries = 99; // 같은 분에 만든 백업 개수 상한(초과 시 실패 처리)

    private readonly BackupCatalogService m_oCatalogService;

    public BackupService(BackupCatalogService oCatalogService)
    {
        m_oCatalogService = oCatalogService;
    }

    public async Task<(OperationResult Result, BackupRecord? Record)> CreateAsync(
        AppSettings settings,
        BackupKind kind,
        string nameSuffix = "",
        IProgress<int>? progress = null,
        CancellationToken cancellationToken = default)
    {
        var errors = settings.Validate();
        if (errors.Count > 0)
            return (OperationResult.Failure(string.Join(Environment.NewLine, errors)), null);

        var suffix = FileNaming.SanitizeSuffix(nameSuffix);

        try
        {
            var record = await Task.Run(() =>
            {
                var sourceRoot = Path.GetFullPath(settings.GetSourceRoot());
                var backupRoot = Path.GetFullPath(settings.BackupRootPath);
                Directory.CreateDirectory(backupRoot);

                var baseName = ResolveAvailableBaseName(backupRoot, kind, suffix);
                var destination = Path.Combine(backupRoot, baseName + SuffixFor(kind));

                var selectiveFolderNames = settings.GetSelectiveFolderNames();
                if (kind == BackupKind.SelectiveFolders)
                    EnsureSelectiveFoldersExist(sourceRoot, selectiveFolderNames);

                var files = EnumerateSourceFiles(sourceRoot, backupRoot)
                    .Where(file => kind == BackupKind.FullZip ||
                                   IsConfiguredFolderItem(sourceRoot, file, selectiveFolderNames))
                    .ToList();

                if (files.Count == 0)
                    throw new InvalidOperationException("백업 조건에 맞는 파일이 없습니다.");

                if (kind == BackupKind.FullZip)
                    CreateZip(sourceRoot, destination, files, progress, cancellationToken);
                else
                    CopyToFolder(sourceRoot, destination, files, progress, cancellationToken);

                m_oCatalogService.Refresh(backupRoot);
                return m_oCatalogService.GetAll().First(record =>
                    string.Equals(record.FullPath, destination, StringComparison.OrdinalIgnoreCase));
            }, cancellationToken);

            return (OperationResult.Success(string.Format("백업이 완료되었습니다.\n{0}", record.FileName)), record);
        }
        catch (OperationCanceledException)
        {
            return (OperationResult.Failure("백업이 취소되었습니다."), null);
        }
        catch (Exception exception)
        {
            return (OperationResult.Failure(string.Format("백업 중 오류가 발생했습니다.\n{0}", exception.Message)), null);
        }
    }

    /// <summary>GUI가 파일명 다이얼로그에서 실시간 미리보기를 표시할 때 쓰는 순수 헬퍼.</summary>
    public static string PreviewFileName(string nameSuffix, BackupKind kind)
    {
        var suffix = FileNaming.SanitizeSuffix(nameSuffix);
        return BuildBaseName(DateTime.Now, suffix, 0) + SuffixFor(kind);
    }

    /// <summary>
    /// 파일명 규칙: "yyyyMMdd - HHmm[ - 접미사(예: 프로젝트 이름)][ - 01]".
    /// 순번(counter)은 같은 분에 두 번째 이후로 만들 때만 붙는다(0이면 표시하지 않음).
    /// </summary>
    private static string BuildBaseName(DateTime createdAt, string suffix, int counter)
    {
        var parts = new List<string> { createdAt.ToString("yyyyMMdd"), createdAt.ToString("HHmm") };
        if (suffix.Length > 0) parts.Add(suffix);
        if (counter > 0) parts.Add(counter.ToString("00"));
        return string.Join(" - ", parts);
    }

    // 전체 백업은 ZIP 파일, 선택 폴더 백업은 날짜 기반 폴더를 만들어 파일을 복사한다.
    private static string SuffixFor(BackupKind kind) =>
        kind == BackupKind.FullZip ? ".zip" : "_files";

    /// <summary>
    /// 현재 시각 기준으로 사용 가능한 파일명을 찾는다. 같은 분에 이미 파일이 있으면 시간을 바꾸지 않고
    /// " - 01", " - 02"... 순번만 붙여 구분한다(첫 번째는 순번 없이 그대로 사용).
    /// </summary>
    private static string ResolveAvailableBaseName(string backupRoot, BackupKind kind, string suffix)
    {
        var now = DateTime.Now;
        var kindSuffix = SuffixFor(kind);
        for (var counter = 0; counter <= MaxNameCollisionRetries; counter++)
        {
            var baseName = BuildBaseName(now, suffix, counter);
            var candidate = Path.Combine(backupRoot, baseName + kindSuffix);
            if (!File.Exists(candidate) && !Directory.Exists(candidate))
                return baseName;
        }

        throw new InvalidOperationException("사용 가능한 백업 파일명을 찾지 못했습니다. 백업 폴더를 정리한 뒤 다시 시도하세요.");
    }

    private static IEnumerable<string> EnumerateSourceFiles(string sourceRoot, string backupRoot)
    {
        var options = new EnumerationOptions
        {
            RecurseSubdirectories = true,
            IgnoreInaccessible = true,
            AttributesToSkip = FileAttributes.ReparsePoint
        };

        foreach (var file in Directory.EnumerateFiles(sourceRoot, "*", options))
        {
            var fullPath = Path.GetFullPath(file);
            if (!IsSameOrChildPath(fullPath, backupRoot))
                yield return fullPath;
        }
    }

    private static void EnsureSelectiveFoldersExist(string sourceRoot, IReadOnlyList<string> folderNames)
    {
        var missingFolders = folderNames
            .Where(name => !Directory.Exists(Path.Combine(sourceRoot, name)))
            .ToList();

        if (missingFolders.Count > 0)
            throw new DirectoryNotFoundException(
                string.Format("선별 백업 폴더를 찾을 수 없습니다: {0}", string.Join(", ", missingFolders)));
    }

    private static bool IsConfiguredFolderItem(
        string sourceRoot,
        string filePath,
        IReadOnlyList<string> folderNames)
    {
        var relativePath = Path.GetRelativePath(sourceRoot, filePath);
        var firstSeparatorIndex = relativePath.IndexOfAny(new[]
        {
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar
        });
        if (firstSeparatorIndex <= 0)
            return false;

        var topLevelFolderName = relativePath[..firstSeparatorIndex];
        return folderNames.Contains(topLevelFolderName, StringComparer.OrdinalIgnoreCase);
    }

    private static void CreateZip(
        string sourceRoot,
        string destination,
        IReadOnlyList<string> files,
        IProgress<int>? progress,
        CancellationToken cancellationToken)
    {
        using var stream = new FileStream(destination, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create);

        try
        {
            for (var index = 0; index < files.Count; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var relativePath = Path.GetRelativePath(sourceRoot, files[index]);
                archive.CreateEntryFromFile(files[index], relativePath, CompressionLevel.Optimal);
                progress?.Report((index + 1) * 100 / files.Count);
            }
        }
        catch
        {
            archive.Dispose();
            stream.Dispose();
            File.Delete(destination);
            throw;
        }
    }

    private static void CopyToFolder(
        string sourceRoot,
        string destination,
        IReadOnlyList<string> files,
        IProgress<int>? progress,
        CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(destination);
        try
        {
            for (var index = 0; index < files.Count; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var relativePath = Path.GetRelativePath(sourceRoot, files[index]);
                var targetPath = Path.Combine(destination, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
                File.Copy(files[index], targetPath, true);
                progress?.Report((index + 1) * 100 / files.Count);
            }
        }
        catch
        {
            try { Directory.Delete(destination, true); }
            catch { }
            throw;
        }
    }

    private static bool IsSameOrChildPath(string candidate, string parent)
    {
        var normalizedCandidate = Path.TrimEndingDirectorySeparator(Path.GetFullPath(candidate));
        var normalizedParent = Path.TrimEndingDirectorySeparator(Path.GetFullPath(parent));
        return normalizedCandidate.Equals(normalizedParent, StringComparison.OrdinalIgnoreCase) ||
               normalizedCandidate.StartsWith(normalizedParent + Path.DirectorySeparatorChar,
                   StringComparison.OrdinalIgnoreCase);
    }
}
