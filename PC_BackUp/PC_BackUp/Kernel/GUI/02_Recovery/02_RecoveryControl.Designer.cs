namespace PC_BackUp;

partial class RecoveryControl
{
    private System.ComponentModel.IContainer? components;

    // 화면의 고정 골격 — 값이 바뀌지 않는 부분이라 전부 필드로 선언해 Visual Studio
    // 디자이너에서도 그대로 열어서 확인할 수 있게 한다. 아래 두 가지는 예외로 코드에만 남긴다:
    //  1) 상단 요약 카드의 원형 아이콘(IconGlyphs) — GDI+로 직접 그리는 Paint 이벤트라 디자이너가
    //     표현할 수 없다. RecoveryControl() 생성자에서 InitializeComponent() 이후에 붙인다.
    //  2) MonthCalendar의 실제 크기(ApplyCalendarSizing) — DPI에 따라 화면에 실제로 표시된 뒤에만
    //     정확히 잴 수 있어서 RefreshCatalog()에서 매번 다시 계산한다.
    private Panel headerPanel = null!;
    private Label headerTitleLabel = null!;
    private Label headerDescriptionLabel = null!;
    private Panel workspace = null!;
    private Label calendarTitle = null!;
    private Panel detailsPanel = null!;
    private Panel selectedBackupCardPanel = null!;
    private Panel detailsGap = null!;
    private Panel actionPanel = null!;

