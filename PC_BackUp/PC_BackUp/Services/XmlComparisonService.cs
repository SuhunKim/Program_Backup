using System.IO.Compression;
using System.Text;
using System.Xml;

namespace PC_BackUp.Services;

public sealed class XmlComparisonService
{
    /// <summary>
    /// sourceRecord가 null이면 "현재 적용된 파일"을 비교 기준(Source)으로 삼고, 값이 있으면 그
    /// 백업을 기준으로 삼는다. destinationRecord(비교 대상)는 항상 특정 백업이다 — 백업끼리도
    /// 비교할 수 있도록 일반화한 것.
    /// </summary>
    public async Task<IReadOnlyList<XmlDifference>> CompareAsync(
        AppSettings settings,
        BackupRecord? sourceRecord,
        BackupRecord destinationRecord,
        IProgress<int>? progress = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await CompareCoreAsync(settings, sourceRecord, destinationRecord, progress, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw new OperationCanceledException("비교가 취소되었습니다.");
        }
    }

    private async Task<IReadOnlyList<XmlDifference>> CompareCoreAsync(
        AppSettings settings,
        BackupRecord? sourceRecord,
        BackupRecord destinationRecord,
        IProgress<int>? progress,
        CancellationToken cancellationToken)
    {
        return await Task.Run(() =>
        {
            var sourceDocuments = sourceRecord is null
                ? ReadCurrentXml(settings.GetSourceRoot(), settings.BackupRootPath, cancellationToken)
                : ReadBackupXml(sourceRecord, cancellationToken);
            var destinationDocuments = ReadBackupXml(destinationRecord, cancellationToken);
            var sourceLabel = sourceRecord is null ? "현재" : sourceRecord.FileName;
            var destinationLabel = destinationRecord.FileName;
            var differences = new List<XmlDifference>();

            var relativePaths = sourceDocuments.Keys.Union(destinationDocuments.Keys, StringComparer.OrdinalIgnoreCase).ToList();
            for (var index = 0; index < relativePaths.Count; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var relativePath = relativePaths[index];
                sourceDocuments.TryGetValue(relativePath, out var sourceXml);
                destinationDocuments.TryGetValue(relativePath, out var destinationXml);

                var sourceValues = FlattenXml(sourceXml, cancellationToken);
                var destinationValues = FlattenXml(destinationXml, cancellationToken);
                var hasDifference = sourceValues.Keys.Union(destinationValues.Keys, StringComparer.Ordinal)
                    .Any(xmlPath => !sourceValues.TryGetValue(xmlPath, out var sourceValue) ||
                                    !destinationValues.TryGetValue(xmlPath, out var destinationValue) ||
                                    !string.Equals(sourceValue, destinationValue, StringComparison.Ordinal));
                if (hasDifference)
                    differences.Add(new XmlDifference
                    {
                        RelativeFilePath = relativePath,
                        XmlPath = "(파일 전체)",
                        CurrentValue = string.Format("{0} XML", sourceLabel),
                        BackupValue = string.Format("{0} XML", destinationLabel)
                    });
                progress?.Report((index + 1) * 100 / Math.Max(1, relativePaths.Count));
            }

            return (IReadOnlyList<XmlDifference>)differences
                .OrderBy(item => item.RelativeFilePath, StringComparer.OrdinalIgnoreCase)
                .ThenBy(item => item.XmlPath, StringComparer.Ordinal)
                .ToList();
        }, cancellationToken);
    }

    /// <summary>
    /// 선택한 차이점들을 destinationRecord(비교 대상 백업)의 값으로 currentRoot(실제 실행 중인
    /// 파일)에 적용한다. Source가 "현재"가 아닌 다른 백업이었더라도 이 메서드는 항상 현재 실행
    /// 파일에 쓰므로, 백업끼리 비교한 결과에 대해서는 호출자(HistoryControl)가 애초에 적용 버튼을
    /// 비활성화해서 호출 자체를 막는다.
    /// </summary>
    public async Task<OperationResult> ApplySelectedAsync(
        string currentRoot,
        BackupRecord destinationRecord,
        IEnumerable<XmlDifference> selected,
        IProgress<int>? progress = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var items = selected.ToList();
            if (items.Count == 0)
                return OperationResult.Failure("적용할 항목을 선택하세요.");

            return await Task.Run(() =>
            {
                var backupDocuments = ReadBackupXml(destinationRecord, cancellationToken);
                var applied = 0;
                var skipped = 0;
                var groups = items.GroupBy(item => item.RelativeFilePath, StringComparer.OrdinalIgnoreCase).ToList();
                for (var index = 0; index < groups.Count; index++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var group = groups[index];
                    var filePath = Path.GetFullPath(Path.Combine(currentRoot, group.Key));
                    EnsureChildPath(filePath, currentRoot);
                    if (!backupDocuments.TryGetValue(group.Key, out var backupXml))
                    {
                        skipped += group.Count();
                        progress?.Report((index + 1) * 100 / Math.Max(1, groups.Count));
                        continue;
                    }
                    var temporaryPath = string.Format("{0}.pcbackup.tmp", filePath);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                    File.WriteAllText(temporaryPath, backupXml, Encoding.UTF8);
                    File.Move(temporaryPath, filePath, true);
                    applied++;
                    progress?.Report((index + 1) * 100 / Math.Max(1, groups.Count));
                }

                return OperationResult.Success(skipped > 0
                    ? string.Format("선택 설정 적용 완료: {0}개 (적용 불가 {1}개)", applied, skipped)
                    : string.Format("선택 설정 적용 완료: {0}개", applied));
            }, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return OperationResult.Failure("적용이 취소되었습니다.");
        }
        catch (Exception exception)
        {
            return OperationResult.Failure(string.Format("설정 적용 중 오류가 발생했습니다.\n{0}", exception.Message));
        }
    }

