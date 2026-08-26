namespace PC_BackUp.Services;

public interface ISettingsService
{
    string SettingsFilePath { get; }
    AppSettings Load();
    void Save(AppSettings settings);
}
