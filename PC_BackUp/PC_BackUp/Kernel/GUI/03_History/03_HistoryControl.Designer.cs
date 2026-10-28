namespace PC_BackUp;

partial class HistoryControl
{
    private System.ComponentModel.IContainer? components;

    // 화면의 고정 골격 — 값이 바뀌지 않는 부분이라 전부 필드로 선언해 Visual Studio
    // 디자이너에서도 그대로 열어서 확인할 수 있게 한다. 아래 두 가지는 예외로 코드에만 남긴다:
    //  1) 상단 요약 카드의 원형 아이콘(IconGlyphs) — GDI+로 직접 그리는 Paint 이벤트라 디자이너가
    //     표현할 수 없다. HistoryControl() 생성자에서 InitializeComponent() 이후에 붙인다.
    //  2) 이력 달력의 실제 크기(ApplyCalendarSizing) — 화면 진입 시 왼쪽 패널 폭에 반영한다.
    //  3) 그리드 열은 비교 화면/작업 로그 화면이 서로 바뀌므로, 여기서는 기본값(비교 화면 열)만
    //     잡아두고 실제 전환은 ConfigureCompareColumns()/ConfigureLogColumns()가 담당한다.
    private Panel headerPanel = null!;
    private Label headerTitleLabel = null!;
    private Label headerDescriptionLabel = null!;
    private CardPanel workspace = null!;
    private TableLayoutPanel m_oWorkspaceLayout = null!;
    private Label calendarTitle = null!;
    private Panel gridHost = null!;
    private TableLayoutPanel selectionBar = null!;
    private Panel backupSelectorPanel = null!;
    private TableLayoutPanel bottomBar = null!;
    private FlowLayoutPanel m_oSelectionActions = null!;
    private FlowLayoutPanel m_oBottomActions = null!;

    private readonly Panel _sourceRow = new();
    private readonly Panel _destinationRow = new();
    private readonly ComboBox _backupCombo = new();
    private readonly StyledButton _exportDiagnosticsButton = new();
    private readonly Label _backupComboLabel = new();
    private readonly CheckBox _sourceIsCurrentCheckBox = new();
    private readonly ComboBox _sourceCombo = new();
    private readonly Label _sourceComboLabel = new();
    private readonly DataGridView _grid = new();
    private readonly Label _emptyStateLabel = new();
    // "파일/폴더 존재 차이"(한쪽에만 있는 파일)는 "설정값 차이"(_grid)와 성격이 달라서 별도
    // 목록으로 분리한다 — 경로만 보여주면 되고, 값 비교 UI(빨강/노랑 강조 등)는 필요 없다.
    private readonly Panel _existenceHost = new();
    private readonly Label _existenceTitleLabel = new();
    private readonly DataGridView _existenceGrid = new();
    private readonly Label _summaryLabel = new();
    private readonly ProgressBar _progress = new();
    private readonly HistoryCalendar _calendar = new();
    private readonly Label _calendarStatsLabel = new();
    private readonly Label _calendarRecentTitle = new();
    private readonly FlowLayoutPanel _calendarRecentPanel = new();
    private readonly Label _summaryDateCaption = new();
    private readonly Label _summaryDateValue = new();
    private readonly Label _summaryCountCaption = new();
    private readonly Label _summaryCountValue = new();
    private StyledButton _compareButton = null!;
    private StyledButton _applyButton = null!;
    private StyledButton _logViewButton = null!;
    private StyledButton _cancelButton = null!;
    private SplitContainer _workspaceSplit = null!;

