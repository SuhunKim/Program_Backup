using PC_BackUp.Services;

namespace PC_BackUp;

public partial class HistoryControl : UserControlBase
{
    private ISettingsService? m_oSettingsService;
    private BackupCatalogService? m_oCatalogService;
    private XmlComparisonService? m_oComparisonService;
    private LogManager? m_oLoggingService;
    private bool m_bIsBusy;
    private bool m_bIsLogView;
    private CancellationTokenSource? m_oCancellationTokenSource;
    private List<BackupRecord> m_oAllRecords = new();
    private IReadOnlyList<LogEntry> m_oAllLogEntries = Array.Empty<LogEntry>();

    public HistoryControl()
    {
        InitializeComponent();

        // 아이콘 배지(IconGlyphs)는 GDI+ Paint 이벤트로 그려서 디자이너가 표현할 수 없는
        // 부분이라, 디자이너가 그려둔 고정 골격(workspace)에 여기서 덧붙인다.
        workspace.Controls.Add(BuildSummaryRow());

    _calendar.DateSelected += (_, eventArgs) => SelectDate(eventArgs.Start);
    _grid.DataBindingComplete += (_, _) => _emptyStateLabel.Visible = _grid.Rows.Count == 0;

    _logViewButton.Click += (_, _) => ToggleLogView();
    _selectAllButton.Click += (_, _) => ToggleAll();
    _cancelButton.Click += (_, _) => m_oCancellationTokenSource?.Cancel();

  }

	public HistoryControl(
        ISettingsService settingsService,
        BackupCatalogService catalogService,
        XmlComparisonService comparisonService,
        LogManager loggingService) : this()
    {
        m_oSettingsService = settingsService;
        m_oCatalogService = catalogService;
        m_oComparisonService = comparisonService;
        m_oLoggingService = loggingService;
    }

    public override void OnMenuSelected() => RefreshBackupList();

    /// <summary>상단 "선택된 이력 날짜 / 이력 요약" 카드 두 개 — 원형 아이콘을 코드로 그려야 해서 디자이너로 옮기지 못했다.</summary>
    private Control BuildSummaryRow()
    {
        var row = new Panel { Dock = DockStyle.Top, Height = 92, Padding = new Padding(16, 16, 16, 16) };
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

        _summaryDateCaption.Text = "선택된 이력 날짜";
        var dateIcon = IconGlyphs.CreateBadge(36, ColorRGB.SidebarActive, ColorRGB.Primary, IconGlyphs.Calendar);
        dateIcon.BackColor = ColorRGB.Surface;
        var dateCard = ColorRGB.CreateStatCard(dateIcon, _summaryDateCaption, _summaryDateValue);
        dateCard.Dock = DockStyle.Fill;
        dateCard.Margin = new Padding(0, 0, 8, 0);

        _summaryCountCaption.Text = "이력 요약";
        var countIcon = IconGlyphs.CreateBadge(36, ColorRGB.SidebarActive, ColorRGB.Primary, IconGlyphs.Archive);
        countIcon.BackColor = ColorRGB.Surface;
        var countCard = ColorRGB.CreateStatCard(countIcon, _summaryCountCaption, _summaryCountValue);
        countCard.Dock = DockStyle.Fill;
        countCard.Margin = new Padding(8, 0, 0, 0);

        layout.Controls.Add(dateCard, 0, 0);
        layout.Controls.Add(countCard, 1, 0);
        row.Controls.Add(layout);
        return row;
    }