    private readonly MonthCalendar _calendar = new();
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
        headerTitleLabel = new Label();
        headerDescriptionLabel = new Label();
        workspace = new Panel();
        _workspaceSplit = new SplitContainer();
        calendarTitle = new Label();
        detailsPanel = new Panel();
        selectedBackupCardPanel = new Panel();
        detailsGap = new Panel();
        actionPanel = new Panel();
        _restoreButton = new PC_BackUp.StyledButton();
        _cancelButton = new PC_BackUp.StyledButton();
        SuspendLayout();
        headerPanel.Dock = DockStyle.Top;
        headerPanel.Height = 66;
        headerPanel.Padding = new Padding(8, 2, 0, 0);
        headerTitleLabel.Text = "복원";
        headerTitleLabel.Dock = DockStyle.Top;
        headerTitleLabel.Height = 31;
        headerTitleLabel.Font = new Font("맑은 고딕", 20F, FontStyle.Bold);
        headerTitleLabel.ForeColor = ColorRGB.Text;
        headerTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
        headerDescriptionLabel.Text = "백업 시점을 선택하고 안전하게 복원하세요.";
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
        _grid.ReadOnly = true;
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "시간", DataPropertyName = nameof(BackupRecord.TimeText), Width = 100 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "백업 형태", DataPropertyName = nameof(BackupRecord.KindText), Width = 130 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "파일명", DataPropertyName = nameof(BackupRecord.FileName), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "크기", DataPropertyName = nameof(BackupRecord.SizeText), Width = 100 });

        //
        // workspace (달력 + 목록 + 상세정보 영역을 담는 바깥 패널)
        //
        workspace.Dock = DockStyle.Fill;
        workspace.BackColor = ColorRGB.Surface;
        workspace.Padding = new Padding(16);
        //
        // _workspaceSplit (왼쪽: 달력, 오른쪽: 백업 목록)
        //
        _workspaceSplit.Dock = DockStyle.Fill;
        _workspaceSplit.SplitterDistance = 280;
        _workspaceSplit.IsSplitterFixed = true;
        _workspaceSplit.FixedPanel = FixedPanel.Panel1;
        _workspaceSplit.BackColor = ColorRGB.Border;
        calendarTitle.Text = "백업 날짜";
        calendarTitle.Dock = DockStyle.Top;
        calendarTitle.Height = CalendarTitleHeight;
        calendarTitle.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
        calendarTitle.ForeColor = ColorRGB.Text;
        _workspaceSplit.Panel1.BackColor = ColorRGB.Surface;
        _workspaceSplit.Panel1.Padding = new Padding(CalendarLeftMargin, 0, 10, 0);
        _workspaceSplit.Panel1.Controls.Add(_calendar);
        _workspaceSplit.Panel1.Controls.Add(calendarTitle);

        _dateLabel.Dock = DockStyle.Top;
        _dateLabel.Height = 36;
        _dateLabel.Font = new Font("맑은 고딕", 11F, FontStyle.Bold);
        _dateLabel.ForeColor = ColorRGB.Text;
        _emptyLabel.Text = "선택한 날짜에 백업 기록이 없습니다.";
        _emptyLabel.Dock = DockStyle.Fill;
        _emptyLabel.ForeColor = ColorRGB.MutedText;
        _emptyLabel.TextAlign = ContentAlignment.MiddleCenter;
        _workspaceSplit.Panel2.BackColor = ColorRGB.Surface;
        _workspaceSplit.Panel2.Padding = new Padding(18, 0, 0, 0);
        _workspaceSplit.Panel2.Controls.Add(_grid);
        _workspaceSplit.Panel2.Controls.Add(_emptyLabel);
        _workspaceSplit.Panel2.Controls.Add(_dateLabel);
        //
        // detailsPanel (선택한 백업 정보 카드 — 안전 안내 박스는 아이콘 때문에 코드에서 덧붙인다)
        //
        detailsPanel.Dock = DockStyle.Bottom;
        detailsPanel.Height = 260;
        selectedBackupCardPanel.BackColor = ColorRGB.DetailPanelBackground;
        selectedBackupCardPanel.Padding = new Padding(16, 10, 16, 10);
        selectedBackupCardPanel.Dock = DockStyle.Fill;
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
        _autoSafetyBackupCheckBox.Text = "복원 전 현재 상태를 자동 ZIP 백업";
        _autoSafetyBackupCheckBox.Dock = DockStyle.Top;
        _autoSafetyBackupCheckBox.Height = 26;
        _autoSafetyBackupCheckBox.ForeColor = ColorRGB.Text;
        _autoSafetyBackupCheckBox.Checked = false;
        _autoSafetyBackupCheckBox.CheckedChanged += AutoSafetyBackupCheckBox_CheckedChanged;
        _compareBeforeRestoreCheckBox.Text = "복원 전 현재 파일과 비교";
        _compareBeforeRestoreCheckBox.Dock = DockStyle.Top;
        _compareBeforeRestoreCheckBox.Height = 26;
        _compareBeforeRestoreCheckBox.ForeColor = ColorRGB.Text;
        _compareBeforeRestoreCheckBox.Checked = false;
        _compareBeforeRestoreCheckBox.CheckedChanged += CompareBeforeRestoreCheckBox_CheckedChanged;
        selectedBackupCardPanel.Controls.Add(_autoSafetyBackupCheckBox);
        selectedBackupCardPanel.Controls.Add(_compareBeforeRestoreCheckBox);
        selectedBackupCardPanel.Controls.Add(_comparisonListBox);
        selectedBackupCardPanel.Controls.Add(_comparisonResultLabel);
        selectedBackupCardPanel.Controls.Add(_restoreTargetLabel);
        selectedBackupCardPanel.Controls.Add(_selectedBackupMetaLabel);
        selectedBackupCardPanel.Controls.Add(_selectedBackupLabel);
        detailsGap.Dock = DockStyle.Right;
        detailsGap.Width = 16;
        detailsPanel.Controls.Add(selectedBackupCardPanel);
        detailsPanel.Controls.Add(detailsGap);
        // (안전 안내 카드는 RecoveryControl() 생성자에서 detailsPanel.Controls에 마지막으로 추가된다)
        //
        // workspace에 쌓기 (요약 카드 행은 RecoveryControl() 생성자에서 마지막에 얹는다)
        //
        workspace.Controls.Add(_workspaceSplit);
        workspace.Controls.Add(detailsPanel);
        //
        // actionPanel (복원/취소 버튼 + 진행 바)
        //
        actionPanel.Dock = DockStyle.Bottom;
        actionPanel.Height = 66;
        actionPanel.Padding = new Padding(0, 12, 0, 0);
        _progress.Dock = DockStyle.Top;
        _progress.Height = 6;
        _cancelButton.ButtonType = StyledButtonType.Secondary;
        _cancelButton.Text = "취소";
        _cancelButton.Dock = DockStyle.Right;
        _cancelButton.Width = 100;
        _cancelButton.Margin = new Padding(0, 0, 10, 0);
        _cancelButton.Visible = false;
        _restoreButton.Text = "선택 백업 복원";
        _restoreButton.Width = 180;
        _restoreButton.Height = 42;
        _restoreButton.Dock = DockStyle.Right;
        _restoreButton.Click += RestoreButton_Click;
        actionPanel.Controls.Add(_cancelButton);
        actionPanel.Controls.Add(_restoreButton);
        actionPanel.Controls.Add(_progress);
        //
        // RecoveryControl
        //
        Controls.Add(workspace);
        Controls.Add(actionPanel);
        Controls.Add(headerPanel);
        Name = "RecoveryControl";
        Size = new Size(1000, 700);
        ResumeLayout(false);
    }
}
