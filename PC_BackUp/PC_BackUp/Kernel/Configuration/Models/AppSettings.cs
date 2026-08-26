using System.Xml.Serialization;

namespace PC_BackUp
{

	[XmlRoot("PCBackupSettings")]
	public sealed class AppSettings
	{
		public string TargetProjectName { get; set; } = string.Empty;
		public string ExecutablePath { get; set; } = string.Empty;
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
			var exeDirectory = Path.GetDirectoryName(ExecutablePath);
			if (string.IsNullOrEmpty(exeDirectory))
				return string.Empty;

			var ancestor = ProgramPaths.FindAncestorNamed(exeDirectory, SelectiveFolderNames);
			return ancestor?.Parent?.FullName ?? exeDirectory;
		}

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
					if (!FileNaming.IsValidFolderName(name.Trim()))
						errors.Add(string.Format("'{0}' 폴더명이 올바르지 않습니다.", name));

				if (rawNames.Select(name => name.Trim())
						.Distinct(StringComparer.OrdinalIgnoreCase).Count() != rawNames.Count)
					errors.Add("선별 백업 폴더명은 서로 달라야 합니다.");
			}

			return errors;
		}
	}

}
