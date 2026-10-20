using System.IO.Compression;
using System.Text;
using System.Text.Json;
using PC_BackUp.Services;

namespace PC_BackUp;

// [Codex - 2026.10.19] 구조화 작업 결과·전체 예외와 기록 실패 상태를 보존하고 진단 ZIP을 내보낸다.
public sealed class LogManager
{
    private static readonly object m_oWriteLock = new();
    private static string m_sLastWriteError = string.Empty;
    private readonly string m_sDirectory;
    public static string LastWriteError => Volatile.Read(ref m_sLastWriteError);
    private sealed class Event
    {
        public Event() { }
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
        public string OperationId { get; set; } = Guid.NewGuid().ToString("N");
        public string Operation { get; set; } = string.Empty;
        public string Level { get; set; } = "INFO";
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; }
        public IReadOnlyList<OperationFileError> Errors { get; set; } = Array.Empty<OperationFileError>();
    }

    public LogManager() => m_sDirectory = Path.Combine(ProgramPaths.GetDataRoot(), "Config", "Logs");
    public void LogInfo(string message) => Write(new Event { Message = message });
    public void LogError(string message, Exception? exception = null) => Write(new Event { Level = "ERROR", Message = message, Exception = exception?.ToString() });
    public void LogResult(string operation, OperationResult result) => Write(new Event { Operation = operation, OperationId = result.OperationId,
        Status = result.Status.ToString(), Message = result.Message, Errors = result.Errors,
        Level = result.Status == OperationStatus.Failed ? "ERROR" : result.Status == OperationStatus.PartialSuccess ? "WARN" : "INFO" });

    private void Write(Event entry)
    {
        try
        {
            lock (m_oWriteLock)
            {
                Directory.CreateDirectory(m_sDirectory);
                using var gate = new FileStream(Path.Combine(m_sDirectory, "write.lock"), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                var file = Path.Combine(m_sDirectory, "pcbackup.jsonl");
                if (File.Exists(file) && new FileInfo(file).Length > 5 * 1024 * 1024)
                    File.Move(file, Path.Combine(m_sDirectory, "pcbackup.1.jsonl"), true);
                File.AppendAllText(file, string.Format("{0}{1}", JsonSerializer.Serialize(entry), Environment.NewLine), new UTF8Encoding(false));
                Volatile.Write(ref m_sLastWriteError, string.Empty);
            }
        }
        catch (Exception exception) { Volatile.Write(ref m_sLastWriteError, exception.Message); }
    }

    public IReadOnlyList<LogEntry> ReadRecent(int maxCount = 300)
    {
        var entries = new List<LogEntry>();
        try
        {
            if (!Directory.Exists(m_sDirectory)) return entries;
            foreach (var file in Directory.EnumerateFiles(m_sDirectory, "*.jsonl"))
            {
                foreach (var line in File.ReadLines(file, Encoding.UTF8))
                {
                    try
                    {
                        var entry = JsonSerializer.Deserialize<Event>(line);
                        if (entry is null) continue;
                        entries.Add(new LogEntry { Timestamp = entry.TimestampUtc.ToLocalTime(), Level = entry.Level,
                            Message = string.Format("[{0}] {1} {2}", entry.OperationId, entry.Status, entry.Message) });
                    }
                    catch (JsonException) { /* 불완전한 마지막 줄은 진단 원본에 보존한다. */ }
                }
            }
            var legacy = Path.Combine(m_sDirectory, "pcbackup.log");
            if (File.Exists(legacy))
            {
                foreach (var line in File.ReadLines(legacy, Encoding.UTF8))
                {
                    var parts = line.Split('\t', 3);
                    if (parts.Length == 3 && DateTime.TryParse(parts[0], out var time))
                        entries.Add(new LogEntry { Timestamp = time, Level = parts[1], Message = parts[2] });
                }
            }
        }
        catch (Exception exception) { Volatile.Write(ref m_sLastWriteError, string.Format("로그 조회 실패: {0}", exception.Message)); }
        return entries.OrderByDescending(entry => entry.Timestamp).Take(Math.Max(0, maxCount)).ToList();
    }

    public void ExportDiagnostics(string destination)
    {
        lock (m_oWriteLock)
        {
            var sources = Directory.Exists(m_sDirectory) ? Directory.GetFiles(m_sDirectory)
                .Where(file => file.EndsWith(".jsonl", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".log", StringComparison.OrdinalIgnoreCase)).ToList() : new List<string>();
            if (sources.Any(file => string.Equals(Path.GetFullPath(file), Path.GetFullPath(destination), StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("진단 파일은 로그 원본과 다른 경로로 저장하세요.");
            using var archive = ZipFile.Open(destination, ZipArchiveMode.Create);
            foreach (var source in sources) archive.CreateEntryFromFile(source, Path.GetFileName(source));
            using var writer = new StreamWriter(archive.CreateEntry("runtime.txt").Open(), Encoding.UTF8);
            writer.WriteLine(string.Format("UTC: {0:o}\nOS: {1}\nRuntime: {2}\nDataRoot: {3}\nLastLogError: {4}", DateTime.UtcNow,
                Environment.OSVersion, Environment.Version, ProgramPaths.GetDataRoot(), LastWriteError));
        }
    }
}