    protected override void Dispose(bool disposing) { if (disposing) { components?.Dispose(); } base.Dispose(disposing); }

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
        gridHost = new Panel();
        selectionBar = new TableLayoutPanel();
        backupSelectorPanel = new Panel();
        _logViewButton = new StyledButton();
        m_oSelectionActions = new FlowLayoutPanel();
        m_oBottomActions = new FlowLayoutPanel();
        _compareButton = new StyledButton();
        bottomBar = new TableLayoutPanel();
        _cancelButton = new StyledButton();
        _applyButton = new StyledButton();
        headerPanel.SuspendLayout();
        workspace.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_workspaceSplit).BeginInit();
        _workspaceSplit.Panel1.SuspendLayout();
        _workspaceSplit.Panel2.SuspendLayout();
        _workspaceSplit.SuspendLayout();
        selectionBar.SuspendLayout();
        bottomBar.SuspendLayout();
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
        headerDescriptionLabel.Text = "날짜를 선택해 그날의 백업과 작업 로그를 확인하고, 필요한 항목만 비교/적용합니다.";
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
        headerTitleLabel.Text = "이력 관리";
        headerTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // workspace
        // 
        workspace.BackColor = Color.White;
        // [Codex - 2026.09.21] 요약 카드가 추가되어도 본문을 덮지 않도록 영역을 도킹한다.
        workspace.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        workspace.Location = new Point(0, 100);
        workspace.Name = "workspace";
        workspace.Padding = new Padding(16);
        workspace.Size = new Size(1000, 568);
        workspace.TabIndex = 0;
        // [Codex - 2026.10.28] 요약/본문/작업 영역을 별도 행으로 배치해 도킹 순서에 의한 겹침을 없앤다.
        m_oWorkspaceLayout.Dock = DockStyle.Fill;
        m_oWorkspaceLayout.ColumnCount = 1;
        m_oWorkspaceLayout.RowCount = 3;
        m_oWorkspaceLayout.Margin = Padding.Empty;
        m_oWorkspaceLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        m_oWorkspaceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        m_oWorkspaceLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        m_oWorkspaceLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        _workspaceSplit.Margin = Padding.Empty;
        bottomBar.Margin = Padding.Empty;
        bottomBar.Dock = DockStyle.Fill;
        m_oWorkspaceLayout.Controls.Add(_workspaceSplit, 0, 1);
        m_oWorkspaceLayout.Controls.Add(bottomBar, 0, 2);
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
        // [Codex - 2026.09.21] 이력 달력과 월간 요약을 왼쪽 패널에 배치한다.
        _workspaceSplit.Panel1.Controls.Add(_calendarRecentPanel);
        _workspaceSplit.Panel1.Controls.Add(_calendarRecentTitle);
        _workspaceSplit.Panel1.Controls.Add(_calendarStatsLabel);
        _workspaceSplit.Panel1.Controls.Add(_calendar);
        _workspaceSplit.Panel1.Controls.Add(calendarTitle);
        _calendar.Location = new Point(CalendarLeftMargin, CalendarTitleHeight);
        _calendar.AccessibleName = "이력 날짜";
        _calendarStatsLabel.Font = new Font("맑은 고딕", 8.5F);
        _calendarStatsLabel.ForeColor = ColorRGB.MutedText;
        _calendarRecentTitle.Text = "최근 이력";
        _calendarRecentTitle.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        _calendarRecentTitle.ForeColor = ColorRGB.Text;
        _calendarRecentPanel.FlowDirection = FlowDirection.TopDown;
        _calendarRecentPanel.WrapContents = false;
        _calendarRecentPanel.AutoScroll = true;
        _calendarRecentPanel.Padding = new Padding(0, 2, 0, 0); // 맨 위 카드의 윗 테두리가 잘리지 않게 한다
        // 
        // _workspaceSplit.Panel2
        // 
        _workspaceSplit.Panel2.BackColor = Color.White;
        _workspaceSplit.Panel2.Padding = new Padding(16, 0, 0, 0);
        _workspaceSplit.Panel2.Controls.Add(gridHost);
        _workspaceSplit.Panel2.Controls.Add(selectionBar);
        _workspaceSplit.Size = new Size(960, 447);
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
        calendarTitle.Text = "이력 날짜";
        // 
        // gridHost
        // 
        gridHost.BackColor = Color.White;
        gridHost.Dock = DockStyle.Fill;
        gridHost.Location = new Point(0, 84);
        gridHost.Name = "gridHost";
        gridHost.Size = new Size(835, 363);
        gridHost.TabIndex = 0;
        // [Codex - 2026.09.21] 이슈 6 레이아웃에서 빠진 그리드와 빈 상태 표시를 다시 연결한다.
        _grid.Dock = DockStyle.Fill;
        _grid.BackgroundColor = ColorRGB.Surface;
        _grid.BorderStyle = BorderStyle.None;
        ColorRGB.ApplyDataGridStyle(_grid);
        _grid.AutoGenerateColumns = false;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.RowHeadersVisible = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _emptyStateLabel.Dock = DockStyle.Fill;
        _emptyStateLabel.Text = "비교할 백업을 선택한 뒤 XML 비교를 실행하세요.";
        _emptyStateLabel.TextAlign = ContentAlignment.MiddleCenter;
        _emptyStateLabel.ForeColor = ColorRGB.MutedText;
        _existenceHost.Dock = DockStyle.Top;
        _existenceHost.Height = 150;
        _existenceHost.Visible = false;
        _existenceTitleLabel.Dock = DockStyle.Top;
        _existenceTitleLabel.Height = 24;
        _existenceTitleLabel.Text = "파일/폴더 존재 차이";
        _existenceGrid.Dock = DockStyle.Fill;
        ColorRGB.ApplyDataGridStyle(_existenceGrid);
        _existenceGrid.AutoGenerateColumns = false;
        _existenceGrid.AllowUserToAddRows = false;
        _existenceGrid.RowHeadersVisible = false;
        _existenceGrid.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "적용", DataPropertyName = nameof(XmlDifference.Apply), Width = 58 });
        _existenceGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "상태", DataPropertyName = nameof(XmlDifference.Kind), Width = 110, ReadOnly = true });
        _existenceGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "경로", DataPropertyName = nameof(XmlDifference.RelativeFilePath), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
        _existenceHost.Controls.Add(_existenceGrid);
        _existenceHost.Controls.Add(_existenceTitleLabel);
        gridHost.Controls.Add(_grid);
        gridHost.Controls.Add(_emptyStateLabel);
        gridHost.Controls.Add(_existenceHost);
        // 
        // selectionBar
        //
        // [Codex - 2026.10.28] 입력 행과 작업 버튼 행을 분리해 작은 창에서도 서로 공간을 침범하지 않는다.
        selectionBar.BackColor = ColorRGB.Surface;
        selectionBar.Dock = DockStyle.Top;
        selectionBar.Name = "selectionBar";
        selectionBar.AutoSize = true;
        selectionBar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        selectionBar.ColumnCount = 1;
        selectionBar.RowCount = 2;
        selectionBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        selectionBar.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        selectionBar.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        selectionBar.Padding = new Padding(0, 0, 0, 12);
        selectionBar.TabIndex = 1;
        selectionBar.Controls.Add(backupSelectorPanel, 0, 0);
        selectionBar.Controls.Add(m_oSelectionActions, 0, 1);
        //
        // backupSelectorPanel
        //
        backupSelectorPanel.Dock = DockStyle.Fill;
        backupSelectorPanel.Margin = Padding.Empty;
        backupSelectorPanel.Name = "backupSelectorPanel";
        backupSelectorPanel.Size = new Size(617, 64);
        backupSelectorPanel.TabIndex = 0;
        _sourceRow.Dock = DockStyle.Top;
        _sourceRow.Height = 36;
        _sourceRow.Padding = new Padding(0, 0, 0, ColorRGB.ControlGap);
        _sourceComboLabel.Dock = DockStyle.Left;
        _sourceComboLabel.Width = 62;
        _sourceComboLabel.Text = "Source :";
        _sourceComboLabel.TextAlign = ContentAlignment.MiddleLeft;
        _sourceIsCurrentCheckBox.Dock = DockStyle.Left;
        _sourceIsCurrentCheckBox.Width = 158;
        _sourceIsCurrentCheckBox.Padding = new Padding(0, 0, ColorRGB.ControlGap, 0);
        _sourceIsCurrentCheckBox.Text = "현재 적용된 파일 기준";
        _sourceIsCurrentCheckBox.Checked = true;
        _sourceIsCurrentCheckBox.CheckedChanged += UiChange_SourceIsCurrent;
        _sourceCombo.Dock = DockStyle.Fill;
        _sourceCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        _sourceCombo.Enabled = false;
        _sourceRow.Controls.Add(_sourceCombo);
        _sourceRow.Controls.Add(_sourceIsCurrentCheckBox);
        _sourceRow.Controls.Add(_sourceComboLabel);
        _destinationRow.Dock = DockStyle.Top;
        _destinationRow.Height = 28;
        _backupComboLabel.Dock = DockStyle.Left;
        _backupComboLabel.Width = 62;
        _backupComboLabel.Text = "Dest :";
        _backupComboLabel.TextAlign = ContentAlignment.MiddleLeft;
        _backupCombo.Dock = DockStyle.Fill;
        _backupCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        _destinationRow.Controls.Add(_backupCombo);
        _destinationRow.Controls.Add(_backupComboLabel);
        backupSelectorPanel.Controls.Add(_destinationRow);
        backupSelectorPanel.Controls.Add(_sourceRow);
        //
        // m_oSelectionActions
        //
        m_oSelectionActions.Name = "m_oSelectionActions";
        m_oSelectionActions.Dock = DockStyle.Fill;
        m_oSelectionActions.AutoSize = true;
        m_oSelectionActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        m_oSelectionActions.FlowDirection = FlowDirection.RightToLeft;
        m_oSelectionActions.WrapContents = false;
        m_oSelectionActions.Margin = new Padding(0, ColorRGB.ControlGap, 0, 0);
        m_oSelectionActions.TabIndex = 1;
        m_oSelectionActions.Controls.Add(_compareButton);
        m_oSelectionActions.Controls.Add(_logViewButton);
        //
        // _compareButton
        //
        _compareButton.ButtonType = StyledButtonType.Primary;
        _compareButton.Dock = DockStyle.None;
        _compareButton.Margin = new Padding(ColorRGB.ControlGap, 0, 0, 0);
        _compareButton.Name = "_compareButton";
        _compareButton.Size = new Size(112, ColorRGB.ActionButtonHeight);
        _compareButton.TabIndex = 1;
        _compareButton.Text = "XML 비교";
        _compareButton.Click += UiClick_Compare;
        //
        // _logViewButton
        //
        _logViewButton.ButtonType = StyledButtonType.Secondary;
        _logViewButton.Dock = DockStyle.None;
        _logViewButton.Margin = new Padding(ColorRGB.ControlGap, 0, 0, 0);
        _logViewButton.Name = "_logViewButton";
        _logViewButton.Size = new Size(112, ColorRGB.ActionButtonHeight);
        _logViewButton.TabIndex = 0;
        _logViewButton.Text = "작업 로그";
        //
        // bottomBar
        //
        // [Codex - 2026.10.28] 상태 문구와 작업 버튼도 별도 행으로 배치하고 실제 간격은 FlowLayoutPanel이 적용한다.
        bottomBar.Dock = DockStyle.Fill;
        bottomBar.Name = "bottomBar";
        bottomBar.AutoSize = true;
        bottomBar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        bottomBar.ColumnCount = 1;
        bottomBar.RowCount = 3;
        bottomBar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        bottomBar.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        bottomBar.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        bottomBar.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        bottomBar.Padding = new Padding(0, 12, 0, 0);
        bottomBar.TabIndex = 1;
        _progress.Dock = DockStyle.Fill;
        _progress.Height = 6;
        _progress.Margin = new Padding(0, 0, 0, ColorRGB.ControlGap);
        _summaryLabel.Dock = DockStyle.Fill;
        _summaryLabel.Height = 20;
        _summaryLabel.Margin = new Padding(0, 0, 0, ColorRGB.ControlGap);
        _summaryLabel.Text = "백업을 선택하고 XML 비교를 실행하세요.";
        _summaryLabel.TextAlign = ContentAlignment.MiddleLeft;
        _summaryLabel.AutoEllipsis = true;
        _summaryLabel.ForeColor = ColorRGB.MutedText;
        bottomBar.Controls.Add(_progress, 0, 0);
        bottomBar.Controls.Add(_summaryLabel, 0, 1);
        bottomBar.Controls.Add(m_oBottomActions, 0, 2);
        //
        // m_oBottomActions
        //
        m_oBottomActions.Name = "m_oBottomActions";
        m_oBottomActions.Dock = DockStyle.Fill;
        m_oBottomActions.AutoSize = true;
        m_oBottomActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        m_oBottomActions.FlowDirection = FlowDirection.RightToLeft;
        m_oBottomActions.WrapContents = false;
        m_oBottomActions.Margin = Padding.Empty;
        m_oBottomActions.TabIndex = 0;
        m_oBottomActions.Controls.Add(_applyButton);
        m_oBottomActions.Controls.Add(_cancelButton);
        m_oBottomActions.Controls.Add(_exportDiagnosticsButton);
        //
        // _applyButton
        //
        _applyButton.ButtonType = StyledButtonType.Primary;
        _applyButton.Dock = DockStyle.None;
        _applyButton.Margin = new Padding(ColorRGB.ControlGap, 0, 0, 0);
        _applyButton.Enabled = false;
        _applyButton.Name = "_applyButton";
        _applyButton.Size = new Size(176, ColorRGB.ActionButtonHeight);
        _applyButton.TabIndex = 2;
        _applyButton.Text = "선택 파일 전체 복원";
        _applyButton.Click += UiClick_Apply;
        //
        // _cancelButton
        //
        _cancelButton.ButtonType = StyledButtonType.Secondary;
        _cancelButton.Dock = DockStyle.None;
        _cancelButton.Margin = new Padding(ColorRGB.ControlGap, 0, 0, 0);
        _cancelButton.Name = "_cancelButton";
        _cancelButton.Size = new Size(96, ColorRGB.ActionButtonHeight);
        _cancelButton.TabIndex = 1;
        _cancelButton.Text = "취소";
        _cancelButton.Visible = false;
        //
        // _exportDiagnosticsButton
        //
        _exportDiagnosticsButton.ButtonType = StyledButtonType.Secondary;
        _exportDiagnosticsButton.Dock = DockStyle.None;
        _exportDiagnosticsButton.Margin = new Padding(ColorRGB.ControlGap, 0, 0, 0);
        _exportDiagnosticsButton.Name = "_exportDiagnosticsButton";
        _exportDiagnosticsButton.Size = new Size(112, ColorRGB.ActionButtonHeight);
        _exportDiagnosticsButton.TabIndex = 0;
        _exportDiagnosticsButton.Text = "진단 내보내기";
        _exportDiagnosticsButton.Click += UiClick_ExportDiagnostics;
        //
        // HistoryControl
        // 
        Controls.Add(workspace);
        Controls.Add(headerPanel);
        Name = "HistoryControl";
        Size = new Size(1000, 700);
        headerPanel.ResumeLayout(false);
        workspace.ResumeLayout(false);
        _workspaceSplit.Panel1.ResumeLayout(false);
        _workspaceSplit.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_workspaceSplit).EndInit();
        _workspaceSplit.ResumeLayout(false);
        selectionBar.ResumeLayout(false);
        bottomBar.ResumeLayout(false);
        ResumeLayout(false);
    }
}
