using System.Xml.Serialization;

namespace PC_BackUp
{

	[XmlRoot("PCBackupSettings")]
	public sealed class AppSettings
	{
		public string TargetProjectName { get; set; } = string.Empty;
		public string ExecutablePath { get; set; } = string.Empty;
        public string SourceRootPath { get; set; } = string.Empty;
		public string BackupRootPath { get; set; } = string.Empty;

		// 기본값을 여기서 미리 채워두면 안 된다 — XmlSerializer는 컬렉션 속성을 역직렬화할 때
		// 새 리스트로 교체하지 않고 getter가 반환한 기존 리스트에 Add()로 이어붙이기 때문에,
		// 저장된 XML을 불러올 때마다 기본값 위에 계속 누적되어 중복이 생긴다.
		// (신규 파일/레거시 파일 기본값 채우기는 XmlSettingsService.Load()에서 처리한다.)
		[XmlArrayItem("Name")]
		public List<string> SelectiveFolderNames { get; set; } = new();

		public bool AutoBackupBeforeRestore { get; set; } = false;

		// 종종 "있든 없든 그냥 백업으로 덮어쓰기"만 필요한 경우가 많아 기본값은 비교 생략(false)이다.
		// 복원 화면 체크박스로 켜면 복원 전에 현재 파일과 백업을 비교해 변경 내역을 보여준다.
		public bool CompareBeforeRestore { get; set; } = false;

		/// <summary>
		/// 원본 루트를 계산한다. 실행 파일이 선별 폴더(예: _bin) 안에 중첩되어 있으면
		/// 그 조상 폴더의 부모를 반환하고, 못 찾으면(과거처럼 exe가 루트에 바로 있는 구조)
		/// exe의 직계 부모 폴더로 폴백한다.
		/// </summary>
		public string GetSourceRoot()
		{
            if (!string.IsNullOrWhiteSpace(SourceRootPath)) return SourceRootPath;
			var exeDirectory = Path.GetDirectoryName(ExecutablePath);
			if (string.IsNullOrEmpty(exeDirectory))
				return string.Empty;

			var ancestor = ProgramPaths.FindAncestorNamed(exeDirectory, SelectiveFolderNames);
			return ancestor?.Parent?.FullName ?? exeDirectory;
		}

        // [Codex - 2026.10.19] 작업 도중 UI 설정 변경과 분리된 스냅샷을 사용한다.
        public AppSettings Snapshot() => new()
        {
            TargetProjectName = TargetProjectName, ExecutablePath = ExecutablePath, SourceRootPath = GetSourceRoot(),
            BackupRootPath = BackupRootPath, SelectiveFolderNames = new List<string>(SelectiveFolderNames),
            AutoBackupBeforeRestore = AutoBackupBeforeRestore, CompareBeforeRestore = CompareBeforeRestore
        };

		public IReadOnlyList<string> GetSelectiveFolderNames()
		{
			return SelectiveFolderNames
					.Where(name => !string.IsNullOrWhiteSpace(name))
					.Select(name => name.Trim())
					.Distinct(StringComparer.OrdinalIgnoreCase)
					.ToList();
		}

		/// <summary>
		/// requireExecutableExists를 false로 주면 실행 파일이 지금 디스크에 실제로 있는지는 검사하지
		/// 않고 경로가 설정되어 있는지만 확인한다. 복원(Restore)은 대상 파일(실행 파일 포함)이
		/// 사라진 상황을 되살리는 기능이라 exe가 없는 게 오히려 정상적인 호출 상황이기 때문이다.
		/// </summary>
		public IReadOnlyList<string> Validate(bool requireExecutableExists = true)
		{
			var errors = new List<string>();

			if (string.IsNullOrWhiteSpace(TargetProjectName))
				errors.Add("타겟 프로젝트 이름을 입력하세요.");
			if (string.IsNullOrWhiteSpace(ExecutablePath) || (requireExecutableExists && !File.Exists(ExecutablePath)))
				errors.Add("유효한 실행 파일 경로를 선택하세요.");
			if (string.IsNullOrWhiteSpace(BackupRootPath))
				errors.Add("백업 저장 경로를 선택하세요.");

			var rawNames = SelectiveFolderNames.Where(name => !string.IsNullOrWhiteSpace(name)).ToList();
			if (rawNames.Count == 0)
			{
				errors.Add("선별 백업 폴더를 최소 1개 이상 지정하세요.");
			}
			else
			{
				foreach (var name in rawNames)
					if (!FileNaming.IsValidRelativeFolder(name.Trim()))
						errors.Add(string.Format("'{0}' 폴더명이 올바르지 않습니다.", name));

				if (rawNames.Select(name => name.Trim())
						.Distinct(StringComparer.OrdinalIgnoreCase).Count() != rawNames.Count)
					errors.Add("선별 백업 폴더명은 서로 달라야 합니다.");
			}

            // [Codex - 2026.10.19] 원본·실행 파일·저장소 관계를 경계 구분자를 포함해 검증한다.
            try
            {
                var root = GetSourceRoot();
                if (!Path.IsPathFullyQualified(root) || !Path.IsPathFullyQualified(ExecutablePath) || !Path.IsPathFullyQualified(BackupRootPath))
                    errors.Add("원본·실행 파일·백업은 절대 경로로 지정하세요.");
                else
                {
                    root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
                    var backup = Path.TrimEndingDirectorySeparator(Path.GetFullPath(BackupRootPath));
                    var separator = Path.DirectorySeparatorChar;
                    if (string.Equals(root, Path.TrimEndingDirectorySeparator(Path.GetPathRoot(root)!), StringComparison.OrdinalIgnoreCase))
                        errors.Add("드라이브·공유 루트를 원본으로 사용할 수 없습니다.");
                    if (root.Equals(backup, StringComparison.OrdinalIgnoreCase) || root.StartsWith(string.Format("{0}{1}", backup, separator), StringComparison.OrdinalIgnoreCase))
                        errors.Add("백업 저장소는 원본과 같거나 원본의 상위 폴더일 수 없습니다.");
                    if (!Path.GetFullPath(ExecutablePath).StartsWith(string.Format("{0}{1}", root, separator), StringComparison.OrdinalIgnoreCase))
                        errors.Add("실행 파일은 원본 루트 아래에 있어야 합니다.");
                }
            }
            catch (Exception exception) when (exception is ArgumentException or NotSupportedException or IOException)
            { errors.Add("원본·백업 경로 형식을 확인하세요."); }
			return errors;
		}
	}

}
