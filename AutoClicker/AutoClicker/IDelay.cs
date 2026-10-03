namespace AutoClicker;

internal interface IDelay
{
    void Sleep(int milliseconds);
    bool WaitCancelled(int milliseconds, CancellationToken token);
}
