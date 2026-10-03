using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AutoClicker;

internal sealed partial class Win32Mouse : IMouse
{
    private const uint InputMouse = 0;
    private const uint LeftDown = 0x0002;
    private const uint LeftUp = 0x0004;
    private const uint RightDown = 0x0008;
    private const uint RightUp = 0x0010;
    private const uint MiddleDown = 0x0020;
    private const uint MiddleUp = 0x0040;

    [StructLayout(LayoutKind.Sequential)]
    private struct MouseInput
    {
        public int Dx;
        public int Dy;
        public uint MouseData;
        public uint Flags;
        public uint Time;
        public nuint ExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Input
    {
        public uint Type;
        public MouseInput Mouse;
    }

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetCursorPos(int x, int y);

    [LibraryImport("user32.dll", SetLastError = true)]
    private static partial uint SendInput(uint inputCount, ref Input input, int size);

    internal static int InputSize => Marshal.SizeOf<Input>();

    public Point Position => Cursor.Position;

    public void MoveTo(int x, int y)
    {
        if (!SetCursorPos(x, y))
        {
            Warn();
        }
    }

    public bool ButtonDown(MouseButtons button) => Send(button, down: true);

    public bool ButtonUp(MouseButtons button) => Send(button, down: false);

    private static bool Send(MouseButtons button, bool down)
    {
        uint flags = button switch
        {
            MouseButtons.Right => down ? RightDown : RightUp,
            MouseButtons.Middle => down ? MiddleDown : MiddleUp,
            _ => down ? LeftDown : LeftUp
        };

        var input = new Input
        {
            Type = InputMouse,
            Mouse = new MouseInput { Flags = flags }
        };

        if (SendInput(1, ref input, InputSize) == 1)
        {
            return true;
        }

        Warn();
        return false;
    }

    // fails routinely against elevated windows, so log it and keep going
    private static void Warn() =>
        Trace.TraceWarning($"Windows refused a mouse action (win32 error {Marshal.GetLastWin32Error()}).");
}
