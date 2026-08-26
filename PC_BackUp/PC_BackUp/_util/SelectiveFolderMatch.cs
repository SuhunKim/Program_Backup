namespace PC_BackUp;

/// <summary>
/// 백업/복원/이력 비교가 공통으로 쓰는 "이 상대경로가 Settings에 지정된 선택 폴더
/// (AppSettings.SelectiveFolderNames) 중 하나 밑에 있는가"를 판별하는 헬퍼. 최상위 폴더명만
/// 비교한다(그 안의 하위 경로는 상관없이 전부 포함).
/// </summary>
internal static class SelectiveFolderMatch
{
    /// <summary>relativePath는 소스 루트 기준 상대경로여야 한다(절대경로면 항상 false).</summary>
    public static bool IsInsideAnyFolder(string relativePath, IReadOnlyList<string> folderNames)
    {
        var firstSeparatorIndex = relativePath.IndexOfAny(new[]
        {
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar
        });
        if (firstSeparatorIndex <= 0)
            return false;

        var topLevelFolderName = relativePath[..firstSeparatorIndex];
        return folderNames.Contains(topLevelFolderName, StringComparer.OrdinalIgnoreCase);
    }
}