    private void ConfigureCompareColumns()
    {
        _grid.Columns.Clear();
        _grid.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "적용", DataPropertyName = nameof(XmlDifference.Apply), Width = 58 });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "XML 파일", DataPropertyName = nameof(XmlDifference.RelativeFilePath), Width = 340, ReadOnly = true });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "현재", DataPropertyName = nameof(XmlDifference.CurrentValue), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "백업", DataPropertyName = nameof(XmlDifference.BackupValue), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
    }

    private void ConfigureLogColumns()
    {
        _grid.Columns.Clear();
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "시간", DataPropertyName = nameof(LogEntry.TimestampText), Width = 150, ReadOnly = true });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "레벨", DataPropertyName = nameof(LogEntry.Level), Width = 80, ReadOnly = true });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "메시지", DataPropertyName = nameof(LogEntry.Message), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
    }

    /// <summary>
    /// MonthCalendar가 실제 화면에 붙어 핸들이 만들어진 뒤에야 PreferredSize가 정확해지므로,
    /// 화면이 표시될 때마다(= 이 화면으로 올 때마다) 실제 크기를 다시 재서 패널 폭에 반영한다.
    /// 생성 시점에 미리 계산해 두면(=핸들이 없을 때) 한 달 격자가 깨져서 요일/날짜 줄이 겹쳐 보인다.
    /// </summary>
    private void ApplyCalendarSizing()
    {
        _calendar.Size = _calendar.PreferredSize;
        var panel1Width = _calendar.Width + CalendarLeftMargin + 20;
        if (_workspaceSplit.SplitterDistance != panel1Width)
            _workspaceSplit.SplitterDistance = panel1Width;
    }

    private void RefreshBackupList()
    {
        if (m_oSettingsService is null || m_oCatalogService is null) return;
        ApplyCalendarSizing();
        var settings = m_oSettingsService.Load();
        m_oAllRecords = m_oCatalogService.Refresh(settings.BackupRootPath).Values
            .SelectMany(items => items)
            .OrderByDescending(item => item.CreatedAt)
            .ToList();
        m_oAllLogEntries = m_oLoggingService?.ReadRecent(int.MaxValue) ?? Array.Empty<LogEntry>();

        var datesWithHistory = m_oAllRecords.Select(record => record.CreatedAt.Date)
            .Union(m_oAllLogEntries.Select(entry => entry.Timestamp.Date))
            .Distinct()
            .ToArray();
        _calendar.BoldedDates = datesWithHistory;
        _calendar.UpdateBoldedDates();

        SelectDate(_calendar.SelectionStart);
    }

    /// <summary>
    /// 달력에서 날짜를 고르면 그날의 백업 개수/로그 개수를 요약하고, 비교 대상 콤보박스를
    /// 그날의 백업으로만 채운다. 작업 로그 보기 상태라면 로그 목록도 그날 것으로 갈아 끼운다.
    /// </summary>
    private void SelectDate(DateTime date)
    {
        var dayRecords = m_oAllRecords.Where(record => record.CreatedAt.Date == date.Date).ToList();
        var dayLogCount = m_oAllLogEntries.Count(entry => entry.Timestamp.Date == date.Date);
        _summaryDateValue.Text = string.Format("{0:yyyy년 M월 d일}", date);
        _summaryCountValue.Text = string.Format("백업 {0}건  ·  로그 {1}건", dayRecords.Count, dayLogCount);

        _backupCombo.DataSource = null;
        _backupCombo.DataSource = dayRecords;
        // Source 콤보(백업끼리 비교할 때)도 같은 날짜의 백업 목록을 공유한다.
        _sourceCombo.DataSource = null;
        _sourceCombo.DataSource = dayRecords;
        SetEmptyState(dayRecords.Count == 0 ? "선택한 날짜에는 백업 이력이 없습니다." : string.Empty);
        _compareButton.Enabled = !m_bIsBusy && dayRecords.Count > 0;

        if (m_bIsLogView)
            ShowLogEntries(date);
    }

    private void ShowLogEntries(DateTime date)
    {
        var entries = m_oAllLogEntries.Where(entry => entry.Timestamp.Date == date.Date).ToList();
        _grid.DataSource = entries;
        SetEmptyState(entries.Count == 0 ? "이 날짜에 기록된 작업 로그가 없습니다." : string.Empty);
        _summaryLabel.Text = entries.Count == 0
            ? "이 날짜에 로그가 없습니다."
            : string.Format("{0:yyyy년 M월 d일} 로그 {1:N0}건", date, entries.Count);
    }

    private async void UiClick_Compare(object? sender, EventArgs e)
    {
        if (m_oSettingsService is null || m_oComparisonService is null || m_bIsBusy) return;
        if (_backupCombo.SelectedItem is not BackupRecord destinationRecord)
            return;

        var sourceIsCurrent = _sourceIsCurrentCheckBox.Checked;
        BackupRecord? sourceRecord = null;
        if (!sourceIsCurrent)
        {
            if (_sourceCombo.SelectedItem is not BackupRecord selectedSource)
            {
                MessageBox.Show(this, "비교 기준(Source)으로 사용할 백업을 선택하세요.", "선택 필요", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            sourceRecord = selectedSource;
        }

        m_bIsBusy = true;
        SetBusy(true, "XML 설정을 비교하고 있습니다...");
        m_oCancellationTokenSource = new CancellationTokenSource();
        try
        {
            var progress = new Progress<int>(value => _progress.Value = value);
            var differences = await m_oComparisonService.CompareAsync(m_oSettingsService.Load(), sourceRecord, destinationRecord, progress, m_oCancellationTokenSource.Token);
            _grid.DataSource = differences.ToList();
            UpdateCompareColumnHeaders(sourceIsCurrent ? "현재" : sourceRecord!.FileName, destinationRecord.FileName);
            SetEmptyState(differences.Count == 0 ? "Source와 Destination 설정이 같습니다." : string.Empty);
            _summaryLabel.Text = differences.Count == 0
                ? "Source와 Destination 설정이 같습니다."
                : string.Format("차이점 {0:N0}개를 찾았습니다.", differences.Count);
            UpdateApplyAvailability();
            m_oLoggingService?.LogInfo(string.Format(
                "XML 비교 완료: 차이점 {0}개 (Source: {1}, Destination: {2})",
                differences.Count, sourceIsCurrent ? "현재" : sourceRecord!.FileName, destinationRecord.FileName));
        }
        catch (Exception exception)
        {
            m_oLoggingService?.LogError("XML 비교 중 오류가 발생했습니다.", exception);
            MessageBox.Show(this, exception.Message, "비교 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _summaryLabel.Text = "비교 중 오류가 발생했습니다.";
        }
        finally
        {
            m_bIsBusy = false;
            SetBusy(false, _summaryLabel.Text);
            m_oCancellationTokenSource?.Dispose();
            m_oCancellationTokenSource = null;
        }
    }

    /// <summary>
    /// Source가 "현재"가 아니라 다른 백업이면(백업끼리 비교) 적용 대상이 모호해지므로 —
    /// Apply는 항상 Destination 값을 "현재 실행 파일"에 쓰는 동작이라 Source가 현재일 때만
    /// 의미가 있다 — Apply 버튼을 비활성화해서 조회 전용으로 둔다.
    /// </summary>
    private void UpdateApplyAvailability()
    {
        var hasDifferences = _grid.DataSource is List<XmlDifference> items && items.Count > 0;
        _applyButton.Enabled = hasDifferences && _sourceIsCurrentCheckBox.Checked;
    }

    /// <summary>ConfigureCompareColumns()가 고정 순서(적용/XML 파일/현재/백업)로 만든 열 헤더를
    /// 실제 선택된 Source/Destination 이름으로 갱신한다.</summary>
    private void UpdateCompareColumnHeaders(string sourceLabel, string destinationLabel)
    {
        if (_grid.Columns.Count < 4) return;
        _grid.Columns[2].HeaderText = string.Format("Source ({0})", sourceLabel);
        _grid.Columns[3].HeaderText = string.Format("Destination ({0})", destinationLabel);
    }

    private void UiChange_SourceIsCurrent(object? sender, EventArgs e)
    {
        _sourceCombo.Enabled = !_sourceIsCurrentCheckBox.Checked;
        UpdateApplyAvailability();
    }

    private async void UiClick_Apply(object? sender, EventArgs e)
    {
        if (m_oSettingsService is null || m_oComparisonService is null || m_bIsBusy) return;
        if (!_sourceIsCurrentCheckBox.Checked)
            return; // Source가 현재가 아니면(백업끼리 비교) Apply는 의미가 없다 — 버튼도 비활성화되어 있다.
        _grid.EndEdit();
        var selected = (_grid.DataSource as List<XmlDifference>)?
            .Where(item => item.Apply)
            .ToList() ?? new List<XmlDifference>();
        if (selected.Count == 0)
        {
            MessageBox.Show(this, "적용할 항목의 체크박스를 선택하세요.", "선택 필요", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (_backupCombo.SelectedItem is not BackupRecord record)
            return;
        var confirm = MessageBox.Show(this, string.Format("선택한 XML 파일 {0}개를 백업본으로 교체할까요?", selected.Count),
            "부분 복원 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        if (confirm != DialogResult.Yes)
            return;

        m_bIsBusy = true;
        SetBusy(true, "선택한 설정을 적용하고 있습니다...");
        m_oCancellationTokenSource = new CancellationTokenSource();
        try
        {
            var progress = new Progress<int>(value => _progress.Value = value);
            var result = await m_oComparisonService.ApplySelectedAsync(m_oSettingsService.Load().GetSourceRoot(), record, selected, progress, m_oCancellationTokenSource.Token);
            if (result.Succeeded)
                m_oLoggingService?.LogInfo(string.Format("설정 적용: {0}", result.Message));
            else
                m_oLoggingService?.LogError(string.Format("설정 적용 실패: {0}", result.Message));
            MessageBox.Show(this, result.Message, result.Succeeded ? "설정 적용" : "적용 실패", MessageBoxButtons.OK,
                result.Succeeded ? MessageBoxIcon.Information : MessageBoxIcon.Error);
            _summaryLabel.Text = result.Message.Replace(Environment.NewLine, "  ");
        }
        catch (Exception exception)
        {
            m_oLoggingService?.LogError("설정 적용 중 예외가 발생했습니다.", exception);
            MessageBox.Show(this, exception.Message, "적용 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _summaryLabel.Text = "적용 중 오류가 발생했습니다.";
        }
        finally
        {
            m_bIsBusy = false;
            SetBusy(false, _summaryLabel.Text);
            m_oCancellationTokenSource?.Dispose();
            m_oCancellationTokenSource = null;
        }
    }

    private void ToggleAll()
    {
        if (_grid.DataSource is not List<XmlDifference> items || items.Count == 0)
            return;
        var next = !items.All(item => item.Apply);
        foreach (var item in items) item.Apply = next;
        _grid.Refresh();
    }

    private void ToggleLogView()
    {
        m_bIsLogView = !m_bIsLogView;
        if (m_bIsLogView)
        {
            ConfigureLogColumns();
            ShowLogEntries(_calendar.SelectionStart);
            _logViewButton.Text = "비교 결과 보기";
        }
        else
        {
            ConfigureCompareColumns();
            _grid.DataSource = null;
            SetEmptyState("비교할 백업을 선택한 뒤 XML 비교를 실행하세요.");
            _summaryLabel.Text = "백업을 선택하고 XML 비교를 실행하세요.";
            _applyButton.Enabled = false;
            _logViewButton.Text = "작업 로그";
        }

        _backupComboLabel.Visible = !m_bIsLogView;
        _backupCombo.Visible = !m_bIsLogView;
        _sourceComboLabel.Visible = !m_bIsLogView;
        _sourceIsCurrentCheckBox.Visible = !m_bIsLogView;
        _sourceCombo.Visible = !m_bIsLogView;
        _compareButton.Visible = !m_bIsLogView;
        _selectAllButton.Visible = !m_bIsLogView;
        _applyButton.Visible = !m_bIsLogView;
    }

    private void SetBusy(bool busy, string text)
    {
        _compareButton.Enabled = !busy;
        _logViewButton.Enabled = !busy;
        _cancelButton.Visible = busy;
        _summaryLabel.Text = text;
        // 복원 화면과 동일하게 막연한 진행 애니메이션 대신 실제 진행률(%)로 표시한다.
        _progress.Style = ProgressBarStyle.Blocks;
        _progress.Value = 0;
    }

    private void SetEmptyState(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
            _emptyStateLabel.Text = text;
        _emptyStateLabel.Visible = _grid.Rows.Count == 0;
    }
}
