using System.Drawing;
using System.Windows.Forms;
using AutoClicker;
using AutoClickerTest.Fakes;

namespace AutoClickerTest;

public sealed class AutoClickerManagerTests
{
    private static List<MouseEvent> RunClicks(Settings settings, Point cursor, int clicks, out FakeDelay delay)
    {
        var mouse = new FakeMouse(cursor);
        delay = new FakeDelay(clicks);
        var manager = new AutoClickerManager(new FakeSettings(settings), mouse, delay, new Random(42));

        Assert.True(manager.Toggle());
        Assert.True(delay.Reached.Wait(TimeSpan.FromSeconds(5)), "worker did not complete the requested clicks");
        Assert.False(manager.Toggle());

        return mouse.Events;
    }

    private static Settings Base() => Settings.Default with
    {
        Interval = 100,
        RandomDelay = 0,
        JitterRadius = 0
    };

    [Fact]
    public void SpecificPosition_WithoutJitter_MovesToTargetBeforeClicking()
    {
        var settings = Base() with { PositionMode = ClickPositionMode.SpecificPosition, X = 300, Y = 400 };

        var events = RunClicks(settings, new Point(10, 10), clicks: 3, out _);

        var firstDown = events.First(e => e.Kind == "down");
        Assert.Equal((300, 400), (firstDown.X, firstDown.Y));
        Assert.All(events.Where(e => e.Kind == "down"), e => Assert.Equal((300, 400), (e.X, e.Y)));
    }

    [Fact]
    public void CurrentCursorMode_ClicksAtCursorPosition()
    {
        var events = RunClicks(Base(), new Point(55, 66), clicks: 2, out _);

        Assert.All(events.Where(e => e.Kind == "down"), e => Assert.Equal((55, 66), (e.X, e.Y)));
    }

    [Fact]
    public void Jitter_StaysWithinRadius_AndReturnsToAnchor()
    {
        const int Radius = 10;
        var anchor = new Point(500, 500);
        var settings = Base() with { JitterRadius = Radius };

        var events = RunClicks(settings, anchor, clicks: 50, out _);

        var downs = events.Where(e => e.Kind == "down").ToList();
        Assert.NotEmpty(downs);
        Assert.All(downs, e =>
            Assert.True(Math.Sqrt(Math.Pow(e.X - anchor.X, 2) + Math.Pow(e.Y - anchor.Y, 2)) <= Radius + 1));
        Assert.Contains(downs, e => (e.X, e.Y) != (anchor.X, anchor.Y));

        var last = events.Last(e => e.Kind == "move");
        Assert.Equal((anchor.X, anchor.Y), (last.X, last.Y));
    }

    [Theory]
    [InlineData(MouseButtons.Left)]
    [InlineData(MouseButtons.Right)]
    [InlineData(MouseButtons.Middle)]
    public void Click_UsesConfiguredButton_AndReleasesIt(MouseButtons button)
    {
        var events = RunClicks(Base() with { MouseButton = button }, new Point(1, 1), clicks: 2, out _);

        var buttonEvents = events.Where(e => e.Kind is "down" or "up").ToList();
        Assert.All(buttonEvents, e => Assert.Equal(button, e.Button));
        Assert.Equal(buttonEvents.Count(e => e.Kind == "down"), buttonEvents.Count(e => e.Kind == "up"));
    }

    [Fact]
    public void Interval_IsBaseIntervalPlusBoundedRandomDelay()
    {
        var settings = Base() with { Interval = 100, RandomDelay = 20 };

        RunClicks(settings, new Point(1, 1), clicks: 40, out var delay);

        Assert.All(delay.Waits, ms => Assert.InRange(ms, 100, 119));
    }

    [Fact]
    public void ZeroInterval_IsRaisedToMinimum()
    {
        RunClicks(Base() with { Interval = 0 }, new Point(1, 1), clicks: 3, out var delay);

        Assert.All(delay.Waits, ms => Assert.True(ms >= 1));
    }

    [Fact]
    public void Toggle_ReportsStateAndReleasesTheWorker()
    {
        var mouse = new FakeMouse(new Point(1, 1));
        var delay = new FakeDelay(allowedWaits: 2);
        var manager = new AutoClickerManager(new FakeSettings(Base()), mouse, delay, new Random(1));

        Assert.False(manager.IsRunning);
        Assert.True(manager.Toggle());
        Assert.True(manager.IsRunning);
        Assert.True(delay.Reached.Wait(TimeSpan.FromSeconds(5)));
        Assert.Equal(1, delay.ParkedWorkers);

        Assert.False(manager.Toggle());
        Assert.False(manager.IsRunning);
        Assert.Equal(0, delay.ParkedWorkers);
    }

    [Fact]
    public void RapidToggling_NeverLeavesAWorkerRunning()
    {
        var delay = new FakeDelay(allowedWaits: 1);
        var manager = new AutoClickerManager(
            new FakeSettings(Base()), new FakeMouse(new Point(1, 1)), delay, new Random(1));

        // an even number of toggles, so it should end up stopped
        for (int i = 0; i < 20; i++)
        {
            manager.Toggle();
        }

        Assert.False(manager.IsRunning);
        Assert.Equal(0, delay.ParkedWorkers);
    }

    [Fact]
    public void BlockedClicks_RaiseOneNonFatalWarning()
    {
        var mouse = new FakeMouse(new Point(1, 1)) { ButtonsSucceed = false };
        var delay = new FakeDelay(allowedWaits: 8);
        var manager = new AutoClickerManager(new FakeSettings(Base()), mouse, delay, new Random(1));
        var problems = new List<ClickerProblem>();
        manager.ProblemDetected += (_, problem) => problems.Add(problem);

        manager.Toggle();
        Assert.True(delay.Reached.Wait(TimeSpan.FromSeconds(5)));
        manager.Toggle();

        var problem = Assert.Single(problems);
        Assert.False(problem.Stopped);
    }

    [Fact]
    public void WorkerCrash_IsReportedAsStopped()
    {
        var mouse = new FakeMouse(new Point(1, 1)) { ThrowOnButtonDown = true };
        var manager = new AutoClickerManager(
            new FakeSettings(Base()), mouse, new FakeDelay(allowedWaits: 1), new Random(1));
        using var raised = new ManualResetEventSlim();
        ClickerProblem? problem = null;
        manager.ProblemDetected += (_, p) =>
        {
            problem = p;
            raised.Set();
        };

        manager.Toggle();

        Assert.True(raised.Wait(TimeSpan.FromSeconds(5)));
        Assert.True(problem!.Stopped);
        Assert.Contains("boom", problem.Message);
        manager.Dispose();
    }
}
