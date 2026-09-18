namespace PC_BackUp;

partial class RecoveryControl
{
    private System.ComponentModel.IContainer components;

    // [Codex - 2026.09.14] 모든 시각 컨트롤을 InitializeComponent에서 생성한다.
    // 필드 선언부의 new()와 생성자 Build...()를 없애 Visual Studio 디자이너가 전체 화면을
    // 인식하고, 각 컨트롤을 마우스로 선택하여 위치와 크기를 수정할 수 있게 한다.
    private Panel headerPanel = null!;
    private Label headerTitleLabel = null!;
    private Label headerDescriptionLabel = null!;
    private CardPanel workspace = null!;
    private Panel m_oSummaryPanel = null!;
    private CardPanel m_oSummaryDateCard = null!;
    private GlyphBadge m_oSummaryDateBadge = null!;
    private Label _summaryDateCaption = null!;
    private Label _summaryDateValue = null!;
    private CardPanel m_oSummaryCountCard = null!;
    private GlyphBadge m_oSummaryCountBadge = null!;
    private Label _summaryCountCaption = null!;
    private Label _summaryCountValue = null!;
    private SplitContainer _workspaceSplit = null!;
    private Label calendarTitle = null!;
    private MonthCalendar _calendar = null!;
    private Label _dateLabel = null!;
    private DataGridView _grid = null!;
    private DataGridViewTextBoxColumn m_oTimeColumn = null!;
    private DataGridViewTextBoxColumn m_oKindColumn = null!;
    private DataGridViewTextBoxColumn m_oFileNameColumn = null!;
    private DataGridViewTextBoxColumn m_oSizeColumn = null!;
    private Label _emptyLabel = null!;
    private Panel detailsPanel = null!;
    private CardPanel selectedBackupCardPanel = null!;
    private Label _selectedBackupLabel = null!;
    private Label _selectedBackupMetaLabel = null!;
    private Label _restoreTargetLabel = null!;
    private Label _comparisonResultLabel = null!;
    private ListBox _comparisonListBox = null!;
    private CheckBox _compareBeforeRestoreCheckBox = null!;
    private CheckBox _autoSafetyBackupCheckBox = null!;
    private Panel detailsGap = null!;
    private CardPanel m_oSafetyInfoCard = null!;
    private Panel m_oSafetyTitlePanel = null!;
    private GlyphBadge m_oSafetyBadge = null!;
    private Label m_oSafetyTitleLabel = null!;
    private Label m_oSafetyBodyLabel = null!;
    private Panel actionPanel = null!;
    private ProgressBar _progress = null!;
    private StyledButton _cancelButton = null!;
    private StyledButton _restoreButton = null!;

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

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        headerPanel = new Panel();
        headerDescriptionLabel = new Label();
        headerTitleLabel = new Label();
        workspace = new CardPanel();
        _workspaceSplit = new SplitContainer();
        _calendar = new MonthCalendar();
        calendarTitle = new Label();
        _grid = new DataGridView();
        m_oTimeColumn = new DataGridViewTextBoxColumn();
        m_oKindColumn = new DataGridViewTextBoxColumn();
        m_oFileNameColumn = new DataGridViewTextBoxColumn();
        m_oSizeColumn = new DataGridViewTextBoxColumn();
        _emptyLabel = new Label();
        _dateLabel = new Label();
        detailsPanel = new Panel();
        selectedBackupCardPanel = new CardPanel();
        _autoSafetyBackupCheckBox = new CheckBox();
        _compareBeforeRestoreCheckBox = new CheckBox();
        _comparisonListBox = new ListBox();
        _comparisonResultLabel = new Label();
        _restoreTargetLabel = new Label();
        _selectedBackupMetaLabel = new Label();
        _selectedBackupLabel = new Label();
        detailsGap = new Panel();
        m_oSafetyInfoCard = new CardPanel();
        m_oSafetyBodyLabel = new Label();
        m_oSafetyTitlePanel = new Panel();
        m_oSafetyTitleLabel = new Label();
        m_oSafetyBadge = new GlyphBadge();
        m_oSummaryPanel = new Panel();
        m_oSummaryCountCard = new CardPanel();
        _summaryCountValue = new Label();
        _summaryCountCaption = new Label();
        m_oSummaryCountBadge = new GlyphBadge();
        m_oSummaryDateCard = new CardPanel();
        _summaryDateValue = new Label();
        _summaryDateCaption = new Label();
        m_oSummaryDateBadge = new GlyphBadge();
        actionPanel = new Panel();
        _cancelButton = new StyledButton();
        _restoreButton = new StyledButton();
        _progress = new ProgressBar();
        headerPanel.SuspendLayout();
        workspace.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_workspaceSplit).BeginInit();
        _workspaceSplit.Panel1.SuspendLayout();
        _workspaceSplit.Panel2.SuspendLayout();
        _workspaceSplit.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        detailsPanel.SuspendLayout();
        selectedBackupCardPanel.SuspendLayout();
        m_oSafetyInfoCard.SuspendLayout();
        m_oSafetyTitlePanel.SuspendLayout();
        m_oSummaryPanel.SuspendLayout();
        m_oSummaryCountCard.SuspendLayout();
        m_oSummaryDateCard.SuspendLayout();
        actionPanel.SuspendLayout();
        SuspendLayout();
        //
        // headerPanel
        //
        headerPanel.Controls.Add(headerDescriptionLabel);
        headerPanel.Controls.Add(headerTitleLabel);
        headerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
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
        headerDescriptionLabel.ForeColor = ColorRGB.MutedText;
        headerDescriptionLabel.Location = new Point(8, 33);
        headerDescriptionLabel.Name = "headerDescriptionLabel";
        headerDescriptionLabel.Size = new Size(992, 24);
        headerDescriptionLabel.TabIndex = 1;
        headerDescriptionLabel.Text = "백업 시점을 선택하고 안전하게 복원하세요.";
        headerDescriptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        //
        // headerTitleLabel
        //
        headerTitleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        headerTitleLabel.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
        headerTitleLabel.ForeColor = ColorRGB.Text;
        headerTitleLabel.Location = new Point(8, 2);
        headerTitleLabel.Name = "headerTitleLabel";
        headerTitleLabel.Size = new Size(992, 31);
        headerTitleLabel.TabIndex = 0;
        headerTitleLabel.Text = "복원";
        headerTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        //
        // workspace
        //
        workspace.BackColor = ColorRGB.Surface;
        workspace.Controls.Add(_workspaceSplit);
        workspace.Controls.Add(detailsPanel);
        workspace.Controls.Add(m_oSummaryPanel);
        workspace.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        workspace.Location = new Point(0, 66);
        workspace.Name = "workspace";
        workspace.Padding = new Padding(16);
        workspace.Size = new Size(1000, 568);
        workspace.TabIndex = 1;
        //
        // _workspaceSplit
        //
        _workspaceSplit.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _workspaceSplit.BackColor = ColorRGB.Border;
        _workspaceSplit.FixedPanel = FixedPanel.Panel1;
        _workspaceSplit.Location = new Point(16, 108);
        _workspaceSplit.Name = "_workspaceSplit";
        //
        // _workspaceSplit.Panel1
        //
        _workspaceSplit.Panel1.BackColor = ColorRGB.Surface;
        _workspaceSplit.Panel1.Controls.Add(_calendar);
        _workspaceSplit.Panel1.Controls.Add(calendarTitle);
        _workspaceSplit.Panel1.Padding = new Padding(6, 0, 10, 0);
        //
        // _workspaceSplit.Panel2
        //
        _workspaceSplit.Panel2.BackColor = ColorRGB.Surface;
        _workspaceSplit.Panel2.Controls.Add(_grid);
        _workspaceSplit.Panel2.Controls.Add(_emptyLabel);
        _workspaceSplit.Panel2.Controls.Add(_dateLabel);
        _workspaceSplit.Panel2.Padding = new Padding(18, 0, 0, 0);
        _workspaceSplit.Size = new Size(968, 224);
        _workspaceSplit.SplitterDistance = 280;
        _workspaceSplit.TabIndex = 2;
        //
        // _calendar
        //
        _calendar.Location = new Point(12, 40);
        _calendar.MaxSelectionCount = 1;
        _calendar.Name = "_calendar";
        _calendar.ShowTodayCircle = true;
        _calendar.TabIndex = 1;
        //
        // calendarTitle
        //
        calendarTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        calendarTitle.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
        calendarTitle.ForeColor = ColorRGB.Text;
        calendarTitle.Location = new Point(6, 0);
        calendarTitle.Name = "calendarTitle";
        calendarTitle.Size = new Size(264, 36);
        calendarTitle.TabIndex = 0;
        calendarTitle.Text = "백업 날짜";
        //
        // _grid
        //
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.AllowUserToResizeRows = false;
        _grid.AutoGenerateColumns = false;
        _grid.BackgroundColor = ColorRGB.Surface;
        _grid.BorderStyle = BorderStyle.None;
        _grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        _grid.ColumnHeadersDefaultCellStyle.BackColor = ColorRGB.GridHeaderBackground;
        _grid.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        _grid.ColumnHeadersDefaultCellStyle.ForeColor = ColorRGB.Text;
        _grid.ColumnHeadersHeight = 40;
        _grid.Columns.AddRange(new DataGridViewColumn[] { m_oTimeColumn, m_oKindColumn, m_oFileNameColumn, m_oSizeColumn });
        _grid.DefaultCellStyle.SelectionBackColor = ColorRGB.SidebarActive;
        _grid.DefaultCellStyle.SelectionForeColor = ColorRGB.Text;
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.EnableHeadersVisualStyles = false;
        _grid.GridColor = ColorRGB.Border;
        _grid.Location = new Point(18, 36);
        _grid.MultiSelect = false;
        _grid.Name = "_grid";
        _grid.ReadOnly = true;
        _grid.RowHeadersVisible = false;
        _grid.RowTemplate.Height = 36;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.Size = new Size(662, 188);
        _grid.TabIndex = 1;
        //
        // m_oTimeColumn
        //
        m_oTimeColumn.DataPropertyName = "TimeText";
        m_oTimeColumn.HeaderText = "시간";
        m_oTimeColumn.Name = "m_oTimeColumn";
        m_oTimeColumn.ReadOnly = true;
        //
        // m_oKindColumn
        //
        m_oKindColumn.DataPropertyName = "KindText";
        m_oKindColumn.HeaderText = "백업 형태";
        m_oKindColumn.Name = "m_oKindColumn";
        m_oKindColumn.ReadOnly = true;
        m_oKindColumn.Width = 130;
        //
        // m_oFileNameColumn
        //
        m_oFileNameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        m_oFileNameColumn.DataPropertyName = "FileName";
        m_oFileNameColumn.HeaderText = "파일명";
        m_oFileNameColumn.Name = "m_oFileNameColumn";
        m_oFileNameColumn.ReadOnly = true;
        //
        // m_oSizeColumn
        //
        m_oSizeColumn.DataPropertyName = "SizeText";
        m_oSizeColumn.HeaderText = "크기";
        m_oSizeColumn.Name = "m_oSizeColumn";
        m_oSizeColumn.ReadOnly = true;
        //
        // _emptyLabel
        //
        _emptyLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _emptyLabel.ForeColor = ColorRGB.MutedText;
        _emptyLabel.Location = new Point(18, 36);
        _emptyLabel.Name = "_emptyLabel";
        _emptyLabel.Size = new Size(662, 188);
        _emptyLabel.TabIndex = 2;
        _emptyLabel.Text = "선택한 날짜에 백업 기록이 없습니다.";
        _emptyLabel.TextAlign = ContentAlignment.MiddleCenter;
        _emptyLabel.Visible = false;
        //
        // _dateLabel
        //
        _dateLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _dateLabel.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
        _dateLabel.ForeColor = ColorRGB.Text;
        _dateLabel.Location = new Point(18, 0);
        _dateLabel.Name = "_dateLabel";
        _dateLabel.Size = new Size(662, 36);
        _dateLabel.TabIndex = 0;
        _dateLabel.Text = "선택한 날짜의 백업 목록";
        //
        // detailsPanel
        //
        detailsPanel.Controls.Add(selectedBackupCardPanel);
        detailsPanel.Controls.Add(detailsGap);
        detailsPanel.Controls.Add(m_oSafetyInfoCard);
        detailsPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        detailsPanel.Location = new Point(16, 332);
        detailsPanel.Name = "detailsPanel";
        detailsPanel.Size = new Size(968, 220);
        detailsPanel.TabIndex = 1;
        //
        // selectedBackupCardPanel
        //
        selectedBackupCardPanel.BackColor = ColorRGB.DetailPanelBackground;
        selectedBackupCardPanel.Controls.Add(_autoSafetyBackupCheckBox);
        selectedBackupCardPanel.Controls.Add(_compareBeforeRestoreCheckBox);
        selectedBackupCardPanel.Controls.Add(_comparisonListBox);
        selectedBackupCardPanel.Controls.Add(_comparisonResultLabel);
        selectedBackupCardPanel.Controls.Add(_restoreTargetLabel);
        selectedBackupCardPanel.Controls.Add(_selectedBackupMetaLabel);
        selectedBackupCardPanel.Controls.Add(_selectedBackupLabel);
        selectedBackupCardPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        selectedBackupCardPanel.Location = new Point(0, 0);
        selectedBackupCardPanel.Name = "selectedBackupCardPanel";
        selectedBackupCardPanel.Padding = new Padding(16, 10, 16, 10);
        selectedBackupCardPanel.Size = new Size(652, 220);
        selectedBackupCardPanel.TabIndex = 0;
        //
        // _autoSafetyBackupCheckBox
        //
        _autoSafetyBackupCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _autoSafetyBackupCheckBox.ForeColor = ColorRGB.Text;
        _autoSafetyBackupCheckBox.Location = new Point(16, 184);
        _autoSafetyBackupCheckBox.Name = "_autoSafetyBackupCheckBox";
        _autoSafetyBackupCheckBox.Size = new Size(620, 26);
        _autoSafetyBackupCheckBox.TabIndex = 6;
        _autoSafetyBackupCheckBox.Text = "복원 전 현재 상태를 자동 ZIP 백업";
        _autoSafetyBackupCheckBox.CheckedChanged += UiChange_AutoSafetyBackup;
        //
        // _compareBeforeRestoreCheckBox
        //
        _compareBeforeRestoreCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _compareBeforeRestoreCheckBox.ForeColor = ColorRGB.Text;
        _compareBeforeRestoreCheckBox.Location = new Point(16, 158);
        _compareBeforeRestoreCheckBox.Name = "_compareBeforeRestoreCheckBox";
        _compareBeforeRestoreCheckBox.Size = new Size(620, 26);
        _compareBeforeRestoreCheckBox.TabIndex = 5;
        _compareBeforeRestoreCheckBox.Text = "복원 전 현재 파일과 비교";
        _compareBeforeRestoreCheckBox.CheckedChanged += UiChange_CompareBeforeRestore;
        //
        // _comparisonListBox
        //
        _comparisonListBox.BackColor = ColorRGB.DetailPanelBackground;
        _comparisonListBox.BorderStyle = BorderStyle.None;
        _comparisonListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _comparisonListBox.Font = new Font("맑은 고딕", 8.5F);
        _comparisonListBox.ForeColor = ColorRGB.MutedText;
        _comparisonListBox.FormattingEnabled = true;
        _comparisonListBox.ItemHeight = 15;
        _comparisonListBox.Items.AddRange(new object[] { "복원 전 비교 결과가 여기에 표시됩니다." });
        _comparisonListBox.Location = new Point(16, 120);
        _comparisonListBox.Name = "_comparisonListBox";
        _comparisonListBox.Size = new Size(620, 38);
        _comparisonListBox.TabIndex = 4;
        //
        // _comparisonResultLabel
        //
        _comparisonResultLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _comparisonResultLabel.AutoEllipsis = true;
        _comparisonResultLabel.ForeColor = ColorRGB.MutedText;
        _comparisonResultLabel.Location = new Point(16, 78);
        _comparisonResultLabel.Name = "_comparisonResultLabel";
        _comparisonResultLabel.Size = new Size(620, 32);
        _comparisonResultLabel.TabIndex = 3;
        _comparisonResultLabel.Text = "복원 시작 전 현재 파일과 비교합니다.";
        //
        // _restoreTargetLabel
        //
        _restoreTargetLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _restoreTargetLabel.ForeColor = ColorRGB.MutedText;
        _restoreTargetLabel.Location = new Point(16, 56);
        _restoreTargetLabel.Name = "_restoreTargetLabel";
        _restoreTargetLabel.Size = new Size(620, 22);
        _restoreTargetLabel.TabIndex = 2;
        _restoreTargetLabel.Text = "복원 위치: 설정된 원본 폴더";
        //
        // _selectedBackupMetaLabel
        //
        _selectedBackupMetaLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _selectedBackupMetaLabel.ForeColor = ColorRGB.MutedText;
        _selectedBackupMetaLabel.Location = new Point(16, 34);
        _selectedBackupMetaLabel.Name = "_selectedBackupMetaLabel";
        _selectedBackupMetaLabel.Size = new Size(620, 22);
        _selectedBackupMetaLabel.TabIndex = 1;
        _selectedBackupMetaLabel.Text = "백업 일시  ·  백업 형태  ·  파일 크기";
        //
        // _selectedBackupLabel
        //
        _selectedBackupLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _selectedBackupLabel.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        _selectedBackupLabel.ForeColor = ColorRGB.Text;
        _selectedBackupLabel.Location = new Point(16, 10);
        _selectedBackupLabel.Name = "_selectedBackupLabel";
        _selectedBackupLabel.Size = new Size(620, 24);
        _selectedBackupLabel.TabIndex = 0;
        _selectedBackupLabel.Text = "선택한 백업 정보";
        //
        // detailsGap
        //
        detailsGap.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        detailsGap.Location = new Point(652, 0);
        detailsGap.Name = "detailsGap";
        detailsGap.Size = new Size(16, 220);
        detailsGap.TabIndex = 1;
        //
        // m_oSafetyInfoCard
        //
        m_oSafetyInfoCard.BackColor = ColorRGB.SafetyBackground;
        m_oSafetyInfoCard.BorderColor = ColorRGB.SafetyBorder;
        m_oSafetyInfoCard.Controls.Add(m_oSafetyBodyLabel);
        m_oSafetyInfoCard.Controls.Add(m_oSafetyTitlePanel);
        m_oSafetyInfoCard.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        m_oSafetyInfoCard.Location = new Point(668, 0);
        m_oSafetyInfoCard.Name = "m_oSafetyInfoCard";
        m_oSafetyInfoCard.Padding = new Padding(14, 10, 14, 10);
        m_oSafetyInfoCard.Size = new Size(300, 220);
        m_oSafetyInfoCard.TabIndex = 2;
        //
        // m_oSafetyBodyLabel
        //
        m_oSafetyBodyLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        m_oSafetyBodyLabel.Font = new Font("맑은 고딕", 8.5F);
        m_oSafetyBodyLabel.ForeColor = ColorRGB.SafetyText;
        m_oSafetyBodyLabel.Location = new Point(14, 42);
        m_oSafetyBodyLabel.Name = "m_oSafetyBodyLabel";
        m_oSafetyBodyLabel.Padding = new Padding(0, 8, 0, 0);
        m_oSafetyBodyLabel.Size = new Size(272, 164);
        m_oSafetyBodyLabel.TabIndex = 1;
        m_oSafetyBodyLabel.Text = "•  복원을 시작하면 선택한 백업으로 현재 데이터를 덮어씁니다.\r\n\r\n•  복원 중에는 다른 프로그램을 종료해 주세요.\r\n\r\n•  복원 완료 후 프로그램을 재시작해야 할 수 있습니다.";
        //
        // m_oSafetyTitlePanel
        //
        m_oSafetyTitlePanel.Controls.Add(m_oSafetyTitleLabel);
        m_oSafetyTitlePanel.Controls.Add(m_oSafetyBadge);
        m_oSafetyTitlePanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        m_oSafetyTitlePanel.Location = new Point(14, 10);
        m_oSafetyTitlePanel.Name = "m_oSafetyTitlePanel";
        m_oSafetyTitlePanel.Size = new Size(272, 24);
        m_oSafetyTitlePanel.TabIndex = 0;
        //
        // m_oSafetyTitleLabel
        //
        m_oSafetyTitleLabel.AutoSize = true;
        m_oSafetyTitleLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        m_oSafetyTitleLabel.ForeColor = ColorRGB.SafetyText;
        m_oSafetyTitleLabel.Location = new Point(26, 2);
        m_oSafetyTitleLabel.Name = "m_oSafetyTitleLabel";
        m_oSafetyTitleLabel.Size = new Size(137, 15);
        m_oSafetyTitleLabel.TabIndex = 1;
        m_oSafetyTitleLabel.Text = "안전한 복원을 위한 안내";
        //
        // m_oSafetyBadge
        //
        m_oSafetyBadge.BadgeColor = ColorRGB.SafetyIcon;
        m_oSafetyBadge.Glyph = GlyphBadgeKind.Check;
        m_oSafetyBadge.GlyphColor = Color.White;
        m_oSafetyBadge.Location = new Point(0, 2);
        m_oSafetyBadge.Name = "m_oSafetyBadge";
        m_oSafetyBadge.Size = new Size(20, 20);
        m_oSafetyBadge.TabIndex = 0;
        //
        // m_oSummaryPanel
        //
        m_oSummaryPanel.Controls.Add(m_oSummaryCountCard);
        m_oSummaryPanel.Controls.Add(m_oSummaryDateCard);
        m_oSummaryPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        m_oSummaryPanel.Location = new Point(16, 16);
        m_oSummaryPanel.Name = "m_oSummaryPanel";
        m_oSummaryPanel.Padding = new Padding(0, 0, 0, 16);
        m_oSummaryPanel.Size = new Size(968, 92);
        m_oSummaryPanel.TabIndex = 0;
        //
        // m_oSummaryCountCard
        //
        m_oSummaryCountCard.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        m_oSummaryCountCard.BackColor = ColorRGB.Surface;
        m_oSummaryCountCard.Controls.Add(_summaryCountValue);
        m_oSummaryCountCard.Controls.Add(_summaryCountCaption);
        m_oSummaryCountCard.Controls.Add(m_oSummaryCountBadge);
        m_oSummaryCountCard.Location = new Point(492, 0);
        m_oSummaryCountCard.Name = "m_oSummaryCountCard";
        m_oSummaryCountCard.Size = new Size(476, 76);
        m_oSummaryCountCard.TabIndex = 1;
        //
        // _summaryCountValue
        //
        _summaryCountValue.AutoSize = true;
        _summaryCountValue.Font = new Font("맑은 고딕", 13F, FontStyle.Bold);
        _summaryCountValue.ForeColor = ColorRGB.Text;
        _summaryCountValue.Location = new Point(64, 30);
        _summaryCountValue.Name = "_summaryCountValue";
        _summaryCountValue.Size = new Size(153, 24);
        _summaryCountValue.TabIndex = 2;
        _summaryCountValue.Text = "백업 0개  ·  총 0 B";
        //
        // _summaryCountCaption
        //
        _summaryCountCaption.AutoSize = true;
        _summaryCountCaption.Font = new Font("맑은 고딕", 9F);
        _summaryCountCaption.ForeColor = ColorRGB.MutedText;
        _summaryCountCaption.Location = new Point(64, 12);
        _summaryCountCaption.Name = "_summaryCountCaption";
        _summaryCountCaption.Size = new Size(55, 15);
        _summaryCountCaption.TabIndex = 1;
        _summaryCountCaption.Text = "백업 요약";
        //
        // m_oSummaryCountBadge
        //
        m_oSummaryCountBadge.Glyph = GlyphBadgeKind.Archive;
        m_oSummaryCountBadge.Location = new Point(16, 18);
        m_oSummaryCountBadge.Name = "m_oSummaryCountBadge";
        m_oSummaryCountBadge.Size = new Size(36, 36);
        m_oSummaryCountBadge.TabIndex = 0;
        //
        // m_oSummaryDateCard
        //
        m_oSummaryDateCard.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        m_oSummaryDateCard.BackColor = ColorRGB.Surface;
        m_oSummaryDateCard.Controls.Add(_summaryDateValue);
        m_oSummaryDateCard.Controls.Add(_summaryDateCaption);
        m_oSummaryDateCard.Controls.Add(m_oSummaryDateBadge);
        m_oSummaryDateCard.Location = new Point(0, 0);
        m_oSummaryDateCard.Name = "m_oSummaryDateCard";
        m_oSummaryDateCard.Size = new Size(476, 76);
        m_oSummaryDateCard.TabIndex = 0;
        //
        // _summaryDateValue
        //
        _summaryDateValue.AutoSize = true;
        _summaryDateValue.Font = new Font("맑은 고딕", 13F, FontStyle.Bold);
        _summaryDateValue.ForeColor = ColorRGB.Text;
        _summaryDateValue.Location = new Point(64, 30);
        _summaryDateValue.Name = "_summaryDateValue";
        _summaryDateValue.Size = new Size(144, 24);
        _summaryDateValue.TabIndex = 2;
        _summaryDateValue.Text = "날짜를 선택하세요";
        //
        // _summaryDateCaption
        //
        _summaryDateCaption.AutoSize = true;
        _summaryDateCaption.Font = new Font("맑은 고딕", 9F);
        _summaryDateCaption.ForeColor = ColorRGB.MutedText;
        _summaryDateCaption.Location = new Point(64, 12);
        _summaryDateCaption.Name = "_summaryDateCaption";
        _summaryDateCaption.Size = new Size(103, 15);
        _summaryDateCaption.TabIndex = 1;
        _summaryDateCaption.Text = "선택된 백업 날짜";
        //
        // m_oSummaryDateBadge
        //
        m_oSummaryDateBadge.Location = new Point(16, 18);
        m_oSummaryDateBadge.Name = "m_oSummaryDateBadge";
        m_oSummaryDateBadge.Size = new Size(36, 36);
        m_oSummaryDateBadge.TabIndex = 0;
        //
        // actionPanel
        //
        actionPanel.Controls.Add(_cancelButton);
        actionPanel.Controls.Add(_restoreButton);
        actionPanel.Controls.Add(_progress);
        actionPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        actionPanel.Location = new Point(0, 634);
        actionPanel.Name = "actionPanel";
        actionPanel.Padding = new Padding(0, 12, 0, 0);
        actionPanel.Size = new Size(1000, 66);
        actionPanel.TabIndex = 2;
        //
        // _cancelButton
        //
        _cancelButton.ButtonType = StyledButtonType.Secondary;
        _cancelButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _cancelButton.Location = new Point(710, 18);
        _cancelButton.Margin = new Padding(0, 0, 10, 0);
        _cancelButton.Name = "_cancelButton";
        _cancelButton.Size = new Size(100, 42);
        _cancelButton.TabIndex = 1;
        _cancelButton.Text = "취소";
        //
        // _restoreButton
        //
        _restoreButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _restoreButton.Location = new Point(820, 18);
        _restoreButton.Name = "_restoreButton";
        _restoreButton.Size = new Size(180, 42);
        _restoreButton.TabIndex = 2;
        _restoreButton.Text = "선택 백업 복원";
        _restoreButton.Click += UiClick_Restore;
        //
        // _progress
        //
        _progress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _progress.Location = new Point(0, 12);
        _progress.Name = "_progress";
        _progress.Size = new Size(1000, 6);
        _progress.TabIndex = 0;
        //
        // RecoveryControl
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(workspace);
        Controls.Add(actionPanel);
        Controls.Add(headerPanel);
        Name = "RecoveryControl";
        Size = new Size(1000, 700);
        headerPanel.ResumeLayout(false);
        workspace.ResumeLayout(false);
        _workspaceSplit.Panel1.ResumeLayout(false);
        _workspaceSplit.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_workspaceSplit).EndInit();
        _workspaceSplit.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        detailsPanel.ResumeLayout(false);
        selectedBackupCardPanel.ResumeLayout(false);
        m_oSafetyInfoCard.ResumeLayout(false);
        m_oSafetyTitlePanel.ResumeLayout(false);
        m_oSafetyTitlePanel.PerformLayout();
        m_oSummaryPanel.ResumeLayout(false);
        m_oSummaryCountCard.ResumeLayout(false);
        m_oSummaryCountCard.PerformLayout();
        m_oSummaryDateCard.ResumeLayout(false);
        m_oSummaryDateCard.PerformLayout();
        actionPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
