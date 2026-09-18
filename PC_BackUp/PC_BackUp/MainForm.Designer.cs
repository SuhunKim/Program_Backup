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
        mainMenu.Size = new Size(220, 700);
        mainMenu.TabIndex = 1;
        // 
        // contentPanel
        // 
        contentPanel.BackColor = Color.FromArgb(255, 255, 255);
        contentPanel.Dock = DockStyle.Right;
        contentPanel.Location = new Point(220, 0);
        contentPanel.Name = "contentPanel";
        contentPanel.Padding = new Padding(30, 24, 30, 28);
        contentPanel.Size = new Size(1000, 700);
        contentPanel.TabIndex = 0;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(255, 255, 255);
        ClientSize = new Size(1220, 700);
        Controls.Add(contentPanel);
        Controls.Add(mainMenu);
        Font = new Font("맑은 고딕", 9F);
        MaximizeBox = false;
        MinimumSize = new Size(1020, 700);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PC Backup Manager";
        ResumeLayout(false);
    }
}
