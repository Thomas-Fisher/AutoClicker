namespace AutoClicker;

public record Settings
{
    // limits match the NumericUpDown controls on the main form
    public const int MinInterval = 1;
    public const int MaxInterval = 10000;
    public const int MaxRandomDelay = 10000;
    public const int MaxCoordinate = 32767; // Win32 limit
    public const int MaxJitterRadius = 1000;

    public static Settings Default { get; } = new();

    public int Interval { get; init; } = 200;
    public int RandomDelay { get; init; } = 10;
    public int X { get; init; }
    public int Y { get; init; }
    public int JitterRadius { get; init; } = 5;
    public ClickPositionMode PositionMode { get; init; }
    public bool DisplayOverlay { get; init; }
    public Keys StartStopKey { get; init; } = Keys.Escape;
    public Keys StartStopModifiers { get; init; } = Keys.Shift;
    public MouseButtons MouseButton { get; init; } = MouseButtons.Left;

    public Settings Normalized() => this with
    {
        Interval = Math.Clamp(Interval, MinInterval, MaxInterval),
        RandomDelay = Math.Clamp(RandomDelay, 0, MaxRandomDelay),
        X = Math.Clamp(X, -MaxCoordinate, MaxCoordinate),
        Y = Math.Clamp(Y, -MaxCoordinate, MaxCoordinate),
        JitterRadius = Math.Clamp(JitterRadius, 0, MaxJitterRadius),
        PositionMode = Enum.IsDefined(PositionMode) ? PositionMode : ClickPositionMode.CurrentCursor,
        MouseButton = MouseButton is MouseButtons.Left or MouseButtons.Middle or MouseButtons.Right
            ? MouseButton
            : MouseButtons.Left,
        StartStopKey = StartStopKey != Keys.None && Enum.IsDefined(StartStopKey)
            ? StartStopKey
            : Keys.Escape,
        // a bare key would be taken away from every other app, so always use exactly one modifier
        StartStopModifiers = StartStopModifiers is Keys.Shift or Keys.Control or Keys.Alt
            ? StartStopModifiers
            : Keys.Shift
    };
}
