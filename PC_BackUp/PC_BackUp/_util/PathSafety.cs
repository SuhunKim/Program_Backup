namespace PC_BackUp;

// [Codex - 2026.10.19] 기존 경로 구성요소 전체의 연결 경로를 거부하고 파일 I/O 직전에 재검사한다.
internal static class PathSafety
{
    public static string ResolveChild(string root, string relative)
    {
        if (string.IsNullOrWhiteSpace(relative) || Path.IsPathRooted(relative) ||
            relative.Split(new[] { '/', '\\' }).Any(part => !ValidPart(part)))
            throw new InvalidDataException("파일 상대 경로가 올바르지 않습니다.");
        var fullRoot = Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
        var path = Path.GetFullPath(Path.Combine(fullRoot, relative));
        if (!path.StartsWith(string.Format("{0}{1}", fullRoot, Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("파일 경로가 대상 루트를 벗어났습니다.");
        EnsureNoReparsePoints(path);
        return path;
    }

    private static bool ValidPart(string part)
    {
        if (!FileNaming.IsValidFolderName(part) || part != part.TrimEnd(' ', '.')) return false;
        var stem = part.Split('.')[0].ToUpperInvariant();
        return stem is not "CON" and not "PRN" and not "AUX" and not "NUL" &&
            !(stem.Length == 4 && (stem.StartsWith("COM", StringComparison.Ordinal) || stem.StartsWith("LPT", StringComparison.Ordinal)) && stem[3] is >= '1' and <= '9');
    }

    public static void EnsureNoReparsePoints(string path)
    {
        var full = Path.GetFullPath(path);
        var root = Path.GetPathRoot(full) ?? throw new InvalidDataException("경로 루트를 확인할 수 없습니다.");
        var current = root;
        Check(current);
        foreach (var part in full[root.Length..].Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (!ValidPart(part)) throw new InvalidDataException(string.Format("지원하지 않는 경로 항목: {0}", part));
            current = Path.Combine(current, part);
            Check(current);
        }
    }

    private static void Check(string path)
    {
        try
        {
            if ((File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0)
                throw new InvalidDataException(string.Format("연결 경로는 안전한 복원을 보장할 수 없어 사용할 수 없습니다: {0}", path));
        }
        catch (FileNotFoundException) { }
        catch (DirectoryNotFoundException) { }
    }

    public static IEnumerable<string> EnumerateFiles(string root)
    {
        EnsureNoReparsePoints(root);
        var pending = new Stack<string>();
        pending.Push(root);
        while (pending.Count > 0)
        {
            var directory = pending.Pop();
            EnsureNoReparsePoints(directory);
            foreach (var entry in Directory.EnumerateFileSystemEntries(directory))
            {
                EnsureNoReparsePoints(entry);
                if ((File.GetAttributes(entry) & FileAttributes.Directory) != 0) pending.Push(entry);
                else yield return entry;
            }
        }
    }
}
