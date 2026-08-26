using PC_BackUp.Services;

namespace PC_BackUp;

public partial class SettingControl : UserControlBase
{
	private sealed class SelectiveFolderRow
	{
		public required Panel Container { get; init; }
		public required TextBox NameText { get; init; }
		public required StyledButton RemoveButton { get; init; }
	}

	// 선별 폴더 행의 [찾기][－] 버튼 크기 — "＋ 폴더 추가" 버튼 폭을 여기에 맞춘다.
	private const int RowBrowseButtonWidth = 70;
	private const int RowRemoveButtonWidth = 40;
	private const int RowButtonGap = 6;

	private ISettingsService? m_oSettingsService;
	private readonly TextBox m_oProjectText = new();
	private readonly TextBox m_oExecutableText = new();
	private readonly TextBox m_oBackupPathText = new();
	private readonly FlowLayoutPanel m_oFolderRowsPanel = new();
	private readonly List<SelectiveFolderRow> m_oFolderRows = new();
	private readonly Label m_oStatusLabel = new();

	public SettingControl()
	{
		InitializeComponent();

		addFolderButton.Click += (_, _) => AddFolderRow(string.Empty);
		m_oFolderRowsPanel.Resize += (_, _) => ResizeFolderRows();

		// 드롭 영역(dropZonePanel)과 그 안의 라벨(dropZoneLabel) 둘 다 같은 핸들러를 붙인다.
		// 라벨이 패널을 Dock=Fill로 덮고 있어서, 드롭 시점에 커서 아래에 어느 쪽이 잡히든 받아지게 한다.
		dropZonePanel.DragEnter += DropZone_DragEnter;
		dropZonePanel.DragDrop += DropZone_DragDrop;
		dropZoneLabel.DragEnter += DropZone_DragEnter;
		dropZoneLabel.DragDrop += DropZone_DragDrop;
	}

	public SettingControl(ISettingsService settingsService) : this()
	{
		m_oSettingsService = settingsService;
	}

	public override void OnMenuSelected() => LoadSettings();

	private void AddFolderRow(string initialName)
	{
		// FlowLayoutPanel은 자식의 Dock을 stretch에 쓰지 않으므로 폭을 직접 관리한다(ResizeFolderRows 참고).
		var container = new Panel { Width = FolderRowWidth(), Height = 40, Margin = new Padding(0, 0, 0, 6) };
		var nameText = new TextBox { Text = initialName };
		ConfigureTextBox(nameText);

		var browseButton = new StyledButton
		{
			ButtonType = StyledButtonType.Secondary,
			Text = "찾기",
			Dock = DockStyle.Right,
			Width = RowBrowseButtonWidth,
			Margin = new Padding(RowButtonGap, 0, 0, 0),
			NormalBackColor = ColorRGB.SecondaryBackground,
			ForeColor = ColorRGB.Text
		};
		browseButton.BorderColor = ColorRGB.Border;

		var removeButton = new StyledButton
		{
			ButtonType = StyledButtonType.Secondary,
			Text = "－",
			Dock = DockStyle.Right,
			Width = RowRemoveButtonWidth,
			Margin = new Padding(RowButtonGap, 0, 0, 0),
			NormalBackColor = ColorRGB.DangerBackground,
			ForeColor = ColorRGB.Text
		};
		removeButton.BorderColor = ColorRGB.Border;

		var row = new SelectiveFolderRow { Container = container, NameText = nameText, RemoveButton = removeButton };
		browseButton.Click += (_, _) => BrowseFolderRow(nameText);
		removeButton.Click += (_, _) => RemoveFolderRow(row);

		container.Controls.Add(nameText);
		container.Controls.Add(browseButton);
		container.Controls.Add(removeButton);

		m_oFolderRows.Add(row);
		m_oFolderRowsPanel.Controls.Add(container);
		UpdateFolderRowRemoveButtons();
	}

	private int FolderRowWidth() => Math.Max(200, m_oFolderRowsPanel.ClientSize.Width - 4);

	private void ResizeFolderRows()
	{
		var width = FolderRowWidth();
		foreach (var row in m_oFolderRows)
			row.Container.Width = width;
	}

	private void RemoveFolderRow(SelectiveFolderRow row)
	{
		if (m_oFolderRows.Count <= 1)
			return;

		m_oFolderRows.Remove(row);
		m_oFolderRowsPanel.Controls.Remove(row.Container);
		row.Container.Dispose();
		UpdateFolderRowRemoveButtons();
	}

