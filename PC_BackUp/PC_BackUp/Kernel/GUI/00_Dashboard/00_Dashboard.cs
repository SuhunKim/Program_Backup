using PC_BackUp.Services;

namespace PC_BackUp.Kernel.GUI._00_Dashboard;

/// <summary>
/// 백업 현황을 요약하고 기존 백업·복원·이력·환경 설정 화면으로 이동시키는 시작 화면이다.
/// </summary>
public partial class _00_Dashboard : UserControlBase
{
    private readonly ISettingsService? m_oSettingsService;
    private readonly BackupCatalogService? m_oCatalogService;
    private readonly Action<MainMenuId>? m_oNavigate;
    private readonly Action<BackupRecord?>? m_oOpenRecovery;
    private BackupRecord? m_oSelectedRecentRecord;
    public _00_Dashboard()
    {
        InitializeComponent();
    }

    public _00_Dashboard(ISettingsService settingsService, BackupCatalogService catalogService,
        Action<MainMenuId> navigate, Action<BackupRecord?> openRecovery) : this()
    {
        m_oSettingsService = settingsService;
        m_oCatalogService = catalogService;
        m_oNavigate = navigate;
        m_oOpenRecovery = openRecovery;
        m_oRecentGrid.CellClick += UiClick_RecentGrid;
    }

    public override void OnMenuSelected() => RefreshDashboard();

    private void RefreshDashboard()
    {
        if (m_oSettingsService is null || m_oCatalogService is null)
        {
            SetUnavailableState("대시보드 서비스를 초기화하지 못했습니다.");
            return;
        }

        try
        {
            var settings = m_oSettingsService.Load();
            var records = m_oCatalogService.Refresh(settings.BackupRootPath).Values.SelectMany(value => value)
                .OrderByDescending(record => record.CreatedAt).ToList();
            var latest = records.FirstOrDefault();
            var errors = settings.Validate();
            _summaryDateValue.Text = latest is null ? "백업 없음" : latest.CreatedAt.ToString("yyyy-MM-dd HH:mm");
            _summaryCountValue.Text = string.Format("{0:N0}건", records.Count);
            _summarySizeValue.Text = BackupRecord.FormatBytes(records.Sum(record => record.SizeBytes));
            m_oSelectedRecentRecord = null;
            m_oRecentGrid.DataSource = records.Take(5).ToList();
            m_oRecentGrid.ClearSelection();
            _summaryStatusValue.Text = errors.Count == 0 ? "정상" : "확인 필요";
            m_oWarningCard.Visible = errors.Count > 0;
            m_oWarningLabel.Text = string.Join("  ·  ", errors);
        }
        catch (Exception exception)
        {
            SetUnavailableState(string.Format("대시보드 정보를 불러오지 못했습니다. {0}", exception.Message));
        }
    }

    private void SetUnavailableState(string message)
    {
        _summaryDateValue.Text = "확인 불가";
        _summaryCountValue.Text = "-";
        _summarySizeValue.Text = "-";
        _summaryStatusValue.Text = "확인 필요";
        m_oRecentGrid.DataSource = null;
        m_oWarningLabel.Text = message;
        m_oWarningCard.Visible = true;
    }

    // [Codex - 2026.09.15] 디자이너에서 연결한 빠른 작업 버튼은 전용 화면으로만 이동시킨다.
    private void UiClick_Backup(object? sender, EventArgs e) => Navigate(MainMenuId.Backup);

    private void UiClick_Recovery(object? sender, EventArgs e) => m_oOpenRecovery?.Invoke(m_oSelectedRecentRecord);

    private void UiClick_History(object? sender, EventArgs e) => Navigate(MainMenuId.History);

    private void UiClick_Settings(object? sender, EventArgs e) => Navigate(MainMenuId.Settings);

    // [Codex - 2026.09.15] 목록을 사용자가 클릭한 경우에만 복원 화면으로 전달할 대상을 기억한다.
    private void UiClick_RecentGrid(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            m_oSelectedRecentRecord = m_oRecentGrid.Rows[e.RowIndex].DataBoundItem as BackupRecord;
    }

    private void Navigate(MainMenuId id) => m_oNavigate?.Invoke(id);

}
