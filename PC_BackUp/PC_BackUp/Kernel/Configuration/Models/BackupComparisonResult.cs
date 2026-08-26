namespace PC_BackUp;

public sealed class BackupComparisonResult
{
    public bool IsCanceled { get; init; }
    public IReadOnlyList<BackupFileDifference> Differences { get; init; } = Array.Empty<BackupFileDifference>();
}
