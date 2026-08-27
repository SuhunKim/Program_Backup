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
        headerPanel = new Panel();
        headerTitleLabel = new Label();
        headerDescriptionLabel = new Label();
        card = new CardPanel();
        table = new TableLayoutPanel();
        projectFieldLabel = new Label();
        executableFieldLabel = new Label();
        backupPathFieldLabel = new Label();
        executableBrowseButton = new StyledButton();
        backupBrowseButton = new StyledButton();
        divider = new Panel();
        folderSection = new Panel();
        folderTitleBar = new Panel();
        folderSectionTitleLabel = new Label();
        addFolderButton = new StyledButton();
        dropZonePanel = new CardPanel();
        dropZoneLabel = new Label();
        actionPanel = new Panel();
        saveButton = new StyledButton();
        SuspendLayout();
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Height = 66;
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
        headerPanel.Controls.Add(headerDescriptionLabel);
        headerPanel.Controls.Add(headerTitleLabel);
        //
        // body
        //
        body.Dock = DockStyle.Fill;
        body.AutoScroll = true;
        body.Padding = new Padding(0, 0, 0, 12);
        body.BackColor = ColorRGB.Background;
        //
        // card
        //
        card.Dock = DockStyle.Top;
        card.Height = 536;
        card.BackColor = ColorRGB.Surface;
        card.Padding = new Padding(28, 20, 28, 20);
        //
        // table (타겟 프로젝트 이름 / 실행 파일 원본 경로 / 백업 저장 위치)
        //
        table.Dock = DockStyle.Top;
        table.Height = 3 * 72;
        table.ColumnCount = 3;
        table.RowCount = 3;
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 105));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));

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
        actionPanel.Height = 68;
        actionPanel.Padding = new Padding(0, 14, 0, 10);
        actionPanel.BackColor = ColorRGB.Background;
        saveButton.Text = "설정 저장";
        saveButton.Dock = DockStyle.Right;
        saveButton.Width = 140;
        saveButton.Height = 42;
        saveButton.Click += UiClick_Save;
        m_oStatusLabel.Dock = DockStyle.Fill;
        m_oStatusLabel.ForeColor = ColorRGB.MutedText;
        m_oStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
        m_oStatusLabel.AutoEllipsis = true;
        actionPanel.Controls.Add(m_oStatusLabel);
        actionPanel.Controls.Add(saveButton);
        //
        // SettingControl
        //
        Controls.Add(body);
        Controls.Add(actionPanel);
        Controls.Add(headerPanel);
        Name = "SettingControl";
        Size = new Size(1000, 700);
        ResumeLayout(false);
    }
}
