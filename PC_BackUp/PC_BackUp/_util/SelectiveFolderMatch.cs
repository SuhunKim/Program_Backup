namespace PC_BackUp;

// [Codex - 2026.10.19] 상대 경로 폴더를 경계 구분자로 일치시켜 중첩 선택을 지원한다.
internal static class SelectiveFolderMatch
{
    public static bool IsInsideAnyFolder(string relativePath, IReadOnlyList<string> folderNames)
    {
        if (Path.IsPathRooted(relativePath)) return false;
        var candidate = relativePath.Replace('\\', '/');
        return folderNames.Any(folder => FileNaming.IsValidRelativeFolder(folder) &&
            candidate.StartsWith(string.Format("{0}/", folder.Replace('\\', '/').TrimEnd('/')), StringComparison.OrdinalIgnoreCase));
    }
}
