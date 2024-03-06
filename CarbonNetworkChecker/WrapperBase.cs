using System.Drawing;

internal class WrapperBase
{
    internal System.Timers.Timer myTimer;

    internal bool myIsInitialized;

    internal bool IsInitialized { get; set; }

    internal Action<bool> Result { get; set; }

    internal bool IsRunning { get; set; }

    public WrapperBase()
    {
        myTimer = new System.Timers.Timer();
    }

    internal virtual void Start()
    {
        if (myIsInitialized)
        {
            myTimer.Enabled = true;
            myTimer.Start();
        }
    }

    internal virtual void Stop()
    {
        if (myIsInitialized)
        {
            myTimer.Stop();
            myTimer.Enabled = false;
        }
    }

    protected void InvokeResult(bool result)
    {
        if (IsRunning)
        {
            Result.Invoke(result);
            // NSColor.SystemGreen : NSColor.SystemRed); ;
        }
    }
}
