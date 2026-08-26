namespace PC_BackUp.Services;

public sealed class BackupComparisonService
{
    public async Task<BackupComparisonResult> CompareAsync(BackupRecord oRecord, AppSettings oSettings, IProgress<int>? oProgress, CancellationToken oCancellationToken)
    {
        try
        {
            var oDifferences = await Task.Run(() => Compare(oRecord, oSettings.GetSourceRoot(), oSettings.BackupRootPath, oProgress, oCancellationToken));
            return new BackupComparisonResult { Differences = oDifferences };
        }
        catch (OperationCanceledException)
        {
            return new BackupComparisonResult { IsCanceled = true };
        }
    }

    private static IReadOnlyList<BackupFileDifference> Compare(BackupRecord oRecord, string sCurrentRoot, string sBackupRootPath, IProgress<int>? oProgress, CancellationToken oCancellationToken)
    {
        var sTemporaryRoot = string.Empty;
        try
        {
        var oBackupResult = ReadBackupFiles(oRecord, oCancellationToken);
        sTemporaryRoot = oBackupResult.TemporaryRoot;
        var oBackupFiles = oBackupResult.Files;
        var sBackupRoot = Path.TrimEndingDirectorySeparator(Path.GetFullPath(sBackupRootPath));
        var oCurrentFiles = Directory.Exists(sCurrentRoot)
            ? Directory.EnumerateFiles(sCurrentRoot, "*", SearchOption.AllDirectories)
                .Where(sFile => !Path.GetFullPath(sFile).StartsWith(string.Format("{0}{1}", sBackupRoot, Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))
                .ToDictionary(sFile => Path.GetRelativePath(sCurrentRoot, sFile), StringComparer.OrdinalIgnoreCase)
            : new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var oResults = new List<BackupFileDifference>();
        var oPaths = oBackupFiles.Keys.Union(oCurrentFiles.Keys, StringComparer.OrdinalIgnoreCase).ToList();
        for (var iIndex = 0; iIndex < oPaths.Count; iIndex++)
        {
            oCancellationToken.ThrowIfCancellationRequested();
            var sPath = oPaths[iIndex];
            if (!oBackupFiles.TryGetValue(sPath, out var sBackupPath)) oResults.Add(new BackupFileDifference { RelativePath = sPath, Status = "현재에만 있음" });
            else if (!oCurrentFiles.TryGetValue(sPath, out var sCurrentPath)) oResults.Add(new BackupFileDifference { RelativePath = sPath, Status = "백업에만 있음" });
            else if (!FilesEqual(sBackupPath, sCurrentPath, oCancellationToken) &&
                     !TextFilesEqualIgnoringWhitespace(sBackupPath, sCurrentPath, oCancellationToken))
                oResults.Add(new BackupFileDifference { RelativePath = sPath, Status = "내용 변경" });
            oProgress?.Report((iIndex + 1) * 100 / Math.Max(1, oPaths.Count));
        }
        return oResults;
        }
        finally
        {
            if (!string.IsNullOrEmpty(sTemporaryRoot) && Directory.Exists(sTemporaryRoot))
                try { Directory.Delete(sTemporaryRoot, true); } catch { }
        }
    }

    private static (Dictionary<string, string> Files, string TemporaryRoot) ReadBackupFiles(BackupRecord oRecord, CancellationToken oCancellationToken)
    {
        if (Directory.Exists(oRecord.FullPath))
            return (Directory.EnumerateFiles(oRecord.FullPath, "*", SearchOption.AllDirectories).ToDictionary(sFile => Path.GetRelativePath(oRecord.FullPath, sFile), StringComparer.OrdinalIgnoreCase), string.Empty);

        var sTemporaryRoot = Path.Combine(Path.GetTempPath(), "PC_BackUp", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(sTemporaryRoot);
        ZipExtraction.SafeExtractToDirectory(oRecord.FullPath, sTemporaryRoot);
        return (Directory.EnumerateFiles(sTemporaryRoot, "*", SearchOption.AllDirectories).ToDictionary(sFile => Path.GetRelativePath(sTemporaryRoot, sFile), StringComparer.OrdinalIgnoreCase), sTemporaryRoot);
    }

    private static bool FilesEqual(string sLeft, string sRight, CancellationToken oCancellationToken)
    {
        var oLeft = new FileInfo(sLeft); var oRight = new FileInfo(sRight); if (oLeft.Length != oRight.Length) return false;
        using var oLeftStream = File.OpenRead(sLeft); using var oRightStream = File.OpenRead(sRight); var baryLeft = new byte[81920]; var baryRight = new byte[81920];
        while (true) { oCancellationToken.ThrowIfCancellationRequested(); var iLeft = oLeftStream.Read(baryLeft); var iRight = oRightStream.Read(baryRight); if (iLeft != iRight) return false; if (iLeft == 0) return true; if (!baryLeft.AsSpan(0, iLeft).SequenceEqual(baryRight.AsSpan(0, iRight))) return false; }
    }

    private static bool TextFilesEqualIgnoringWhitespace(string sLeft, string sRight, CancellationToken oCancellationToken)
    {
        var sExtension = Path.GetExtension(sLeft);
        if (sExtension is not ".txt" and not ".xml" and not ".json" and not ".config" and not ".ini" and not ".cs" and not ".csproj")
            return false;

        using var oLeftReader = new StreamReader(sLeft, detectEncodingFromByteOrderMarks: true);
        using var oRightReader = new StreamReader(sRight, detectEncodingFromByteOrderMarks: true);
        while (true)
        {
            oCancellationToken.ThrowIfCancellationRequested();
            var iLeft = ReadNextNonWhitespace(oLeftReader);
            var iRight = ReadNextNonWhitespace(oRightReader);
            if (iLeft != iRight)
                return false;
            if (iLeft < 0)
                return true;
        }
    }

    private static int ReadNextNonWhitespace(TextReader oReader)
    {
        int iCharacter;
        do { iCharacter = oReader.Read(); }
        while (iCharacter >= 0 && char.IsWhiteSpace((char)iCharacter));
        return iCharacter;
    }
}
