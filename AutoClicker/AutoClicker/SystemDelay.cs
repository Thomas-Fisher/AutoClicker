namespace AutoClicker;

internal sealed class SystemDelay : IDelay
{
    public void Sleep(int milliseconds) => Thread.Sleep(milliseconds);

    public bool WaitCancelled(int milliseconds, CancellationToken token) =>
        token.WaitHandle.WaitOne(milliseconds);
}
