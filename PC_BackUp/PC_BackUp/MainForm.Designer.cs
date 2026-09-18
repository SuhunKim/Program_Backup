namespace PC_BackUp;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;
    private MainMenuControl mainMenu = null!;
    private Panel contentPanel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components is not null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        mainMenu = new MainMenuControl();
        contentPanel = new Panel();
        SuspendLayout();
        // 
        // mainMenu
        // 
        mainMenu.Dock = DockStyle.Left;
        mainMenu.Location = new Point(0, 0);
        mainMenu.Name = "mainMenu";
<<<<<<< Updated upstream
        mainMenu.Size = new Size(220, 700);
=======
        mainMenu.Size = new Size(PageLayoutMetrics.SidebarWidth, PageLayoutMetrics.FormHeight);
>>>>>>> Stashed changes
        mainMenu.TabIndex = 1;
        // 
        // contentPanel
<<<<<<< Updated upstream
        // 
        contentPanel.BackColor = Color.FromArgb(255, 255, 255);
        contentPanel.Dock = DockStyle.Right;
        contentPanel.Location = new Point(220, 0);
        contentPanel.Name = "contentPanel";
        contentPanel.Padding = new Padding(30, 24, 30, 28);
        contentPanel.Size = new Size(1000, 700);
=======
        //
        contentPanel.BackColor = ColorRGB.Background;
        contentPanel.Dock = DockStyle.Fill;
        contentPanel.Location = new Point(PageLayoutMetrics.SidebarWidth, 0);
        contentPanel.Name = "contentPanel";
        contentPanel.Padding = new Padding(PageLayoutMetrics.ContentHorizontalPadding, PageLayoutMetrics.ContentTopPadding,
            PageLayoutMetrics.ContentHorizontalPadding, PageLayoutMetrics.ContentBottomPadding);
        contentPanel.Size = new Size(PageLayoutMetrics.FormWidth - PageLayoutMetrics.SidebarWidth, PageLayoutMetrics.FormHeight);
>>>>>>> Stashed changes
        contentPanel.TabIndex = 0;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
<<<<<<< Updated upstream
        BackColor = Color.FromArgb(255, 255, 255);
        ClientSize = new Size(1220, 700);
        Controls.Add(contentPanel);
        Controls.Add(mainMenu);
        Font = new Font("맑은 고딕", 9F);
        MaximizeBox = false;
        MinimumSize = new Size(1020, 700);
=======
        BackColor = ColorRGB.Background;
        ClientSize = new Size(PageLayoutMetrics.FormWidth, PageLayoutMetrics.FormHeight);
        Controls.Add(contentPanel);
        Controls.Add(mainMenu);
        Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
        MinimumSize = new Size(PageLayoutMetrics.MinimumFormWidth, PageLayoutMetrics.MinimumFormHeight);
>>>>>>> Stashed changes
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PC Backup Manager";
        ResumeLayout(false);
    }
}
