namespace PC_BackUp;

// [Codex - 2026.10.19] 단일 대용량 파일도 버퍼마다 취소·바이트 진행·상한을 확인한다.
internal sealed class MappedProgress : IProgress<int>
{
    private readonly IProgress<int>? m_oProgress;
    private readonly int m_iStart, m_iSpan;
    public MappedProgress(IProgress<int>? progress, int start, int span) { m_oProgress = progress; m_iStart = start; m_iSpan = span; }
    public void Report(int value) => m_oProgress?.Report(m_iStart + value * m_iSpan / 100);
}

internal static class FileTransfer
{
    public static long Copy(Stream input, Stream output, CancellationToken token, Action<long>? report = null, long maximum = long.MaxValue)
    {
        var buffer = new byte[81920];
        long total = 0;
        while (true)
        {
            token.ThrowIfCancellationRequested();
            var count = input.Read(buffer, 0, buffer.Length);
            if (count == 0) break;
            if (count > maximum - total) throw new InvalidDataException("파일 처리 크기 제한을 초과했습니다.");
            output.Write(buffer, 0, count);
            total += count;
            report?.Invoke(count);
        }
        token.ThrowIfCancellationRequested();
        return total;
    }

    public static void CopyFile(string source, string destination, CancellationToken token, Action<long>? report = null)
    {
        using var input = File.OpenRead(source);
        using var output = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None);
        Copy(input, output, token, report);
        output.Flush(true);
    }
}
