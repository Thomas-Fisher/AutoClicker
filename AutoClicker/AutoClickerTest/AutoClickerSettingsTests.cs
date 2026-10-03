using System.Windows.Forms;
using AutoClicker;

namespace AutoClickerTest;

public sealed class AutoClickerSettingsTests : IDisposable
{
    private readonly string _directory;
    private readonly string _filePath;

    public AutoClickerSettingsTests()
    {
        _directory = Path.Combine(Path.GetTempPath(), "AutoClickerTests", Guid.NewGuid().ToString("N"));
        _filePath = Path.Combine(_directory, "settings.json");
    }

    public void Dispose()
    {
        if (Directory.Exists(_directory))
        {
            Directory.Delete(_directory, recursive: true);
        }
    }

    private static Settings CustomSettings() => new()
    {
        Interval = 500,
        RandomDelay = 50,
        X = 100,
        Y = 200,
        JitterRadius = 10,
        PositionMode = ClickPositionMode.SpecificPosition,
        DisplayOverlay = true,
        StartStopKey = Keys.F12,
        StartStopModifiers = Keys.Control,
        MouseButton = MouseButtons.Right
    };

    [Fact]
    public void Load_MissingFile_UsesDefaultsAndCreatesFile()
    {
        var settings = new AutoClickerSettings(_filePath);

        settings.Load();

        Assert.Equal(Settings.Default, settings.Settings);
        Assert.True(File.Exists(_filePath));
    }

    [Fact]
    public void SaveThenLoad_RoundTripsEveryField()
    {
        var original = CustomSettings();
        var writer = new AutoClickerSettings(_filePath);
        writer.Update(original);
        writer.Save();

        var reloaded = new AutoClickerSettings(_filePath);
        reloaded.Load();

        Assert.Equal(original, reloaded.Settings);
    }

    [Fact]
    public void Load_InvalidJson_UsesDefaultsAndKeepsBackup()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(_filePath, "Invalid JSON");
        var settings = new AutoClickerSettings(_filePath);

        settings.Load();

        Assert.Equal(Settings.Default, settings.Settings);
        Assert.Equal("Invalid JSON", File.ReadAllText(_filePath + ".bak"));
    }

    [Fact]
    public void Load_OutOfRangeValues_AreClamped()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(_filePath,
            """{"Interval":-5,"RandomDelay":-1,"X":99999,"Y":-99999,"JitterRadius":-3,"MouseButton":"None","StartStopKey":"None","StartStopModifiers":"None"}""");
        var settings = new AutoClickerSettings(_filePath);

        settings.Load();

        Assert.Equal(Settings.MinInterval, settings.Settings.Interval);
        Assert.Equal(0, settings.Settings.RandomDelay);
        Assert.Equal(Settings.MaxCoordinate, settings.Settings.X);
        Assert.Equal(-Settings.MaxCoordinate, settings.Settings.Y);
        Assert.Equal(0, settings.Settings.JitterRadius);
        Assert.Equal(MouseButtons.Left, settings.Settings.MouseButton);
        Assert.Equal(Keys.Escape, settings.Settings.StartStopKey);
        Assert.Equal(Keys.Shift, settings.Settings.StartStopModifiers);
    }

    [Fact]
    public void Load_PartialFile_FillsMissingFieldsFromDefaults()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(_filePath, """{"X":42}""");
        var settings = new AutoClickerSettings(_filePath);

        settings.Load();

        // a missing Interval must not end up as 1 ms
        Assert.Equal(Settings.Default with { X = 42 }, settings.Settings);
    }

    [Fact]
    public void Load_MoreThanOneModifier_FallsBackToShift()
    {
        Directory.CreateDirectory(_directory);
        File.WriteAllText(_filePath, """{"StartStopModifiers":"Control, Alt"}""");
        var settings = new AutoClickerSettings(_filePath);

        settings.Load();

        Assert.Equal(Keys.Shift, settings.Settings.StartStopModifiers);
    }

    [Fact]
    public void Save_DoesNotLeaveTempFileBehind()
    {
        var writer = new AutoClickerSettings(_filePath);
        writer.Update(CustomSettings());
        writer.Save();

        Assert.False(File.Exists(_filePath + ".tmp"));
    }
}
