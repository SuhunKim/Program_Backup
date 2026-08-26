using System.Globalization;
using System.Text.RegularExpressions;

namespace PC_BackUp.Services;

public sealed partial class BackupCatalogService
{
    private readonly Dictionary<DateTime, List<BackupRecord>> m_oRecordsByDate = new();

    public IReadOnlyDictionary<DateTime, List<BackupRecord>> RecordsByDate => m_oRecordsByDate;

    public IReadOnlyDictionary<DateTime, List<BackupRecord>> Refresh(string backupRootPath)
    {
        m_oRecordsByDate.Clear();
        if (string.IsNullOrWhiteSpace(backupRootPath) || !Directory.Exists(backupRootPath))
            return RecordsByDate;

        foreach (var path in Directory.EnumerateFileSystemEntries(backupRootPath, "*", SearchOption.TopDirectoryOnly))
        {
            var name = Path.GetFileName(path);
            if (!TryParseBackupFileName(name, out var createdAt, out var kind))
                continue;

            Add(new BackupRecord
            {
                CreatedAt = createdAt,
                Kind = kind,
                FullPath = path,
                FileName = name,
                SizeBytes = File.Exists(path) ? new FileInfo(path).Length : GetDirectorySize(path)
            });
        }

        foreach (var records in m_oRecordsByDate.Values)
            records.Sort((left, right) => right.CreatedAt.CompareTo(left.CreatedAt));

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

    private void Add(BackupRecord record)
    {
        var date = record.CreatedAt.Date;
        if (!m_oRecordsByDate.TryGetValue(date, out var records))
        {
            records = new List<BackupRecord>();
            m_oRecordsByDate.Add(date, records);
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
