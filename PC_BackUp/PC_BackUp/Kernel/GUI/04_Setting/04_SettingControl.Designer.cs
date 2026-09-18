namespace PC_BackUp;

partial class SettingControl
{
    private System.ComponentModel.IContainer? components;

    // [Codex - 2026.09.14] 모든 고정 UI와 동적 폴더 행의 편집용 템플릿을
    // InitializeComponent에서 생성해 Visual Studio 디자이너에서 직접 편집할 수 있게 한다.
    private Panel body = null!;
    private Panel headerPanel = null!;
    private Label headerTitleLabel = null!;
    private Label headerDescriptionLabel = null!;
    private CardPanel card = null!;
    private Label projectFieldLabel = null!;
    private TextBox m_oProjectText = null!;
    private Label executableFieldLabel = null!;
    private TextBox m_oExecutableText = null!;
    private StyledButton executableBrowseButton = null!;
    private Label backupPathFieldLabel = null!;
    private TextBox m_oBackupPathText = null!;
    private StyledButton backupBrowseButton = null!;
    private Panel divider = null!;
    private Panel folderSection = null!;
    private Panel folderTitleBar = null!;
    private Label folderSectionTitleLabel = null!;
    private StyledButton addFolderButton = null!;
    private CardPanel dropZonePanel = null!;
    private Label dropZoneLabel = null!;
    private FlowLayoutPanel m_oFolderRowsPanel = null!;
    private Panel m_oFolderRowTemplate = null!;
    private TextBox m_oFolderNameTemplate = null!;
    private StyledButton m_oFolderBrowseTemplate = null!;
    private StyledButton m_oFolderRemoveTemplate = null!;
    private Panel actionPanel = null!;
    private Label m_oStatusLabel = null!;
    private StyledButton saveButton = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        headerPanel = new Panel();
        headerDescriptionLabel = new Label();
        headerTitleLabel = new Label();
        body = new Panel();
        card = new CardPanel();
        folderSection = new Panel();
        m_oFolderRowsPanel = new FlowLayoutPanel();
        m_oFolderRowTemplate = new Panel();
        m_oFolderRemoveTemplate = new StyledButton();
        m_oFolderBrowseTemplate = new StyledButton();
        m_oFolderNameTemplate = new TextBox();
        dropZonePanel = new CardPanel();
        dropZoneLabel = new Label();
        folderTitleBar = new Panel();
        folderSectionTitleLabel = new Label();
        addFolderButton = new StyledButton();
        divider = new Panel();
        backupBrowseButton = new StyledButton();
        m_oBackupPathText = new TextBox();
        backupPathFieldLabel = new Label();
        executableBrowseButton = new StyledButton();
        m_oExecutableText = new TextBox();
        executableFieldLabel = new Label();
        m_oProjectText = new TextBox();
        projectFieldLabel = new Label();
        actionPanel = new Panel();
        m_oStatusLabel = new Label();
        saveButton = new StyledButton();
        headerPanel.SuspendLayout();
        body.SuspendLayout();
        card.SuspendLayout();
        folderSection.SuspendLayout();
        m_oFolderRowsPanel.SuspendLayout();
        m_oFolderRowTemplate.SuspendLayout();
        dropZonePanel.SuspendLayout();
        folderTitleBar.SuspendLayout();
        actionPanel.SuspendLayout();
        SuspendLayout();
<<<<<<< Updated upstream
        // 
        // headerPanel
        // 
        headerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
=======
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Height = PageLayoutMetrics.HeaderHeight;
        headerPanel.Padding = new Padding(8, 2, 0, 0);
        headerTitleLabel.Text = "환경 설정";
        headerTitleLabel.Dock = DockStyle.Top;
        headerTitleLabel.Height = 31;
        headerTitleLabel.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
        headerTitleLabel.ForeColor = ColorRGB.Text;
        headerTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        headerDescriptionLabel.Text = "백업 대상 프로그램과 저장 위치를 지정합니다.";
        headerDescriptionLabel.Dock = DockStyle.Top;
        headerDescriptionLabel.Height = 24;
        headerDescriptionLabel.Font = new Font("맑은 고딕", 10F);
        headerDescriptionLabel.ForeColor = ColorRGB.MutedText;
        headerDescriptionLabel.TextAlign = ContentAlignment.MiddleLeft;
>>>>>>> Stashed changes
        headerPanel.Controls.Add(headerDescriptionLabel);
        headerPanel.Controls.Add(headerTitleLabel);
        headerPanel.Location = new Point(0, 0);
        headerPanel.Name = "headerPanel";
        headerPanel.Padding = new Padding(8, 2, 0, 0);
        headerPanel.Size = new Size(1000, 66);
        headerPanel.TabIndex = 0;
        // 
        // headerDescriptionLabel
        // 
        headerDescriptionLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        headerDescriptionLabel.Font = new Font("맑은 고딕", 10F);
        headerDescriptionLabel.ForeColor = Color.FromArgb(108, 115, 128);
        headerDescriptionLabel.Location = new Point(8, 33);
        headerDescriptionLabel.Name = "headerDescriptionLabel";
        headerDescriptionLabel.Size = new Size(992, 24);
        headerDescriptionLabel.TabIndex = 1;
        headerDescriptionLabel.Text = "백업 대상 프로그램과 저장 위치를 지정합니다.";
        headerDescriptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // headerTitleLabel
        // 
        headerTitleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        headerTitleLabel.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
        headerTitleLabel.ForeColor = Color.FromArgb(28, 32, 41);
        headerTitleLabel.Location = new Point(8, 2);
        headerTitleLabel.Name = "headerTitleLabel";
        headerTitleLabel.Size = new Size(992, 31);
        headerTitleLabel.TabIndex = 0;
        headerTitleLabel.Text = "환경 설정";
        headerTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // body
        // 
        body.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        body.AutoScroll = true;
        body.AutoScrollMinSize = new Size(0, 536);
        body.BackColor = Color.FromArgb(255, 255, 255);
        body.Controls.Add(card);
        body.Location = new Point(0, 66);
        body.Name = "body";
        body.Padding = new Padding(0, 0, 0, 12);
        body.Size = new Size(1000, 566);
        body.TabIndex = 1;
        // 
        // card
        // 
        card.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        card.BackColor = Color.White;
        card.Controls.Add(folderSection);
        card.Controls.Add(divider);
        card.Controls.Add(backupBrowseButton);
        card.Controls.Add(m_oBackupPathText);
        card.Controls.Add(backupPathFieldLabel);
        card.Controls.Add(executableBrowseButton);
        card.Controls.Add(m_oExecutableText);
        card.Controls.Add(executableFieldLabel);
        card.Controls.Add(m_oProjectText);
        card.Controls.Add(projectFieldLabel);
        card.Location = new Point(0, 0);
        card.Name = "card";
        card.Padding = new Padding(28, 20, 28, 20);
<<<<<<< Updated upstream
        card.Size = new Size(1000, 536);
        card.TabIndex = 0;
        // 
        // folderSection
        // 
        folderSection.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        folderSection.Controls.Add(m_oFolderRowsPanel);
        folderSection.Controls.Add(dropZonePanel);
        folderSection.Controls.Add(folderTitleBar);
        folderSection.Location = new Point(28, 247);
        folderSection.Name = "folderSection";
        folderSection.Padding = new Padding(0, 10, 0, 0);
        folderSection.Size = new Size(944, 269);
        folderSection.TabIndex = 9;
        // 
        // m_oFolderRowsPanel
        // 
        m_oFolderRowsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        m_oFolderRowsPanel.AutoScroll = true;
        m_oFolderRowsPanel.Controls.Add(m_oFolderRowTemplate);
        m_oFolderRowsPanel.FlowDirection = FlowDirection.TopDown;
        m_oFolderRowsPanel.Location = new Point(0, 90);
        m_oFolderRowsPanel.Name = "m_oFolderRowsPanel";
        m_oFolderRowsPanel.Size = new Size(944, 169);
        m_oFolderRowsPanel.TabIndex = 2;
        m_oFolderRowsPanel.WrapContents = false;
        // 
        // m_oFolderRowTemplate
        // 
        m_oFolderRowTemplate.Controls.Add(m_oFolderRemoveTemplate);
        m_oFolderRowTemplate.Controls.Add(m_oFolderBrowseTemplate);
        m_oFolderRowTemplate.Controls.Add(m_oFolderNameTemplate);
        m_oFolderRowTemplate.Location = new Point(0, 0);
        m_oFolderRowTemplate.Margin = new Padding(0, 0, 0, 6);
        m_oFolderRowTemplate.Name = "m_oFolderRowTemplate";
        m_oFolderRowTemplate.Size = new Size(940, 40);
        m_oFolderRowTemplate.TabIndex = 0;
        // 
        // m_oFolderRemoveTemplate
        // 
        m_oFolderRemoveTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        m_oFolderRemoveTemplate.BackColor = Color.FromArgb(250, 235, 235);
        m_oFolderRemoveTemplate.BorderThickness = 1;
        m_oFolderRemoveTemplate.ButtonType = StyledButtonType.Secondary;
        m_oFolderRemoveTemplate.Cursor = Cursors.Hand;
        m_oFolderRemoveTemplate.Enabled = false;
        m_oFolderRemoveTemplate.Font = new Font("맑은 고딕", 9F);
        m_oFolderRemoveTemplate.ForeColor = Color.FromArgb(28, 32, 41);
        m_oFolderRemoveTemplate.Location = new Point(900, 0);
        m_oFolderRemoveTemplate.Name = "m_oFolderRemoveTemplate";
        m_oFolderRemoveTemplate.Size = new Size(40, 40);
        m_oFolderRemoveTemplate.TabIndex = 2;
        m_oFolderRemoveTemplate.Text = "－";
        // 
        // m_oFolderBrowseTemplate
        // 
        m_oFolderBrowseTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        m_oFolderBrowseTemplate.BackColor = Color.FromArgb(233, 238, 245);
        m_oFolderBrowseTemplate.BorderThickness = 1;
        m_oFolderBrowseTemplate.ButtonType = StyledButtonType.Secondary;
        m_oFolderBrowseTemplate.Cursor = Cursors.Hand;
        m_oFolderBrowseTemplate.Font = new Font("맑은 고딕", 9F);
        m_oFolderBrowseTemplate.ForeColor = Color.FromArgb(28, 32, 41);
        m_oFolderBrowseTemplate.Location = new Point(824, 0);
        m_oFolderBrowseTemplate.Name = "m_oFolderBrowseTemplate";
        m_oFolderBrowseTemplate.Size = new Size(70, 40);
        m_oFolderBrowseTemplate.TabIndex = 1;
        m_oFolderBrowseTemplate.Text = "찾기";
        // 
        // m_oFolderNameTemplate
        // 
        m_oFolderNameTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        m_oFolderNameTemplate.BorderStyle = BorderStyle.FixedSingle;
        m_oFolderNameTemplate.Font = new Font("맑은 고딕", 10F);
        m_oFolderNameTemplate.Location = new Point(0, 7);
        m_oFolderNameTemplate.Name = "m_oFolderNameTemplate";
        m_oFolderNameTemplate.Size = new Size(818, 25);
        m_oFolderNameTemplate.TabIndex = 0;
        m_oFolderNameTemplate.Text = "_bin";
        // 
        // dropZonePanel
        // 
        dropZonePanel.AllowDrop = true;
        dropZonePanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        dropZonePanel.BackColor = Color.FromArgb(233, 238, 245);
        dropZonePanel.Controls.Add(dropZoneLabel);
        dropZonePanel.Location = new Point(0, 48);
        dropZonePanel.Name = "dropZonePanel";
        dropZonePanel.Padding = new Padding(3);
        dropZonePanel.Size = new Size(944, 36);
        dropZonePanel.TabIndex = 1;
        // 
        // dropZoneLabel
        // 
        dropZoneLabel.AllowDrop = true;
        dropZoneLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dropZoneLabel.Font = new Font("맑은 고딕", 9F);
        dropZoneLabel.ForeColor = Color.FromArgb(108, 115, 128);
        dropZoneLabel.Location = new Point(3, 3);
        dropZoneLabel.Name = "dropZoneLabel";
        dropZoneLabel.Size = new Size(938, 30);
        dropZoneLabel.TabIndex = 0;
        dropZoneLabel.Text = "여기에 폴더를 끌어다 놓아 추가";
        dropZoneLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // folderTitleBar
        // 
        folderTitleBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        folderTitleBar.Controls.Add(folderSectionTitleLabel);
        folderTitleBar.Controls.Add(addFolderButton);
        folderTitleBar.Location = new Point(0, 10);
        folderTitleBar.Name = "folderTitleBar";
        folderTitleBar.Size = new Size(944, 32);
        folderTitleBar.TabIndex = 0;
        // 
        // folderSectionTitleLabel
        // 
        folderSectionTitleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        folderSectionTitleLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        folderSectionTitleLabel.ForeColor = Color.FromArgb(28, 32, 41);
        folderSectionTitleLabel.Location = new Point(0, 0);
        folderSectionTitleLabel.Name = "folderSectionTitleLabel";
        folderSectionTitleLabel.Size = new Size(820, 32);
        folderSectionTitleLabel.TabIndex = 0;
        folderSectionTitleLabel.Text = "선별 백업 대상 폴더";
        folderSectionTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // addFolderButton
        // 
        addFolderButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        addFolderButton.BackColor = Color.FromArgb(233, 238, 245);
        addFolderButton.BorderThickness = 1;
        addFolderButton.ButtonType = StyledButtonType.Secondary;
        addFolderButton.Font = new Font("맑은 고딕", 9F);
        addFolderButton.ForeColor = Color.FromArgb(28, 32, 41);
        addFolderButton.Location = new Point(828, 2);
        addFolderButton.Name = "addFolderButton";
        addFolderButton.Size = new Size(116, 28);
        addFolderButton.TabIndex = 1;
        addFolderButton.Text = "＋ 폴더 추가";
        // 
        // divider
        // 
        divider.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        divider.BackColor = Color.FromArgb(224, 226, 231);
        divider.Location = new Point(28, 236);
        divider.Name = "divider";
        divider.Size = new Size(944, 1);
        divider.TabIndex = 8;
        // 
        // backupBrowseButton
        // 
        backupBrowseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        backupBrowseButton.BackColor = Color.FromArgb(233, 238, 245);
        backupBrowseButton.BorderThickness = 1;
        backupBrowseButton.ButtonType = StyledButtonType.Secondary;
        backupBrowseButton.Cursor = Cursors.Hand;
        backupBrowseButton.Font = new Font("맑은 고딕", 9F);
        backupBrowseButton.ForeColor = Color.FromArgb(28, 32, 41);
        backupBrowseButton.Location = new Point(867, 182);
        backupBrowseButton.Name = "backupBrowseButton";
        backupBrowseButton.Size = new Size(105, 36);
        backupBrowseButton.TabIndex = 7;
        backupBrowseButton.Text = "폴더 찾기";
        backupBrowseButton.Click += BrowseBackupFolder;
        // 
        // m_oBackupPathText
        // 
        m_oBackupPathText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        m_oBackupPathText.BorderStyle = BorderStyle.FixedSingle;
        m_oBackupPathText.Font = new Font("맑은 고딕", 10F);
        m_oBackupPathText.Location = new Point(208, 188);
        m_oBackupPathText.Name = "m_oBackupPathText";
        m_oBackupPathText.Size = new Size(647, 25);
        m_oBackupPathText.TabIndex = 6;
        m_oBackupPathText.Text = "D:\\Backup";
        // 
        // backupPathFieldLabel
        // 
        backupPathFieldLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        backupPathFieldLabel.ForeColor = Color.FromArgb(28, 32, 41);
        backupPathFieldLabel.Location = new Point(28, 164);
        backupPathFieldLabel.Name = "backupPathFieldLabel";
        backupPathFieldLabel.Size = new Size(180, 72);
        backupPathFieldLabel.TabIndex = 5;
        backupPathFieldLabel.Text = "백업 저장 위치\r\nZIP 및 폴더 백업 보관";
        backupPathFieldLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // executableBrowseButton
        // 
        executableBrowseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        executableBrowseButton.BackColor = Color.FromArgb(233, 238, 245);
        executableBrowseButton.BorderThickness = 1;
        executableBrowseButton.ButtonType = StyledButtonType.Secondary;
        executableBrowseButton.Cursor = Cursors.Hand;
        executableBrowseButton.Font = new Font("맑은 고딕", 9F);
        executableBrowseButton.ForeColor = Color.FromArgb(28, 32, 41);
        executableBrowseButton.Location = new Point(867, 110);
        executableBrowseButton.Name = "executableBrowseButton";
        executableBrowseButton.Size = new Size(105, 36);
        executableBrowseButton.TabIndex = 4;
        executableBrowseButton.Text = "파일 찾기";
        executableBrowseButton.Click += BrowseExecutable;
        // 
        // m_oExecutableText
        // 
        m_oExecutableText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        m_oExecutableText.BorderStyle = BorderStyle.FixedSingle;
        m_oExecutableText.Font = new Font("맑은 고딕", 10F);
        m_oExecutableText.Location = new Point(208, 116);
        m_oExecutableText.Name = "m_oExecutableText";
        m_oExecutableText.Size = new Size(647, 25);
        m_oExecutableText.TabIndex = 3;
        m_oExecutableText.Text = "D:\\Program\\Application.exe";
        // 
        // executableFieldLabel
        // 
        executableFieldLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        executableFieldLabel.ForeColor = Color.FromArgb(28, 32, 41);
        executableFieldLabel.Location = new Point(28, 92);
        executableFieldLabel.Name = "executableFieldLabel";
        executableFieldLabel.Size = new Size(180, 72);
        executableFieldLabel.TabIndex = 2;
        executableFieldLabel.Text = "실행 파일 원본 경로\r\n대상 EXE 파일";
        executableFieldLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // m_oProjectText
        // 
        m_oProjectText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        m_oProjectText.BorderStyle = BorderStyle.FixedSingle;
        m_oProjectText.Font = new Font("맑은 고딕", 10F);
        m_oProjectText.Location = new Point(208, 44);
        m_oProjectText.Name = "m_oProjectText";
        m_oProjectText.Size = new Size(764, 25);
        m_oProjectText.TabIndex = 1;
        m_oProjectText.Text = "PC_BackUp";
        // 
        // projectFieldLabel
        // 
        projectFieldLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        projectFieldLabel.ForeColor = Color.FromArgb(28, 32, 41);
        projectFieldLabel.Location = new Point(28, 20);
        projectFieldLabel.Name = "projectFieldLabel";
        projectFieldLabel.Size = new Size(180, 72);
        projectFieldLabel.TabIndex = 0;
        projectFieldLabel.Text = "타겟 프로젝트 이름";
        projectFieldLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // actionPanel
        // 
        actionPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        actionPanel.BackColor = Color.FromArgb(255, 255, 255);
=======
        //
        // table (타겟 프로젝트 이름 / 실행 파일 원본 경로 / 백업 저장 위치)
        //
        table.Dock = DockStyle.Top;
        table.Height = 3 * PageLayoutMetrics.FormRowHeight;
        table.ColumnCount = 3;
        table.RowCount = 3;
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, PageLayoutMetrics.FormLabelWidth));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, PageLayoutMetrics.FormRowHeight));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, PageLayoutMetrics.FormRowHeight));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, PageLayoutMetrics.FormRowHeight));

        projectFieldLabel.Text = "타겟 프로젝트 이름\r\n ";
        projectFieldLabel.Dock = DockStyle.Fill;
        projectFieldLabel.TextAlign = ContentAlignment.MiddleLeft;
        projectFieldLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        projectFieldLabel.ForeColor = ColorRGB.Text;

        executableFieldLabel.Text = "실행 파일 원본 경로\r\n대상 EXE 파일";
        executableFieldLabel.Dock = DockStyle.Fill;
        executableFieldLabel.TextAlign = ContentAlignment.MiddleLeft;
        executableFieldLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        executableFieldLabel.ForeColor = ColorRGB.Text;

        backupPathFieldLabel.Text = "백업 저장 위치\r\nZIP 및 폴더 백업 보관";
        backupPathFieldLabel.Dock = DockStyle.Fill;
        backupPathFieldLabel.TextAlign = ContentAlignment.MiddleLeft;
        backupPathFieldLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        backupPathFieldLabel.ForeColor = ColorRGB.Text;

        m_oProjectText.Dock = DockStyle.Fill;
        m_oProjectText.Margin = new Padding(0, 18, 12, 18);
        m_oProjectText.Font = new Font("맑은 고딕", 10F);
        m_oProjectText.BorderStyle = BorderStyle.FixedSingle;

        m_oExecutableText.Dock = DockStyle.Fill;
        m_oExecutableText.Margin = new Padding(0, 18, 12, 18);
        m_oExecutableText.Font = new Font("맑은 고딕", 10F);
        m_oExecutableText.BorderStyle = BorderStyle.FixedSingle;

        m_oBackupPathText.Dock = DockStyle.Fill;
        m_oBackupPathText.Margin = new Padding(0, 18, 12, 18);
        m_oBackupPathText.Font = new Font("맑은 고딕", 10F);
        m_oBackupPathText.BorderStyle = BorderStyle.FixedSingle;

        executableBrowseButton.ButtonType = StyledButtonType.Secondary;
        executableBrowseButton.Text = "파일 찾기";
        executableBrowseButton.Dock = DockStyle.Fill;
        executableBrowseButton.Margin = new Padding(0, 18, 0, 18);
        executableBrowseButton.NormalBackColor = ColorRGB.SecondaryBackground;
        executableBrowseButton.ForeColor = ColorRGB.Text;
        executableBrowseButton.Cursor = Cursors.Hand;
        executableBrowseButton.BorderColor = ColorRGB.Border;
        executableBrowseButton.Click += BrowseExecutable;

        backupBrowseButton.ButtonType = StyledButtonType.Secondary;
        backupBrowseButton.Text = "폴더 찾기";
        backupBrowseButton.Dock = DockStyle.Fill;
        backupBrowseButton.Margin = new Padding(0, 18, 0, 18);
        backupBrowseButton.NormalBackColor = ColorRGB.SecondaryBackground;
        backupBrowseButton.ForeColor = ColorRGB.Text;
        backupBrowseButton.Cursor = Cursors.Hand;
        backupBrowseButton.BorderColor = ColorRGB.Border;
        backupBrowseButton.Click += BrowseBackupFolder;

        table.Controls.Add(projectFieldLabel, 0, 0);
        table.Controls.Add(m_oProjectText, 1, 0);
        table.SetColumnSpan(m_oProjectText, 2);
        table.Controls.Add(executableFieldLabel, 0, 1);
        table.Controls.Add(m_oExecutableText, 1, 1);
        table.Controls.Add(executableBrowseButton, 2, 1);
        table.Controls.Add(backupPathFieldLabel, 0, 2);
        table.Controls.Add(m_oBackupPathText, 1, 2);
        table.Controls.Add(backupBrowseButton, 2, 2);
        //
        // divider
        //
        divider.Dock = DockStyle.Top;
        divider.Height = 1;
        divider.BackColor = ColorRGB.Border;
        //
        // folderSection (선별 백업 대상 폴더 — 목록 자체는 동적이라 코드에서 채운다)
        //
        folderSection.Dock = DockStyle.Top;
        folderSection.Height = 274;
        folderSection.Padding = new Padding(0, 10, 0, 0);

        folderTitleBar.Dock = DockStyle.Top;
        folderTitleBar.Height = 32;
        folderSectionTitleLabel.Text = "선별 백업 대상 폴더";
        folderSectionTitleLabel.Dock = DockStyle.Fill;
        folderSectionTitleLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        folderSectionTitleLabel.ForeColor = ColorRGB.Text;
        folderSectionTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        addFolderButton.ButtonType = StyledButtonType.Secondary;
        addFolderButton.Text = "＋ 폴더 추가";
        addFolderButton.Dock = DockStyle.Right;
        // 아래 행의 [찾기][－] 버튼 폭(+사이 여백)과 맞춰서 바로 그 위에 정렬되도록 한다.
        addFolderButton.Width = RowBrowseButtonWidth + RowButtonGap + RowRemoveButtonWidth;
        addFolderButton.Height = 28;
        addFolderButton.NormalBackColor = ColorRGB.SecondaryBackground;
        addFolderButton.ForeColor = ColorRGB.Text;
        addFolderButton.BorderColor = ColorRGB.Border;
        folderTitleBar.Controls.Add(folderSectionTitleLabel);
        folderTitleBar.Controls.Add(addFolderButton);
        //
        // dropZonePanel (탐색기에서 폴더를 끌어다 놓으면 DropZone_DragDrop이 받아서 추가한다)
        //
        dropZonePanel.Dock = DockStyle.Top;
        dropZonePanel.Height = 36;
        dropZonePanel.Margin = new Padding(0, 6, 0, 6);
        dropZonePanel.BackColor = ColorRGB.SecondaryBackground;
        dropZonePanel.AllowDrop = true;
        dropZoneLabel.Text = "여기에 폴더를 끌어다 놓아 추가";
        dropZoneLabel.Dock = DockStyle.Fill;
        dropZoneLabel.TextAlign = ContentAlignment.MiddleCenter;
        dropZoneLabel.Font = new Font("맑은 고딕", 9F);
        dropZoneLabel.ForeColor = ColorRGB.MutedText;
        dropZoneLabel.AllowDrop = true;
        dropZonePanel.Controls.Add(dropZoneLabel);

        m_oFolderRowsPanel.Dock = DockStyle.Top;
        m_oFolderRowsPanel.Height = 194;
        m_oFolderRowsPanel.AutoScroll = true;
        m_oFolderRowsPanel.FlowDirection = FlowDirection.TopDown;
        m_oFolderRowsPanel.WrapContents = false;

        folderSection.Controls.Add(m_oFolderRowsPanel);
        folderSection.Controls.Add(dropZonePanel);
        folderSection.Controls.Add(folderTitleBar);

        card.Controls.Add(divider);
        card.Controls.Add(folderSection);
        card.Controls.Add(table);
        body.Controls.Add(card);
        //
        // actionPanel (저장 버튼 + 상태 표시줄)
        //
        actionPanel.Dock = DockStyle.Bottom;
        actionPanel.Height = PageLayoutMetrics.ActionBarHeight;
        actionPanel.Padding = new Padding(0, 14, 0, 10);
        actionPanel.BackColor = ColorRGB.Background;
        saveButton.Text = "설정 저장";
        saveButton.Dock = DockStyle.Right;
        saveButton.Width = PageLayoutMetrics.PrimaryButtonWidth;
        saveButton.Height = 42;
        saveButton.Click += UiClick_Save;
        m_oStatusLabel.Dock = DockStyle.Fill;
        m_oStatusLabel.ForeColor = ColorRGB.MutedText;
        m_oStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
        m_oStatusLabel.AutoEllipsis = true;
