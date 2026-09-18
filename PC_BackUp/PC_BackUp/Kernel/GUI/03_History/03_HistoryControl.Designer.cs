namespace PC_BackUp;

partial class HistoryControl
{
    private System.ComponentModel.IContainer? components;

    // 화면의 고정 골격 — 값이 바뀌지 않는 부분이라 전부 필드로 선언해 Visual Studio
    // 디자이너에서도 그대로 열어서 확인할 수 있게 한다. 아래 두 가지는 예외로 코드에만 남긴다:
    //  1) 상단 요약 카드의 원형 아이콘(IconGlyphs) — GDI+로 직접 그리는 Paint 이벤트라 디자이너가
    //     표현할 수 없다. HistoryControl() 생성자에서 InitializeComponent() 이후에 붙인다.
    //  2) MonthCalendar의 실제 크기(ApplyCalendarSizing) — DPI에 따라 화면에 실제로 표시된 뒤에만
    //     정확히 잴 수 있어서 RefreshBackupList()에서 매번 다시 계산한다.
    //  3) 그리드 열은 비교 화면/작업 로그 화면이 서로 바뀌므로, 여기서는 기본값(비교 화면 열)만
    //     잡아두고 실제 전환은 ConfigureCompareColumns()/ConfigureLogColumns()가 담당한다.
    private Panel headerPanel = null!;
    private Label headerTitleLabel = null!;
    private Label headerDescriptionLabel = null!;
    private CardPanel workspace = null!;
    private Label calendarTitle = null!;
    private Panel gridHost = null!;
    private Panel selectionBar = null!;
    private Panel backupSelectorPanel = null!;
    private Panel bottomBar = null!;
    private Panel selectionBarActionSpacer = null!;

