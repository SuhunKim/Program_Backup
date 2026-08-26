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
            var selectiveFolderNames = settings.GetSelectiveFolderNames();
            var sourceDocuments = FilterToSelectiveFolders(
                sourceRecord is null
                    ? ReadCurrentXml(settings.GetSourceRoot(), settings.BackupRootPath, cancellationToken)
                    : ReadBackupXml(sourceRecord, cancellationToken),
                selectiveFolderNames);
            var destinationDocuments = FilterToSelectiveFolders(ReadBackupXml(destinationRecord, cancellationToken), selectiveFolderNames);
            var differences = new List<XmlDifference>();

            // 백업/복원은 항상 Settings에 지정된 폴더(_bin, ConfigFile 등)만 다루는데, 전체 백업
            // (FullZip)은 소스 루트를 통째로 담다 보니 지정 안 된 폴더(예: 예전에 있다가 지운
            // "ConfigFile - 복사본" 같은 사본)까지 섞여 들어올 수 있다. 위 FilterToSelectiveFolders가
            // 그런 무관한 폴더를 애초에 걸러내므로, 아래 비교 대상은 항상 지금 Settings 기준으로
            // 의미 있는 파일만 남는다.
            var relativePaths = sourceDocuments.Keys.Union(destinationDocuments.Keys, StringComparer.OrdinalIgnoreCase).ToList();
            for (var index = 0; index < relativePaths.Count; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var relativePath = relativePaths[index];
                sourceDocuments.TryGetValue(relativePath, out var sourceXml);
                destinationDocuments.TryGetValue(relativePath, out var destinationXml);

                // 파일 자체가 한쪽에만 있으면(새로 생기거나 없어진 파일) 필드 단위로 늘어놓는 대신
                // "존재 차이"로 한 줄만 만든다 — 파일 전체가 새 것/없는 것이라 필드별 비교가
                // 의미 없다. 그리드에서는 값 차이와 분리된 별도 목록에 경로만 보여준다.
                if (sourceXml is null || destinationXml is null)
                {
                    differences.Add(new XmlDifference
                    {
                        Kind = sourceXml is null ? XmlDifferenceKind.ExistsOnlyInBackup : XmlDifferenceKind.ExistsOnlyInCurrent,
                        RelativeFilePath = relativePath
                    });
                    progress?.Report((index + 1) * 100 / Math.Max(1, relativePaths.Count));
                    continue;
                }

                // 양쪽 다 파일이 있으면 실제로 어느 설정 항목(XmlPath)이 어떤 값에서 어떤 값으로
                // 다른지 필드 단위로 전부 나열한다 — 예전에는 "파일 전체가 다름" 한 줄만 만들고
                // 실제 값은 버렸는데, 그러면 그리드에서 뭐가 다른지 전혀 알 수 없었다.
                var sourceValues = FlattenXml(sourceXml, cancellationToken);
                var destinationValues = FlattenXml(destinationXml, cancellationToken);
                foreach (var xmlPath in sourceValues.Keys.Union(destinationValues.Keys, StringComparer.Ordinal))
                {
                    var hasSourceValue = sourceValues.TryGetValue(xmlPath, out var sourceValue);
                    var hasDestinationValue = destinationValues.TryGetValue(xmlPath, out var destinationValue);
                    if (hasSourceValue && hasDestinationValue &&
                        string.Equals(sourceValue, destinationValue, StringComparison.Ordinal))
                        continue;

                    differences.Add(new XmlDifference
                    {
                        Kind = XmlDifferenceKind.ValueChanged,
                        RelativeFilePath = relativePath,
                        XmlPath = xmlPath,
                        CurrentValue = hasSourceValue ? sourceValue! : "(없음)",
                        BackupValue = hasDestinationValue ? destinationValue! : "(없음)"
                    });
                }
                progress?.Report((index + 1) * 100 / Math.Max(1, relativePaths.Count));
            }

            // 존재 차이(구조적)는 실제 설정값이 달라진 항목보다 뒤로 보낸다 — 화면에서는 어차피
            // 별도 목록으로 분리해서 보여주지만, Apply 같은 순서 의존 로직을 위해서도 값 차이를
            // 먼저 정렬해 두는 편이 낫다.
            return (IReadOnlyList<XmlDifference>)differences
                .OrderBy(item => item.Kind == XmlDifferenceKind.ValueChanged ? 0 : 1)
                .ThenBy(item => item.RelativeFilePath, StringComparer.OrdinalIgnoreCase)
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

    /// <summary>
    /// 백업/복원은 항상 Settings에 지정된 폴더만 다루는데, 전체 백업(FullZip)은 소스 루트를
    /// 통째로 담다 보니 지정 안 된 폴더까지 딸려 들어올 수 있다. 이력 비교는 백업 종류와 무관하게
    /// 항상 "지금 Settings 기준"으로 스코프를 좁혀야 하므로, 관련 없는 최상위 폴더는 비교 대상에
    /// 아예 올리지 않는다.
    /// </summary>
    private static Dictionary<string, string> FilterToSelectiveFolders(
        Dictionary<string, string> documents,
        IReadOnlyList<string> selectiveFolderNames)
    {
        return documents
            .Where(entry => SelectiveFolderMatch.IsInsideAnyFolder(entry.Key, selectiveFolderNames))
            .ToDictionary(entry => entry.Key, entry => entry.Value, StringComparer.OrdinalIgnoreCase);
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
