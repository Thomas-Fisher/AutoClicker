using AutoClicker;

namespace AutoClickerTest;

public class Win32MouseTests
{
    [Fact]
    public void InputStruct_MatchesNativeSize()
    {
        // native INPUT is 40 bytes on x64, 28 on x86
        int expected = IntPtr.Size == 8 ? 40 : 28;

        Assert.Equal(expected, Win32Mouse.InputSize);
    }
}
