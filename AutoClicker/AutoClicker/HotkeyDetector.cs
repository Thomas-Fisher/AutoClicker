namespace AutoClicker;

internal sealed class HotkeyDetector
{
    private bool _held;

    public HotkeyPress OnKeyDown(Hotkey hotkey, Keys keyCode, Keys modifiers)
    {
        if (keyCode != hotkey.Key || modifiers != hotkey.Modifiers)
        {
            return HotkeyPress.None;
        }

        if (_held)
        {
            return HotkeyPress.Repeat;
        }

        _held = true;
        return HotkeyPress.Pressed;
    }

    // true when this release belongs to a press that was swallowed
    public bool OnKeyUp(Hotkey hotkey, Keys keyCode)
    {
        if (keyCode != hotkey.Key || !_held)
        {
            return false;
        }

        _held = false;
        return true;
    }
}
