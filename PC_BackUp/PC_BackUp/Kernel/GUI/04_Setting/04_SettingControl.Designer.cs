namespace PC_BackUp;

partial class SettingControl
{
    private System.ComponentModel.IContainer? components;

    // 화면의 고정 골격 — 값이 바뀌지 않는 부분이라 전부 필드로 선언해 Visual Studio
    // 디자이너에서도 그대로 열어서 확인할 수 있게 한다. 선별 폴더 행 목록(m_oFolderRowsPanel의
    // 내용물)은 설정값/사용자 조작에 따라 개수가 바뀌는 진짜 동적인 부분이라 여기서 다루지
    // 않고 04_SettingControl.cs의 AddFolderRow()/RemoveFolderRow() 쪽 코드로만 처리한다.
    private Panel body = null!;
    private Panel headerPanel = null!;
    private Label headerTitleLabel = null!;
    private Label headerDescriptionLabel = null!;
    private CardPanel card = null!;
    private TableLayoutPanel table = null!;
    private Label projectFieldLabel = null!, executableFieldLabel = null!, backupPathFieldLabel = null!;
    private StyledButton executableBrowseButton = null!;
    private StyledButton backupBrowseButton = null!;
    private Panel divider = null!;
    private Panel folderSection = null!;
    private Panel folderTitleBar = null!;
    private Label folderSectionTitleLabel = null!;
    private StyledButton addFolderButton = null!;
    private CardPanel dropZonePanel = null!;
    private Label dropZoneLabel = null!;
    private Panel actionPanel = null!;
    private StyledButton saveButton = null!;

    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }

    private void InitializeComponent()
    {
        body = new Panel();
        card = new CardPanel();
        divider = new Panel();
        folderSection = new Panel();
        dropZonePanel = new CardPanel();
        dropZoneLabel = new Label();
        folderTitleBar = new Panel();
        folderSectionTitleLabel = new Label();
        addFolderButton = new StyledButton();
        table = new TableLayoutPanel();
        projectFieldLabel = new Label();
        executableFieldLabel = new Label();
        executableBrowseButton = new StyledButton();
        backupPathFieldLabel = new Label();
        backupBrowseButton = new StyledButton();
        headerPanel = new Panel();
        headerDescriptionLabel = new Label();
        headerTitleLabel = new Label();
        actionPanel = new Panel();
        saveButton = new StyledButton();
        body.SuspendLayout();
        card.SuspendLayout();
        folderSection.SuspendLayout();
        dropZonePanel.SuspendLayout();
        folderTitleBar.SuspendLayout();
        table.SuspendLayout();
        headerPanel.SuspendLayout();
        actionPanel.SuspendLayout();
        SuspendLayout();
        // 
        // body
        // 
        body.AutoScroll = true;
        body.BackColor = Color.FromArgb(255, 255, 255);
        body.Controls.Add(card);
        body.Controls.Add(actionPanel);
        body.Location = new Point(0, 104);
        body.Name = "body";
        body.Padding = new Padding(0, 0, 0, 12);
        body.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        body.Size = new Size(1000, 596);
        body.TabIndex = 0;
        // 
        // card
        // 
        card.BackColor = Color.White;
        card.Controls.Add(divider);
        card.Controls.Add(folderSection);
        card.Controls.Add(table);
        card.Location = new Point(0, 0);
        card.Name = "card";
        card.Padding = new Padding(28, 20, 28, 20);
        card.Size = new Size(960, 477);
        card.TabIndex = 0;
        // 
        // divider
        // 
        divider.BackColor = Color.FromArgb(224, 226, 231);
        divider.Dock = DockStyle.Top;
        divider.Location = new Point(28, 297);
        divider.Name = "divider";
        divider.Size = new Size(904, 1);
        divider.TabIndex = 0;
        // 
        // folderSection
        // 
        folderSection.Controls.Add(m_oFolderRowsPanel);
        folderSection.Controls.Add(dropZonePanel);
        folderSection.Controls.Add(folderTitleBar);
        folderSection.Dock = DockStyle.Top;
        folderSection.Location = new Point(28, 23);
        folderSection.Name = "folderSection";
        folderSection.Padding = new Padding(0, 10, 0, 0);
        folderSection.Size = new Size(904, 220);
        folderSection.TabIndex = 1;
        // 
        // dropZonePanel
        // 
        dropZonePanel.AllowDrop = true;
        dropZonePanel.BackColor = Color.FromArgb(233, 238, 245);
        dropZonePanel.Controls.Add(dropZoneLabel);
        dropZonePanel.Dock = DockStyle.Top;
        dropZonePanel.Location = new Point(0, 42);
        dropZonePanel.Margin = new Padding(0, 6, 0, 6);
        dropZonePanel.Name = "dropZonePanel";
        dropZonePanel.Padding = new Padding(3);
        dropZonePanel.Size = new Size(904, 36);
        dropZonePanel.TabIndex = 0;
        // 
        // dropZoneLabel
        // 
        dropZoneLabel.AllowDrop = true;
        dropZoneLabel.Dock = DockStyle.Fill;
        dropZoneLabel.Font = new Font("맑은 고딕", 9F);
        dropZoneLabel.ForeColor = Color.FromArgb(108, 115, 128);
        dropZoneLabel.Location = new Point(3, 3);
        dropZoneLabel.Name = "dropZoneLabel";
        dropZoneLabel.Size = new Size(898, 30);
        dropZoneLabel.TabIndex = 0;
        dropZoneLabel.Text = "여기에 폴더를 끌어다 놓아 추가";
        dropZoneLabel.TextAlign = ContentAlignment.MiddleCenter;
        m_oFolderRowsPanel.Dock = DockStyle.Top;
        m_oFolderRowsPanel.Height = 140;
        m_oFolderRowsPanel.AutoScroll = true;
        m_oFolderRowsPanel.FlowDirection = FlowDirection.TopDown;
        m_oFolderRowsPanel.WrapContents = false;
        // 
        // folderTitleBar
        // 
        folderTitleBar.Controls.Add(folderSectionTitleLabel);
        folderTitleBar.Controls.Add(addFolderButton);
        folderTitleBar.Dock = DockStyle.Top;
        folderTitleBar.Location = new Point(0, 10);
        folderTitleBar.Name = "folderTitleBar";
        folderTitleBar.Size = new Size(904, 32);
        folderTitleBar.TabIndex = 1;
        // 
        // folderSectionTitleLabel
        // 
        folderSectionTitleLabel.Dock = DockStyle.Fill;
        folderSectionTitleLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        folderSectionTitleLabel.ForeColor = Color.FromArgb(28, 32, 41);
        folderSectionTitleLabel.Location = new Point(0, 0);
        folderSectionTitleLabel.Name = "folderSectionTitleLabel";
        folderSectionTitleLabel.Size = new Size(799, 32);
        folderSectionTitleLabel.TabIndex = 0;
        folderSectionTitleLabel.Text = "선별 백업 대상 폴더";
        folderSectionTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // addFolderButton
        // 
        addFolderButton.BackColor = Color.FromArgb(233, 238, 245);
        addFolderButton.BorderThickness = 1;
        addFolderButton.ButtonType = StyledButtonType.Secondary;
        addFolderButton.Dock = DockStyle.Right;
        addFolderButton.Font = new Font("맑은 고딕", 9F);
        addFolderButton.ForeColor = Color.FromArgb(28, 32, 41);
        addFolderButton.Location = new Point(799, 0);
        addFolderButton.Name = "addFolderButton";
        addFolderButton.Size = new Size(105, 32);
        addFolderButton.TabIndex = 1;
        addFolderButton.Text = "＋ 폴더 추가";
        // 
        // table
        // 
        table.ColumnCount = 3;
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105F));
        // [Codex - 2026.09.21] 이슈 6 레이아웃 변경에서 빠진 설정 입력 컨트롤을 다시 연결한다.
        ConfigureTextBox(m_oProjectText);
        ConfigureTextBox(m_oExecutableText);
        ConfigureTextBox(m_oBackupPathText);
        table.Controls.Add(projectFieldLabel, 0, 0);
        table.Controls.Add(m_oProjectText, 1, 0);
        table.SetColumnSpan(m_oProjectText, 2);
        table.Controls.Add(executableFieldLabel, 0, 1);
        table.Controls.Add(m_oExecutableText, 1, 1);
        table.Controls.Add(executableBrowseButton, 2, 1);
        table.Controls.Add(backupPathFieldLabel, 0, 2);
        table.Controls.Add(m_oBackupPathText, 1, 2);
        table.Controls.Add(backupBrowseButton, 2, 2);
        table.Dock = DockStyle.Top;
        table.Location = new Point(28, 20);
        table.Name = "table";
        table.RowCount = 3;
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
        table.Size = new Size(904, 216);
        table.TabIndex = 2;
        // 
        // projectFieldLabel
        // 
        projectFieldLabel.Dock = DockStyle.Fill;
        projectFieldLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        projectFieldLabel.ForeColor = Color.FromArgb(28, 32, 41);
        projectFieldLabel.Location = new Point(3, 0);
        projectFieldLabel.Name = "projectFieldLabel";
        projectFieldLabel.Size = new Size(174, 72);
        projectFieldLabel.TabIndex = 0;
        projectFieldLabel.Text = "타겟 프로젝트 이름\r\n ";
        projectFieldLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // executableFieldLabel
        // 
        executableFieldLabel.Dock = DockStyle.Fill;
        executableFieldLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        executableFieldLabel.ForeColor = Color.FromArgb(28, 32, 41);
        executableFieldLabel.Location = new Point(3, 72);
        executableFieldLabel.Name = "executableFieldLabel";
        executableFieldLabel.Size = new Size(174, 72);
        executableFieldLabel.TabIndex = 1;
        executableFieldLabel.Text = "실행 파일 원본 경로\r\n대상 EXE 파일";
        executableFieldLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // executableBrowseButton
        // 
        executableBrowseButton.BackColor = Color.FromArgb(233, 238, 245);
        executableBrowseButton.BorderThickness = 1;
        executableBrowseButton.ButtonType = StyledButtonType.Secondary;
        executableBrowseButton.Cursor = Cursors.Hand;
        executableBrowseButton.Dock = DockStyle.Fill;
        executableBrowseButton.Font = new Font("맑은 고딕", 9F);
        executableBrowseButton.ForeColor = Color.FromArgb(28, 32, 41);
        executableBrowseButton.Location = new Point(799, 90);
        executableBrowseButton.Margin = new Padding(0, 18, 0, 18);
        executableBrowseButton.Name = "executableBrowseButton";
        executableBrowseButton.Size = new Size(105, 36);
        executableBrowseButton.TabIndex = 2;
        executableBrowseButton.Text = "파일 찾기";
        executableBrowseButton.Click += BrowseExecutable;
        // 
        // backupPathFieldLabel
        // 
        backupPathFieldLabel.Dock = DockStyle.Fill;
        backupPathFieldLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        backupPathFieldLabel.ForeColor = Color.FromArgb(28, 32, 41);
        backupPathFieldLabel.Location = new Point(3, 144);
        backupPathFieldLabel.Name = "backupPathFieldLabel";
        backupPathFieldLabel.Size = new Size(174, 72);
        backupPathFieldLabel.TabIndex = 3;
        backupPathFieldLabel.Text = "백업 저장 위치\r\nZIP 및 폴더 백업 보관";
        backupPathFieldLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // backupBrowseButton
        // 
        backupBrowseButton.BackColor = Color.FromArgb(233, 238, 245);
        backupBrowseButton.BorderThickness = 1;
        backupBrowseButton.ButtonType = StyledButtonType.Secondary;
        backupBrowseButton.Cursor = Cursors.Hand;
        backupBrowseButton.Dock = DockStyle.Fill;
        backupBrowseButton.Font = new Font("맑은 고딕", 9F);
        backupBrowseButton.ForeColor = Color.FromArgb(28, 32, 41);
        backupBrowseButton.Location = new Point(799, 162);
        backupBrowseButton.Margin = new Padding(0, 18, 0, 18);
        backupBrowseButton.Name = "backupBrowseButton";
        backupBrowseButton.Size = new Size(105, 36);
        backupBrowseButton.TabIndex = 4;
        backupBrowseButton.Text = "폴더 찾기";
        backupBrowseButton.Click += BrowseBackupFolder;
        // 
        // headerPanel
        // 
        headerPanel.Controls.Add(headerDescriptionLabel);
        headerPanel.Controls.Add(headerTitleLabel);
        headerPanel.Location = new Point(20, 16);
        headerPanel.Name = "headerPanel";
        headerPanel.Padding = new Padding(8, 2, 0, 0);
        headerPanel.Size = new Size(960, 66);
        headerPanel.TabIndex = 2;
        // 
        // headerDescriptionLabel
        // 
        headerDescriptionLabel.Dock = DockStyle.Top;
        headerDescriptionLabel.Font = new Font("맑은 고딕", 10F);
        headerDescriptionLabel.ForeColor = Color.FromArgb(108, 115, 128);
        headerDescriptionLabel.Location = new Point(8, 33);
        headerDescriptionLabel.Name = "headerDescriptionLabel";
        headerDescriptionLabel.Size = new Size(952, 24);
        headerDescriptionLabel.TabIndex = 0;
        headerDescriptionLabel.Text = "백업 대상 프로그램과 저장 위치를 지정합니다.";
        headerDescriptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // headerTitleLabel
        // 
        headerTitleLabel.Dock = DockStyle.Top;
        headerTitleLabel.Font = new Font("맑은 고딕", 18F, FontStyle.Bold, GraphicsUnit.Point, 129);
        headerTitleLabel.ForeColor = Color.FromArgb(28, 32, 41);
        headerTitleLabel.Location = new Point(8, 2);
        headerTitleLabel.Name = "headerTitleLabel";
        headerTitleLabel.Size = new Size(952, 31);
        headerTitleLabel.TabIndex = 1;
        headerTitleLabel.Text = "환경 설정";
        headerTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // actionPanel
        // 
        actionPanel.BackColor = Color.FromArgb(255, 255, 255);
        actionPanel.Controls.Add(m_oStatusLabel);
        actionPanel.Controls.Add(saveButton);
        actionPanel.Dock = DockStyle.Bottom;
        actionPanel.Location = new Point(20, 530);
        actionPanel.Name = "actionPanel";
        actionPanel.Padding = new Padding(0, 14, 0, 10);
        actionPanel.Size = new Size(960, 66);
        actionPanel.TabIndex = 1;
        m_oStatusLabel.Dock = DockStyle.Fill;
        m_oStatusLabel.ForeColor = ColorRGB.MutedText;
        m_oStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
        m_oStatusLabel.AutoEllipsis = true;
        // 
        // saveButton
        // 
        saveButton.BackColor = Color.FromArgb(30, 100, 199);
        saveButton.Dock = DockStyle.Right;
        saveButton.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        saveButton.ForeColor = Color.White;
        saveButton.Location = new Point(820, 14);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(140, 42);
        saveButton.TabIndex = 0;
        saveButton.Text = "설정 저장";
        saveButton.Click += UiClick_Save;
        // 
        // SettingControl
        // 
        Controls.Add(body);
        Controls.Add(headerPanel);
        Name = "SettingControl";
        Size = new Size(1000, 700);
        body.ResumeLayout(false);
        card.ResumeLayout(false);
        folderSection.ResumeLayout(false);
        dropZonePanel.ResumeLayout(false);
        folderTitleBar.ResumeLayout(false);
        table.ResumeLayout(false);
        headerPanel.ResumeLayout(false);
        actionPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
