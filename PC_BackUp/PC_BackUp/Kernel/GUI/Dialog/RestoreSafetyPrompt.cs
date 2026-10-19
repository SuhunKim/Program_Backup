using PC_BackUp.Services;

namespace PC_BackUp;

// [Codex - 2026.10.19] 전체 복원과 XML 파일 복원에 동일한 종료 확인·안전 백업 정책을 적용한다.
internal static class RestoreSafetyPrompt
{
    public static async Task<bool> PrepareAsync(IWin32Window owner, AppSettings settings, RecoveryService recovery,
        BackupService backup, IProgress<int>? progress, CancellationToken token)
    {
        var processes = recovery.GetRunningTargetProcesses(settings);
        try
        {
            if (processes.Count > 0)
            {
                if (MessageBox.Show(owner, string.Format("대상 프로그램이 {0}개 실행 중입니다. 종료한 뒤 복원할까요?", processes.Count),
                    "복원 준비", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return false;
                var stopped = await recovery.StopProcessesAsync(processes);
                if (!stopped.Succeeded) throw new IOException(stopped.Message);
            }
        }
        finally { foreach (var process in processes) process.Dispose(); }
        token.ThrowIfCancellationRequested();
        if (settings.AutoBackupBeforeRestore)
        {
            var result = await backup.CreateAsync(settings, BackupKind.FullZip, "autosafety", progress, token);
            if (result.Result.Status == OperationStatus.Canceled) throw new OperationCanceledException(token);
            if (!result.Result.Succeeded) throw new IOException(string.Format("안전 백업 실패로 복원을 중단했습니다.\n{0}", result.Result.Message));
        }
        return true;
    }
}
