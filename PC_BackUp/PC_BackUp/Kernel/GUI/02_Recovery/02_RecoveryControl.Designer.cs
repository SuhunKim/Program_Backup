namespace PC_BackUp;

partial class RecoveryControl
{
    private System.ComponentModel.IContainer? components;

    // 화면의 고정 골격 — 값이 바뀌지 않는 부분이라 전부 필드로 선언해 Visual Studio
    // 디자이너에서도 그대로 열어서 확인할 수 있게 한다. 아래 두 가지는 예외로 코드에만 남긴다:
    //  1) 상단 요약 카드의 원형 아이콘(IconGlyphs) — GDI+로 직접 그리는 Paint 이벤트라 디자이너가
    //     표현할 수 없다. RecoveryControl() 생성자에서 InitializeComponent() 이후에 붙인다.
    //  2) 공용 달력의 실제 크기(ApplyCalendarSizing) — 화면에 표시된 뒤
    //     정확히 잴 수 있어서 RefreshCatalog()에서 매번 다시 계산한다.
    private Panel headerPanel = null!;
    private Label headerTitleLabel = null!;
    private Label headerDescriptionLabel = null!;
    private CardPanel workspace = null!;
    private TableLayoutPanel m_oWorkspaceLayout = null!;
    private Label calendarTitle = null!;
    private Panel detailsPanel = null!;
    private CardPanel selectedBackupCardPanel = null!;
    private Panel actionPanel = null!;

