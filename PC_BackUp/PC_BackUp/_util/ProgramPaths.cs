namespace PC_BackUp;

/// <summary>
/// "실행 파일이 어느 폴더 밑에 중첩되어 있는지"를 판단하는 공용 경로 탐색 헬퍼.
/// PC_BackUp 자신의 프로그램 루트를 찾는 XmlSettingsService와, 백업 대상 프로그램의
/// 원본 루트를 찾는 AppSettings.GetSourceRoot 양쪽에서 재사용한다.
/// </summary>
internal static class ProgramPaths
{
    /// <summary>
    /// <paramref name="startDirectory"/>에서 위로 올라가며 이름이 <paramref name="candidateNames"/>
    /// 중 하나와 일치(대소문자 무시)하는 첫 조상 디렉터리를 찾는다. 없으면 null.
    /// </summary>
    public static DirectoryInfo? FindAncestorNamed(string startDirectory, IEnumerable<string> candidateNames)
    {
        if (string.IsNullOrWhiteSpace(startDirectory))
            return null;

        var names = candidateNames as ICollection<string> ?? candidateNames.ToList();
        if (names.Count == 0)
            return null;

        var directory = new DirectoryInfo(Path.GetFullPath(startDirectory));
        while (directory is not null)
        {
            if (names.Any(name => directory.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return directory;
            directory = directory.Parent;
        }
        return null;
    }

    /// <summary>
    /// PC_BackUp 자신의 프로그램 루트(`_bin`의 형제 경로)를 찾는다.
    /// `XmlSettingsService`(설정 파일 위치)와 `LogManager`(로그 파일 위치)가 함께 사용한다.
    /// </summary>
    // [Codex - 2026.10.19] 명시 portable 모드와 기존 설정 위치를 보존하고 신규 설치는 사용자 데이터 경로를 쓴다.
    public static string GetDataRoot()
    {
        var programRoot = FindOwnProgramRoot();
        if (File.Exists(Path.Combine(programRoot, "portable.mode")) ||
            File.Exists(Path.Combine(programRoot, "Config", "PCBackupSettings.xml"))) return programRoot;
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PC_BackUp");
    }

    public static string FindOwnProgramRoot()
    {
        var binFolder = FindAncestorNamed(AppContext.BaseDirectory, new[] { "_bin" });
        return binFolder?.Parent?.FullName ?? AppContext.BaseDirectory;

    }
}
