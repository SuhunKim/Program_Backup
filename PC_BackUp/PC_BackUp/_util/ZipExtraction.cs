using System.IO.Compression;

namespace PC_BackUp;

// [Codex - 2026.10.19] 경로·중복·용량·엔트리 수를 사전 검증하고 실제 해제 바이트도 제한한다.
internal static class ZipExtraction
{
    internal const int MaximumEntries = 100_000;
    internal const long MaximumBytes = 100L * 1024 * 1024 * 1024;
    public static void SafeExtractToDirectory(string zipPath, string destinationRoot,
        CancellationToken token = default, IProgress<int>? progress = null)
    {
        var root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(destinationRoot));
        PathSafety.EnsureNoReparsePoints(root);
        using var archive = ZipFile.OpenRead(zipPath);
        if (archive.Entries.Count > MaximumEntries) throw new InvalidDataException("ZIP 항목 수 제한(100,000개)을 초과했습니다.");
        var targets = new List<(ZipArchiveEntry Entry, string Path)>();
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        long total = 0;
        foreach (var entry in archive.Entries)
        {
            token.ThrowIfCancellationRequested();
            var name = entry.FullName.Replace('\\', '/');
            if (Path.IsPathRooted(name) || name.Contains(':') || name.Split('/', StringSplitOptions.RemoveEmptyEntries).Any(part => part is "." or ".."))
                throw new InvalidDataException("ZIP 항목의 상대 경로가 올바르지 않습니다.");
            var target = Path.GetFullPath(Path.Combine(root, entry.FullName));
            if (!target.StartsWith(string.Format("{0}{1}", root, Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase) || !names.Add(target))
                throw new InvalidDataException("ZIP에 경로 이탈 또는 중복 항목이 있습니다.");
            if (((entry.ExternalAttributes >> 16) & 0xF000) == 0xA000) throw new InvalidDataException("ZIP 연결 파일은 허용하지 않습니다.");
            if (entry.Length > MaximumBytes - total) throw new InvalidDataException("ZIP 해제 크기 제한(100GiB)을 초과했습니다.");
            total += entry.Length;
            targets.Add((entry, target));
        }
        var drive = new DriveInfo(Path.GetPathRoot(root)!);
        if (drive.AvailableFreeSpace < total + 64L * 1024 * 1024) throw new IOException("ZIP 해제에 필요한 임시 디스크 공간이 부족합니다.");
        long processed = 0;
        foreach (var item in targets)
        {
            token.ThrowIfCancellationRequested();
            if (item.Entry.FullName.EndsWith('/') || item.Entry.FullName.EndsWith('\\'))
            { Directory.CreateDirectory(item.Path); continue; }
            PathSafety.EnsureNoReparsePoints(item.Path);
            Directory.CreateDirectory(Path.GetDirectoryName(item.Path)!);
            PathSafety.EnsureNoReparsePoints(item.Path);
            using var input = item.Entry.Open();
            using var output = new FileStream(item.Path, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            var length = FileTransfer.Copy(input, output, token, count =>
            {
                processed += count;
                progress?.Report((int)Math.Min(100, processed * 100d / Math.Max(1, total)));
            }, item.Entry.Length);
            if (length != item.Entry.Length) throw new InvalidDataException("ZIP 해제 크기가 기록과 다릅니다.");
        }
        progress?.Report(100);
    }
}
