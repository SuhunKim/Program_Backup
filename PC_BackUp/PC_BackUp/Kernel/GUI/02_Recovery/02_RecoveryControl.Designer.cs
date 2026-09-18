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
    private CardPanel workspace = null!;
    private Label calendarTitle = null!;
    private Panel detailsPanel = null!;
    private CardPanel selectedBackupCardPanel = null!;
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
        headerDescriptionLabel = new Label();
        headerTitleLabel = new Label();
        workspace = new CardPanel();
        _workspaceSplit = new SplitContainer();
        calendarTitle = new Label();
        detailsPanel = new Panel();
        selectedBackupCardPanel = new CardPanel();
        detailsGap = new Panel();
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
        workspace.Controls.Add(_workspaceSplit);
        workspace.Controls.Add(actionPanel);
        workspace.Controls.Add(detailsPanel);
        workspace.Location = new Point(0, 100);
        workspace.Name = "workspace";
        workspace.Padding = new Padding(16);
        workspace.Size = new Size(1000, 574);
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
        _workspaceSplit.Panel2.Padding = new Padding(18, 0, 0, 0);
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
        calendarTitle.Size = new Size(121, 23);
        calendarTitle.TabIndex = 0;
        calendarTitle.Text = "백업 날짜";
        // 
        // detailsPanel
        // 
        detailsPanel.Controls.Add(selectedBackupCardPanel);
        detailsPanel.Controls.Add(detailsGap);
        detailsPanel.Location = new Point(20, 317);
        detailsPanel.Name = "detailsPanel";
        detailsPanel.Size = new Size(968, 174);
        detailsPanel.TabIndex = 1;
        // 
        // selectedBackupCardPanel
        // 
        selectedBackupCardPanel.BackColor = Color.FromArgb(247, 248, 250);
        selectedBackupCardPanel.Dock = DockStyle.Fill;
        selectedBackupCardPanel.Location = new Point(0, 0);
        selectedBackupCardPanel.Name = "selectedBackupCardPanel";
        selectedBackupCardPanel.Padding = new Padding(16, 10, 16, 10);
        selectedBackupCardPanel.Size = new Size(952, 174);
        selectedBackupCardPanel.TabIndex = 0;
        // 
        // detailsGap
        // 
        detailsGap.Dock = DockStyle.Right;
        detailsGap.Location = new Point(952, 0);
        detailsGap.Name = "detailsGap";
        detailsGap.Size = new Size(16, 174);
        detailsGap.TabIndex = 1;
        // 
        // actionPanel
        // 
        actionPanel.Controls.Add(_cancelButton);
        actionPanel.Controls.Add(_restoreButton);
        actionPanel.Location = new Point(20, 497);
        actionPanel.Name = "actionPanel";
        actionPanel.Padding = new Padding(0, 12, 0, 0);
        actionPanel.Size = new Size(960, 66);
        actionPanel.TabIndex = 1;
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
