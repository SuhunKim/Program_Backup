using PC_BackUp.Services;
using PC_BackUp.Kernel.GUI._00_Dashboard;

namespace PC_BackUp;

public partial class MainForm : Form
{
    private ActiveMenuManager? m_oMenuManager;

    public MainForm()
    {
        InitializeComponent();
        // [Codex - 2026.10.19] 적용·정리 중 정상 종료로 작업이 끊기는 것을 막는다.
        FormClosing += (_, e) =>
        {
            if (!OperationCoordinator.HasActiveOperations) return;
            e.Cancel = true;
            MessageBox.Show(this, "진행 중인 작업을 완료하거나 취소한 뒤 종료하세요.", "작업 진행 중",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        };

        if (System.ComponentModel.LicenseManager.UsageMode !=
            System.ComponentModel.LicenseUsageMode.Designtime)
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        var settingsService = new XmlSettingsService();
        var catalogService = new BackupCatalogService();
        var backupService = new BackupService(catalogService);
        var recoveryService = new RecoveryService();
        var backupComparisonService = new BackupComparisonService();
        var xmlComparisonService = new XmlComparisonService();
        var loggingService = new LogManager();
        var recoveryControl = new RecoveryControl(settingsService, catalogService, recoveryService, backupService,
            backupComparisonService, loggingService);

        m_oMenuManager = new ActiveMenuManager(contentPanel, mainMenu);
        m_oMenuManager.Register(MainMenuId.Dashboard,
            new _00_Dashboard(settingsService, catalogService, id => m_oMenuManager?.Select(id), record =>
            {
                if (record is not null)
                    recoveryControl.SelectBackupOnNextDisplay(record);
                m_oMenuManager?.Select(MainMenuId.Recovery);
            }));
        m_oMenuManager.Register(MainMenuId.Backup, new BackupControl(settingsService, backupService, loggingService));
        m_oMenuManager.Register(MainMenuId.Recovery, recoveryControl);
        m_oMenuManager.Register(MainMenuId.History,
            new HistoryControl(settingsService, catalogService, xmlComparisonService, loggingService));
        m_oMenuManager.Register(MainMenuId.Settings, new SettingControl(settingsService, loggingService));

        m_oMenuManager.Select(MainMenuId.Dashboard);
    }
}
