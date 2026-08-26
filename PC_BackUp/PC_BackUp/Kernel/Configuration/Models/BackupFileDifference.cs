namespace PC_BackUp;

public sealed class BackupFileDifference
{
    public required string RelativePath { get; init; }
    public required string Status { get; init; }
}
