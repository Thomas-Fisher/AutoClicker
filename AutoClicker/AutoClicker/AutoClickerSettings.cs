using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoClicker;

public sealed class AutoClickerSettings : IAutoClickerSettings
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public Settings Settings { get; private set; } = Settings.Default;

    private readonly string _filePath;

    public AutoClickerSettings(string? filePath = null)
    {
        _filePath = filePath ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AutoClicker",
            "settings.json");
    }

    public void Update(Settings settings) => Settings = settings.Normalized();

    public void Load()
    {
        var loaded = TryRead();

        if (loaded is null)
        {
            if (File.Exists(_filePath))
            {
                BackUpCorruptFile();
            }

            Settings = Settings.Default;
            Save();
            return;
        }

        Settings = loaded.Normalized();
    }

    // a settings file that can't be written shouldn't stop the clicker
    public void Save()
    {
        try
        {
            string directory = Path.GetDirectoryName(_filePath)
                ?? throw new InvalidOperationException($"Invalid settings path: '{_filePath}'.");
            Directory.CreateDirectory(directory);

            string tempPath = _filePath + ".tmp";
            File.WriteAllText(tempPath, JsonSerializer.Serialize(Settings, JsonOptions));
            File.Move(tempPath, _filePath, overwrite: true);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            Trace.TraceWarning($"Could not save settings '{_filePath}': {ex.Message}");
        }
    }

    private Settings? TryRead()
    {
        try
        {
            return File.Exists(_filePath)
                ? JsonSerializer.Deserialize<Settings>(File.ReadAllText(_filePath), JsonOptions)
                : null;
        }
        catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException)
        {
            Trace.TraceWarning($"Could not read settings '{_filePath}': {ex.Message}");
            return null;
        }
    }

    private void BackUpCorruptFile()
    {
        try
        {
            File.Move(_filePath, _filePath + ".bak", overwrite: true);
        }
        catch (IOException ex)
        {
            Trace.TraceWarning($"Could not back up invalid settings file: {ex.Message}");
        }
    }
}