>>>>>>> Stashed changes
        actionPanel.Controls.Add(m_oStatusLabel);
        actionPanel.Controls.Add(saveButton);
        actionPanel.Location = new Point(0, 632);
        actionPanel.Name = "actionPanel";
        actionPanel.Padding = new Padding(0, 14, 0, 10);
        actionPanel.Size = new Size(1000, 68);
        actionPanel.TabIndex = 2;
        // 
        // m_oStatusLabel
        // 
        m_oStatusLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        m_oStatusLabel.AutoEllipsis = true;
        m_oStatusLabel.ForeColor = Color.FromArgb(108, 115, 128);
        m_oStatusLabel.Location = new Point(18, 14);
        m_oStatusLabel.Name = "m_oStatusLabel";
        m_oStatusLabel.Size = new Size(824, 42);
        m_oStatusLabel.TabIndex = 0;
        m_oStatusLabel.Text = "설정 파일 경로와 저장 결과가 표시됩니다.";
        m_oStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // saveButton
        // 
        saveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        saveButton.BackColor = Color.FromArgb(30, 100, 199);
        saveButton.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        saveButton.ForeColor = Color.White;
        saveButton.Location = new Point(860, 14);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(140, 42);
        saveButton.TabIndex = 1;
        saveButton.Text = "설정 저장";
        saveButton.Click += UiClick_Save;
        // 
        // SettingControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(body);
        Controls.Add(actionPanel);
        Controls.Add(headerPanel);
        Name = "SettingControl";
        Size = new Size(1000, 700);
        headerPanel.ResumeLayout(false);
        body.ResumeLayout(false);
        card.ResumeLayout(false);
        card.PerformLayout();
        folderSection.ResumeLayout(false);
        m_oFolderRowsPanel.ResumeLayout(false);
        m_oFolderRowTemplate.ResumeLayout(false);
        m_oFolderRowTemplate.PerformLayout();
        dropZonePanel.ResumeLayout(false);
        folderTitleBar.ResumeLayout(false);
        actionPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