	private void UpdateFolderRowRemoveButtons()
	{
		var canRemove = m_oFolderRows.Count > 1;
		foreach (var row in m_oFolderRows)
			row.RemoveButton.Enabled = canRemove;
	}

	private void ClearFolderRows()
	{
		// 추적 목록(m_oFolderRows)과 실제 패널 자식이 어긋나 있어도 확실히 비워지도록
		// 패널의 실제 자식 컨트롤 전체를 기준으로 제거한다.
		var containers = m_oFolderRowsPanel.Controls.OfType<Control>().ToList();
		m_oFolderRowsPanel.Controls.Clear();
		foreach (var container in containers)
			container.Dispose();
		m_oFolderRows.Clear();
	}

	private void BrowseFolderRow(TextBox nameText)
	{
		using var dialog = new FolderBrowserDialog
		{
			Description = "선별 백업에 포함할 폴더를 선택하세요.",
			UseDescriptionForTitle = true,
			ShowNewFolderButton = false
		};
		var sourceRoot = Path.GetDirectoryName(m_oExecutableText.Text);
		dialog.InitialDirectory = !string.IsNullOrWhiteSpace(sourceRoot) && Directory.Exists(sourceRoot)
				? sourceRoot
				: GetDefaultBrowseDirectory();

		if (dialog.ShowDialog(this) == DialogResult.OK)
			nameText.Text = Path.GetFileName(Path.TrimEndingDirectorySeparator(dialog.SelectedPath));
	}

