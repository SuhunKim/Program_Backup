using System.Security.Cryptography;
using System.Text;

namespace PC_BackUp.Services;

// [Codex - 2026.10.19] 동일 경로의 읽기/쓰기를 프로세스 간 비차단 세마포어로 보호한다.
internal sealed class OperationCoordinator : IDisposable
{
    private static int m_iActiveCount;
    private readonly List<Semaphore> m_oLocks = new();
    private bool m_bDisposed;
    public static bool HasActiveOperations => Volatile.Read(ref m_iActiveCount) > 0;

    public static OperationCoordinator Acquire(params string[] paths)
    {
        var operation = new OperationCoordinator();
        try
        {
            foreach (var key in paths.Where(path => !string.IsNullOrWhiteSpace(path))
                         .Select(path => Path.TrimEndingDirectorySeparator(Path.GetFullPath(path)).ToUpperInvariant())
                         .Distinct(StringComparer.Ordinal).OrderBy(path => path, StringComparer.Ordinal))
            {
                var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(key)));
                var gate = new Semaphore(1, 1, string.Format("Global\\PC_BackUp_{0}", hash));
                if (!gate.WaitOne(0))
                {
                    gate.Dispose();
                    throw new InvalidOperationException("같은 경로에서 다른 백업·복원·비교 작업이 진행 중입니다.");
                }
                operation.m_oLocks.Add(gate);
            }
            Interlocked.Increment(ref m_iActiveCount);
            return operation;
        }
        catch
        {
            foreach (var gate in operation.m_oLocks) { gate.Release(); gate.Dispose(); }
            throw;
        }
    }

    public void Dispose()
    {
        if (m_bDisposed) return;
        m_bDisposed = true;
        foreach (var gate in m_oLocks) { gate.Release(); gate.Dispose(); }
        Interlocked.Decrement(ref m_iActiveCount);
    }
}
