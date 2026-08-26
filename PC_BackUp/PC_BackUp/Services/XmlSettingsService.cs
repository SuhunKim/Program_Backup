using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PC_BackUp.Services;

public sealed class XmlSettingsService : ISettingsService
{
    private const string ConfigFolderName = "Config";
    private const string SettingsFileName = "PCBackupSettings.xml";
    private static readonly XmlSerializer Serializer = new(typeof(AppSettings));

    public XmlSettingsService()
    {
        SettingsFilePath = Path.Combine(ProgramPaths.FindOwnProgramRoot(), ConfigFolderName, SettingsFileName);
    }

    public string SettingsFilePath { get; }

    private static readonly List<string> DefaultSelectiveFolderNames = new() { "_bin", "Config" };

    public AppSettings Load()
    {
        if (!File.Exists(SettingsFilePath))
        {
            var defaultSettings = new AppSettings { SelectiveFolderNames = new List<string>(DefaultSelectiveFolderNames) };
            Save(defaultSettings);
            return defaultSettings;
        }

        try
        {
            var readerSettings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Prohibit,
                XmlResolver = null
            };

            using var stream = new FileStream(SettingsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = XmlReader.Create(stream, readerSettings);
            var settings = Serializer.Deserialize(reader) as AppSettings
                ?? throw new InvalidDataException("설정 XML의 루트 데이터를 읽을 수 없습니다.");

            // 예전 형식(SelectiveFolderName1/2)의 파일이거나 목록이 비어있으면 기본값으로 채운다.
            if (settings.SelectiveFolderNames.Count == 0)
                settings.SelectiveFolderNames = new List<string>(DefaultSelectiveFolderNames);

            return settings;
        }
        catch (Exception exception) when (exception is InvalidOperationException or XmlException)
        {
            throw new InvalidDataException(
                string.Format("설정 XML을 읽을 수 없습니다.\n{0}\n{1}", SettingsFilePath, exception.Message), exception);
        }
    }

    public void Save(AppSettings settings)
    {
        var configFolder = Path.GetDirectoryName(SettingsFilePath)
            ?? throw new InvalidOperationException("Config 폴더 경로를 확인할 수 없습니다.");
        Directory.CreateDirectory(configFolder);

        var temporaryPath = SettingsFilePath + ".tmp";
        try
        {
            var writerSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                Encoding = new UTF8Encoding(false),
                NewLineChars = Environment.NewLine
            };

            using (var stream = new FileStream(temporaryPath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = XmlWriter.Create(stream, writerSettings))
            {
                Serializer.Serialize(writer, settings);
            }

            File.Move(temporaryPath, SettingsFilePath, true);
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }
}