	private void DropZone_DragEnter(object? sender, DragEventArgs e)
	{
		e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) == true
				? DragDropEffects.Copy
				: DragDropEffects.None;
	}

	private void DropZone_DragDrop(object? sender, DragEventArgs e)
	{
		if (e.Data?.GetData(DataFormats.FileDrop) is not string[] paths)
			return;

		var folders = paths.Where(Directory.Exists).ToList();
		if (folders.Count == 0)
		{
			MessageBox.Show(this, "폴더만 끌어다 놓을 수 있습니다.", "폴더 추가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}

		var added = 0;
		foreach (var folder in folders)
			if (AddOrUpdateFolderName(Path.GetFileName(Path.TrimEndingDirectorySeparator(folder))))
				added++;

		m_oStatusLabel.Text = added > 0
				? string.Format("폴더 {0}개를 추가했습니다. 저장 버튼을 눌러 확정하세요.", added)
				: "이미 목록에 있는 폴더입니다.";
	}

	/// <summary>
	/// 이름이 이미 목록에 있으면 아무 것도 하지 않는다. 비어있는 행이 있으면 그 행을 채우고,
	/// 없으면 새 행을 추가한다. 실제로 반영되면 true를 반환한다.
	/// </summary>
	private bool AddOrUpdateFolderName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			return false;

		if (m_oFolderRows.Any(row => string.Equals(row.NameText.Text.Trim(), name, StringComparison.OrdinalIgnoreCase)))
			return false;

		var emptyRow = m_oFolderRows.FirstOrDefault(row => string.IsNullOrWhiteSpace(row.NameText.Text));
		if (emptyRow is not null)
			emptyRow.NameText.Text = name;
		else
			AddFolderRow(name);
		return true;
	}

	/// <summary>
	/// 실행 파일을 수동으로 선택했을 때, 그 경로 바로 바깥(형제 폴더)에 현재 목록의 이름과 같은 폴더가
	/// 실제로 있으면 자동으로 채워 넣고, 없는 이름은 건너뛴다.
	/// </summary>
	private void AutoDetectSiblingFolders(string executablePath)
	{
		var exeDirectory = Path.GetDirectoryName(executablePath);
		if (string.IsNullOrWhiteSpace(exeDirectory))
			return;

		var candidateNames = m_oFolderRows
				.Select(row => row.NameText.Text.Trim())
				.Where(name => !string.IsNullOrWhiteSpace(name))
				.Concat(new[] { "_bin", "Config" })
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList();

		var ancestor = ProgramPaths.FindAncestorNamed(exeDirectory, candidateNames);
		var root = ancestor?.Parent?.FullName ?? exeDirectory;

		var added = 0;
		foreach (var name in candidateNames)
		{
			if (!Directory.Exists(Path.Combine(root, name)))
				continue;
			if (AddOrUpdateFolderName(name))
				added++;
		}

		if (added > 0)
			m_oStatusLabel.Text = string.Format("실행 파일 옆에서 폴더 {0}개를 찾아 자동으로 추가했습니다. 저장 버튼을 눌러 확정하세요.", added);
	}

	private const string PreferredBrowseRoot = @"D:\_program";
	private const string FallbackBrowseRoot = @"D:\";

	private static string GetDefaultBrowseDirectory()
	{
		if (Directory.Exists(PreferredBrowseRoot)) return PreferredBrowseRoot;
		if (Directory.Exists(FallbackBrowseRoot)) return FallbackBrowseRoot;
		return string.Empty;
	}

	private static void ConfigureTextBox(TextBox textBox)
	{
		textBox.Dock = DockStyle.Fill;
		textBox.Margin = new Padding(0, 18, 12, 18);
		textBox.Font = new Font("맑은 고딕", 10F);
		textBox.BorderStyle = BorderStyle.FixedSingle;
	}

	private void BrowseExecutable(object? sender, EventArgs e)
	{
		var currentDirectory = Path.GetDirectoryName(m_oExecutableText.Text);
		using var dialog = new OpenFileDialog
		{
			Title = "백업 대상 실행 파일 선택",
			Filter = "실행 파일 (*.exe)|*.exe|모든 파일 (*.*)|*.*",
			CheckFileExists = true,
			FileName = m_oExecutableText.Text,
			InitialDirectory = !string.IsNullOrWhiteSpace(currentDirectory) && Directory.Exists(currentDirectory)
						? currentDirectory
						: GetDefaultBrowseDirectory()
		};
		if (dialog.ShowDialog(this) != DialogResult.OK)
			return;

		m_oExecutableText.Text = dialog.FileName;
		AutoDetectSiblingFolders(dialog.FileName);
	}

	private void BrowseBackupFolder(object? sender, EventArgs e)
	{
		using var dialog = new FolderBrowserDialog
		{
			Description = "백업 파일을 저장할 폴더를 선택하세요.",
			UseDescriptionForTitle = true,
			ShowNewFolderButton = true,
			InitialDirectory = Directory.Exists(m_oBackupPathText.Text) ? m_oBackupPathText.Text : GetDefaultBrowseDirectory()
		};
		if (dialog.ShowDialog(this) == DialogResult.OK)
			m_oBackupPathText.Text = dialog.SelectedPath;
	}

	private void LoadSettings()
	{
		if (m_oSettingsService is null) return;
		var settings = m_oSettingsService.Load();
		m_oProjectText.Text = settings.TargetProjectName;
		m_oExecutableText.Text = settings.ExecutablePath;
		m_oBackupPathText.Text = settings.BackupRootPath;

		ClearFolderRows();
		// 이전 버전에서 저장된 파일에 중복 항목이 남아있을 수 있어 로드 시 한 번 더 정리한다.
		var names = settings.SelectiveFolderNames
				.Where(name => !string.IsNullOrWhiteSpace(name))
				.Select(name => name.Trim())
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.ToList();
		if (names.Count == 0)
			names.Add(string.Empty);
		foreach (var name in names)
			AddFolderRow(name);

		m_oStatusLabel.Text = string.Format("설정 파일: {0}", m_oSettingsService.SettingsFilePath);
	}

	private void SaveButton_Click(object? sender, EventArgs e)
	{
		if (m_oSettingsService is null) return;
		var settings = new AppSettings
		{
			TargetProjectName = m_oProjectText.Text.Trim(),
			ExecutablePath = m_oExecutableText.Text.Trim(),
			BackupRootPath = m_oBackupPathText.Text.Trim(),
			SelectiveFolderNames = m_oFolderRows.Select(row => row.NameText.Text.Trim()).ToList(),
			// 복원 화면에서 선택하는 안전 백업 기본값은 여기서 보이지 않더라도 저장 시 보존한다.
			AutoBackupBeforeRestore = m_oSettingsService.Load().AutoBackupBeforeRestore
		};
		var errors = settings.Validate();
		if (errors.Count > 0)
		{
			MessageBox.Show(this, string.Join(Environment.NewLine, errors), "설정 확인",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}

		try
		{
			Directory.CreateDirectory(settings.BackupRootPath);
			m_oSettingsService.Save(settings);
			m_oStatusLabel.Text = string.Format("저장 완료  ·  {0}", m_oSettingsService.SettingsFilePath);
			MessageBox.Show(this, "환경 설정을 저장했습니다.", "저장 완료",
					MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		catch (Exception exception)
		{
			MessageBox.Show(this, string.Format("설정을 저장하지 못했습니다.\n{0}", exception.Message), "저장 실패",
					MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
