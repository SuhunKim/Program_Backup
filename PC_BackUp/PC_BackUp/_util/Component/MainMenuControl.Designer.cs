namespace PC_BackUp;

partial class MainMenuControl
{
    private System.ComponentModel.IContainer? components = null;
    private Panel sidebarPanel = null!;
    private Label logoLabel = null!;
    private Label versionLabel = null!;
    private FlowLayoutPanel menuPanel = null!;
    private StyledButton m_menuBackup = null!;
    private StyledButton m_menuRecovery = null!;
    private StyledButton m_menuHistory = null!;
    private StyledButton m_menuSettings = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        sidebarPanel = new Panel();
        menuPanel = new FlowLayoutPanel();
        m_menuBackup = new StyledButton();
        m_menuRecovery = new StyledButton();
        m_menuHistory = new StyledButton();
        m_menuSettings = new StyledButton();
        versionLabel = new Label();
        logoLabel = new Label();
        sidebarPanel.SuspendLayout();
        menuPanel.SuspendLayout();
        SuspendLayout();
        // 
        // sidebarPanel
        // 
        sidebarPanel.BackColor = Color.FromArgb(240, 241, 244);
        sidebarPanel.Controls.Add(menuPanel);
        sidebarPanel.Controls.Add(versionLabel);
        sidebarPanel.Controls.Add(logoLabel);
        sidebarPanel.Dock = DockStyle.Fill;
        sidebarPanel.Location = new Point(0, 0);
        sidebarPanel.Name = "sidebarPanel";
        sidebarPanel.Padding = new Padding(14, 22, 14, 20);
        sidebarPanel.Size = new Size(220, 800);
        sidebarPanel.TabIndex = 1;
        // 
        // menuPanel
        // 
        menuPanel.Controls.Add(m_menuBackup);
        menuPanel.Controls.Add(m_menuRecovery);
        menuPanel.Controls.Add(m_menuHistory);
        menuPanel.Controls.Add(m_menuSettings);
        menuPanel.Dock = DockStyle.Fill;
        menuPanel.FlowDirection = FlowDirection.TopDown;
        menuPanel.Location = new Point(14, 108);
        menuPanel.Name = "menuPanel";
        menuPanel.Padding = new Padding(0, 14, 0, 0);
        menuPanel.Size = new Size(192, 642);
        menuPanel.TabIndex = 0;
        menuPanel.WrapContents = false;
        // 
        // m_menuBackup
        // 
        m_menuBackup.BackColor = Color.FromArgb(30, 100, 199);
        m_menuBackup.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        m_menuBackup.ForeColor = Color.White;
        m_menuBackup.Location = new Point(3, 17);
        m_menuBackup.Margin = new Padding(3, 3, 3, 12);
        m_menuBackup.Name = "m_menuBackup";
        m_menuBackup.Size = new Size(140, 42);
        m_menuBackup.TabIndex = 0;
        m_menuBackup.Text = "백업";
        m_menuBackup.ButtonType = StyledButtonType.Sidebar;
        // 
        // m_menuRecovery
        // 
        m_menuRecovery.BackColor = Color.FromArgb(30, 100, 199);
        m_menuRecovery.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        m_menuRecovery.ForeColor = Color.White;
        m_menuRecovery.Location = new Point(3, 74);
        m_menuRecovery.Margin = new Padding(3, 3, 3, 12);
        m_menuRecovery.Name = "m_menuRecovery";
        m_menuRecovery.Size = new Size(140, 42);
        m_menuRecovery.TabIndex = 1;
        m_menuRecovery.Text = "복원";
        m_menuRecovery.ButtonType = StyledButtonType.Sidebar;
        // 
        // m_menuHistory
        // 
        m_menuHistory.BackColor = Color.FromArgb(30, 100, 199);
        m_menuHistory.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        m_menuHistory.ForeColor = Color.White;
        m_menuHistory.Location = new Point(3, 131);
        m_menuHistory.Margin = new Padding(3, 3, 3, 12);
        m_menuHistory.Name = "m_menuHistory";
        m_menuHistory.Size = new Size(140, 42);
        m_menuHistory.TabIndex = 2;
        m_menuHistory.Text = "이력 관리";
        m_menuHistory.ButtonType = StyledButtonType.Sidebar;
        // 
        // m_menuSettings
        // 
        m_menuSettings.BackColor = Color.FromArgb(30, 100, 199);
        m_menuSettings.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        m_menuSettings.ForeColor = Color.White;
        m_menuSettings.Location = new Point(3, 188);
        m_menuSettings.Margin = new Padding(3, 3, 3, 12);
        m_menuSettings.Name = "m_menuSettings";
        m_menuSettings.Size = new Size(140, 42);
        m_menuSettings.TabIndex = 3;
        m_menuSettings.Text = "환경 설정";
        m_menuSettings.ButtonType = StyledButtonType.Sidebar;
        // 
        // versionLabel
        // 
        versionLabel.Dock = DockStyle.Bottom;
        versionLabel.ForeColor = Color.FromArgb(108, 115, 128);
        versionLabel.Location = new Point(14, 750);
        versionLabel.Name = "versionLabel";
        versionLabel.Size = new Size(192, 30);
        versionLabel.TabIndex = 1;
        versionLabel.Text = "v0.1  ·  Foundation";
        versionLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // logoLabel
        // 
        logoLabel.Dock = DockStyle.Top;
        logoLabel.Font = new Font("맑은 고딕", 16F, FontStyle.Bold);
        logoLabel.ForeColor = Color.FromArgb(28, 32, 41);
        logoLabel.Location = new Point(14, 22);
        logoLabel.Name = "logoLabel";
        logoLabel.Padding = new Padding(12, 0, 0, 0);
        logoLabel.Size = new Size(192, 86);
        logoLabel.TabIndex = 2;
        logoLabel.Text = "PC BACKUP";
        logoLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // MainMenuControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(sidebarPanel);
        Name = "MainMenuControl";
        Size = new Size(220, 800);
        sidebarPanel.ResumeLayout(false);
        menuPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
