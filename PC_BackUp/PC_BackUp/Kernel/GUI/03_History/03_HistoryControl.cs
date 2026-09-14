using System.Windows.Forms.VisualStyles;
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
        _grid.CellFormatting += (_, e) =>
        {
            if (_grid.Columns[e.ColumnIndex].HeaderText == "No.")
                e.Value = (e.RowIndex + 1).ToString();
        };
        _grid.CellPainting += UiPaint_ApplyHeader;
        _grid.CellMouseClick += UiClick_ApplyHeader;
        _grid.CellPainting += UiPaint_DiffCell;
        // "XML 파일" 열이 짧아진 만큼(말줄임 처리) 잘린 전체 경로와, 화면엔 안 보이는 정확한 위치
        // (XmlDifference.XmlPath)를 툴팁으로 보여준다. "현재"/"백업" 헤더에는 형광펜 강조 색의
        // 의미를 안내한다(별도 라벨을 추가하지 않고 기존 툴팁 경로를 재사용).
        _grid.CellToolTipTextNeeded += (_, e) =>
        {
            if (e.RowIndex >= 0)
            {
                if (_grid.Columns[e.ColumnIndex].HeaderText != "XML 파일") return;
                if (_grid.Rows[e.RowIndex].DataBoundItem is XmlDifference item)
                    e.ToolTipText = string.Format("{0}\n{1}", item.RelativeFilePath, item.XmlPath);
                return;
            }
            if (_grid.Columns[e.ColumnIndex] is not DataGridViewTextBoxColumn column) return;
            if (column.DataPropertyName == nameof(XmlDifference.CurrentValue))
                e.ToolTipText = "빨강으로 강조된 부분이 백업 값과 다른 부분입니다.";
            else if (column.DataPropertyName == nameof(XmlDifference.BackupValue))
                e.ToolTipText = "노랑으로 강조된 부분이 현재 값과 다른 부분입니다.";
        };

        // "파일/폴더 존재 차이" 목록의 "상태" 열은 XmlDifferenceKind를 그대로 바인딩하면
        // "ExistsOnlyInCurrent" 같은 영문이 나오니 사람이 읽을 텍스트로 바꿔 준다.
        _existenceGrid.CellFormatting += (_, e) =>
        {
            if (_existenceGrid.Columns[e.ColumnIndex].DataPropertyName != nameof(XmlDifference.Kind))
                return;
            e.Value = (XmlDifferenceKind)e.Value! switch
            {
                XmlDifferenceKind.ExistsOnlyInCurrent => "현재에만 있음",
                XmlDifferenceKind.ExistsOnlyInBackup => "백업에만 있음",
                _ => e.Value
            };
        };

        _logViewButton.Click += (_, _) => ToggleLogView();
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
        //dateCard.Margin = new Padding(3); //Padding(0, 0, 8, 0);

        _summaryCountCaption.Text = "이력 요약";
        var countIcon = IconGlyphs.CreateBadge(36, ColorRGB.SidebarActive, ColorRGB.Primary, IconGlyphs.Archive);
        countIcon.BackColor = ColorRGB.Surface;
        var countCard = ColorRGB.CreateStatCard(countIcon, _summaryCountCaption, _summaryCountValue);
        countCard.Dock = DockStyle.Fill;
        //countCard.Margin = new Padding(8, 0, 0, 0);

        layout.Controls.Add(dateCard, 0, 0);
        layout.Controls.Add(countCard, 1, 0);
        row.Controls.Add(layout);
        return row;
    }

    private void ConfigureCompareColumns()
    {
        _grid.Columns.Clear();
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
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "XML 파일", DataPropertyName = nameof(XmlDifference.RelativeFilePath), Width = 150, ReadOnly = true });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "현재", DataPropertyName = nameof(XmlDifference.CurrentValue), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "백업", DataPropertyName = nameof(XmlDifference.BackupValue), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
    }

    private void ConfigureLogColumns()
    {
        _grid.Columns.Clear();
        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "No.",
            Width = 45,
            ReadOnly = true,
            SortMode = DataGridViewColumnSortMode.NotSortable,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter },
            HeaderCell = { Style = { Alignment = DataGridViewContentAlignment.MiddleCenter } }
        });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "시간", DataPropertyName = nameof(LogEntry.TimestampText), Width = 150, ReadOnly = true });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "레벨", DataPropertyName = nameof(LogEntry.Level), Width = 80, ReadOnly = true });
        _grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "메시지", DataPropertyName = nameof(LogEntry.Message), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ReadOnly = true });
    }

    /// <summary>"적용" 헤더 칸에 전체 선택/해제 체크박스를 직접 그린다 — DataGridView 헤더는
    /// 기본적으로 체크박스를 지원하지 않아 CellPainting으로 수동으로 그려야 한다. 컬럼 순서가
    /// 바뀌어도 안전하도록 인덱스가 아니라 컬럼 타입(DataGridViewCheckBoxColumn)으로 대상을
    /// 찾는다.</summary>
    private void UiPaint_ApplyHeader(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex != -1 || _grid.Columns[e.ColumnIndex] is not DataGridViewCheckBoxColumn)
            return;
        if (e.Graphics is null || e.CellStyle is null)
            return;

        e.PaintBackground(e.CellBounds, true);

        var items = _grid.DataSource as List<XmlDifference>;
        var state = items is { Count: > 0 } && items.All(item => item.Apply)
            ? CheckBoxState.CheckedNormal
            : CheckBoxState.UncheckedNormal;
        // 라벨을 왼쪽, 체크박스를 오른쪽에 둔다 — "적용 [체크박스]" 순서가 "[체크박스] 적용"보다
        // 자연스럽게 읽힌다는 피드백(2026-08-26)을 반영.
        var glyphSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, state);
        var checkboxLocation = new Point(
            e.CellBounds.Right - glyphSize.Width - 8,
            e.CellBounds.Top + (e.CellBounds.Height - glyphSize.Height) / 2);
        CheckBoxRenderer.DrawCheckBox(e.Graphics, checkboxLocation, state);

        var textBounds = new Rectangle(e.CellBounds.Left + 4, e.CellBounds.Top,
            checkboxLocation.X - e.CellBounds.Left - 4, e.CellBounds.Height);
        TextRenderer.DrawText(e.Graphics, e.Value?.ToString() ?? string.Empty, e.CellStyle.Font, textBounds,
            e.CellStyle.ForeColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

        e.Handled = true;
    }

    /// <summary>"적용" 헤더 칸을 클릭하면 기존 ToggleAll()을 그대로 재사용해 전체 선택/해제한다
    /// ("전체 적용 선택/해제" 버튼이 하던 일을 헤더 체크박스가 대신한다).</summary>
    private void UiClick_ApplyHeader(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.RowIndex != -1 || _grid.Columns[e.ColumnIndex] is not DataGridViewCheckBoxColumn)
            return;
        ToggleAll();
    }

    /// <summary>
    /// "현재"/"백업" 열에서 값이 다른 부분만 형광펜처럼 강조해서 그린다(TextDiff 참고). 열 판별은
    /// 헤더 텍스트가 아니라 DataPropertyName으로 한다 — UpdateCompareColumnHeaders()가 헤더 텍스트를
    /// "Source (...)"/"Destination (...)"으로 바꿔버리므로 텍스트 매칭은 그 뒤에 깨진다. 이 그리드에는
    /// 이제 Kind == ValueChanged인 행만 들어오므로(존재 차이는 _existenceGrid로 분리됨) 값 자체는
    /// 항상 실제 비교 대상이다.
    /// </summary>
    private void UiPaint_DiffCell(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0 || e.Graphics is null || e.CellStyle is null)
            return;
        if (_grid.Columns[e.ColumnIndex] is not DataGridViewTextBoxColumn column)
            return;
        var isCurrentColumn = column.DataPropertyName == nameof(XmlDifference.CurrentValue);
        var isBackupColumn = column.DataPropertyName == nameof(XmlDifference.BackupValue);
        if (!isCurrentColumn && !isBackupColumn)
            return;
        if (_grid.Rows[e.RowIndex].DataBoundItem is not XmlDifference item)
            return;

        e.PaintBackground(e.CellBounds, true);

        var segments = TextDiff.Compute(item.CurrentValue, item.BackupValue)
            .Where(segment => segment.Kind == TextDiff.SegmentKind.Equal ||
                               (isCurrentColumn && segment.Kind == TextDiff.SegmentKind.RemovedFromLeft) ||
                               (isBackupColumn && segment.Kind == TextDiff.SegmentKind.AddedInRight));

        var x = e.CellBounds.Left + 4;
        foreach (var segment in segments)
        {
            var size = TextRenderer.MeasureText(e.Graphics, segment.Text, e.CellStyle.Font, e.CellBounds.Size, TextFormatFlags.NoPadding);
            var segmentBounds = new Rectangle(x, e.CellBounds.Top, size.Width, e.CellBounds.Height);
            if (segment.Kind != TextDiff.SegmentKind.Equal)
            {
                var highlightBackground = segment.Kind == TextDiff.SegmentKind.RemovedFromLeft
                    ? ColorRGB.DiffRemovedBackground
                    : ColorRGB.DiffAddedBackground;
                using var brush = new SolidBrush(highlightBackground);
                e.Graphics.FillRectangle(brush, segmentBounds);
            }
            var textColor = segment.Kind switch
            {
                TextDiff.SegmentKind.RemovedFromLeft => ColorRGB.DiffRemovedText,
                TextDiff.SegmentKind.AddedInRight => ColorRGB.DiffAddedText,
                _ => e.CellStyle.ForeColor
            };
            TextRenderer.DrawText(e.Graphics, segment.Text, e.CellStyle.Font, segmentBounds, textColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            x += size.Width;
        }

        e.Handled = true;
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

        // 캘린더 아래로 남는 빈 공간을 "이번 달 요약" + "최근 이력 바로가기"로 채운다.
        var contentTop = _calendar.Bottom + 14;
        _calendarStatsLabel.Location = new Point(_calendar.Left, contentTop);
        _calendarStatsLabel.Size = new Size(_calendar.Width, 20);
        _calendarRecentTitle.Location = new Point(_calendar.Left, contentTop + 26);
        _calendarRecentTitle.Size = new Size(_calendar.Width, 18);
        _calendarRecentPanel.Location = new Point(_calendar.Left, contentTop + 46);
        _calendarRecentPanel.Size = new Size(_calendar.Width, 160);
    }

    /// <summary>
    /// 캘린더 아래 "이번 달 요약"과 "최근 이력" 바로가기 목록을 갱신한다. referenceDate가 속한
    /// 달을 기준으로 통계를 내고, 전체 기록 중 최근 날짜 5개를 클릭 가능한 링크로 보여준다 —
    /// 클릭하면 그 날짜로 캘린더 선택이 바로 이동한다.
    /// </summary>
    private void UpdateCalendarSidebar(DateTime referenceDate)
    {
        var monthBackupCount = m_oAllRecords.Count(record =>
            record.CreatedAt.Year == referenceDate.Year && record.CreatedAt.Month == referenceDate.Month);
        var monthLogCount = m_oAllLogEntries.Count(entry =>
            entry.Timestamp.Year == referenceDate.Year && entry.Timestamp.Month == referenceDate.Month);
        _calendarStatsLabel.Text = string.Format("이번 달 백업 {0}건  ·  로그 {1}건", monthBackupCount, monthLogCount);

        _calendarRecentPanel.Controls.Clear();
        var recentDates = m_oAllRecords.Select(record => record.CreatedAt.Date)
            .Union(m_oAllLogEntries.Select(entry => entry.Timestamp.Date))
            .Distinct()
            .OrderByDescending(date => date)
            .Take(5);
        foreach (var date in recentDates)
        {
            var link = new LinkLabel
            {
                Text = string.Format("{0:yyyy-MM-dd}", date),
                AutoSize = true,
                Font = new Font("맑은 고딕", 8.5F),
                LinkColor = ColorRGB.Primary,
                Margin = new Padding(0, 2, 0, 2)
            };
            link.Click += (_, _) =>
            {
                _calendar.SelectionStart = date;
                _calendar.SelectionEnd = date;
                SelectDate(date);
            };
            _calendarRecentPanel.Controls.Add(link);
        }
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
        // Source 콤보(백업끼리 비교할 때)도 같은 날짜의 백업 목록을 쓰지만, dayRecords를 그대로
        // 다시 넘기면 두 콤보가 완전히 같은 List 인스턴스를 공유하게 되어 WinForms가 둘을 같은
        // BindingContext 커런시로 묶어버린다 — 한쪽에서 선택을 바꾸면 다른 쪽도 같이 바뀌어서
        // Source/Dest를 다르게 고를 수 없는 원인이었다. 별도 리스트 인스턴스로 복사해 끊어준다.
        _sourceCombo.DataSource = null;
        _sourceCombo.DataSource = new List<BackupRecord>(dayRecords);
        SetEmptyState(dayRecords.Count == 0 ? "선택한 날짜에는 백업 이력이 없습니다." : string.Empty);
        _compareButton.Enabled = !m_bIsBusy && dayRecords.Count > 0;
        UpdateCalendarSidebar(date);

        if (m_bIsLogView)
            ShowLogEntries(date);
    }

    private void ShowLogEntries(DateTime date)
    {
        var entries = m_oAllLogEntries.Where(entry => entry.Timestamp.Date == date.Date).ToList();
        _grid.DataSource = entries;
        SetEmptyState(entries.Count == 0 ? "이 날짜에 기록된 작업 로그가 없습니다." : string.Empty);
        SetSummaryStatus(
            entries.Count == 0
                ? "이 날짜에 로그가 없습니다."
                : string.Format("{0:yyyy년 M월 d일} 로그 {1:N0}건", date, entries.Count),
            isSuccess: false);
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
        SetSummaryStatus("XML 설정을 비교하고 있습니다...", isSuccess: false);
        SetBusy(true);
        m_oCancellationTokenSource = new CancellationTokenSource();
        try
        {
            var progress = new Progress<int>(value => _progress.Value = value);
            var differences = await m_oComparisonService.CompareAsync(m_oSettingsService.Load(), sourceRecord, destinationRecord, progress, m_oCancellationTokenSource.Token);
            // 성격이 다른 두 종류를 서로 다른 목록에 나눠 담는다 — "설정값 차이"(실제 값이 다름)는
            // _grid(빨강/노랑 강조), "존재 차이"(파일이 한쪽에만 있음)는 _existenceGrid(경로만).
            var valueChanges = differences.Where(item => item.Kind == XmlDifferenceKind.ValueChanged).ToList();
            var existenceDiffs = differences.Where(item => item.Kind != XmlDifferenceKind.ValueChanged).ToList();
            _grid.DataSource = valueChanges;
            _existenceGrid.DataSource = existenceDiffs;
            _existenceHost.Visible = existenceDiffs.Count > 0;
            UpdateCompareColumnHeaders(sourceIsCurrent ? "현재" : sourceRecord!.FileName, destinationRecord.FileName);
            SetEmptyState(valueChanges.Count == 0 ? "Source와 Destination 설정이 같습니다." : string.Empty);
            var isFullyEqual = valueChanges.Count == 0 && existenceDiffs.Count == 0;
            SetSummaryStatus(
                isFullyEqual
                    ? "Source와 Destination 설정이 같습니다."
                    : string.Format("설정값 차이 {0:N0}건  ·  존재 차이 {1:N0}건", valueChanges.Count, existenceDiffs.Count),
                isSuccess: isFullyEqual);
            UpdateApplyAvailability();
            m_oLoggingService?.LogInfo(string.Format(
                "XML 비교 완료: 설정값 차이 {0}개, 존재 차이 {1}개 (Source: {2}, Destination: {3})",
                valueChanges.Count, existenceDiffs.Count, sourceIsCurrent ? "현재" : sourceRecord!.FileName, destinationRecord.FileName));
        }
        catch (Exception exception)
        {
            m_oLoggingService?.LogError("XML 비교 중 오류가 발생했습니다.", exception);
            MessageBox.Show(this, exception.Message, "비교 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetSummaryStatus("비교 중 오류가 발생했습니다.", isSuccess: false);
        }
        finally
        {
            m_bIsBusy = false;
            SetBusy(false);
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
        var hasValueDifferences = _grid.DataSource is List<XmlDifference> items && items.Count > 0;
        var hasExistenceDifferences = _existenceGrid.DataSource is List<XmlDifference> existenceItems && existenceItems.Count > 0;
        _applyButton.Enabled = (hasValueDifferences || hasExistenceDifferences) && _sourceIsCurrentCheckBox.Checked;
    }

    /// <summary>ConfigureCompareColumns()가 고정 순서(적용/XML 파일/현재/백업)로 만든 열 헤더를
    /// 실제 선택된 Source/Destination 이름으로 갱신한다.</summary>
    private void UpdateCompareColumnHeaders(string sourceLabel, string destinationLabel)
    {
        if (_grid.Columns.Count < 5) return;
        _grid.Columns[3].HeaderText = string.Format("Source ({0})", sourceLabel);
        _grid.Columns[4].HeaderText = string.Format("Destination ({0})", destinationLabel);
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
        _existenceGrid.EndEdit();
        // 설정값 차이/존재 차이 두 목록에서 체크한 항목을 합쳐서 한 번에 적용한다 — Apply 로직
        // 자체는 RelativeFilePath 단위로 백업 파일을 통째로 덮어쓰므로 두 목록을 섞어도 문제없다.
        var selected = new List<XmlDifference>();
        if (_grid.DataSource is List<XmlDifference> valueItems)
            selected.AddRange(valueItems.Where(item => item.Apply));
        if (_existenceGrid.DataSource is List<XmlDifference> existenceItems)
            selected.AddRange(existenceItems.Where(item => item.Apply));
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
        SetSummaryStatus("선택한 설정을 적용하고 있습니다...", isSuccess: false);
        SetBusy(true);
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
            SetSummaryStatus(result.Message.Replace(Environment.NewLine, "  "), isSuccess: result.Succeeded);
        }
        catch (Exception exception)
        {
            m_oLoggingService?.LogError("설정 적용 중 예외가 발생했습니다.", exception);
            MessageBox.Show(this, exception.Message, "적용 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetSummaryStatus("적용 중 오류가 발생했습니다.", isSuccess: false);
        }
        finally
        {
            m_bIsBusy = false;
            SetBusy(false);
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
        // 작업 로그 보기로 전환하거나, 비교 화면으로 돌아왔지만 아직 새로 비교를 안 돌린 상태라면
        // "존재 차이" 목록은 이전 비교 결과라 의미가 없으므로 같이 지운다.
        _existenceGrid.DataSource = null;
        _existenceHost.Visible = false;
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
            SetSummaryStatus("백업을 선택하고 XML 비교를 실행하세요.", isSuccess: false);
            _applyButton.Enabled = false;
            _logViewButton.Text = "작업 로그";
        }

        _backupComboLabel.Visible = !m_bIsLogView;
        _backupCombo.Visible = !m_bIsLogView;
        _sourceComboLabel.Visible = !m_bIsLogView;
        _sourceIsCurrentCheckBox.Visible = !m_bIsLogView;
        _sourceCombo.Visible = !m_bIsLogView;
        _compareButton.Visible = !m_bIsLogView;
        _applyButton.Visible = !m_bIsLogView;
    }

    private void SetBusy(bool busy)
    {
        _compareButton.Enabled = !busy;
        _logViewButton.Enabled = !busy;
        _cancelButton.Visible = busy;
        // 복원 화면과 동일하게 막연한 진행 애니메이션 대신 실제 진행률(%)로 표시한다.
        _progress.Style = ProgressBarStyle.Blocks;
        _progress.Value = 0;
    }

    /// <summary>하단 상태 표시줄(_summaryLabel)의 스타일을 상태에 맞게 바꾼다. "설정이 같습니다"
    /// 같은 긍정적인 결과가 회색 글자에 묻혀 눈에 안 띈다는 피드백(2026-08-26)을 반영해, 성공
    /// 결과일 때만 초록 배지 스타일(체크 표시 + 초록 배경)로 강조한다.</summary>
    private void SetSummaryStatus(string text, bool isSuccess)
    {
        _summaryLabel.Text = isSuccess ? string.Format("✓ {0}", text) : text;
        _summaryLabel.BackColor = isSuccess ? ColorRGB.SafetyBackground : Color.Transparent;
        _summaryLabel.ForeColor = isSuccess ? ColorRGB.SafetyText : ColorRGB.MutedText;
        _summaryLabel.Font = new Font("맑은 고딕", 9F, isSuccess ? FontStyle.Bold : FontStyle.Regular);
    }

    private void SetEmptyState(string text)
    {
        if (!string.IsNullOrWhiteSpace(text))
            _emptyStateLabel.Text = text;
        _emptyStateLabel.Visible = _grid.Rows.Count == 0;
    }
}
