namespace PC_BackUp;

/// <summary>
/// 폴더명 유효성 검사 및 파일명 접미사 정리 공용 헬퍼.
/// (구 AppSettings.IsValidFolderName — 04_SettingControl 등 여러 곳에서 재사용하기 위해 이관)
/// </summary>
internal static class FileNaming
{
    public static bool IsValidFolderName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name is "." or "..")
            return false;

        return name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0 &&
               !name.Contains(Path.DirectorySeparatorChar) &&
               !name.Contains(Path.AltDirectorySeparatorChar);
    }

    /// <summary>
    /// 백업 파일명에 선택적으로 덧붙이는 접미사(예: 프로젝트 이름)를 정리한다.
    /// 폴더명과 달리 접미사는 통째로 거부하지 않고, Windows 파일명에 쓸 수 없는 문자만 조용히 제거한다.
    /// 결과가 비어있어도 문제 없다(접미사는 기본적으로 없어도 되는 값이다).
    /// </summary>
    public static string SanitizeSuffix(string suffix)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var cleaned = new string(suffix.Where(c => !invalidChars.Contains(c)).ToArray()).Trim();
        return cleaned is "." or ".." ? string.Empty : cleaned;
    }
}
