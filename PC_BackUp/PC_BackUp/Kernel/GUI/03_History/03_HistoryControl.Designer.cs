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
    private Panel workspace = null!;
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
        headerTitleLabel = new Label();
        headerDescriptionLabel = new Label();
        workspace = new Panel();
        _workspaceSplit = new SplitContainer();
        calendarTitle = new Label();
        gridHost = new Panel();
        selectionBar = new Panel();
        backupSelectorPanel = new Panel();
        selectionBarActionSpacer = new Panel();
        bottomBar = new Panel();
        _compareButton = new PC_BackUp.StyledButton();
        _applyButton = new PC_BackUp.StyledButton();
        _logViewButton = new StyledButton();
        _cancelButton = new StyledButton();
        SuspendLayout();
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Height = 66;
        headerPanel.Padding = new Padding(8, 2, 0, 0);
        headerTitleLabel.Text = "이력 관리";
        headerTitleLabel.Dock = DockStyle.Top;
        headerTitleLabel.Height = 31;
        headerTitleLabel.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
        headerTitleLabel.ForeColor = ColorRGB.Text;
        headerTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        headerDescriptionLabel.Text = "날짜를 선택해 그날의 백업과 작업 로그를 확인하고, 필요한 항목만 비교/적용합니다.";
        headerDescriptionLabel.Dock = DockStyle.Top;
        headerDescriptionLabel.Height = 24;
        headerDescriptionLabel.Font = new Font("맑은 고딕", 10F);
        headerDescriptionLabel.ForeColor = ColorRGB.MutedText;
        headerDescriptionLabel.TextAlign = ContentAlignment.MiddleLeft;
        headerPanel.Controls.Add(headerDescriptionLabel);
        headerPanel.Controls.Add(headerTitleLabel);
        //
        // _calendar (실제 크기는 ApplyCalendarSizing()에서 화면 표시 시점에 다시 잰다)
        //
        _calendar.Location = new Point(CalendarLeftMargin, CalendarTitleHeight + 4);
        _calendar.MaxSelectionCount = 1;
        _calendar.ShowTodayCircle = true;
        // 캘린더 아래로 남는 빈 공간을 "이번 달 요약"과 "최근 이력 바로가기"로 채운다. 실제
        // 위치/크기는 캘린더의 실제 크기가 확정된 뒤(ApplyCalendarSizing())에 다시 잡는다.
        _calendarStatsLabel.ForeColor = ColorRGB.Text;
        _calendarStatsLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        _calendarStatsLabel.TextAlign = ContentAlignment.TopLeft;
        _calendarStatsLabel.AutoSize = false;
        _calendarRecentTitle.Text = "최근 이력";
        _calendarRecentTitle.ForeColor = ColorRGB.MutedText;
        _calendarRecentTitle.Font = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        _calendarRecentTitle.TextAlign = ContentAlignment.TopLeft;
        _calendarRecentTitle.AutoSize = false;
        _calendarRecentPanel.FlowDirection = FlowDirection.TopDown;
        _calendarRecentPanel.WrapContents = false;
        _calendarRecentPanel.AutoSize = false;
        // _grid 열은 "비교" 화면의 기본값이고, 작업 로그 보기로 전환하면 ConfigureLogColumns()가 갈아 끼운다.
        _grid.BackgroundColor = ColorRGB.Surface;
        _grid.BorderStyle = BorderStyle.None;
        _grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        _grid.GridColor = ColorRGB.Border;
        _grid.RowHeadersVisible = false;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.AllowUserToResizeRows = false;
        _grid.AutoGenerateColumns = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.RowTemplate.Height = 36;
        _grid.ColumnHeadersHeight = 40;
        _grid.ColumnHeadersDefaultCellStyle.BackColor = ColorRGB.GridHeaderBackground;
        _grid.ColumnHeadersDefaultCellStyle.ForeColor = ColorRGB.Text;
        _grid.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        _grid.EnableHeadersVisualStyles = false;
        _grid.DefaultCellStyle.SelectionBackColor = ColorRGB.SidebarActive;
        _grid.DefaultCellStyle.SelectionForeColor = ColorRGB.Text;
        _grid.Dock = DockStyle.Fill;
        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "No.",
            Width = 45,
            ReadOnly = true,
            SortMode = DataGridViewColumnSortMode.NotSortable,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter },
            HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
        });
        _grid.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "적용", DataPropertyName = nameof(XmlDifference.Apply), Width = 78 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "XML 파일", DataPropertyName = nameof(XmlDifference.RelativeFilePath), Width = 340, ReadOnly = true });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "현재", DataPropertyName = nameof(XmlDifference.CurrentValue), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "백업", DataPropertyName = nameof(XmlDifference.BackupValue), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
        //
        // gridHost
        //
        gridHost.Dock = DockStyle.Fill;
        gridHost.BackColor = ColorRGB.Surface;
        _emptyStateLabel.Text = "비교할 백업을 선택한 뒤 XML 비교를 실행하세요.";
        _emptyStateLabel.Dock = DockStyle.Fill;
        _emptyStateLabel.ForeColor = ColorRGB.MutedText;
        _emptyStateLabel.Font = new Font("맑은 고딕", 10F);
        _emptyStateLabel.TextAlign = ContentAlignment.MiddleCenter;
        gridHost.Controls.Add(_grid);
        gridHost.Controls.Add(_emptyStateLabel);
        //
        // selectionBar (비교 기준/대상 선택 + 작업 로그/XML 비교 버튼)
        // Source/Destination을 좌우 절반으로 나누지 않고 각각 전체 폭을 쓰는 한 줄씩(2행)으로
        // 배치해서 콤보에 표시되는 백업 정보("날짜 | 종류 | 파일명")가 잘리지 않게 한다.
        //
        selectionBar.Dock = DockStyle.Top;
        selectionBar.Height = 84;
        selectionBar.BackColor = ColorRGB.Surface;
        selectionBar.Padding = new Padding(18, 8, 18, 8);
        //
        // Source(비교 기준) 행 — 기본은 "현재 적용된 파일"(체크박스), 해제하면 같은 날짜의
        // 다른 백업을 콤보에서 고를 수 있다. 라벨/체크박스는 왼쪽에 고정 폭으로 두고 콤보가
        // 나머지 폭을 전부 쓴다.
        //
        _sourceComboLabel.Text = "Source :";
        _sourceComboLabel.Dock = DockStyle.Left;
        _sourceComboLabel.Width = 62;
        _sourceComboLabel.TextAlign = ContentAlignment.MiddleLeft;
        _sourceComboLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        _sourceComboLabel.ForeColor = ColorRGB.Text;
        _sourceIsCurrentCheckBox.Text = "현재 적용된 파일 기준";
        _sourceIsCurrentCheckBox.Dock = DockStyle.Left;
        _sourceIsCurrentCheckBox.Width = 150;
        _sourceIsCurrentCheckBox.Checked = true;
        _sourceIsCurrentCheckBox.ForeColor = ColorRGB.Text;
        _sourceIsCurrentCheckBox.CheckedChanged += UiChange_SourceIsCurrent;
        _sourceCombo.Dock = DockStyle.Fill;
        _sourceCombo.Height = 26;
        _sourceCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        _sourceCombo.Font = new Font("맑은 고딕", 9.5F);
        _sourceCombo.Enabled = false;
        _sourceRow.Dock = DockStyle.Top;
        _sourceRow.Height = 30;
        _sourceRow.Padding = new Padding(0, 0, 0, 4);
        _sourceRow.Controls.Add(_sourceCombo);
        _sourceRow.Controls.Add(_sourceIsCurrentCheckBox);
        _sourceRow.Controls.Add(_sourceComboLabel);
        //
        // Destination(비교 대상) 행 — 지금까지와 동일하게 콤보에서 백업을 고른다.
        //
        _backupComboLabel.Text = "Dest :";
        _backupComboLabel.Dock = DockStyle.Left;
        _backupComboLabel.Width = 62;
        _backupComboLabel.TextAlign = ContentAlignment.MiddleLeft;
        _backupComboLabel.Font = new Font("맑은 고딕", 9F, FontStyle.Bold);
        _backupComboLabel.ForeColor = ColorRGB.Text;
        _backupCombo.Dock = DockStyle.Fill;
        _backupCombo.Height = 26;
        _backupCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        _backupCombo.Font = new Font("맑은 고딕", 9.5F);
        _destinationRow.Dock = DockStyle.Top;
        _destinationRow.Height = 30;
        _destinationRow.Controls.Add(_backupCombo);
        _destinationRow.Controls.Add(_backupComboLabel);

        _compareButton.Text = "XML 비교";
        _compareButton.Size = new Size(90, 32);
        _compareButton.Dock = DockStyle.Right;
        _compareButton.Click += UiClick_Compare;
        _logViewButton.ButtonType = StyledButtonType.Secondary;
        _logViewButton.Text = "작업 로그";
        _logViewButton.Dock = DockStyle.Right;
        _logViewButton.Width = 84;
        _logViewButton.Margin = new Padding(8, 0, 0, 0);
        _logViewButton.NormalBackColor = ColorRGB.Surface;
        _logViewButton.ForeColor = ColorRGB.Text;
        _logViewButton.BorderColor = ColorRGB.Border;
        selectionBarActionSpacer.Dock = DockStyle.Right;
        selectionBarActionSpacer.Width = 8;
        backupSelectorPanel.Dock = DockStyle.Fill;
        backupSelectorPanel.Controls.Add(_destinationRow);
        backupSelectorPanel.Controls.Add(_sourceRow);
        selectionBar.Controls.Add(backupSelectorPanel);
        selectionBar.Controls.Add(_logViewButton);
        selectionBar.Controls.Add(selectionBarActionSpacer);
        selectionBar.Controls.Add(_compareButton);
        //
        // _workspaceSplit (왼쪽: 달력, 오른쪽: 비교 대상 선택 + 목록)
        //
        _workspaceSplit.Dock = DockStyle.Fill;
        _workspaceSplit.SplitterDistance = 280;
        _workspaceSplit.IsSplitterFixed = true;
        _workspaceSplit.FixedPanel = FixedPanel.Panel1;
        _workspaceSplit.BackColor = ColorRGB.Border;
        calendarTitle.Text = "이력 날짜";
        calendarTitle.Dock = DockStyle.Top;
        calendarTitle.Height = CalendarTitleHeight;
        calendarTitle.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
        calendarTitle.ForeColor = ColorRGB.Text;
        _workspaceSplit.Panel1.BackColor = ColorRGB.Surface;
        _workspaceSplit.Panel1.Padding = new Padding(CalendarLeftMargin, 0, 10, 0);
        _workspaceSplit.Panel1.Controls.Add(_calendar);
        _workspaceSplit.Panel1.Controls.Add(_calendarStatsLabel);
        _workspaceSplit.Panel1.Controls.Add(_calendarRecentTitle);
        _workspaceSplit.Panel1.Controls.Add(_calendarRecentPanel);
        _workspaceSplit.Panel1.Controls.Add(calendarTitle);
        _workspaceSplit.Panel2.BackColor = ColorRGB.Surface;
        _workspaceSplit.Panel2.Controls.Add(gridHost);
        _workspaceSplit.Panel2.Controls.Add(selectionBar);
        //
        // workspace (요약 카드 행은 HistoryControl() 생성자에서 마지막에 얹는다)
        //
        workspace.Dock = DockStyle.Fill;
        workspace.BackColor = ColorRGB.Surface;
        workspace.Controls.Add(_workspaceSplit);
        //
        // bottomBar (전체 선택/취소/설정 적용 버튼 + 진행 바)
        //
        bottomBar.Dock = DockStyle.Bottom;
        bottomBar.Height = 66;
        bottomBar.Padding = new Padding(0, 12, 0, 0);
        _cancelButton.ButtonType = StyledButtonType.Secondary;
        _cancelButton.Text = "취소";
        _cancelButton.Dock = DockStyle.Right;
        _cancelButton.Width = 100;
        _cancelButton.Margin = new Padding(0, 0, 10, 0);
        _cancelButton.Visible = false;
        _summaryLabel.Dock = DockStyle.Fill;
        _summaryLabel.Padding = new Padding(18, 0, 0, 0);
        _summaryLabel.TextAlign = ContentAlignment.MiddleLeft;
        _summaryLabel.ForeColor = ColorRGB.MutedText;
        _summaryLabel.Text = "백업을 선택하고 XML 비교를 실행하세요.";
        _applyButton.Text = "선택 설정 적용";
        _applyButton.Size = new Size(160, 42);
        _applyButton.Dock = DockStyle.Right;
        _applyButton.Enabled = false;
        _applyButton.Click += UiClick_Apply;
        _progress.Dock = DockStyle.Top;
        _progress.Height = 6;
        bottomBar.Controls.Add(_summaryLabel);
        bottomBar.Controls.Add(_cancelButton);
        bottomBar.Controls.Add(_applyButton);
        bottomBar.Controls.Add(_progress);
        //
        // HistoryControl
        //
        Controls.Add(workspace);
        Controls.Add(bottomBar);
        Controls.Add(headerPanel);
        Name = "HistoryControl";
        Size = new Size(1000, 700);
        ResumeLayout(false);
    }
}