    private readonly Panel _sourceRow = new();
    private readonly Panel _destinationRow = new();
    private readonly ComboBox _backupCombo = new();
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
    private readonly MonthCalendar _calendar = new();
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
        _workspaceSplit = new SplitContainer();
        calendarTitle = new Label();
        gridHost = new Panel();
        selectionBar = new Panel();
        backupSelectorPanel = new Panel();
        _logViewButton = new StyledButton();
        selectionBarActionSpacer = new Panel();
        _compareButton = new StyledButton();
        bottomBar = new Panel();
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
        workspace.Controls.Add(_workspaceSplit);
        workspace.Controls.Add(bottomBar);
        workspace.Location = new Point(0, 100);
        workspace.Name = "workspace";
        workspace.Padding = new Padding(1);
        workspace.Size = new Size(1000, 568);
        workspace.TabIndex = 0;
        // 
        // _workspaceSplit
        // 
        _workspaceSplit.BackColor = Color.FromArgb(224, 226, 231);
        _workspaceSplit.FixedPanel = FixedPanel.Panel1;
        _workspaceSplit.IsSplitterFixed = true;
        _workspaceSplit.Location = new Point(20, 5);
        _workspaceSplit.Name = "_workspaceSplit";
        // 
        // _workspaceSplit.Panel1
        // 
        _workspaceSplit.Panel1.BackColor = Color.White;
        _workspaceSplit.Panel1.Controls.Add(calendarTitle);
        // 
        // _workspaceSplit.Panel2
        // 
        _workspaceSplit.Panel2.BackColor = Color.White;
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
        calendarTitle.Size = new Size(121, 23);
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
        // 
        // selectionBar
        // 
        selectionBar.BackColor = Color.White;
        selectionBar.Controls.Add(backupSelectorPanel);
        selectionBar.Controls.Add(_logViewButton);
        selectionBar.Controls.Add(selectionBarActionSpacer);
        selectionBar.Controls.Add(_compareButton);
        selectionBar.Dock = DockStyle.Top;
        selectionBar.Location = new Point(0, 0);
        selectionBar.Name = "selectionBar";
        selectionBar.Padding = new Padding(18, 8, 18, 8);
        selectionBar.Size = new Size(835, 84);
        selectionBar.TabIndex = 1;
        // 
        // backupSelectorPanel
        // 
        backupSelectorPanel.Dock = DockStyle.Fill;
        backupSelectorPanel.Location = new Point(18, 8);
        backupSelectorPanel.Name = "backupSelectorPanel";
        backupSelectorPanel.Size = new Size(617, 68);
        backupSelectorPanel.TabIndex = 0;
        // 
        // _logViewButton
        // 
        _logViewButton.BackColor = Color.White;
        _logViewButton.BorderThickness = 1;
        _logViewButton.ButtonType = StyledButtonType.Secondary;
        _logViewButton.Dock = DockStyle.Right;
        _logViewButton.Font = new Font("맑은 고딕", 9F);
        _logViewButton.ForeColor = Color.FromArgb(28, 32, 41);
        _logViewButton.Location = new Point(635, 8);
        _logViewButton.Margin = new Padding(8, 0, 0, 0);
        _logViewButton.Name = "_logViewButton";
        _logViewButton.Size = new Size(84, 68);
        _logViewButton.TabIndex = 1;
        _logViewButton.Text = "작업 로그";
        // 
        // selectionBarActionSpacer
        // 
        selectionBarActionSpacer.Dock = DockStyle.Right;
        selectionBarActionSpacer.Location = new Point(719, 8);
        selectionBarActionSpacer.Name = "selectionBarActionSpacer";
        selectionBarActionSpacer.Size = new Size(8, 68);
        selectionBarActionSpacer.TabIndex = 2;
        // 
        // _compareButton
        // 
        _compareButton.BackColor = Color.FromArgb(30, 100, 199);
        _compareButton.Dock = DockStyle.Right;
        _compareButton.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        _compareButton.ForeColor = Color.White;
        _compareButton.Location = new Point(727, 8);
        _compareButton.Name = "_compareButton";
        _compareButton.Size = new Size(90, 68);
        _compareButton.TabIndex = 3;
        _compareButton.Text = "XML 비교";
        _compareButton.Click += UiClick_Compare;
        // 
        // bottomBar
        // 
        bottomBar.Controls.Add(_cancelButton);
        bottomBar.Controls.Add(_applyButton);
        bottomBar.Location = new Point(20, 497);
        bottomBar.Name = "bottomBar";
        bottomBar.Padding = new Padding(0, 12, 0, 0);
        bottomBar.Size = new Size(960, 66);
        bottomBar.TabIndex = 1;
        // 
        // _cancelButton
        // 
        _cancelButton.BackColor = Color.FromArgb(233, 238, 245);
        _cancelButton.BorderThickness = 1;
        _cancelButton.ButtonType = StyledButtonType.Secondary;
        _cancelButton.Dock = DockStyle.Right;
        _cancelButton.Font = new Font("맑은 고딕", 9F);
        _cancelButton.ForeColor = Color.FromArgb(28, 32, 41);
        _cancelButton.Location = new Point(700, 12);
        _cancelButton.Margin = new Padding(0, 0, 10, 0);
        _cancelButton.Name = "_cancelButton";
        _cancelButton.Size = new Size(100, 54);
        _cancelButton.TabIndex = 0;
        _cancelButton.Text = "취소";
        _cancelButton.Visible = false;
        // 
        // _applyButton
        // 
        _applyButton.BackColor = Color.FromArgb(30, 100, 199);
        _applyButton.Dock = DockStyle.Right;
        _applyButton.Enabled = false;
        _applyButton.Font = new Font("맑은 고딕", 10F, FontStyle.Bold);
        _applyButton.ForeColor = Color.White;
        _applyButton.Location = new Point(800, 12);
        _applyButton.Name = "_applyButton";
        _applyButton.Size = new Size(160, 54);
        _applyButton.TabIndex = 1;
        _applyButton.Text = "선택 설정 적용";
        _applyButton.Click += UiClick_Apply;
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
