using System.Text;

namespace PC_BackUp;

/// <summary>
/// PC_BackUp 자신의 작업 로그(성공/실패)를 파일로 남기고 다시 읽어오는 역할.
/// UI를 참조하지 않으며, 로그 쓰기/읽기 실패가 앱 동작에 영향을 주지 않도록 내부에서 예외를 삼킨다.
/// </summary>
public sealed class LogManager
{
    private const string LogFolderName = "Logs";
    private const string LogFileName = "pcbackup.log";
    private readonly string m_sLogFilePath;

    public LogManager()
    {
        string programRoot;
        try
        {
            programRoot = ProgramPaths.FindOwnProgramRoot();
        }
        catch
        {
            programRoot = AppContext.BaseDirectory;
        }

        m_sLogFilePath = Path.Combine(programRoot, "Config", LogFolderName, LogFileName);
    }

    public void LogInfo(string message) => Write("INFO", message);

    public void LogError(string message, Exception? exception = null) =>
        Write("ERROR", exception is null ? message : string.Format("{0} — {1}", message, exception.Message));

    public IReadOnlyList<LogEntry> ReadRecent(int maxCount = 300)
    {
        try
        {
            if (!File.Exists(m_sLogFilePath))
                return Array.Empty<LogEntry>();

            var entries = new List<LogEntry>();
            foreach (var line in File.ReadAllLines(m_sLogFilePath, Encoding.UTF8))
            {
                var parts = line.Split('\t', 3);
                if (parts.Length != 3)
                    continue;
                if (!DateTime.TryParse(parts[0], out var timestamp))
                    continue;

                entries.Add(new LogEntry { Timestamp = timestamp, Level = parts[1], Message = parts[2] });
            }

            entries.Reverse();
            return entries.Take(maxCount).ToList();
        }
        catch
        {
            return Array.Empty<LogEntry>();
        }
    }

    private void Write(string level, string message)
    {
        try
        {
            var directory = Path.GetDirectoryName(m_sLogFilePath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            var sanitized = message.Replace("\r\n", " ").Replace('\n', ' ').Replace('\t', ' ');
            var line = string.Format("{0:yyyy-MM-dd HH:mm:ss}\t{1}\t{2}{3}", DateTime.Now, level, sanitized, Environment.NewLine);
            File.AppendAllText(m_sLogFilePath, line, Encoding.UTF8);
        }
        catch
        {
            // 로깅 실패가 앱 동작에 영향을 주면 안 되므로 조용히 무시한다.
        }
    }
}
