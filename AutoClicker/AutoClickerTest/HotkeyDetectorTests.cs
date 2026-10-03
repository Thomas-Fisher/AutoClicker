using System.Windows.Forms;
using AutoClicker;

namespace AutoClickerTest;

public class HotkeyDetectorTests
{
    private static readonly Hotkey ShiftF6 = new(Keys.F6, Keys.Shift);

    [Fact]
    public void HotkeyWithRightModifiers_IsPressed()
    {
        var detector = new HotkeyDetector();

        Assert.Equal(HotkeyPress.Pressed, detector.OnKeyDown(ShiftF6, Keys.F6, Keys.Shift));
    }

    [Fact]
    public void ModifiersAreConfigurable()
    {
        var detector = new HotkeyDetector();
        var ctrlF6 = new Hotkey(Keys.F6, Keys.Control);

        Assert.Equal(HotkeyPress.None, detector.OnKeyDown(ctrlF6, Keys.F6, Keys.Shift));
        Assert.Equal(HotkeyPress.Pressed, detector.OnKeyDown(ctrlF6, Keys.F6, Keys.Control));
    }

    [Fact]
    public void HeldKey_IsPressedOnce_ThenRepeats_UntilReleased()
    {
        var detector = new HotkeyDetector();

        Assert.Equal(HotkeyPress.Pressed, detector.OnKeyDown(ShiftF6, Keys.F6, Keys.Shift));
        Assert.Equal(HotkeyPress.Repeat, detector.OnKeyDown(ShiftF6, Keys.F6, Keys.Shift));
        Assert.Equal(HotkeyPress.Repeat, detector.OnKeyDown(ShiftF6, Keys.F6, Keys.Shift));

        Assert.True(detector.OnKeyUp(ShiftF6, Keys.F6));

        Assert.Equal(HotkeyPress.Pressed, detector.OnKeyDown(ShiftF6, Keys.F6, Keys.Shift));
    }

    [Theory]
    [InlineData(Keys.None)]
    [InlineData(Keys.Control)]
    [InlineData(Keys.Shift | Keys.Control)]
    [InlineData(Keys.Alt)]
    public void WrongModifiers_AreIgnored(Keys modifiers)
    {
        var detector = new HotkeyDetector();

        Assert.Equal(HotkeyPress.None, detector.OnKeyDown(ShiftF6, Keys.F6, modifiers));
    }

    [Fact]
    public void KeyUp_OfAnUnhandledPress_IsLeftAlone()
    {
        var detector = new HotkeyDetector();

        Assert.False(detector.OnKeyUp(ShiftF6, Keys.F6));
    }

    [Fact]
    public void OtherKey_IsIgnored_AndDoesNotReleaseTheHotkey()
    {
        var detector = new HotkeyDetector();
        detector.OnKeyDown(ShiftF6, Keys.F6, Keys.Shift);

        Assert.Equal(HotkeyPress.None, detector.OnKeyDown(ShiftF6, Keys.A, Keys.Shift));
        Assert.False(detector.OnKeyUp(ShiftF6, Keys.A));

        Assert.Equal(HotkeyPress.Repeat, detector.OnKeyDown(ShiftF6, Keys.F6, Keys.Shift));
    }
}
