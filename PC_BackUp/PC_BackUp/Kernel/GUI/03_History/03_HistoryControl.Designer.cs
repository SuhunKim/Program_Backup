namespace PC_BackUp;

partial class HistoryControl
{
    private System.ComponentModel.IContainer? components;

    // [Codex - 2026.09.14] 모든 주요 시각 요소를 디자이너 필드로 선언하고
    // InitializeComponent에서 생성한다. 실행 코드의 동적 생성에 의존하지 않으므로
    // Visual Studio 디자이너에서 직접 선택, 이동, 크기 조절할 수 있다.
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
    private Label _calendarStatsLabel = null!;
    private Label _calendarRecentTitle = null!;
    private Panel _calendarRecentPanel = null!;
    private LinkLabel m_oRecentLink1 = null!;
    private LinkLabel m_oRecentLink2 = null!;
    private LinkLabel m_oRecentLink3 = null!;
    private LinkLabel m_oRecentLink4 = null!;
    private LinkLabel m_oRecentLink5 = null!;
    private Panel selectionBar = null!;
    private Panel backupSelectorPanel = null!;
    private Panel _sourceRow = null!;
    private Label _sourceComboLabel = null!;
    private CheckBox _sourceIsCurrentCheckBox = null!;
    private ComboBox _sourceCombo = null!;
    private Panel _destinationRow = null!;
    private Label _backupComboLabel = null!;
    private ComboBox _backupCombo = null!;
    private StyledButton _logViewButton = null!;
    private Panel selectionBarActionSpacer = null!;
    private StyledButton _compareButton = null!;
    private Panel gridHost = null!;
    private DataGridView _grid = null!;
    private DataGridViewTextBoxColumn m_oNumberColumn = null!;
    private DataGridViewCheckBoxColumn m_oApplyColumn = null!;
    private DataGridViewTextBoxColumn m_oXmlFileColumn = null!;
    private DataGridViewTextBoxColumn m_oCurrentColumn = null!;
    private DataGridViewTextBoxColumn m_oBackupColumn = null!;
    private Label _emptyStateLabel = null!;
    private Panel _existenceHost = null!;
    private Label _existenceTitleLabel = null!;
    private DataGridView _existenceGrid = null!;
    private DataGridViewCheckBoxColumn m_oExistenceApplyColumn = null!;
    private DataGridViewTextBoxColumn m_oExistenceKindColumn = null!;
    private DataGridViewTextBoxColumn m_oExistencePathColumn = null!;
    private Panel bottomBar = null!;
    private Label _summaryLabel = null!;
    private ProgressBar _progress = null!;
    private StyledButton _cancelButton = null!;
    private StyledButton _applyButton = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
        DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
        headerPanel = new Panel();
        headerDescriptionLabel = new Label();
        headerTitleLabel = new Label();
        workspace = new CardPanel();
        _workspaceSplit = new SplitContainer();
        _calendarRecentPanel = new Panel();
        m_oRecentLink5 = new LinkLabel();
        m_oRecentLink4 = new LinkLabel();
        m_oRecentLink3 = new LinkLabel();
        m_oRecentLink2 = new LinkLabel();
        m_oRecentLink1 = new LinkLabel();
        _calendarRecentTitle = new Label();
        _calendarStatsLabel = new Label();
        _calendar = new MonthCalendar();
        calendarTitle = new Label();
        gridHost = new Panel();
        _grid = new DataGridView();
        m_oNumberColumn = new DataGridViewTextBoxColumn();
        _emptyStateLabel = new Label();
        _existenceHost = new Panel();
        _existenceGrid = new DataGridView();
        _existenceTitleLabel = new Label();
        selectionBar = new Panel();
        backupSelectorPanel = new Panel();
        _destinationRow = new Panel();
        _backupCombo = new ComboBox();
        _backupComboLabel = new Label();
        _sourceRow = new Panel();
        _sourceCombo = new ComboBox();
        _sourceIsCurrentCheckBox = new CheckBox();
        _sourceComboLabel = new Label();
        _logViewButton = new StyledButton();
        selectionBarActionSpacer = new Panel();
        _compareButton = new StyledButton();
        m_oSummaryPanel = new Panel();
        m_oSummaryCountCard = new CardPanel();
        _summaryCountValue = new Label();
        _summaryCountCaption = new Label();
        m_oSummaryCountBadge = new GlyphBadge();
        m_oSummaryDateCard = new CardPanel();
        _summaryDateValue = new Label();
        _summaryDateCaption = new Label();
        m_oSummaryDateBadge = new GlyphBadge();
        bottomBar = new Panel();
        _summaryLabel = new Label();
        _cancelButton = new StyledButton();
        _applyButton = new StyledButton();
        _progress = new ProgressBar();
        headerPanel.SuspendLayout();
        workspace.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_workspaceSplit).BeginInit();
        _workspaceSplit.Panel1.SuspendLayout();
        _workspaceSplit.Panel2.SuspendLayout();
        _workspaceSplit.SuspendLayout();
        _calendarRecentPanel.SuspendLayout();
        gridHost.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_grid).BeginInit();
        _existenceHost.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_existenceGrid).BeginInit();
        selectionBar.SuspendLayout();
        backupSelectorPanel.SuspendLayout();
        _destinationRow.SuspendLayout();
        _sourceRow.SuspendLayout();
        m_oSummaryPanel.SuspendLayout();
        m_oSummaryCountCard.SuspendLayout();
        m_oSummaryDateCard.SuspendLayout();
        bottomBar.SuspendLayout();
        SuspendLayout();
        // 
        // headerPanel
        // 
        headerPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
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
        headerDescriptionLabel.Text = "날짜를 선택해 그날의 백업과 작업 로그를 확인하고, 필요한 항목만 비교/적용합니다.";
        headerDescriptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // headerTitleLabel
        // 
        headerTitleLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        headerTitleLabel.BackColor = Color.White;
        headerTitleLabel.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
        headerTitleLabel.ForeColor = Color.FromArgb(28, 32, 41);
        headerTitleLabel.Location = new Point(8, 2);
        headerTitleLabel.Name = "headerTitleLabel";
        headerTitleLabel.Size = new Size(992, 31);
        headerTitleLabel.TabIndex = 0;
        headerTitleLabel.Text = "이력 관리";
        headerTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // workspace
        // 
        workspace.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        workspace.BackColor = Color.White;
        workspace.Controls.Add(_workspaceSplit);
        workspace.Controls.Add(m_oSummaryPanel);
        workspace.Location = new Point(0, 66);
        workspace.Name = "workspace";
        workspace.Padding = new Padding(1);
        workspace.Size = new Size(1000, 568);
        workspace.TabIndex = 1;
        // 
        // _workspaceSplit
        // 
        _workspaceSplit.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _workspaceSplit.BackColor = Color.FromArgb(224, 226, 231);
        _workspaceSplit.FixedPanel = FixedPanel.Panel1;
        _workspaceSplit.Location = new Point(1, 92);
        _workspaceSplit.Name = "_workspaceSplit";
        // 
        // _workspaceSplit.Panel1
        // 
        _workspaceSplit.Panel1.BackColor = Color.White;
        _workspaceSplit.Panel1.Controls.Add(_calendarRecentPanel);
        _workspaceSplit.Panel1.Controls.Add(_calendarRecentTitle);
        _workspaceSplit.Panel1.Controls.Add(_calendarStatsLabel);
        _workspaceSplit.Panel1.Controls.Add(_calendar);
        _workspaceSplit.Panel1.Controls.Add(calendarTitle);
        _workspaceSplit.Panel1.Padding = new Padding(6, 0, 10, 0);
        // 
        // _workspaceSplit.Panel2
        // 
        _workspaceSplit.Panel2.BackColor = Color.White;
        _workspaceSplit.Panel2.Controls.Add(gridHost);
        _workspaceSplit.Panel2.Controls.Add(selectionBar);
        _workspaceSplit.Size = new Size(998, 475);
        _workspaceSplit.SplitterDistance = 280;
        _workspaceSplit.TabIndex = 1;
        // 
        // _calendarRecentPanel
        // 
        _calendarRecentPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _calendarRecentPanel.Controls.Add(m_oRecentLink5);
        _calendarRecentPanel.Controls.Add(m_oRecentLink4);
        _calendarRecentPanel.Controls.Add(m_oRecentLink3);
        _calendarRecentPanel.Controls.Add(m_oRecentLink2);
        _calendarRecentPanel.Controls.Add(m_oRecentLink1);
        _calendarRecentPanel.Location = new Point(12, 264);
        _calendarRecentPanel.Name = "_calendarRecentPanel";
        _calendarRecentPanel.Size = new Size(242, 198);
        _calendarRecentPanel.TabIndex = 4;
        // 
        // m_oRecentLink5
        // 
        m_oRecentLink5.AutoSize = true;
        m_oRecentLink5.Font = new Font("맑은 고딕", 8.5F);
        m_oRecentLink5.LinkColor = Color.FromArgb(30, 100, 199);
        m_oRecentLink5.Location = new Point(0, 88);
        m_oRecentLink5.Name = "m_oRecentLink5";
        m_oRecentLink5.Size = new Size(73, 15);
        m_oRecentLink5.TabIndex = 4;
        m_oRecentLink5.TabStop = true;
        m_oRecentLink5.Text = "2026-08-20";
        m_oRecentLink5.Click += UiClick_RecentDate;
        // 
        // m_oRecentLink4
        // 
        m_oRecentLink4.AutoSize = true;
        m_oRecentLink4.Font = new Font("맑은 고딕", 8.5F);
        m_oRecentLink4.LinkColor = Color.FromArgb(30, 100, 199);
        m_oRecentLink4.Location = new Point(0, 66);
        m_oRecentLink4.Name = "m_oRecentLink4";
        m_oRecentLink4.Size = new Size(73, 15);
        m_oRecentLink4.TabIndex = 3;
        m_oRecentLink4.TabStop = true;
        m_oRecentLink4.Text = "2026-08-21";
        m_oRecentLink4.Click += UiClick_RecentDate;
        // 
        // m_oRecentLink3
        // 
        m_oRecentLink3.AutoSize = true;
        m_oRecentLink3.Font = new Font("맑은 고딕", 8.5F);
        m_oRecentLink3.LinkColor = Color.FromArgb(30, 100, 199);
        m_oRecentLink3.Location = new Point(0, 44);
        m_oRecentLink3.Name = "m_oRecentLink3";
        m_oRecentLink3.Size = new Size(73, 15);
        m_oRecentLink3.TabIndex = 2;
        m_oRecentLink3.TabStop = true;
        m_oRecentLink3.Text = "2026-08-24";
        m_oRecentLink3.Click += UiClick_RecentDate;
        // 
        // m_oRecentLink2
        // 
        m_oRecentLink2.AutoSize = true;
        m_oRecentLink2.Font = new Font("맑은 고딕", 8.5F);
        m_oRecentLink2.LinkColor = Color.FromArgb(30, 100, 199);
        m_oRecentLink2.Location = new Point(0, 22);
        m_oRecentLink2.Name = "m_oRecentLink2";
        m_oRecentLink2.Size = new Size(73, 15);
        m_oRecentLink2.TabIndex = 1;
        m_oRecentLink2.TabStop = true;
        m_oRecentLink2.Text = "2026-08-25";
        m_oRecentLink2.Click += UiClick_RecentDate;
        // 
        // m_oRecentLink1
        // 
        m_oRecentLink1.AutoSize = true;
        m_oRecentLink1.Font = new Font("맑은 고딕", 8.5F);
        m_oRecentLink1.LinkColor = Color.FromArgb(30, 100, 199);
        m_oRecentLink1.Location = new Point(0, 0);
        m_oRecentLink1.Name = "m_oRecentLink1";
        m_oRecentLink1.Size = new Size(73, 15);
        m_oRecentLink1.TabIndex = 0;
        m_oRecentLink1.TabStop = true;
        m_oRecentLink1.Text = "2026-08-26";
        m_oRecentLink1.Click += UiClick_RecentDate;
        // 
        // _calendarRecentTitle
        // 
        _calendarRecentTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _calendarRecentTitle.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        _calendarRecentTitle.ForeColor = Color.FromArgb(108, 115, 128);
        _calendarRecentTitle.Location = new Point(12, 244);
        _calendarRecentTitle.Name = "_calendarRecentTitle";
        _calendarRecentTitle.Size = new Size(242, 18);
        _calendarRecentTitle.TabIndex = 3;
        _calendarRecentTitle.Text = "최근 이력";
        // 
        // _calendarStatsLabel
        // 
        _calendarStatsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _calendarStatsLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        _calendarStatsLabel.ForeColor = Color.FromArgb(28, 32, 41);
        _calendarStatsLabel.Location = new Point(12, 218);
        _calendarStatsLabel.Name = "_calendarStatsLabel";
        _calendarStatsLabel.Size = new Size(242, 20);
        _calendarStatsLabel.TabIndex = 2;
        _calendarStatsLabel.Text = "이번 달 백업 0건  ·  로그 0건";
        // 
        // _calendar
        // 
        _calendar.Location = new Point(12, 40);
        _calendar.MaxSelectionCount = 1;
        _calendar.Name = "_calendar";
        _calendar.TabIndex = 1;
        // 
        // calendarTitle
        // 
        calendarTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        calendarTitle.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
        calendarTitle.ForeColor = Color.FromArgb(28, 32, 41);
        calendarTitle.Location = new Point(6, 0);
        calendarTitle.Name = "calendarTitle";
        calendarTitle.Size = new Size(264, 36);
        calendarTitle.TabIndex = 0;
        calendarTitle.Text = "이력 날짜";
        // 
        // gridHost
        // 
        gridHost.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        gridHost.BackColor = Color.White;
        gridHost.Controls.Add(_grid);
        gridHost.Controls.Add(_emptyStateLabel);
        gridHost.Controls.Add(_existenceHost);
        gridHost.Location = new Point(0, 84);
        gridHost.Name = "gridHost";
        gridHost.Size = new Size(714, 391);
        gridHost.TabIndex = 1;
        // 
        // _grid
        // 
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.AllowUserToResizeRows = false;
        _grid.BackgroundColor = Color.White;
        _grid.BorderStyle = BorderStyle.None;
        _grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(238, 240, 244);
        dataGridViewCellStyle1.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        dataGridViewCellStyle1.ForeColor = Color.FromArgb(28, 32, 41);
        dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        _grid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        _grid.ColumnHeadersHeight = 40;
        _grid.Columns.AddRange(new DataGridViewColumn[] { m_oNumberColumn });
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = SystemColors.Window;
        dataGridViewCellStyle3.Font = new Font("맑은 고딕", 9F);
        dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(224, 231, 241);
        dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(28, 32, 41);
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        _grid.DefaultCellStyle = dataGridViewCellStyle3;
        _grid.Dock = DockStyle.Fill;
        _grid.EnableHeadersVisualStyles = false;
        _grid.GridColor = Color.FromArgb(224, 226, 231);
        _grid.Location = new Point(0, 150);
        _grid.MultiSelect = false;
        _grid.Name = "_grid";
        _grid.RowHeadersVisible = false;
        _grid.RowTemplate.Height = 36;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.Size = new Size(714, 241);
        _grid.TabIndex = 0;
        // 
        // m_oNumberColumn
        // 
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
        m_oNumberColumn.DefaultCellStyle = dataGridViewCellStyle2;
        m_oNumberColumn.HeaderText = "No.";
        m_oNumberColumn.Name = "m_oNumberColumn";
        m_oNumberColumn.ReadOnly = true;
        m_oNumberColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        m_oNumberColumn.Width = 45;
        // 
        // _emptyStateLabel
        // 
        _emptyStateLabel.Dock = DockStyle.Fill;
        _emptyStateLabel.Font = new Font("맑은 고딕", 10F);
        _emptyStateLabel.ForeColor = Color.FromArgb(108, 115, 128);
        _emptyStateLabel.Location = new Point(0, 150);
        _emptyStateLabel.Name = "_emptyStateLabel";
        _emptyStateLabel.Size = new Size(714, 241);
        _emptyStateLabel.TabIndex = 1;
        _emptyStateLabel.Text = "비교할 백업을 선택한 뒤 XML 비교를 실행하세요.";
        _emptyStateLabel.TextAlign = ContentAlignment.MiddleCenter;
        _emptyStateLabel.Visible = false;
        // 
        // _existenceHost
        // 
        _existenceHost.BackColor = Color.White;
        _existenceHost.Controls.Add(_existenceGrid);
        _existenceHost.Controls.Add(_existenceTitleLabel);
        _existenceHost.Dock = DockStyle.Top;
        _existenceHost.Location = new Point(0, 0);
        _existenceHost.Name = "_existenceHost";
        _existenceHost.Padding = new Padding(0, 0, 0, 8);
        _existenceHost.Size = new Size(714, 150);
        _existenceHost.TabIndex = 2;
        _existenceHost.Visible = false;
        // 
        // _existenceGrid
        // 
        _existenceGrid.AllowUserToAddRows = false;
        _existenceGrid.AllowUserToDeleteRows = false;
        _existenceGrid.AllowUserToResizeRows = false;
        _existenceGrid.BackgroundColor = Color.White;
        _existenceGrid.BorderStyle = BorderStyle.None;
        _existenceGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle4.BackColor = Color.FromArgb(238, 240, 244);
        dataGridViewCellStyle4.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        dataGridViewCellStyle4.ForeColor = Color.FromArgb(28, 32, 41);
        dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
        _existenceGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
        _existenceGrid.ColumnHeadersHeight = 32;
        dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle5.BackColor = SystemColors.Window;
        dataGridViewCellStyle5.Font = new Font("맑은 고딕", 9F);
        dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
        dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(224, 231, 241);
        dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(28, 32, 41);
        dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
        _existenceGrid.DefaultCellStyle = dataGridViewCellStyle5;
        _existenceGrid.Dock = DockStyle.Fill;
        _existenceGrid.EnableHeadersVisualStyles = false;
        _existenceGrid.GridColor = Color.FromArgb(224, 226, 231);
        _existenceGrid.Location = new Point(0, 24);
        _existenceGrid.MultiSelect = false;
        _existenceGrid.Name = "_existenceGrid";
        _existenceGrid.RowHeadersVisible = false;
        _existenceGrid.RowTemplate.Height = 30;
        _existenceGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _existenceGrid.Size = new Size(714, 118);
        _existenceGrid.TabIndex = 1;
        // 
        // _existenceTitleLabel
        // 
        _existenceTitleLabel.Dock = DockStyle.Top;
        _existenceTitleLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        _existenceTitleLabel.ForeColor = Color.FromArgb(28, 32, 41);
        _existenceTitleLabel.Location = new Point(0, 0);
        _existenceTitleLabel.Name = "_existenceTitleLabel";
        _existenceTitleLabel.Padding = new Padding(4, 4, 0, 0);
        _existenceTitleLabel.Size = new Size(714, 24);
        _existenceTitleLabel.TabIndex = 0;
        _existenceTitleLabel.Text = "파일/폴더 존재 차이";
        // 
        // selectionBar
        // 
        selectionBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        selectionBar.BackColor = Color.White;
        selectionBar.Controls.Add(backupSelectorPanel);
        selectionBar.Controls.Add(_logViewButton);
        selectionBar.Controls.Add(selectionBarActionSpacer);
        selectionBar.Controls.Add(_compareButton);
        selectionBar.Location = new Point(0, 0);
        selectionBar.Name = "selectionBar";
        selectionBar.Padding = new Padding(18, 8, 18, 8);
        selectionBar.Size = new Size(714, 84);
        selectionBar.TabIndex = 0;
        // 
        // backupSelectorPanel
        // 
        backupSelectorPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        backupSelectorPanel.Controls.Add(_destinationRow);
        backupSelectorPanel.Controls.Add(_sourceRow);
        backupSelectorPanel.Location = new Point(18, 8);
        backupSelectorPanel.Name = "backupSelectorPanel";
        backupSelectorPanel.Size = new Size(496, 68);
        backupSelectorPanel.TabIndex = 0;
        // 
        // _destinationRow
        // 
        _destinationRow.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _destinationRow.Controls.Add(_backupCombo);
        _destinationRow.Controls.Add(_backupComboLabel);
        _destinationRow.Location = new Point(0, 34);
        _destinationRow.Name = "_destinationRow";
        _destinationRow.Size = new Size(496, 30);
        _destinationRow.TabIndex = 1;
        // 
        // _backupCombo
        // 
        _backupCombo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _backupCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        _backupCombo.Font = new Font("맑은 고딕", 9.5F);
        _backupCombo.FormattingEnabled = true;
        _backupCombo.Location = new Point(62, 0);
        _backupCombo.Name = "_backupCombo";
        _backupCombo.Size = new Size(434, 25);
        _backupCombo.TabIndex = 1;
        // 
        // _backupComboLabel
        // 
        _backupComboLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        _backupComboLabel.ForeColor = Color.FromArgb(28, 32, 41);
        _backupComboLabel.Location = new Point(0, 0);
        _backupComboLabel.Name = "_backupComboLabel";
        _backupComboLabel.Size = new Size(62, 26);
        _backupComboLabel.TabIndex = 0;
        _backupComboLabel.Text = "Dest :";
        _backupComboLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _sourceRow
        // 
        _sourceRow.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _sourceRow.Controls.Add(_sourceCombo);
        _sourceRow.Controls.Add(_sourceIsCurrentCheckBox);
        _sourceRow.Controls.Add(_sourceComboLabel);
        _sourceRow.Location = new Point(0, 0);
        _sourceRow.Name = "_sourceRow";
        _sourceRow.Size = new Size(496, 30);
        _sourceRow.TabIndex = 0;
        // 
        // _sourceCombo
        // 
        _sourceCombo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _sourceCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        _sourceCombo.Enabled = false;
        _sourceCombo.Font = new Font("맑은 고딕", 9.5F);
        _sourceCombo.FormattingEnabled = true;
        _sourceCombo.Location = new Point(212, 0);
        _sourceCombo.Name = "_sourceCombo";
        _sourceCombo.Size = new Size(284, 25);
        _sourceCombo.TabIndex = 2;
        // 
        // _sourceIsCurrentCheckBox
        // 
        _sourceIsCurrentCheckBox.Checked = true;
        _sourceIsCurrentCheckBox.CheckState = CheckState.Checked;
        _sourceIsCurrentCheckBox.ForeColor = Color.FromArgb(28, 32, 41);
        _sourceIsCurrentCheckBox.Location = new Point(62, 0);
        _sourceIsCurrentCheckBox.Name = "_sourceIsCurrentCheckBox";
        _sourceIsCurrentCheckBox.Size = new Size(150, 26);
        _sourceIsCurrentCheckBox.TabIndex = 1;
        _sourceIsCurrentCheckBox.Text = "현재 적용된 파일 기준";
        _sourceIsCurrentCheckBox.CheckedChanged += UiChange_SourceIsCurrent;
        // 
        // _sourceComboLabel
        // 
        _sourceComboLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        _sourceComboLabel.ForeColor = Color.FromArgb(28, 32, 41);
        _sourceComboLabel.Location = new Point(0, 0);
        _sourceComboLabel.Name = "_sourceComboLabel";
        _sourceComboLabel.Size = new Size(62, 26);
        _sourceComboLabel.TabIndex = 0;
        _sourceComboLabel.Text = "Source :";
        _sourceComboLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _logViewButton
        // 
        _logViewButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _logViewButton.BackColor = Color.White;
        _logViewButton.BorderThickness = 1;
        _logViewButton.ButtonType = StyledButtonType.Secondary;
        _logViewButton.Font = new Font("맑은 고딕", 9F);
        _logViewButton.ForeColor = Color.FromArgb(28, 32, 41);
        _logViewButton.Location = new Point(516, 8);
        _logViewButton.Name = "_logViewButton";
        _logViewButton.Size = new Size(84, 68);
        _logViewButton.TabIndex = 1;
        _logViewButton.Text = "작업 로그";
        // 
        // selectionBarActionSpacer
        // 
        selectionBarActionSpacer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        selectionBarActionSpacer.Location = new Point(600, 8);
        selectionBarActionSpacer.Name = "selectionBarActionSpacer";
        selectionBarActionSpacer.Size = new Size(8, 68);
        selectionBarActionSpacer.TabIndex = 2;
        // 
        // _compareButton
        // 
        _compareButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _compareButton.BackColor = Color.FromArgb(30, 100, 199);
        _compareButton.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        _compareButton.ForeColor = Color.White;
        _compareButton.Location = new Point(608, 8);
        _compareButton.Name = "_compareButton";
        _compareButton.Size = new Size(90, 68);
        _compareButton.TabIndex = 3;
        _compareButton.Text = "XML 비교";
        _compareButton.Click += UiClick_Compare;
        // 
        // m_oSummaryPanel
        // 
        m_oSummaryPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        m_oSummaryPanel.Controls.Add(m_oSummaryCountCard);
        m_oSummaryPanel.Controls.Add(m_oSummaryDateCard);
        m_oSummaryPanel.Location = new Point(0, 0);
        m_oSummaryPanel.Name = "m_oSummaryPanel";
        m_oSummaryPanel.Padding = new Padding(16);
        m_oSummaryPanel.Size = new Size(1000, 92);
        m_oSummaryPanel.TabIndex = 0;
        // 
        // m_oSummaryCountCard
        // 
        m_oSummaryCountCard.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        m_oSummaryCountCard.BackColor = Color.White;
        m_oSummaryCountCard.Controls.Add(_summaryCountValue);
        m_oSummaryCountCard.Controls.Add(_summaryCountCaption);
        m_oSummaryCountCard.Controls.Add(m_oSummaryCountBadge);
        m_oSummaryCountCard.Location = new Point(508, 16);
        m_oSummaryCountCard.Name = "m_oSummaryCountCard";
        m_oSummaryCountCard.Padding = new Padding(3);
        m_oSummaryCountCard.Size = new Size(476, 60);
        m_oSummaryCountCard.TabIndex = 1;
        // 
        // _summaryCountValue
        // 
        _summaryCountValue.AutoSize = true;
        _summaryCountValue.Font = new Font("맑은 고딕", 13F, FontStyle.Bold);
        _summaryCountValue.ForeColor = Color.FromArgb(28, 32, 41);
        _summaryCountValue.Location = new Point(64, 28);
        _summaryCountValue.Name = "_summaryCountValue";
        _summaryCountValue.Size = new Size(181, 25);
        _summaryCountValue.TabIndex = 2;
        _summaryCountValue.Text = "백업 0건  ·  로그 0건";
        // 
        // _summaryCountCaption
        // 
        _summaryCountCaption.AutoSize = true;
        _summaryCountCaption.Font = new Font("맑은 고딕", 9F);
        _summaryCountCaption.ForeColor = Color.FromArgb(108, 115, 128);
        _summaryCountCaption.Location = new Point(64, 8);
        _summaryCountCaption.Name = "_summaryCountCaption";
        _summaryCountCaption.Size = new Size(59, 15);
        _summaryCountCaption.TabIndex = 1;
        _summaryCountCaption.Text = "이력 요약";
        // 
        // m_oSummaryCountBadge
        // 
        m_oSummaryCountBadge.BackColor = Color.Transparent;
        m_oSummaryCountBadge.Glyph = GlyphBadgeKind.Archive;
        m_oSummaryCountBadge.Location = new Point(16, 12);
        m_oSummaryCountBadge.Name = "m_oSummaryCountBadge";
        m_oSummaryCountBadge.Size = new Size(36, 36);
        m_oSummaryCountBadge.TabIndex = 0;
        // 
        // m_oSummaryDateCard
        // 
        m_oSummaryDateCard.BackColor = Color.White;
        m_oSummaryDateCard.Controls.Add(_summaryDateValue);
        m_oSummaryDateCard.Controls.Add(_summaryDateCaption);
        m_oSummaryDateCard.Controls.Add(m_oSummaryDateBadge);
        m_oSummaryDateCard.Location = new Point(16, 16);
        m_oSummaryDateCard.Name = "m_oSummaryDateCard";
        m_oSummaryDateCard.Padding = new Padding(3);
        m_oSummaryDateCard.Size = new Size(476, 60);
        m_oSummaryDateCard.TabIndex = 0;
        // 
        // _summaryDateValue
        // 
        _summaryDateValue.AutoSize = true;
        _summaryDateValue.Font = new Font("맑은 고딕", 13F, FontStyle.Bold);
        _summaryDateValue.ForeColor = Color.FromArgb(28, 32, 41);
        _summaryDateValue.Location = new Point(64, 28);
        _summaryDateValue.Name = "_summaryDateValue";
        _summaryDateValue.Size = new Size(162, 25);
        _summaryDateValue.TabIndex = 2;
        _summaryDateValue.Text = "날짜를 선택하세요";
        // 
        // _summaryDateCaption
        // 
        _summaryDateCaption.AutoSize = true;
        _summaryDateCaption.Font = new Font("맑은 고딕", 9F);
        _summaryDateCaption.ForeColor = Color.FromArgb(108, 115, 128);
        _summaryDateCaption.Location = new Point(64, 8);
        _summaryDateCaption.Name = "_summaryDateCaption";
        _summaryDateCaption.Size = new Size(99, 15);
        _summaryDateCaption.TabIndex = 1;
        _summaryDateCaption.Text = "선택된 이력 날짜";
        // 
        // m_oSummaryDateBadge
        // 
        m_oSummaryDateBadge.BackColor = Color.Transparent;
        m_oSummaryDateBadge.Location = new Point(16, 12);
        m_oSummaryDateBadge.Name = "m_oSummaryDateBadge";
        m_oSummaryDateBadge.Size = new Size(36, 36);
        m_oSummaryDateBadge.TabIndex = 0;
        // 
        // bottomBar
        // 
        bottomBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        bottomBar.Controls.Add(_summaryLabel);
        bottomBar.Controls.Add(_cancelButton);
        bottomBar.Controls.Add(_applyButton);
        bottomBar.Controls.Add(_progress);
        bottomBar.Location = new Point(0, 634);
        bottomBar.Name = "bottomBar";
        bottomBar.Padding = new Padding(0, 12, 0, 0);
        bottomBar.Size = new Size(1000, 66);
        bottomBar.TabIndex = 2;
        // 
        // _summaryLabel
        // 
        _summaryLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _summaryLabel.ForeColor = Color.FromArgb(108, 115, 128);
        _summaryLabel.Location = new Point(18, 18);
        _summaryLabel.Name = "_summaryLabel";
        _summaryLabel.Size = new Size(692, 42);
        _summaryLabel.TabIndex = 0;
        _summaryLabel.Text = "백업을 선택하고 XML 비교를 실행하세요.";
        _summaryLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // _cancelButton
        // 
        _cancelButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _cancelButton.BackColor = Color.FromArgb(233, 238, 245);
        _cancelButton.BorderThickness = 1;
        _cancelButton.ButtonType = StyledButtonType.Secondary;
        _cancelButton.Font = new Font("맑은 고딕", 9F);
        _cancelButton.ForeColor = Color.FromArgb(28, 32, 41);
        _cancelButton.Location = new Point(710, 18);
        _cancelButton.Name = "_cancelButton";
        _cancelButton.Size = new Size(100, 42);
        _cancelButton.TabIndex = 1;
        _cancelButton.Text = "취소";
        // 
        // _applyButton
        // 
        _applyButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _applyButton.BackColor = Color.FromArgb(30, 100, 199);
        _applyButton.Enabled = false;
        _applyButton.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        _applyButton.ForeColor = Color.White;
        _applyButton.Location = new Point(820, 18);
        _applyButton.Name = "_applyButton";
        _applyButton.Size = new Size(160, 42);
        _applyButton.TabIndex = 2;
        _applyButton.Text = "선택 설정 적용";
        _applyButton.Click += UiClick_Apply;
        // 
        // _progress
        // 
        _progress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _progress.Location = new Point(0, 12);
        _progress.Name = "_progress";
        _progress.Size = new Size(1000, 6);
        _progress.TabIndex = 3;
        // 
        // HistoryControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(workspace);
        Controls.Add(bottomBar);
        Controls.Add(headerPanel);
        Name = "HistoryControl";
        Size = new Size(1000, 700);
        headerPanel.ResumeLayout(false);
        workspace.ResumeLayout(false);
        _workspaceSplit.Panel1.ResumeLayout(false);
        _workspaceSplit.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_workspaceSplit).EndInit();
        _workspaceSplit.ResumeLayout(false);
        _calendarRecentPanel.ResumeLayout(false);
        _calendarRecentPanel.PerformLayout();
        gridHost.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_grid).EndInit();
        _existenceHost.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_existenceGrid).EndInit();
        selectionBar.ResumeLayout(false);
        backupSelectorPanel.ResumeLayout(false);
        _destinationRow.ResumeLayout(false);
        _sourceRow.ResumeLayout(false);
        m_oSummaryPanel.ResumeLayout(false);
        m_oSummaryCountCard.ResumeLayout(false);
        m_oSummaryCountCard.PerformLayout();
        m_oSummaryDateCard.ResumeLayout(false);
        m_oSummaryDateCard.PerformLayout();
        bottomBar.ResumeLayout(false);
        ResumeLayout(false);
    }
}
