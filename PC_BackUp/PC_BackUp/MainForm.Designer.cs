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
        mainMenu.Size = new Size(220, 800);
        mainMenu.TabIndex = 1;
        //
        // contentPanel
        //
        contentPanel.BackColor = ColorRGB.Background;
        contentPanel.Dock = DockStyle.Fill;
        contentPanel.Location = new Point(220, 0);
        contentPanel.Name = "contentPanel";
        contentPanel.Padding = new Padding(30, 24, 30, 28);
        contentPanel.Size = new Size(1060, 800);
        contentPanel.TabIndex = 0;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = ColorRGB.Background;
        ClientSize = new Size(1280, 800);
        Controls.Add(contentPanel);
        Controls.Add(mainMenu);
        Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
        MinimumSize = new Size(1080, 700);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PC Backup Manager";
        ResumeLayout(false);
    }
}