    private readonly HistoryCalendar _calendar = new();
    private readonly DataGridView _grid = new();
    private readonly Label _dateLabel = new();
    private readonly Label _emptyLabel = new();
    private readonly Label _selectedBackupLabel = new();
    private readonly Label _selectedBackupMetaLabel = new();
    private readonly Label _restoreTargetLabel = new();
    private readonly Label _comparisonResultLabel = new();
    private readonly ListBox _comparisonListBox = new();
    private readonly CheckBox _autoSafetyBackupCheckBox = new();
    private readonly CheckBox _compareBeforeRestoreCheckBox = new();
    private readonly ProgressBar _progress = new();
    private readonly Label _summaryDateCaption = new();
    private readonly Label _summaryDateValue = new();
    private readonly Label _summaryCountCaption = new();
    private readonly Label _summaryCountValue = new();
    private StyledButton _restoreButton = null!;
    private StyledButton _cancelButton = null!;
    private SplitContainer _workspaceSplit = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            m_oBackupFolderWatcher?.Dispose();
            m_oRefreshDebounceTimer.Dispose();
        }
        base.Dispose(disposing);
    }

    private const int CalendarTitleHeight = 36;
    private const int CalendarLeftMargin = 6;

    private void InitializeComponent()
    {
        headerPanel = new Panel();
        headerDescriptionLabel = new Label();
        headerTitleLabel = new Label();
        workspace = new CardPanel();
        m_oWorkspaceLayout = new TableLayoutPanel();
        _workspaceSplit = new SplitContainer();
        calendarTitle = new Label();
        detailsPanel = new Panel();
        selectedBackupCardPanel = new CardPanel();
        actionPanel = new Panel();
        _cancelButton = new StyledButton();
        _restoreButton = new StyledButton();
        headerPanel.SuspendLayout();
        workspace.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_workspaceSplit).BeginInit();
        _workspaceSplit.Panel1.SuspendLayout();
        _workspaceSplit.SuspendLayout();
        detailsPanel.SuspendLayout();
        actionPanel.SuspendLayout();
        SuspendLayout();
        // 
        // headerPanel
        // 
        headerPanel.Controls.Add(headerDescriptionLabel);
        headerPanel.Controls.Add(headerTitleLabel);
        headerPanel.Location = new Point(20, 16);
        headerPanel.Name = "headerPanel";
        headerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        headerPanel.Padding = new Padding(8, 2, 0, 0);
        headerPanel.Size = new Size(960, 62);
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
        headerDescriptionLabel.Text = "백업 시점을 선택하고 안전하게 복원하세요.";
        headerDescriptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // headerTitleLabel
        // 
        headerTitleLabel.Dock = DockStyle.Top;
        headerTitleLabel.Font = new Font("맑은 고딕", 18F, FontStyle.Bold);
        headerTitleLabel.ForeColor = Color.FromArgb(28, 32, 41);
        headerTitleLabel.Location = new Point(8, 2);
        headerTitleLabel.Name = "headerTitleLabel";
        headerTitleLabel.Size = new Size(952, 31);
        headerTitleLabel.TabIndex = 1;
        headerTitleLabel.Text = "복원";
        headerTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // workspace
        // 
        workspace.BackColor = Color.White;
        // [Codex - 2026.09.21] 상단 요약 카드와 본문이 겹치지 않도록 본문 영역을 도킹한다.
        workspace.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        workspace.Location = new Point(0, 100);
        workspace.Name = "workspace";
        workspace.Padding = new Padding(16);
        workspace.Size = new Size(1000, 574);
        workspace.TabIndex = 0;
        // [Codex - 2026.10.28] 요약/본문/작업 영역을 별도 행으로 배치해 도킹 순서에 의한 겹침을 없앤다.
        m_oWorkspaceLayout.Dock = DockStyle.Fill;
        m_oWorkspaceLayout.ColumnCount = 1;
        m_oWorkspaceLayout.RowCount = 3;
        m_oWorkspaceLayout.Margin = Padding.Empty;
        m_oWorkspaceLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        m_oWorkspaceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        m_oWorkspaceLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        m_oWorkspaceLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66));
        _workspaceSplit.Margin = Padding.Empty;
        actionPanel.Margin = Padding.Empty;
        actionPanel.Dock = DockStyle.Fill;
        m_oWorkspaceLayout.Controls.Add(_workspaceSplit, 0, 1);
        m_oWorkspaceLayout.Controls.Add(actionPanel, 0, 2);
        workspace.Controls.Add(m_oWorkspaceLayout);
        // 
        // _workspaceSplit
        // 
        _workspaceSplit.BackColor = Color.FromArgb(224, 226, 231);
        _workspaceSplit.Dock = DockStyle.Fill;
        _workspaceSplit.FixedPanel = FixedPanel.Panel1;
        _workspaceSplit.IsSplitterFixed = true;
        _workspaceSplit.Location = new Point(20, 5);
        _workspaceSplit.Name = "_workspaceSplit";
        // 
        // _workspaceSplit.Panel1
        // 
        _workspaceSplit.Panel1.BackColor = Color.White;
        _workspaceSplit.Panel1.AutoScroll = true;
        _workspaceSplit.Panel1.Controls.Add(_calendar);
        _workspaceSplit.Panel1.Controls.Add(calendarTitle);
        _calendar.Location = new Point(CalendarLeftMargin, CalendarTitleHeight);
        _calendar.AccessibleName = "복원 백업 날짜";
        // 
        // _workspaceSplit.Panel2
        // 
        _workspaceSplit.Panel2.BackColor = Color.White;
        _workspaceSplit.Panel2.Padding = new Padding(18, 0, 0, 0);
        _workspaceSplit.Panel2.Controls.Add(_grid);
        _workspaceSplit.Panel2.Controls.Add(_emptyLabel);
        _workspaceSplit.Panel2.Controls.Add(_dateLabel);
        _workspaceSplit.Size = new Size(968, 306);
        _workspaceSplit.SplitterDistance = 121;
        _workspaceSplit.TabIndex = 0;
        // 
        // calendarTitle
        // 
        calendarTitle.Dock = DockStyle.Top;
        calendarTitle.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
        calendarTitle.ForeColor = Color.FromArgb(28, 32, 41);
        calendarTitle.Location = new Point(0, 0);
        calendarTitle.Name = "calendarTitle";
        calendarTitle.Size = new Size(121, CalendarTitleHeight);
        calendarTitle.TabIndex = 0;
        calendarTitle.Text = "백업 날짜";
        // [Codex - 2026.09.21] 이슈 6 레이아웃 변경에서 누락된 백업 목록 컨트롤을 복원한다.
        _dateLabel.Dock = DockStyle.Top;
        _dateLabel.Height = 36;
        _dateLabel.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
        _dateLabel.ForeColor = ColorRGB.Text;
        _emptyLabel.Dock = DockStyle.Fill;
        _emptyLabel.Text = "선택한 날짜에 백업 기록이 없습니다.";
        _emptyLabel.ForeColor = ColorRGB.MutedText;
        _emptyLabel.TextAlign = ContentAlignment.MiddleCenter;
        _grid.Dock = DockStyle.Fill;
        ColorRGB.ApplyDataGridStyle(_grid);
        _grid.AutoGenerateColumns = false;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.ReadOnly = true;
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "시간", DataPropertyName = nameof(BackupRecord.TimeText), Width = 100 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "백업 형태", DataPropertyName = nameof(BackupRecord.KindText), Width = 130 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "파일명", DataPropertyName = nameof(BackupRecord.FileName), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "크기", DataPropertyName = nameof(BackupRecord.SizeText), Width = 100 });
        // 
        // detailsPanel
        // 
        detailsPanel.Controls.Add(selectedBackupCardPanel);
        // [Codex - 2026.10.28] 선택 백업 상세를 목록 아래에 도킹해 본문과 겹치지 않게 한다.
        _workspaceSplit.Panel2.Controls.Add(detailsPanel);
        detailsPanel.Dock = DockStyle.Bottom;
        detailsPanel.Padding = new Padding(0, 12, 0, 0);
        detailsPanel.Location = new Point(20, 317);
        detailsPanel.Name = "detailsPanel";
        detailsPanel.Size = new Size(968, 240);
        detailsPanel.TabIndex = 1;
        // 
        // selectedBackupCardPanel
        // 
        selectedBackupCardPanel.BackColor = Color.FromArgb(247, 248, 250);
        selectedBackupCardPanel.Dock = DockStyle.Fill;
        selectedBackupCardPanel.Location = new Point(0, 0);
        selectedBackupCardPanel.Name = "selectedBackupCardPanel";
        selectedBackupCardPanel.Padding = new Padding(16, 10, 16, 10);
        _selectedBackupLabel.AutoEllipsis = true;
        _selectedBackupMetaLabel.AutoEllipsis = true;
        _restoreTargetLabel.AutoEllipsis = true;
        selectedBackupCardPanel.Size = new Size(952, 174);
        selectedBackupCardPanel.TabIndex = 0;
        _selectedBackupLabel.Dock = DockStyle.Top;
        _selectedBackupLabel.Height = 24;
        _selectedBackupLabel.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        _selectedBackupLabel.ForeColor = ColorRGB.Text;
        _selectedBackupMetaLabel.Dock = DockStyle.Top;
        _selectedBackupMetaLabel.Height = 22;
        _selectedBackupMetaLabel.ForeColor = ColorRGB.MutedText;
        _restoreTargetLabel.Dock = DockStyle.Top;
        _restoreTargetLabel.Height = 22;
        _restoreTargetLabel.ForeColor = ColorRGB.MutedText;
        _comparisonResultLabel.Dock = DockStyle.Top;
        _comparisonResultLabel.Height = 42;
        _comparisonResultLabel.ForeColor = ColorRGB.MutedText;
        _comparisonResultLabel.AutoEllipsis = true;
        _comparisonListBox.Dock = DockStyle.Fill;
        _comparisonListBox.BorderStyle = BorderStyle.None;
        _comparisonListBox.BackColor = ColorRGB.DetailPanelBackground;
        _comparisonListBox.ForeColor = ColorRGB.MutedText;
        _comparisonListBox.Font = new Font("맑은 고딕", 8.5F);
        _autoSafetyBackupCheckBox.Dock = DockStyle.Top;
        _autoSafetyBackupCheckBox.Height = 26;
        _autoSafetyBackupCheckBox.Text = "복원 전 현재 상태를 자동 ZIP 백업";
        _autoSafetyBackupCheckBox.ForeColor = ColorRGB.Text;
        _autoSafetyBackupCheckBox.CheckedChanged += UiChange_AutoSafetyBackup;
        _compareBeforeRestoreCheckBox.Dock = DockStyle.Top;
        _compareBeforeRestoreCheckBox.Height = 26;
        _compareBeforeRestoreCheckBox.Text = "복원 전 현재 파일과 비교";
        _compareBeforeRestoreCheckBox.ForeColor = ColorRGB.Text;
        _compareBeforeRestoreCheckBox.CheckedChanged += UiChange_CompareBeforeRestore;
        selectedBackupCardPanel.Controls.Add(_comparisonListBox);
        selectedBackupCardPanel.Controls.Add(_autoSafetyBackupCheckBox);
        selectedBackupCardPanel.Controls.Add(_compareBeforeRestoreCheckBox);
        selectedBackupCardPanel.Controls.Add(_comparisonResultLabel);
        selectedBackupCardPanel.Controls.Add(_restoreTargetLabel);
        selectedBackupCardPanel.Controls.Add(_selectedBackupMetaLabel);
        selectedBackupCardPanel.Controls.Add(_selectedBackupLabel);
        // 
        // actionPanel
        // 
        actionPanel.Controls.Add(_cancelButton);
        actionPanel.Controls.Add(_restoreButton);
        actionPanel.Controls.Add(_progress);
        actionPanel.Dock = DockStyle.Fill;
        actionPanel.Location = new Point(20, 497);
        actionPanel.Name = "actionPanel";
        actionPanel.Padding = new Padding(0, 12, 0, 0);
        actionPanel.Size = new Size(960, 66);
        actionPanel.TabIndex = 1;
        _progress.Dock = DockStyle.Top;
        _progress.Height = 6;
        // 
        // _cancelButton
        // 
        _cancelButton.BackColor = Color.FromArgb(233, 238, 245);
        _cancelButton.BorderThickness = 1;
        _cancelButton.ButtonType = StyledButtonType.Secondary;
        _cancelButton.Dock = DockStyle.Right;
        _cancelButton.Font = new Font("맑은 고딕", 9F);
        _cancelButton.ForeColor = Color.FromArgb(28, 32, 41);
        _cancelButton.Location = new Point(680, 12);
        _cancelButton.Margin = new Padding(0, 0, 10, 0);
        _cancelButton.Name = "_cancelButton";
        _cancelButton.Size = new Size(100, 54);
        _cancelButton.TabIndex = 0;
        _cancelButton.Text = "취소";
        _cancelButton.Visible = false;
        // 
        // _restoreButton
        // 
        _restoreButton.BackColor = Color.FromArgb(30, 100, 199);
        _restoreButton.Dock = DockStyle.Right;
        _restoreButton.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        _restoreButton.ForeColor = Color.White;
        _restoreButton.Location = new Point(780, 12);
        _restoreButton.Name = "_restoreButton";
        _restoreButton.Size = new Size(180, 54);
        _restoreButton.TabIndex = 1;
        _restoreButton.Text = "선택 백업 복원";
        _restoreButton.Click += UiClick_Restore;
        // 
        // RecoveryControl
        // 
        Controls.Add(workspace);
        Controls.Add(headerPanel);
        Name = "RecoveryControl";
        Size = new Size(1000, 700);
        headerPanel.ResumeLayout(false);
        workspace.ResumeLayout(false);
        _workspaceSplit.Panel1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_workspaceSplit).EndInit();
        _workspaceSplit.ResumeLayout(false);
        detailsPanel.ResumeLayout(false);
        actionPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
