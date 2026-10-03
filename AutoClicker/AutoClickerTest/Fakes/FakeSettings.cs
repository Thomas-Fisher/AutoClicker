using AutoClicker;

namespace AutoClickerTest.Fakes;

internal sealed class FakeSettings(Settings settings) : IAutoClickerSettings
{
    public Settings Settings { get; private set; } = settings;
    public void Update(Settings newSettings) => Settings = newSettings;
    public void Load() { }
    public void Save() { }
}
