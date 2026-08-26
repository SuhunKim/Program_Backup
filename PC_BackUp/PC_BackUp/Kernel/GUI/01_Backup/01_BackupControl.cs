using PC_BackUp.Services;

namespace PC_BackUp;

public partial class BackupControl : UserControlBase
{
    private ISettingsService? m_oSettingsService;
    private BackupService? m_oBackupService;
    private LogManager? m_oLoggingService;
    private bool m_bIsBusy;
    private CancellationTokenSource? m_oCancellationTokenSource;

    public BackupControl() => InitializeComponent();

    public BackupControl(ISettingsService settingsService, BackupService backupService, LogManager loggingService) : this()
    {
        m_oSettingsService = settingsService;
        m_oBackupService = backupService;
        m_oLoggingService = loggingService;
    }

    public override void OnMenuSelected()
    {
        if (m_oSettingsService is null) return;
        var settings = m_oSettingsService.Load();
        projectValueLabel.Text = EmptyText(settings.TargetProjectName);
        sourceValueLabel.Text = EmptyText(settings.GetSourceRoot());
        destinationValueLabel.Text = EmptyText(settings.BackupRootPath);
        fullZipRadioButton.Text = "전체 백업 — ZIP 압축 파일로 저장";
        selectiveRadioButton.Text = string.Format("선택 폴더 백업 — 날짜 폴더에 복사 (압축 안 함): {0}", string.Join("/", settings.GetSelectiveFolderNames()));
        var errors = settings.Validate();
        backupButton.Enabled = !m_bIsBusy && errors.Count == 0;
        if (!m_bIsBusy)
            statusLabel.Text = backupButton.Enabled
                ? "백업 준비가 완료되었습니다."
                : string.Join("  ·  ", errors);
    }

    private async void UiClick_Backup(object? sender, EventArgs e)
    {
        if (m_oSettingsService is null || m_oBackupService is null || m_bIsBusy) return;

        AppSettings settings;
        BackupKind kind;
        string suffix;
        try
        {
            settings = m_oSettingsService.Load();
            kind = fullZipRadioButton.Checked ? BackupKind.FullZip : BackupKind.SelectiveFolders;

            using var nameDialog = new BackupNameDialog(settings.TargetProjectName, kind);
            if (nameDialog.ShowDialog(this) != DialogResult.OK)
                return;
            suffix = nameDialog.Suffix;
        }
        catch (Exception exception)
        {
            m_oLoggingService?.LogError("백업 준비 중 예외가 발생했습니다.", exception);
            MessageBox.Show(this, exception.Message, "백업 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        m_bIsBusy = true;
        SetBusy(true);
        m_oCancellationTokenSource = new CancellationTokenSource();
        try
        {
            var progress = new Progress<int>(value => progressBar.Value = value);
            var result = await m_oBackupService.CreateAsync(settings, kind, suffix, progress, m_oCancellationTokenSource.Token);
            statusLabel.Text = result.Result.Message.Replace(Environment.NewLine, "  ");
            if (result.Result.Succeeded)
                m_oLoggingService?.LogInfo(string.Format("백업 완료: {0}", result.Record?.FileName));
            else
                m_oLoggingService?.LogError(string.Format("백업 실패: {0}", result.Result.Message));
            MessageBox.Show(this, result.Result.Message, result.Result.Succeeded ? "백업 완료" : "백업 실패",
                MessageBoxButtons.OK, result.Result.Succeeded ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }
        catch (Exception exception)
        {
            m_oLoggingService?.LogError("백업 중 예외가 발생했습니다.", exception);
            MessageBox.Show(this, exception.Message, "백업 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            statusLabel.Text = "백업 중 오류가 발생했습니다.";
        }
        finally
        {
            m_bIsBusy = false;
            SetBusy(false);
            m_oCancellationTokenSource?.Dispose();
            m_oCancellationTokenSource = null;
        }
    }

    private void UiClick_Cancel(object? sender, EventArgs e) => m_oCancellationTokenSource?.Cancel();

    private void SetBusy(bool busy)
    {
        backupButton.Enabled = !busy; fullZipRadioButton.Enabled = !busy; selectiveRadioButton.Enabled = !busy;
        cancelButton.Visible = busy;
        progressBar.Value = 0; if (busy) statusLabel.Text = "백업 파일을 생성하고 있습니다...";
    }

    private static string EmptyText(string value) => string.IsNullOrWhiteSpace(value) ? "설정되지 않음" : value;
}
