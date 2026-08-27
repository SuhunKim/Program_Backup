using PC_BackUp.Services;

namespace PC_BackUp;

public partial class RecoveryControl : UserControlBase
{
	private ISettingsService? m_oSettingsService;
	private BackupCatalogService? m_oCatalogService;
	private RecoveryService? m_oRecoveryService;
	private BackupService? m_oBackupService;
	private BackupComparisonService? m_oBackupComparisonService;
	private LogManager? m_oLoggingService;
	private bool m_bIsBusy;
	private CancellationTokenSource? m_oCancellationTokenSource;
	private readonly System.Windows.Forms.Timer m_oRefreshDebounceTimer = new() { Interval = 500 };
	private FileSystemWatcher? m_oBackupFolderWatcher;

	public RecoveryControl()
	{
		InitializeComponent();

		// 아이콘 배지(IconGlyphs)는 GDI+ Paint 이벤트로 그려서 디자이너가 표현할 수 없는
		// 부분이라, 디자이너가 그려둔 고정 골격(workspace/detailsPanel)에 여기서 덧붙인다.

		_grid.DataBindingComplete += (_, _) =>
		{
			_emptyLabel.Visible = _grid.Rows.Count == 0;
			UpdateSelectedBackupDetails();
		};

		_grid.SelectionChanged += (_, _) => UpdateSelectedBackupDetails();

		_cancelButton.Click += (_, _) => m_oCancellationTokenSource?.Cancel();
		_calendar.DateSelected += (_, eventArgs) => LoadDate(eventArgs.Start);

		workspace.Controls.Add(BuildSummaryRow());
		var safetyInfoCard = BuildSafetyInfoCard();
		safetyInfoCard.Dock = DockStyle.Right;
		safetyInfoCard.Width = 300;
		detailsPanel.Controls.Add(safetyInfoCard);

		m_oRefreshDebounceTimer.Tick += (_, _) =>
		{
			m_oRefreshDebounceTimer.Stop();
			RefreshCatalog();
		};
	}

	public RecoveryControl(
			ISettingsService settingsService,
			BackupCatalogService catalogService,
			RecoveryService recoveryService,
			BackupService backupService,
			BackupComparisonService backupComparisonService,
			LogManager loggingService) : this()
	{
		m_oSettingsService = settingsService;
		m_oCatalogService = catalogService;
		m_oRecoveryService = recoveryService;
		m_oBackupService = backupService;
		m_oBackupComparisonService = backupComparisonService;
		m_oLoggingService = loggingService;
	}

	public override void OnMenuSelected() => RefreshCatalog();

