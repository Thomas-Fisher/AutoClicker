using System.Diagnostics;

namespace AutoClicker;

internal sealed class AutoClickerManager : IDisposable
{
    private const double TwoPi = Math.PI * 2;

    // jitter timings in ms
    private const int MoveSettleMs = 5;
    private const int SnapBackMaxAttempts = 15;
    private const int SnapBackPollMs = 1;
    private const int PostJitterBufferMs = 2;

    private const int MinPressMs = 2;
    private const int MaxPressMs = 8;

    private const int BlockedClicksBeforeWarning = 5;

    private static readonly TimeSpan StopTimeout = TimeSpan.FromSeconds(1);

    private readonly IAutoClickerSettings _settings;
    private readonly IMouse _mouse;
    private readonly IDelay _delay;
    private readonly Random _random;
    private readonly object _gate = new();

    private CancellationTokenSource? _cts;
    private Thread? _worker;
    private volatile bool _isJittering;

    private MouseButtons _button;
    private int _blockedClicks;

    public AutoClickerManager(IAutoClickerSettings settings)
        : this(settings, new Win32Mouse(), new SystemDelay(), new Random())
    {
    }

    public AutoClickerManager(IAutoClickerSettings settings, IMouse mouse, IDelay delay, Random random)
    {
        _settings = settings;
        _mouse = mouse;
        _delay = delay;
        _random = random;
    }

    public event EventHandler<ClickerProblem>? ProblemDetected;

    public bool IsJittering => _isJittering;

    public bool IsRunning
    {
        get
        {
            lock (_gate)
            {
                return _worker is { IsAlive: true } && _cts is { IsCancellationRequested: false };
            }
        }
    }

    public bool Toggle()
    {
        lock (_gate)
        {
            if (IsRunning)
            {
                Stop();
                return false;
            }

            // the worker may have died on its own
            Stop();
            Start();
            return true;
        }
    }

    public void Dispose() => Stop();

    public void Stop()
    {
        lock (_gate)
        {
            if (_cts is null)
            {
                return;
            }

            _cts.Cancel();

            // a worker that outlives the timeout may still use the token, so only dispose once it's gone
            if (_worker?.Join(StopTimeout) ?? true)
            {
                _cts.Dispose();
            }

            _cts = null;
            _worker = null;
            _isJittering = false;
        }
    }

    private void Start()
    {
        var settings = _settings.Settings.Normalized();
        _button = settings.MouseButton;
        _blockedClicks = 0;

        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        _worker = new Thread(() => RunLoop(settings, token))
        {
            IsBackground = true,
            Name = "AutoClicker worker"
        };
        _worker.Start();
    }

    private void RunLoop(Settings settings, CancellationToken token)
    {
        var staticPoint = new Point(settings.X, settings.Y);
        bool useCurrentPos = settings.PositionMode == ClickPositionMode.CurrentCursor;

        try
        {
            while (!token.IsCancellationRequested)
            {
                ClickAt(useCurrentPos ? _mouse.Position : staticPoint, settings.JitterRadius);

                int sleepTime = settings.Interval;
                if (settings.RandomDelay > 0)
                {
                    sleepTime += _random.Next(0, settings.RandomDelay);
                }

                if (_delay.WaitCancelled(sleepTime, token))
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            // an unhandled exception here would kill the whole app
            Trace.TraceError($"AutoClicker worker stopped unexpectedly: {ex}");
            ProblemDetected?.Invoke(this, new ClickerProblem($"Clicking stopped unexpectedly: {ex.Message}", Stopped: true));
        }
        finally
        {
            _isJittering = false;
        }
    }

    private void ClickAt(Point position, int radius)
    {
        int anchorX = position.X;
        int anchorY = position.Y;

        if (radius <= 0)
        {
            // the click lands wherever the cursor is, so go there first
            _mouse.MoveTo(anchorX, anchorY);
            PerformClick();
            return;
        }

        try
        {
            _isJittering = true;

            double angle = _random.NextDouble() * TwoPi;
            double dist = Math.Sqrt(_random.NextDouble()) * radius;

            int jitterX = anchorX + (int)Math.Round(Math.Cos(angle) * dist);
            int jitterY = anchorY + (int)Math.Round(Math.Sin(angle) * dist);

            _mouse.MoveTo(jitterX, jitterY);
            _delay.Sleep(MoveSettleMs);

            PerformClick();

            _mouse.MoveTo(anchorX, anchorY);

            for (int attempt = 0; attempt < SnapBackMaxAttempts; attempt++)
            {
                Point cur = _mouse.Position;
                if (cur.X == anchorX && cur.Y == anchorY)
                {
                    break;
                }

                _delay.Sleep(SnapBackPollMs);
            }
        }
        finally
        {
            _delay.Sleep(PostJitterBufferMs);
            _isJittering = false;
        }
    }

    private void PerformClick()
    {
        bool pressed = _mouse.ButtonDown(_button);
        _delay.Sleep(_random.Next(MinPressMs, MaxPressMs));
        bool released = _mouse.ButtonUp(_button);

        if (pressed && released)
        {
            _blockedClicks = 0;
        }
        else if (++_blockedClicks == BlockedClicksBeforeWarning)
        {
            ProblemDetected?.Invoke(this, new ClickerProblem(
                "Windows is blocking the clicks. If the target window runs as administrator, run AutoClicker as administrator too.",
                Stopped: false));
        }
    }
}
