namespace PC_BackUp.Services;

public sealed class BackupComparisonService
{
    public async Task<BackupComparisonResult> CompareAsync(
        BackupRecord record,
        AppSettings settings,
        IProgress<int>? progress,
        CancellationToken cancellationToken)
    {
        try
        {
            var differences = await Task.Run(() =>
                Compare(record, settings.GetSourceRoot(), settings.BackupRootPath, progress, cancellationToken));
            return new BackupComparisonResult { Differences = differences };
        }
        catch (OperationCanceledException)
        {
            return new BackupComparisonResult { IsCanceled = true };
        }
    }

    private static IReadOnlyList<BackupFileDifference> Compare(
        BackupRecord record,
        string currentRoot,
        string backupRootPath,
        IProgress<int>? progress,
        CancellationToken cancellationToken)
    {
        var temporaryRoot = string.Empty;
        try
        {
            var backupResult = ReadBackupFiles(record, cancellationToken);
            temporaryRoot = backupResult.TemporaryRoot;
            var backupFiles = backupResult.Files;
            var backupRoot = Path.TrimEndingDirectorySeparator(Path.GetFullPath(backupRootPath));
            var currentFiles = Directory.Exists(currentRoot)
                ? Directory.EnumerateFiles(currentRoot, "*", CurrentRootEnumerationOptions)
                    .Where(file => !Path.GetFullPath(file).StartsWith(
                        string.Format("{0}{1}", backupRoot, Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))
                    .ToDictionary(file => Path.GetRelativePath(currentRoot, file), StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var results = new List<BackupFileDifference>();
            var relativePaths = backupFiles.Keys.Union(currentFiles.Keys, StringComparer.OrdinalIgnoreCase).ToList();
            for (var index = 0; index < relativePaths.Count; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var relativePath = relativePaths[index];
                if (!backupFiles.TryGetValue(relativePath, out var backupPath))
                {
                    results.Add(new BackupFileDifference { RelativePath = relativePath, Status = "현재에만 있음" });
                }
                else if (!currentFiles.TryGetValue(relativePath, out var currentPath))
                {
                    results.Add(new BackupFileDifference { RelativePath = relativePath, Status = "백업에만 있음" });
                }
                else if (!FilesEqual(backupPath, currentPath, cancellationToken) &&
                         !TextFilesEqualIgnoringWhitespace(backupPath, currentPath, cancellationToken))
                {
                    results.Add(new BackupFileDifference { RelativePath = relativePath, Status = "내용 변경" });
                }
                progress?.Report((index + 1) * 100 / Math.Max(1, relativePaths.Count));
            }
            return results;
        }
        finally
        {
            if (!string.IsNullOrEmpty(temporaryRoot) && Directory.Exists(temporaryRoot))
            {
                try { Directory.Delete(temporaryRoot, true); }
                catch { /* 임시 폴더 정리 실패는 비교 결과에 영향을 주지 않으므로 무시한다. */ }
            }
        }
    }

    // 비교 대상은 지금 실행 중인(=잠금/권한 문제가 일시적으로 있을 수 있는) 소스 트리이므로,
    // BackupService.EnumerateSourceFiles/XmlComparisonService.ReadCurrentXml과 동일하게
    // 접근 불가 항목은 건너뛰고 계속 진행한다(중간에 실패시키지 않는다).
    private static readonly EnumerationOptions CurrentRootEnumerationOptions = new()
    {
        RecurseSubdirectories = true,
        IgnoreInaccessible = true,
        AttributesToSkip = FileAttributes.ReparsePoint
    };

    private static (Dictionary<string, string> Files, string TemporaryRoot) ReadBackupFiles(
        BackupRecord record, CancellationToken cancellationToken)
    {
        if (Directory.Exists(record.FullPath))
        {
            var files = Directory.EnumerateFiles(record.FullPath, "*", SearchOption.AllDirectories)
                .ToDictionary(file => Path.GetRelativePath(record.FullPath, file), StringComparer.OrdinalIgnoreCase);
            return (files, string.Empty);
        }

        var temporaryRoot = Path.Combine(Path.GetTempPath(), "PC_BackUp", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(temporaryRoot);
        ZipExtraction.SafeExtractToDirectory(record.FullPath, temporaryRoot);
        var extractedFiles = Directory.EnumerateFiles(temporaryRoot, "*", SearchOption.AllDirectories)
            .ToDictionary(file => Path.GetRelativePath(temporaryRoot, file), StringComparer.OrdinalIgnoreCase);
        return (extractedFiles, temporaryRoot);
    }

    private static bool FilesEqual(string leftPath, string rightPath, CancellationToken cancellationToken)
    {
        var leftInfo = new FileInfo(leftPath);
        var rightInfo = new FileInfo(rightPath);
        if (leftInfo.Length != rightInfo.Length)
            return false;

        using var leftStream = File.OpenRead(leftPath);
        using var rightStream = File.OpenRead(rightPath);
        var leftBuffer = new byte[81920];
        var rightBuffer = new byte[81920];
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var leftRead = leftStream.Read(leftBuffer);
            var rightRead = rightStream.Read(rightBuffer);
            if (leftRead != rightRead)
                return false;
            if (leftRead == 0)
                return true;
            if (!leftBuffer.AsSpan(0, leftRead).SequenceEqual(rightBuffer.AsSpan(0, rightRead)))
                return false;
        }
    }

    private static bool TextFilesEqualIgnoringWhitespace(string leftPath, string rightPath, CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(leftPath);
        if (extension is not ".txt" and not ".xml" and not ".json" and not ".config" and not ".ini" and not ".cs" and not ".csproj")
            return false;

        using var leftReader = new StreamReader(leftPath, detectEncodingFromByteOrderMarks: true);
        using var rightReader = new StreamReader(rightPath, detectEncodingFromByteOrderMarks: true);
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var leftChar = ReadNextNonWhitespace(leftReader);
            var rightChar = ReadNextNonWhitespace(rightReader);
            if (leftChar != rightChar)
                return false;
            if (leftChar < 0)
                return true;
        }
    }

    private static int ReadNextNonWhitespace(TextReader reader)
    {
        int character;
        do { character = reader.Read(); }
        while (character >= 0 && char.IsWhiteSpace((char)character));
        return character;
    }
}
