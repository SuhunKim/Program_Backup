using PC_BackUp.Services;
using PC_BackUp.Kernel.GUI._00_Dashboard;

namespace PC_BackUp;

public partial class MainForm : Form
{
    private ActiveMenuManager? m_oMenuManager;

    public MainForm()
    {
        InitializeComponent();

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
