using System.Security.Cryptography;
using System.Text;

namespace PC_BackUp.Services;

// [Codex - 2026.10.19] 동일 경로의 읽기/쓰기를 프로세스 간 비차단 세마포어로 보호한다.
internal sealed class OperationCoordinator : IDisposable
{
    private static int m_iActiveCount;
    private static readonly AsyncLocal<HashSet<string>?> m_oOwnedPaths = new();
    private readonly HashSet<string>? m_oPreviousPaths = m_oOwnedPaths.Value;
    private readonly List<Semaphore> m_oLocks = new();
    private bool m_bDisposed;
    public static bool HasActiveOperations => Volatile.Read(ref m_iActiveCount) > 0;

    public static OperationCoordinator Acquire(params string[] paths)
    {
        var operation = new OperationCoordinator();
        var owned = new HashSet<string>(m_oOwnedPaths.Value ?? new HashSet<string>(), StringComparer.Ordinal);
        try
        {
            foreach (var key in paths.Where(path => !string.IsNullOrWhiteSpace(path))
                         .Select(path => Path.TrimEndingDirectorySeparator(Path.GetFullPath(path)).ToUpperInvariant())
                         .Distinct(StringComparer.Ordinal).OrderBy(path => path, StringComparer.Ordinal))
            {
                if (!owned.Add(key)) continue; // 같은 비동기 복원 흐름의 안전 백업·적용은 이미 잠근 경로를 재사용한다.
                var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(key)));
                var gate = new Semaphore(1, 1, string.Format("Global\\PC_BackUp_{0}", hash));
                if (!gate.WaitOne(0))
                {
                    gate.Dispose();
                    throw new InvalidOperationException("같은 경로에서 다른 백업·복원·비교 작업이 진행 중입니다.");
                }
                operation.m_oLocks.Add(gate);
            }
            m_oOwnedPaths.Value = owned;
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
        m_oOwnedPaths.Value = m_oPreviousPaths;
        Interlocked.Decrement(ref m_iActiveCount);
    }
}
