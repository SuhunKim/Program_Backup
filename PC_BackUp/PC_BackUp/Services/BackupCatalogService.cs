using System.Globalization;
using System.Text.RegularExpressions;

namespace PC_BackUp.Services;

public sealed partial class BackupCatalogService
{
    // 여러 화면(01~04)이 이 서비스 인스턴스 하나를 공유하고, 01 화면의 백업 실행은
    // 백그라운드 스레드(Task.Run)에서 Refresh()를 호출하는 동안 다른 화면은 UI 스레드에서
    // 동시에 Refresh()/GetByDate()/GetAll()을 호출할 수 있다(예: 백업 진행 중 탭 전환).
    // Dictionary는 동시 읽기/쓰기에 안전하지 않으므로, 기존 딕셔너리를 제자리에서
    // Clear()+Add()로 고치는 대신 새 딕셔너리를 다 만든 뒤 필드를 한 번에 교체하는
    // copy-on-write 방식을 쓴다. 참조 대입은 원자적이라 읽는 쪽(GetAll/GetByDate/
    // RecordsByDate)은 락 없이 항상 교체 전후 어느 한쪽의 완전한 딕셔너리만 보게 된다.
    // m_oRefreshLock은 Refresh() 호출 두 개가 겹칠 때 필드 교체 순서만 직렬화한다.
    private readonly object m_oRefreshLock = new();
    private volatile Dictionary<DateTime, List<BackupRecord>> m_oRecordsByDate = new();

    public IReadOnlyDictionary<DateTime, List<BackupRecord>> RecordsByDate => m_oRecordsByDate;

    public IReadOnlyDictionary<DateTime, List<BackupRecord>> Refresh(string backupRootPath)
    {
        var newRecordsByDate = new Dictionary<DateTime, List<BackupRecord>>();

        if (string.IsNullOrWhiteSpace(backupRootPath) || !Directory.Exists(backupRootPath))
        {
            lock (m_oRefreshLock)
                m_oRecordsByDate = newRecordsByDate;
            return RecordsByDate;
        }

        foreach (var path in Directory.EnumerateFileSystemEntries(backupRootPath, "*", SearchOption.TopDirectoryOnly))
        {
            var name = Path.GetFileName(path);
            if (!TryParseBackupFileName(name, out var createdAt, out var kind))
                continue;

            Add(newRecordsByDate, new BackupRecord
            {
                CreatedAt = createdAt,
                Kind = kind,
                FullPath = path,
                FileName = name,
                SizeBytes = File.Exists(path) ? new FileInfo(path).Length : GetDirectorySize(path)
            });
        }

        foreach (var records in newRecordsByDate.Values)
            records.Sort((left, right) => right.CreatedAt.CompareTo(left.CreatedAt));

        lock (m_oRefreshLock)
            m_oRecordsByDate = newRecordsByDate;
        return RecordsByDate;
    }

    public IReadOnlyList<BackupRecord> GetByDate(DateTime date) =>
        m_oRecordsByDate.TryGetValue(date.Date, out var records)
            ? records
            : Array.Empty<BackupRecord>();

    public IReadOnlyList<BackupRecord> GetAll() => m_oRecordsByDate.Values
        .SelectMany(value => value)
        .OrderByDescending(record => record.CreatedAt)
        .ToList();

    private static void Add(Dictionary<DateTime, List<BackupRecord>> recordsByDate, BackupRecord record)
    {
        var date = record.CreatedAt.Date;
        if (!recordsByDate.TryGetValue(date, out var records))
        {
            records = new List<BackupRecord>();
            recordsByDate.Add(date, records);
        }
        records.Add(record);
    }

    private static long GetDirectorySize(string path)
    {
        try
        {
            return Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)
                .Sum(file => new FileInfo(file).Length);
        }
        catch
        {
            return 0;
        }
    }

    /// <summary>
    ///
    /// </summary>
    private static bool TryParseBackupFileName(string name, out DateTime createdAt, out BackupKind kind)
    {
        var match = NewFormatRegex().Match(name);
        if (match.Success && DateTime.TryParseExact(
                match.Groups["date"].Value + match.Groups["time"].Value, "yyyyMMddHHmm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out createdAt))
        {
            kind = KindFromToken(match.Groups["kind"].Value);
            return true;
        }

        var legacyMatch = LegacyFormatRegex().Match(name);
        if (legacyMatch.Success && DateTime.TryParseExact(legacyMatch.Groups["ts"].Value, "yyyyMMddHHmmss",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out createdAt))
        {
            kind = KindFromToken(legacyMatch.Groups["kind"].Value);
            return true;
        }

        createdAt = default;
        kind = BackupKind.FullZip;
        return false;
    }

    private static BackupKind KindFromToken(string kindToken) =>
        kindToken.StartsWith("_files", StringComparison.OrdinalIgnoreCase)
            ? BackupKind.SelectiveFolders
            : BackupKind.FullZip;

    // 신규 형식: "yyyyMMdd - HHmm" 뒤에 선택적으로 " - 접미사"(예: 프로젝트 이름)와 " - 01" 같은 순번이 붙을 수 있다.
    // 대시 앞뒤 공백은 있어도 되고 없어도 된다(이전 세션에서 쓰던 공백 없는 형식도 그대로 인식).
    [GeneratedRegex(@"^(?<date>\d{8})\s*-\s*(?<time>\d{4})(?:\s*-\s*.+?)?(?<kind>_files\.zip|_files|\.zip)$", RegexOptions.IgnoreCase)]
    private static partial Regex NewFormatRegex();

    // 구버전 형식: <접두사>_<14자리 타임스탬프><kind>.
    [GeneratedRegex(@"^(?<prefix>.+)_(?<ts>\d{14})(?<kind>_files\.zip|_files|\.zip)$", RegexOptions.IgnoreCase)]
    private static partial Regex LegacyFormatRegex();
}
