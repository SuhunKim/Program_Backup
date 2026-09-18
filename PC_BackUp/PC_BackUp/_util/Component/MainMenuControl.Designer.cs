namespace PC_BackUp;

partial class MainMenuControl
{
    private System.ComponentModel.IContainer? components = null;
    private Panel sidebarPanel = null!;
    private Label logoLabel = null!;
    private Label versionLabel = null!;
    private FlowLayoutPanel menuPanel = null!;
<<<<<<< Updated upstream
    private StyledButton m_menuDashboard = null!;
=======
    private Panel extensionMenuPanel = null!;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        m_menuDashboard = new StyledButton();
        splitter1 = new Splitter();
=======
        extensionMenuPanel = new Panel();
>>>>>>> Stashed changes
        m_menuBackup = new StyledButton();
        splitter2 = new Splitter();
        m_menuRecovery = new StyledButton();
        splitter3 = new Splitter();
        m_menuHistory = new StyledButton();
        splitter4 = new Splitter();
        m_menuSettings = new StyledButton();
        versionLabel = new Label();
        logoLabel = new Label();
        sidebarPanel.SuspendLayout();
        menuPanel.SuspendLayout();
        SuspendLayout();
        // 
        // sidebarPanel
<<<<<<< Updated upstream
        // 
        sidebarPanel.BackColor = Color.FromArgb(240, 241, 244);
=======
        //
        sidebarPanel.BackColor = ColorRGB.Sidebar;
        sidebarPanel.Controls.Add(extensionMenuPanel);
>>>>>>> Stashed changes
        sidebarPanel.Controls.Add(menuPanel);
        sidebarPanel.Controls.Add(versionLabel);
        sidebarPanel.Controls.Add(logoLabel);
        sidebarPanel.Dock = DockStyle.Fill;
        sidebarPanel.Location = new Point(0, 0);
        sidebarPanel.Name = "sidebarPanel";
        sidebarPanel.Padding = new Padding(14, 22, 14, 20);
        sidebarPanel.Size = new Size(220, 681);
        sidebarPanel.TabIndex = 1;
        // 
        // menuPanel
        // 
        menuPanel.Controls.Add(m_menuDashboard);
        menuPanel.Controls.Add(splitter1);
        menuPanel.Controls.Add(m_menuBackup);
        menuPanel.Controls.Add(splitter2);
        menuPanel.Controls.Add(m_menuRecovery);
        menuPanel.Controls.Add(splitter3);
        menuPanel.Controls.Add(m_menuHistory);
        menuPanel.Controls.Add(splitter4);
        menuPanel.Controls.Add(m_menuSettings);
        menuPanel.Dock = DockStyle.Top;
        menuPanel.FlowDirection = FlowDirection.TopDown;
        menuPanel.AutoScroll = true;
        menuPanel.Location = new Point(14, 108);
        menuPanel.Name = "menuPanel";
        menuPanel.Padding = new Padding(0, 14, 0, 0);
<<<<<<< Updated upstream
        menuPanel.Size = new Size(192, 523);
        menuPanel.TabIndex = 0;
        menuPanel.WrapContents = false;
        // 
        // m_menuDashboard
        // 
        m_menuDashboard.BackColor = Color.FromArgb(240, 241, 244);
        m_menuDashboard.ButtonType = StyledButtonType.Sidebar;
        m_menuDashboard.Dock = DockStyle.Top;
        m_menuDashboard.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        m_menuDashboard.ForeColor = Color.FromArgb(28, 32, 41);
        m_menuDashboard.Location = new Point(3, 17);
        m_menuDashboard.Margin = new Padding(3, 3, 3, 12);
        m_menuDashboard.Name = "m_menuDashboard";
        m_menuDashboard.Padding = new Padding(16, 0, 0, 0);
        m_menuDashboard.Size = new Size(190, 50);
        m_menuDashboard.TabIndex = 0;
        m_menuDashboard.Text = "대시보드";
        m_menuDashboard.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // splitter1
        // 
        splitter1.BorderStyle = BorderStyle.Fixed3D;
        splitter1.Dock = DockStyle.Top;
        splitter1.Location = new Point(3, 82);
        splitter1.Name = "splitter1";
        splitter1.Size = new Size(190, 3);
        splitter1.TabIndex = 5;
        splitter1.TabStop = false;
        // 
=======
        menuPanel.Size = new Size(192, 242);
        menuPanel.TabIndex = 0;
        menuPanel.WrapContents = false;
        //
        // extensionMenuPanel
        //
        extensionMenuPanel.AccessibleName = "향후 메뉴 확장 영역";
        extensionMenuPanel.AutoScroll = true;
        extensionMenuPanel.Dock = DockStyle.Fill;
        extensionMenuPanel.Name = "extensionMenuPanel";
        extensionMenuPanel.Padding = new Padding(0, 16, 0, 0);
        extensionMenuPanel.TabIndex = 4;
        //
>>>>>>> Stashed changes
        // m_menuBackup
        // 
        m_menuBackup.BackColor = Color.FromArgb(240, 241, 244);
        m_menuBackup.ButtonType = StyledButtonType.Sidebar;
        m_menuBackup.Dock = DockStyle.Top;
        m_menuBackup.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        m_menuBackup.ForeColor = Color.FromArgb(28, 32, 41);
        m_menuBackup.Location = new Point(3, 91);
        m_menuBackup.Margin = new Padding(3, 3, 3, 12);
        m_menuBackup.Name = "m_menuBackup";
        m_menuBackup.Padding = new Padding(16, 0, 0, 0);
        m_menuBackup.Size = new Size(190, 50);
        m_menuBackup.TabIndex = 1;
        m_menuBackup.Text = "백업";
        m_menuBackup.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // splitter2
        // 
        splitter2.BorderStyle = BorderStyle.FixedSingle;
        splitter2.Dock = DockStyle.Top;
        splitter2.Location = new Point(3, 156);
        splitter2.Name = "splitter2";
        splitter2.Size = new Size(190, 3);
        splitter2.TabIndex = 6;
        splitter2.TabStop = false;
        // 
        // m_menuRecovery
        // 
        m_menuRecovery.BackColor = Color.FromArgb(240, 241, 244);
        m_menuRecovery.ButtonType = StyledButtonType.Sidebar;
        m_menuRecovery.Dock = DockStyle.Top;
        m_menuRecovery.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        m_menuRecovery.ForeColor = Color.FromArgb(28, 32, 41);
        m_menuRecovery.Location = new Point(3, 165);
        m_menuRecovery.Margin = new Padding(3, 3, 3, 12);
        m_menuRecovery.Name = "m_menuRecovery";
        m_menuRecovery.Padding = new Padding(16, 0, 0, 0);
        m_menuRecovery.Size = new Size(190, 50);
        m_menuRecovery.TabIndex = 2;
        m_menuRecovery.Text = "복원";
        m_menuRecovery.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // splitter3
        // 
        splitter3.BorderStyle = BorderStyle.FixedSingle;
        splitter3.Dock = DockStyle.Top;
        splitter3.Location = new Point(3, 230);
        splitter3.Name = "splitter3";
        splitter3.Size = new Size(190, 3);
        splitter3.TabIndex = 7;
        splitter3.TabStop = false;
        // 
        // m_menuHistory
        // 
        m_menuHistory.BackColor = Color.FromArgb(240, 241, 244);
        m_menuHistory.ButtonType = StyledButtonType.Sidebar;
        m_menuHistory.Dock = DockStyle.Top;
        m_menuHistory.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        m_menuHistory.ForeColor = Color.FromArgb(28, 32, 41);
        m_menuHistory.Location = new Point(3, 239);
        m_menuHistory.Margin = new Padding(3, 3, 3, 12);
        m_menuHistory.Name = "m_menuHistory";
        m_menuHistory.Padding = new Padding(16, 0, 0, 0);
        m_menuHistory.Size = new Size(190, 50);
        m_menuHistory.TabIndex = 3;
        m_menuHistory.Text = "이력 관리";
        m_menuHistory.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // splitter4
        // 
        splitter4.BorderStyle = BorderStyle.FixedSingle;
        splitter4.Dock = DockStyle.Top;
        splitter4.Location = new Point(3, 304);
        splitter4.Name = "splitter4";
        splitter4.Size = new Size(190, 3);
        splitter4.TabIndex = 8;
        splitter4.TabStop = false;
        // 
        // m_menuSettings
        // 
        m_menuSettings.BackColor = Color.FromArgb(240, 241, 244);
        m_menuSettings.ButtonType = StyledButtonType.Sidebar;
        m_menuSettings.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        m_menuSettings.ForeColor = Color.FromArgb(28, 32, 41);
        m_menuSettings.Location = new Point(3, 313);
        m_menuSettings.Margin = new Padding(3, 3, 3, 12);
        m_menuSettings.Name = "m_menuSettings";
        m_menuSettings.Padding = new Padding(16, 0, 0, 0);
        m_menuSettings.Size = new Size(190, 50);
        m_menuSettings.TabIndex = 4;
        m_menuSettings.Text = "환경 설정";
        m_menuSettings.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // versionLabel
        // 
        versionLabel.Dock = DockStyle.Bottom;
        versionLabel.ForeColor = Color.FromArgb(108, 115, 128);
        versionLabel.Location = new Point(14, 631);
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
        Size = new Size(220, 681);
        sidebarPanel.ResumeLayout(false);
        menuPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Splitter splitter1;
    private Splitter splitter2;
    private Splitter splitter3;
    private Splitter splitter4;
}
