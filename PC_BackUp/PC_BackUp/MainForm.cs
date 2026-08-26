using PC_BackUp.Services;

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

        m_oMenuManager = new ActiveMenuManager(contentPanel, mainMenu);
        m_oMenuManager.Register(MainMenuId.Backup, new BackupControl(settingsService, backupService, loggingService));
        m_oMenuManager.Register(MainMenuId.Recovery,
            new RecoveryControl(settingsService, catalogService, recoveryService, backupService, backupComparisonService, loggingService));
        m_oMenuManager.Register(MainMenuId.History,
            new HistoryControl(settingsService, catalogService, xmlComparisonService, loggingService));
        m_oMenuManager.Register(MainMenuId.Settings, new SettingControl(settingsService));

        m_oMenuManager.Select(MainMenuId.Backup);
    }
}