    private static Dictionary<string, string> ReadCurrentXml(
        string sourceRoot,
        string backupRoot,
        CancellationToken cancellationToken)
    {
        var documents = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (!Directory.Exists(sourceRoot))
            return documents;

        var normalizedBackupRoot = string.IsNullOrWhiteSpace(backupRoot)
            ? string.Empty
            : Path.TrimEndingDirectorySeparator(Path.GetFullPath(backupRoot));
        var options = new EnumerationOptions
        {
            RecurseSubdirectories = true,
            IgnoreInaccessible = true,
            AttributesToSkip = FileAttributes.ReparsePoint
        };

        foreach (var file in Directory.EnumerateFiles(sourceRoot, "*.xml", options))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var fullPath = Path.GetFullPath(file);
            if (!string.IsNullOrEmpty(normalizedBackupRoot) &&
                (fullPath.Equals(normalizedBackupRoot, StringComparison.OrdinalIgnoreCase) ||
                 fullPath.StartsWith(string.Format("{0}{1}", normalizedBackupRoot, Path.DirectorySeparatorChar),
                     StringComparison.OrdinalIgnoreCase)))
                continue;
            documents[Path.GetRelativePath(sourceRoot, file)] = File.ReadAllText(file);
        }
        return documents;
    }

    private static Dictionary<string, string> ReadBackupXml(BackupRecord record, CancellationToken cancellationToken)
    {
        var documents = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (Directory.Exists(record.FullPath))
        {
            // 구버전 선별 백업(폴더 복사 방식)은 폴더 그대로 읽는다.
            foreach (var file in Directory.EnumerateFiles(record.FullPath, "*.xml", SearchOption.AllDirectories))
            {
                cancellationToken.ThrowIfCancellationRequested();
                documents[Path.GetRelativePath(record.FullPath, file)] = File.ReadAllText(file);
            }
            return documents;
        }

        // 전체 백업 또는 압축된 선별 백업(zip)은 압축을 풀지 않고 바로 읽는다.
        using var archive = ZipFile.OpenRead(record.FullPath);
        foreach (var entry in archive.Entries.Where(entry => entry.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using var reader = new StreamReader(entry.Open(), Encoding.UTF8, true);
            documents[entry.FullName.Replace('/', Path.DirectorySeparatorChar)] = ReadText(reader, cancellationToken);
        }
        return documents;
    }

    private static Dictionary<string, string> FlattenXml(string? xml, CancellationToken cancellationToken)
    {
        var values = new Dictionary<string, string>(StringComparer.Ordinal);
        if (string.IsNullOrWhiteSpace(xml))
            return values;

        try
        {
            var document = LoadXml(xml);
            if (document.DocumentElement is not null)
                VisitElement(document.DocumentElement, 1, string.Empty, values, cancellationToken);
        }
        catch (XmlException)
        {
            // 손상된 XML은 비교 목록에서 제외하고 서비스 호출 자체는 계속 진행한다.
        }
        return values;
    }

    private static string ReadText(TextReader reader, CancellationToken cancellationToken)
    {
        var builder = new StringBuilder();
        var buffer = new char[81920];
        int count;
        while ((count = reader.Read(buffer, 0, buffer.Length)) > 0)
        {
            cancellationToken.ThrowIfCancellationRequested();
            builder.Append(buffer, 0, count);
        }
        return builder.ToString();
    }

    /// <summary>
    /// sameNameIndex(같은 이름의 형제 중 몇 번째인지)는 호출자(부모)가 자식들을 한 번 순회하며
    /// 이름별 개수를 누적해 미리 계산해서 넘겨준다. 예전에는 이 메서드 안에서 매번 이전 형제를
    /// 전부 거꾸로 훑어 계산했는데(O(N²), 형제 수가 많은 XML에서 수십 분씩 멈추는 원인이었다),
    /// 이제는 형제 하나당 한 번씩만 보므로 O(N)이다. cancellationToken도 매 엘리먼트마다 확인해서
    /// 큰 XML을 처리하는 도중에도 취소가 바로 먹히게 한다.
    /// </summary>
    private static void VisitElement(XmlElement element, int sameNameIndex, string parentPath,
        IDictionary<string, string> values, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var path = string.Format("{0}/*[local-name()='{1}'][{2}]", parentPath, element.LocalName, sameNameIndex);
        foreach (XmlAttribute attribute in element.Attributes)
            values[string.Format("{0}/@*[local-name()='{1}']", path, attribute.LocalName)] = attribute.Value;

        var childElements = element.ChildNodes.OfType<XmlElement>().ToList();
        if (childElements.Count == 0)
        {
            values[path] = element.InnerText;
            return;
        }

        var nameCounts = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var child in childElements)
        {
            nameCounts.TryGetValue(child.LocalName, out var count);
            count++;
            nameCounts[child.LocalName] = count;
            VisitElement(child, count, path, values, cancellationToken);
        }
    }

    private static XmlDocument LoadXml(string xml)
    {
        var document = new XmlDocument { XmlResolver = null };
        document.LoadXml(xml);
        return document;
    }

    private static void EnsureChildPath(string candidate, string parent)
    {
        var normalizedParent = Path.TrimEndingDirectorySeparator(Path.GetFullPath(parent));
        if (!candidate.StartsWith(string.Format("{0}{1}", normalizedParent, Path.DirectorySeparatorChar),
                StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("설정 파일 경로가 대상 폴더를 벗어났습니다.");
    }
}
