using System.Text.Json;

namespace PC_BackUp.Services;

// [Codex - 2026.10.19] 원본 보존·쓰기 전 저널·역순 롤백으로 다중 파일 복원을 보호한다.
internal static class RestoreTransaction
{
    private static string JournalRoot => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PC_BackUp", "RestoreTransactions");
    private sealed class Journal
    {
        public Journal() { }
        public string DestinationRoot { get; set; } = string.Empty;
        public bool Committed { get; set; }
        public List<Entry> Entries { get; set; } = new();
        public List<string> CreatedDirectories { get; set; } = new();
    }
    private sealed class Entry
    {
        public Entry() { }
        public string RelativePath { get; set; } = string.Empty;
        public bool Existed { get; set; }
        public bool Started { get; set; }
    }

    public static void Apply(IReadOnlyDictionary<string, string> sources, string destinationRoot, IProgress<int>? progress, CancellationToken token, Action? beforeApply = null)
    {
        RecoverForRoot(destinationRoot);
        var work = Path.Combine(JournalRoot, Guid.NewGuid().ToString("N"));
        var journal = new Journal { DestinationRoot = Path.GetFullPath(destinationRoot) };
        Directory.CreateDirectory(work);
        var canClean = false;
        try
        {
            // 모든 복원본과 원본을 먼저 확보한다. 이 단계 실패는 대상을 변경하지 않는다.
            foreach (var pair in sources)
            {
                token.ThrowIfCancellationRequested();
                var target = Target(journal.DestinationRoot, pair.Key);
                var index = journal.Entries.Count;
                File.Copy(pair.Value, Path.Combine(work, string.Format("new-{0}", index)));
                var existed = File.Exists(target);
                if (existed) File.Copy(target, Path.Combine(work, string.Format("old-{0}", index)));
                journal.Entries.Add(new Entry { RelativePath = pair.Key, Existed = existed });
            }
            beforeApply?.Invoke();
            Save(work, journal);
            for (var index = 0; index < journal.Entries.Count; index++)
            {
                token.ThrowIfCancellationRequested();
                var entry = journal.Entries[index];
                var target = Target(journal.DestinationRoot, entry.RelativePath);
                EnsureDirectory(Path.GetDirectoryName(target)!, work, journal);
                entry.Started = true;
                Save(work, journal); // 변경 전에 기록하여 강제 종료 시에도 원본을 복원한다.
                var pending = Pending(target, work);
                try
                {
                    File.Copy(Path.Combine(work, string.Format("new-{0}", index)), pending, true);
                    token.ThrowIfCancellationRequested();
                    File.Move(pending, target, true);
                }
                finally { if (File.Exists(pending)) File.Delete(pending); }
                progress?.Report((index + 1) * 100 / Math.Max(1, journal.Entries.Count));
            }
            journal.Committed = true;
            Save(work, journal);
            canClean = true;
        }
        catch (Exception failure)
        {
            journal.Committed = false;
            try { Rollback(work, journal); canClean = true; }
            catch (Exception rollbackFailure)
            {
                throw new AggregateException(string.Format("복원과 원상복구에 실패했습니다. 복구 기록: {0}", work), failure, rollbackFailure);
            }
            throw;
        }
        finally
        {
            if (canClean) TryClean(work);
        }
    }

    public static void RecoverPending()
    {
        if (!Directory.Exists(JournalRoot)) return;
        foreach (var work in Directory.GetDirectories(JournalRoot))
        {
            var file = Path.Combine(work, "journal.json");
            if (!File.Exists(file)) continue; // 대상 쓰기 이전에 종료된 사본은 자동 적용하지 않는다.
            var journal = Load(file);
            using var operation = OperationCoordinator.Acquire(journal.DestinationRoot);
            if (!journal.Committed) Rollback(work, journal);
            TryClean(work);
        }
    }

    private static void RecoverForRoot(string root)
    {
        if (!Directory.Exists(JournalRoot)) return;
        foreach (var work in Directory.GetDirectories(JournalRoot))
        {
            var file = Path.Combine(work, "journal.json");
            if (!File.Exists(file)) continue;
            var journal = Load(file);
            if (!string.Equals(Path.GetFullPath(root), journal.DestinationRoot, StringComparison.OrdinalIgnoreCase)) continue;
            if (!journal.Committed) Rollback(work, journal);
            TryClean(work);
        }
    }