	/// <summary>상단 "선택된 백업 날짜 / 백업 요약" 카드 두 개 — 원형 아이콘을 코드로 그려야 해서 디자이너로 옮기지 못했다.</summary>
	private Control BuildSummaryRow()
	{
		var row = new Panel { Dock = DockStyle.Top, Height = 92, Padding = new Padding(0, 0, 0, 16) };
		var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
		layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
		layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

		_summaryDateCaption.Text = "선택된 백업 날짜";
		var dateIcon = IconGlyphs.CreateBadge(36, ColorRGB.SidebarActive, ColorRGB.Primary, IconGlyphs.Calendar);
		dateIcon.BackColor = ColorRGB.Surface; // 카드(흰 배경) 모서리와 원 바깥쪽이 자연스럽게 이어지도록
		var dateCard = ColorRGB.CreateStatCard(dateIcon, _summaryDateCaption, _summaryDateValue);
		dateCard.Dock = DockStyle.Fill;
		dateCard.Margin = new Padding(0, 0, 8, 0);

		_summaryCountCaption.Text = "백업 요약";
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

	/// <summary>안전한 복원을 위한 안내 — 체크 아이콘을 코드로 그려야 해서 디자이너로 옮기지 못했다.</summary>
	private static Control BuildSafetyInfoCard()
	{
		var card = new CardPanel { BackColor = ColorRGB.SafetyBackground, BorderColor = ColorRGB.SafetyBorder, Padding = new Padding(14, 10, 14, 10) };

		var titleRow = new Panel { Dock = DockStyle.Top, Height = 24 };
		var titleIcon = IconGlyphs.CreateBadge(20, ColorRGB.SafetyIcon, Color.White, IconGlyphs.Check);
		titleIcon.BackColor = ColorRGB.SafetyBackground; // 안내 박스 배경과 아이콘 사각 모서리를 맞춘다
		titleIcon.Location = new Point(0, 2);
		var titleLabel = new Label
		{
			Text = "안전한 복원을 위한 안내",
			Location = new Point(titleIcon.Right + 6, 2),
			AutoSize = true,
			Font = new Font("맑은 고딕", 9F, FontStyle.Bold),
			ForeColor = ColorRGB.SafetyText
		};
		titleRow.Controls.Add(titleLabel);
		titleRow.Controls.Add(titleIcon);

		var bodyLabel = new Label
		{
			Dock = DockStyle.Fill,
			Text = "•  복원을 시작하면 선택한 백업으로 현재 데이터를 덮어씁니다.\r\n" +
							 "•  복원 중에는 다른 프로그램을 종료해 주세요.\r\n" +
							 "•  복원 완료 후 프로그램을 재시작해야 할 수 있습니다.",
			Font = new Font("맑은 고딕", 8.5F),
			ForeColor = ColorRGB.SafetyText
		};

		card.Controls.Add(bodyLabel);
		card.Controls.Add(titleRow);
		return card;
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

	private void RefreshCatalog()
	{
		if (m_oSettingsService is null || m_oCatalogService is null) return;
		ApplyCalendarSizing();
		var settings = m_oSettingsService.Load();
		_autoSafetyBackupCheckBox.Checked = settings.AutoBackupBeforeRestore;
		_compareBeforeRestoreCheckBox.Checked = settings.CompareBeforeRestore;
		var catalog = m_oCatalogService.Refresh(settings.BackupRootPath);
		_calendar.BoldedDates = catalog.Keys.ToArray();
		_calendar.UpdateBoldedDates();
		LoadDate(_calendar.SelectionStart);
		WatchBackupFolder(settings.BackupRootPath);
	}

	/// <summary>
	/// 백업 폴더를 감시해서 탐색기 등 프로그램 외부에서 파일이 추가/삭제/이동/이름변경되면
	/// 자동으로 목록을 새로고침한다. 짧은 시간에 이벤트가 여러 번 몰려도 디바운스 타이머로 한 번만 갱신한다.
	/// </summary>
	private void WatchBackupFolder(string backupRootPath)
	{
		if (m_oBackupFolderWatcher is not null &&
				string.Equals(m_oBackupFolderWatcher.Path, backupRootPath, StringComparison.OrdinalIgnoreCase))
			return;

		m_oBackupFolderWatcher?.Dispose();
		m_oBackupFolderWatcher = null;

		if (string.IsNullOrWhiteSpace(backupRootPath) || !Directory.Exists(backupRootPath))
			return;

		try
		{
			var watcher = new FileSystemWatcher(backupRootPath)
			{
				IncludeSubdirectories = false,
				NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite
			};
			watcher.Created += (_, _) => QueueRefresh();
			watcher.Deleted += (_, _) => QueueRefresh();
			watcher.Renamed += (_, _) => QueueRefresh();
			watcher.Changed += (_, _) => QueueRefresh();
			watcher.EnableRaisingEvents = true;
			m_oBackupFolderWatcher = watcher;
		}
		catch
		{
			// 감시 설정에 실패해도 화면 진입 시 새로고침(RefreshCatalog)은 계속 동작한다.
		}
	}

	private void QueueRefresh()
	{
		if (!IsHandleCreated) return;
		try
		{
			BeginInvoke(new Action(() =>
			{
				m_oRefreshDebounceTimer.Stop();
				m_oRefreshDebounceTimer.Start();
			}));
		}
		catch (ObjectDisposedException)
		{
			// 컨트롤이 종료되는 시점에 감시 이벤트가 뒤늦게 도착한 경우 무시한다.
		}
	}

	/// <summary>
	/// 환경설정 화면에는 더 이상 이 옵션이 없으므로, 복원 화면에서 체크박스를 바꾸는 즉시 저장한다.
	/// RefreshCatalog()가 저장된 값을 반영할 때는 값이 그대로라 이벤트가 실행 취소되지 않는다.
	/// </summary>
	private void UiChange_AutoSafetyBackup(object? sender, EventArgs e)
	{
		if (m_oSettingsService is null) return;
		var settings = m_oSettingsService.Load();
		if (settings.AutoBackupBeforeRestore == _autoSafetyBackupCheckBox.Checked)
			return;

		settings.AutoBackupBeforeRestore = _autoSafetyBackupCheckBox.Checked;
		try
		{
			m_oSettingsService.Save(settings);
		}
		catch (Exception exception)
		{
			m_oLoggingService?.LogError("자동 안전 백업 설정을 저장하지 못했습니다.", exception);
		}
	}

	/// <summary>
	/// 위 자동 안전 백업 체크박스와 동일하게, 복원 화면에서 바꾸는 즉시 저장한다.
	/// 미체크(기본값)면 비교 없이 바로 덮어쓰고, 체크하면 비교 후 확인을 거쳐 복원한다.
	/// </summary>
	private void UiChange_CompareBeforeRestore(object? sender, EventArgs e)
	{
		if (m_oSettingsService is null) return;
		var settings = m_oSettingsService.Load();
		if (settings.CompareBeforeRestore != _compareBeforeRestoreCheckBox.Checked)
		{
			settings.CompareBeforeRestore = _compareBeforeRestoreCheckBox.Checked;
			try
			{
				m_oSettingsService.Save(settings);
			}
			catch (Exception exception)
			{
				m_oLoggingService?.LogError("복원 전 비교 설정을 저장하지 못했습니다.", exception);
			}
		}

		UpdateSelectedBackupDetails();
	}

	private void LoadDate(DateTime date)
	{
		if (m_oCatalogService is null) return;
		_dateLabel.Text = string.Format("{0:yyyy년 M월 d일} 백업 목록", date);
		var records = m_oCatalogService.GetByDate(date).ToList();
		_grid.DataSource = records;
		_emptyLabel.Text = records.Count == 0
			? "선택한 날짜에는 복원 가능한 백업 자료가 없습니다."
			: string.Empty;
		_emptyLabel.Visible = _grid.Rows.Count == 0;

		_summaryDateValue.Text = string.Format("{0:yyyy년 M월 d일}", date);
		var totalBytes = records.Sum(record => record.SizeBytes);
		_summaryCountValue.Text = string.Format("백업 {0}개  ·  총 {1}", records.Count, BackupRecord.FormatBytes(totalBytes));

		UpdateSelectedBackupDetails();
	}

	private async void UiClick_Restore(object? sender, EventArgs e)
	{
		if (m_oSettingsService is null || m_oRecoveryService is null || m_oBackupComparisonService is null || m_bIsBusy) return;
		if (_grid.CurrentRow?.DataBoundItem is not BackupRecord record)
			return;

		var settings = m_oSettingsService.Load();

		// 확인 메시지 조립(비교 후 복원 / 비교 없이 바로 덮어쓰기) — 비교 모드에서 취소되었거나
		// 변경사항이 없으면 null을 반환해 여기서 그대로 끝낸다.
		var confirmMessageText = _compareBeforeRestoreCheckBox.Checked
			? await BuildCompareConfirmMessageAsync(record, settings)
			: BuildDirectOverwriteConfirmMessage(record, settings);
		if (confirmMessageText is null)
			return;

		var confirm = MessageBox.Show(this,
				confirmMessageText,
				"복원 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
		if (confirm != DialogResult.Yes)
			return;

		if (!await EnsureTargetProcessesStoppedAsync(settings))
			return;

		await RunRestoreAsync(record, settings);
	}

	/// <summary>
	/// 비교 후 복원 모드: 현재 파일과 백업을 먼저 비교해서 무엇이 바뀌는지 보여준 뒤 확인 메시지를
	/// 만든다. 비교가 취소되었거나 변경사항이 없으면 null을 반환해 호출자가 복원을 진행하지 않고
	/// 그대로 끝내게 한다.
	/// </summary>
	private async Task<string?> BuildCompareConfirmMessageAsync(BackupRecord record, AppSettings settings)
	{
		if (m_oBackupComparisonService is null) return null;

		m_bIsBusy = true;
		_restoreButton.Enabled = false;
		_cancelButton.Visible = true;
		m_oCancellationTokenSource = new CancellationTokenSource();
		IReadOnlyList<BackupFileDifference> differences;
		try
		{
			var progress = new Progress<int>(value => _progress.Value = value);
			_selectedBackupMetaLabel.Text = "복원 전 변경사항을 확인하고 있습니다...";
			var comparison = await m_oBackupComparisonService.CompareAsync(record, settings, progress, m_oCancellationTokenSource.Token);
			if (comparison.IsCanceled)
			{
				_selectedBackupMetaLabel.Text = "복원 전 비교를 취소했습니다.";
				_comparisonResultLabel.Text = string.Empty;
				_comparisonListBox.Items.Clear();
				return null;
			}

			differences = comparison.Differences;
			if (differences.Count == 0)
			{
				_selectedBackupMetaLabel.Text = "변경사항 없음 — 복원할 내용이 없습니다.";
				_comparisonResultLabel.Text = "백업과 현재 파일은 동일합니다.";
				_comparisonListBox.Items.Clear();
				return null;
			}

			_selectedBackupMetaLabel.Text = string.Format("복원 전 비교 완료: 변경 {0}개", differences.Count);
			_comparisonListBox.Items.Clear();
			foreach (var difference in differences)
				_comparisonListBox.Items.Add(string.Format("{0}: {1}", difference.Status, difference.RelativePath));
		}
		catch (Exception exception)
		{
			// BackupComparisonService.CompareAsync는 취소 외의 예외를 그대로 전파하므로 여기서 받아
			// 처리한다 — 그렇지 않으면 async void 이벤트 핸들러를 타고 올라가 전역 예외 처리기까지
			// 가게 되어 다른 화면들과 달리 "복원 실패" 안내 없이 거칠게 실패한다.
			m_oLoggingService?.LogError("복원 전 비교 중 예외가 발생했습니다.", exception);
			MessageBox.Show(this, exception.Message, "복원 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return null;
		}
		finally
		{
			m_bIsBusy = false;
			_cancelButton.Visible = false;
			_progress.Value = 0;
			m_oCancellationTokenSource?.Dispose();
			m_oCancellationTokenSource = null;
			_restoreButton.Enabled = _grid.Rows.Count > 0;
		}

		// 대상 위치에 없는 파일(백업에만 있음)은 내용 비교가 불가능해 그대로 복사될 수밖에 없으므로,
		// 몇 개나 새로 생성되는지 미리 알려준 뒤 덮어쓸지 확인받는다.
		var missingCount = differences.Count(difference => difference.Status == "백업에만 있음");
		var changedCount = differences.Count(difference => difference.Status == "내용 변경");
		var extraCount = differences.Count(difference => difference.Status == "현재에만 있음");

		var confirmMessage = new System.Text.StringBuilder()
			.AppendLine(string.Format("'{0}' 백업을 다음 위치에 복원합니다.", record.FileName))
			.AppendLine()
			.AppendLine(settings.GetSourceRoot())
			.AppendLine();
		if (missingCount > 0)
			confirmMessage.AppendLine(string.Format("- 대상 위치에 없는 파일 {0}개: 비교할 수 없어 그대로 복사됩니다.", missingCount));
		if (changedCount > 0)
			confirmMessage.AppendLine(string.Format("- 내용이 다른 파일 {0}개: 백업 내용으로 덮어씁니다.", changedCount));
		if (extraCount > 0)
			confirmMessage.AppendLine(string.Format("- 백업에 없는 파일 {0}개: 삭제하지 않고 그대로 둡니다.", extraCount));
		confirmMessage.AppendLine().Append("계속할까요?");
		return confirmMessage.ToString();
	}

	/// <summary>비교 없이 바로 덮어쓰기(기본값): 있든 없든 백업의 모든 파일로 그대로 덮어쓴다.</summary>
	private string BuildDirectOverwriteConfirmMessage(BackupRecord record, AppSettings settings)
	{
		_selectedBackupMetaLabel.Text = "비교를 생략하고 전체 파일을 덮어씁니다.";
		_comparisonResultLabel.Text = "비교 없이 백업 내용으로 전체 덮어씁니다.";
		_comparisonListBox.Items.Clear();
		return string.Format(
			"'{0}' 백업을 다음 위치에 덮어씁니다.\n\n{1}\n\n비교 없이 백업의 모든 파일로 덮어씁니다.\n계속할까요?",
			record.FileName, settings.GetSourceRoot());
	}

	/// <summary>실행 중인 대상 프로그램이 있으면 종료 확인 후 종료한다. 복원을 계속 진행해도 되면 true.</summary>
	private async Task<bool> EnsureTargetProcessesStoppedAsync(AppSettings settings)
	{
		if (m_oRecoveryService is null) return false;

		IReadOnlyList<System.Diagnostics.Process> running;
		try
		{
			running = m_oRecoveryService.GetRunningTargetProcesses(settings);
		}
		catch (Exception exception)
		{
			m_oLoggingService?.LogError("실행 중인 대상 프로그램을 확인하지 못했습니다.", exception);
			MessageBox.Show(this, exception.Message, "복원 중단", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return false;
		}

		if (running.Count == 0)
			return true;

		var close = MessageBox.Show(this,
				string.Format("대상 프로그램이 {0}개 실행 중입니다. 종료한 뒤 복원할까요?", running.Count),
				"프로그램 실행 중", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button2);
		if (close != DialogResult.Yes)
		{
			foreach (var process in running) process.Dispose();
			return false;
		}

		var stopResult = await m_oRecoveryService.StopProcessesAsync(running);
		if (!stopResult.Succeeded)
		{
			MessageBox.Show(this, stopResult.Message, "복원 중단", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return false;
		}
		return true;
	}

	/// <summary>필요하면 복원 전 자동 안전 백업을 실행하고, 이어서 실제 복원을 실행한다.</summary>
	private async Task RunRestoreAsync(BackupRecord record, AppSettings settings)
	{
		if (m_oRecoveryService is null) return;

		m_bIsBusy = true;
		_restoreButton.Enabled = false;
		_cancelButton.Visible = true;
		_progress.Value = 0;
		m_oCancellationTokenSource = new CancellationTokenSource();
		try
		{
			if (_autoSafetyBackupCheckBox.Checked && m_oBackupService is not null &&
				!await RunAutoSafetyBackupAsync(settings))
			{
				return;
			}

			var progress = new Progress<int>(value => _progress.Value = value);
			var result = await m_oRecoveryService.RestoreAsync(record, settings, progress, m_oCancellationTokenSource.Token);
			if (result.Succeeded)
				m_oLoggingService?.LogInfo(string.Format("복원 완료: {0}", record.FileName));
			else
				m_oLoggingService?.LogError(string.Format("복원 실패: {0}", result.Message));
			MessageBox.Show(this, result.Message, result.Succeeded ? "복원 완료" : "복원 실패",
					MessageBoxButtons.OK, result.Succeeded ? MessageBoxIcon.Information : MessageBoxIcon.Error);
		}
		catch (Exception exception)
		{
			m_oLoggingService?.LogError("복원 중 예외가 발생했습니다.", exception);
			MessageBox.Show(this, exception.Message, "복원 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		finally
		{
			m_bIsBusy = false;
			_restoreButton.Enabled = _grid.Rows.Count > 0;
			_cancelButton.Visible = false;
			_progress.Value = 0;
			m_oCancellationTokenSource?.Dispose();
			m_oCancellationTokenSource = null;
		}
	}

	/// <summary>
	/// 복원 전 자동 안전 백업을 실행한다. 실패하면 안전 백업 없이 계속할지 확인받고,
	/// 계속 진행해도 되면(백업 성공 또는 사용자가 계속을 선택) true를 반환한다.
	/// </summary>
	private async Task<bool> RunAutoSafetyBackupAsync(AppSettings settings)
	{
		var safety = await m_oBackupService!.CreateAsync(settings, BackupKind.FullZip, "autosafety", null, m_oCancellationTokenSource!.Token);
		if (safety.Result.Succeeded)
		{
			m_oLoggingService?.LogInfo(string.Format("복원 전 자동 안전 백업 완료: {0}", safety.Record?.FileName));
			return true;
		}

		m_oLoggingService?.LogError(string.Format("복원 전 자동 안전 백업 실패: {0}", safety.Result.Message));
		var continueAnyway = MessageBox.Show(this,
				string.Format("복원 전 자동 안전 백업에 실패했습니다.\n{0}\n\n안전 백업 없이 복원을 계속할까요?", safety.Result.Message),
				"자동 안전 백업 실패", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
		if (continueAnyway != DialogResult.Yes)
		{
			_progress.Value = 0;
			return false;
		}
		return true;
	}

	private void UpdateSelectedBackupDetails()
	{
		var record = _grid.CurrentRow?.DataBoundItem as BackupRecord;
		if (record is null)
		{
			_selectedBackupLabel.Text = "선택한 백업 없음";
            _selectedBackupMetaLabel.Text = "목록에서 복원할 백업을 선택하세요.";
            _comparisonResultLabel.Text = string.Empty;
            _comparisonListBox.Items.Clear();
			_restoreTargetLabel.Text = string.Empty;
			_restoreButton.Enabled = false;
			return;
		}

		var sourceRoot = m_oSettingsService?.Load().GetSourceRoot() ?? string.Empty;
		_selectedBackupLabel.Text = string.Format("선택 백업: {0}", record.FileName);
		_selectedBackupMetaLabel.Text = string.Format("{0:yyyy년 M월 d일 HH:mm}  ·  {1}  ·  {2}", record.CreatedAt, record.KindText, record.SizeText);
        _restoreTargetLabel.Text = string.Format("복원 위치: {0}", sourceRoot);
        _comparisonResultLabel.Text = _compareBeforeRestoreCheckBox.Checked
            ? "복원 시작 전 현재 파일과 비교합니다."
            : "비교 없이 백업 내용으로 전체 덮어씁니다.";
        _comparisonListBox.Items.Clear();
		_restoreButton.Enabled = !m_bIsBusy;
	}
}
