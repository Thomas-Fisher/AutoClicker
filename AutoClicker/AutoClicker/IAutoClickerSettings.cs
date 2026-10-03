namespace AutoClicker;

public interface IAutoClickerSettings
{
    Settings Settings { get; }
    void Update(Settings settings);
    void Load();
    void Save();
}