    private static void Rollback(string work, Journal journal)
    {
        var failures = new List<Exception>();
        for (var index = journal.Entries.Count - 1; index >= 0; index--)
        {
            var entry = journal.Entries[index];
            if (!entry.Started) continue;
            try
            {
                var target = Target(journal.DestinationRoot, entry.RelativePath);
                var pending = Pending(target, work);
                if (entry.Existed)
                {
                    File.Copy(Path.Combine(work, string.Format("old-{0}", index)), pending, true);
                    File.Move(pending, target, true);
                }
                else if (File.Exists(target)) File.Delete(target);
                if (File.Exists(pending)) File.Delete(pending);
                entry.Started = false;
                Save(work, journal);
            }
            catch (Exception exception) { failures.Add(exception); }
        }
        if (failures.Count > 0) throw new AggregateException("원상복구가 완료되지 않았습니다. 복구 기록을 보존합니다.", failures);
        foreach (var directory in journal.CreatedDirectories.AsEnumerable().Reverse())
        {
            if (Directory.Exists(directory) && !Directory.EnumerateFileSystemEntries(directory).Any()) Directory.Delete(directory);
        }
    }

    private static void EnsureDirectory(string directory, string work, Journal journal)
    {
        var missing = new Stack<string>();
        for (var current = directory; !Directory.Exists(current); current = Path.GetDirectoryName(current)!)
        {
            if (string.IsNullOrEmpty(current)) throw new IOException("대상 폴더를 생성할 수 없습니다.");
            missing.Push(current);
        }
        while (missing.Count > 0)
        {
            var path = missing.Pop();
            journal.CreatedDirectories.Add(path);
            Save(work, journal);
            Directory.CreateDirectory(path);
        }
    }

    private static Journal Load(string file)
    {
        var journal = JsonSerializer.Deserialize<Journal>(File.ReadAllText(file)) ?? throw new InvalidDataException("복구 저널을 읽을 수 없습니다.");
        if (string.IsNullOrWhiteSpace(journal.DestinationRoot) || !Path.IsPathFullyQualified(journal.DestinationRoot) || journal.Entries is null || journal.CreatedDirectories is null)
            throw new InvalidDataException("복구 저널 경로·항목이 올바르지 않습니다.");
        foreach (var entry in journal.Entries) Target(journal.DestinationRoot, entry.RelativePath);
        foreach (var directory in journal.CreatedDirectories)
        {
            if (!string.Equals(directory, journal.DestinationRoot, StringComparison.OrdinalIgnoreCase) &&
                !directory.StartsWith(string.Format("{0}{1}", Path.TrimEndingDirectorySeparator(journal.DestinationRoot), Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException("복구 저널의 폴더가 대상 범위를 벗어났습니다.");
        }
        return journal;
    }

    private static void Save(string work, Journal journal)
    {
        var file = Path.Combine(work, "journal.json");
        var pending = string.Format("{0}.tmp", file);
        using (var stream = new FileStream(pending, FileMode.Create, FileAccess.Write, FileShare.None))
        { JsonSerializer.Serialize(stream, journal); stream.Flush(true); }
        File.Move(pending, file, true);
    }

    private static string Target(string root, string relative)
    {
        if (string.IsNullOrWhiteSpace(relative) || Path.IsPathRooted(relative) || relative.Contains(':'))
            throw new InvalidDataException("복원 상대 경로가 올바르지 않습니다.");
        var target = Path.GetFullPath(Path.Combine(root, relative));
        if (!target.StartsWith(string.Format("{0}{1}", Path.TrimEndingDirectorySeparator(Path.GetFullPath(root)), Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("복원 경로가 대상 폴더를 벗어났습니다.");
        return target;
    }

    private static string Pending(string target, string work) => string.Format("{0}.pcbackup-{1}.tmp", target, Path.GetFileName(work));
    private static void TryClean(string work) { try { Directory.Delete(work, true); } catch { /* 재시작 시 완료 기록 정리를 다시 시도한다. */ } }
}
