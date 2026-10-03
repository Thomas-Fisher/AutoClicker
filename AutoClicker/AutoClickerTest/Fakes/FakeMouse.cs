using System.Drawing;
using System.Windows.Forms;
using AutoClicker;

namespace AutoClickerTest.Fakes;

internal sealed class FakeMouse(Point start) : IMouse
{
    private readonly object _lock = new();
    private Point _position = start;
    private readonly List<MouseEvent> _events = [];

    public bool ButtonsSucceed { get; set; } = true;

    public bool ThrowOnButtonDown { get; set; }

    public Point Position { get { lock (_lock) { return _position; } } }

    public List<MouseEvent> Events { get { lock (_lock) { return [.. _events]; } } }

    public void MoveTo(int x, int y)
    {
        lock (_lock)
        {
            _position = new Point(x, y);
            _events.Add(new MouseEvent("move", x, y));
        }
    }

    public bool ButtonDown(MouseButtons button)
    {
        if (ThrowOnButtonDown)
        {
            throw new InvalidOperationException("boom");
        }

        lock (_lock)
        {
            _events.Add(new MouseEvent("down", _position.X, _position.Y, button));
        }

        return ButtonsSucceed;
    }

    public bool ButtonUp(MouseButtons button)
    {
        lock (_lock)
        {
            _events.Add(new MouseEvent("up", _position.X, _position.Y, button));
        }

        return ButtonsSucceed;
    }
}
