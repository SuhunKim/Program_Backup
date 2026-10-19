using System.IO.Compression;
using System.Security.Cryptography;
using System.Text.Json;

namespace PC_BackUp.Services;

// [Codex - 2026.10.19] 백업 파일 자체에 출처·범위·내용 해시를 보관한다.
public sealed class BackupManifest
{
    public int Version { get; set; } = 1;
    public string BackupId { get; set; } = Guid.NewGuid().ToString("N");
    public string ProjectName { get; set; } = string.Empty;
    public string SourceRoot { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public BackupKind Kind { get; set; }
    public List<string> SelectedFolders { get; set; } = new();
    public bool Complete { get; set; }
    public List<BackupManifestFile> Files { get; set; } = new();
}

public sealed class BackupManifestFile
{
    public string Path { get; set; } = string.Empty;
    public long Length { get; set; }
    public string Sha256 { get; set; } = string.Empty;
}

internal static class BackupManifestService
{
    public const string FileName = ".pcbackup-manifest.json";
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static BackupManifest Create(AppSettings settings, BackupKind kind, IReadOnlyList<string> files, CancellationToken token)
    {
        var root = Path.GetFullPath(settings.GetSourceRoot());
        var manifest = new BackupManifest { ProjectName = settings.TargetProjectName, SourceRoot = root,
            Kind = kind, SelectedFolders = settings.GetSelectiveFolderNames().ToList(), Complete = true };
        foreach (var file in files)
        {
            token.ThrowIfCancellationRequested();
            var relative = Normalize(Path.GetRelativePath(root, file));
            if (relative.Equals(FileName, StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException("원본에 백업 메타데이터 예약 파일명이 있습니다.");
            using var stream = File.OpenRead(file);
            manifest.Files.Add(new BackupManifestFile { Path = relative, Length = stream.Length, Sha256 = Hash(stream, token) });
        }
        return manifest;
    }

    public static void Write(string backup, BackupKind kind, BackupManifest manifest)
    {
        var json = JsonSerializer.SerializeToUtf8Bytes(manifest, JsonOptions);
        if (json.Length > 16 * 1024 * 1024) throw new InvalidDataException("백업 메타데이터 크기 제한을 초과했습니다.");
        if (kind == BackupKind.FullZip)
        {
            using var archive = ZipFile.Open(backup, ZipArchiveMode.Update);
            using var stream = archive.CreateEntry(FileName).Open();
            stream.Write(json);
        }
        else File.WriteAllBytes(Path.Combine(backup, FileName), json);
    }

    public static BackupManifest? Read(string backup)
    {
        if (Directory.Exists(backup))
        {
            var file = Path.Combine(backup, FileName);
            if (!File.Exists(file)) return null;
            using var stream = File.OpenRead(file);
            return Deserialize(stream, stream.Length);
        }
        using var archive = ZipFile.OpenRead(backup);
        var entries = archive.Entries.Where(entry => Normalize(entry.FullName).Equals(FileName, StringComparison.OrdinalIgnoreCase)).ToList();
        if (entries.Count == 0) return null;
        if (entries.Count != 1) throw new InvalidDataException("백업 메타데이터가 중복되었습니다.");
        using var input = entries[0].Open();
        return Deserialize(input, entries[0].Length);
    }

    private static BackupManifest Deserialize(Stream stream, long length)
    {
        if (length > 16 * 1024 * 1024) throw new InvalidDataException("백업 메타데이터 크기 제한을 초과했습니다.");
        var manifest = JsonSerializer.Deserialize<BackupManifest>(stream) ?? throw new InvalidDataException("백업 메타데이터가 비어 있습니다.");
        if (manifest.Version != 1 || !manifest.Complete || string.IsNullOrWhiteSpace(manifest.BackupId) ||
            string.IsNullOrWhiteSpace(manifest.SourceRoot) || manifest.Files is null || manifest.SelectedFolders is null || manifest.Files.Count == 0)
            throw new InvalidDataException("백업 메타데이터 버전·완료 상태·파일 목록이 올바르지 않습니다.");
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in manifest.Files)
        {
            if (item is null || string.IsNullOrWhiteSpace(item.Path) || Path.IsPathRooted(item.Path) ||
                Normalize(item.Path).Split('/').Any(part => part is ".." or "." or "") ||
                item.Path.Contains(':') || item.Length < 0 || item.Sha256 is null || item.Sha256.Length != 64 ||
                !seen.Add(Normalize(item.Path)) || Normalize(item.Path).Equals(FileName, StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException("백업 메타데이터의 파일 경로·해시가 올바르지 않습니다.");
        }
        return manifest;
    }

    public static (BackupManifest? Manifest, string Error) Inspect(string backup)
    {
        try { return (Read(backup), string.Empty); }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or JsonException or InvalidDataException)
        { return (null, exception.Message); }
    }

    public static void Verify(string backup, BackupManifest manifest, CancellationToken token)
    {
        var expected = manifest.Files.ToDictionary(item => Normalize(item.Path), StringComparer.OrdinalIgnoreCase);
        if (Directory.Exists(backup))
        {
            var paths = Directory.EnumerateFiles(backup, "*", new EnumerationOptions { RecurseSubdirectories = true,
                IgnoreInaccessible = false, AttributesToSkip = FileAttributes.ReparsePoint })
                .Where(file => !Normalize(Path.GetRelativePath(backup, file)).Equals(FileName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (paths.Count != expected.Count) throw new InvalidDataException("백업 파일 수가 메타데이터와 다릅니다.");
            foreach (var path in paths)
            {
                using var stream = File.OpenRead(path);
                Check(Normalize(Path.GetRelativePath(backup, path)), stream.Length, stream, expected, token);
            }
        }
        else
        {
            using var archive = ZipFile.OpenRead(backup);
            var entries = archive.Entries.Where(entry => !entry.FullName.EndsWith('/') && !entry.FullName.EndsWith('\\') &&
                !Normalize(entry.FullName).Equals(FileName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (entries.Count != expected.Count) throw new InvalidDataException("백업 파일 수가 메타데이터와 다릅니다.");
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var entry in entries)
            {
                if (!seen.Add(Normalize(entry.FullName))) throw new InvalidDataException("중복 ZIP 항목입니다.");
                using var stream = entry.Open();
                Check(Normalize(entry.FullName), entry.Length, stream, expected, token);
            }
        }
    }

    public static BackupManifest? ValidateForRestore(BackupRecord record, AppSettings settings, CancellationToken token)
    {
        var manifest = Read(record.FullPath);
        if (manifest is null) return null; // 구버전 백업은 출처·검증 미확인 상태로 호환한다.
        if (!string.Equals(Path.TrimEndingDirectorySeparator(Path.GetFullPath(manifest.SourceRoot)),
                Path.TrimEndingDirectorySeparator(Path.GetFullPath(settings.GetSourceRoot())), StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(manifest.ProjectName, settings.TargetProjectName, StringComparison.Ordinal))
            throw new InvalidDataException("백업 출처와 현재 프로젝트·원본 경로가 다릅니다.");
        Verify(record.FullPath, manifest, token);
        return manifest;
    }

    private static void Check(string path, long length, Stream stream, IReadOnlyDictionary<string, BackupManifestFile> expected, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        if (!expected.TryGetValue(path, out var item) || item.Length != length ||
            !string.Equals(item.Sha256, Hash(stream, token), StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException(string.Format("백업 검증 실패: {0}", path));
    }

    public static string Hash(Stream stream, CancellationToken token)
    {
        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        var buffer = new byte[81920];
        int count;
        while ((count = stream.Read(buffer, 0, buffer.Length)) > 0) { token.ThrowIfCancellationRequested(); hash.AppendData(buffer, 0, count); }
        return Convert.ToHexString(hash.GetHashAndReset());
    }

    public static string Normalize(string path) => path.Replace('\\', '/');
}
