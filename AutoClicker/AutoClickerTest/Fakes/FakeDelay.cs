using AutoClicker;

namespace AutoClickerTest.Fakes;

internal sealed class FakeDelay(int allowedWaits) : IDelay
{
    private readonly object _lock = new();
    private readonly List<int> _waits = [];
    private int _parked;

    public ManualResetEventSlim Reached { get; } = new();

    public int ParkedWorkers => Volatile.Read(ref _parked);

    public List<int> Waits { get { lock (_lock) { return [.. _waits]; } } }

    public void Sleep(int milliseconds) { }

    public bool WaitCancelled(int milliseconds, CancellationToken token)
    {
        int count;
        lock (_lock)
        {
            _waits.Add(milliseconds);
            count = _waits.Count;
        }

        if (count < allowedWaits)
        {
            return token.IsCancellationRequested;
        }

        Interlocked.Increment(ref _parked);
        Reached.Set();
        token.WaitHandle.WaitOne();
        Interlocked.Decrement(ref _parked);
        return true;
    }
}
