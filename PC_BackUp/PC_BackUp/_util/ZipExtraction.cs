using System.IO.Compression;

namespace PC_BackUp;

/// <summary>
/// .NET의 ZipFile.ExtractToDirectory는 .NET Core 2.1부터 zip-slip("..")을 내부적으로 막아주지만,
/// 그건 프레임워크가 우연히 제공하는 보호일 뿐 우리 코드가 보장하는 방어는 아니다.
/// 백업/복원처럼 신뢰 경계를 넘는 zip 해제에는 엔트리별 경로를 직접 검증해 명시적으로 방어한다.
/// </summary>
internal static class ZipExtraction
{
    public static void SafeExtractToDirectory(string zipPath, string destinationRoot)
    {
        var normalizedRoot = Path.TrimEndingDirectorySeparator(Path.GetFullPath(destinationRoot));
        using var archive = ZipFile.OpenRead(zipPath);
        foreach (var entry in archive.Entries)
        {
            var targetPath = Path.GetFullPath(Path.Combine(normalizedRoot, entry.FullName));
            if (!targetPath.Equals(normalizedRoot, StringComparison.OrdinalIgnoreCase) &&
                !targetPath.StartsWith(string.Format("{0}{1}", normalizedRoot, Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException(string.Format("백업 zip에 대상 폴더를 벗어나는 항목이 있습니다: {0}", entry.FullName));

            // 디렉터리 엔트리(zip 안에서 이름이 '/' 또는 '\'로 끝나는 항목)는 폴더만 만들고 넘어간다.
            if (entry.FullName.EndsWith('/') || entry.FullName.EndsWith('\\'))
            {
                Directory.CreateDirectory(targetPath);
                continue;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(targetPath)!);
            entry.ExtractToFile(targetPath, overwrite: true);
        }
    }
}
