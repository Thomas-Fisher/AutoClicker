namespace AutoClicker;

internal interface IMouse
{
    Point Position { get; }
    void MoveTo(int x, int y);
    bool ButtonDown(MouseButtons button);
    bool ButtonUp(MouseButtons button);
}
