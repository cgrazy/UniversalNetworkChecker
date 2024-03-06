using System.Net.NetworkInformation;
using System.Timers;

internal class LongRunnerPingCommandWrapper : WrapperBase
{
    internal string IpAddress { get; set; }
    public long RoundtripTime { get; private set; }

    internal Action<DateTime, PingReply> Roundtrip { get; set; }

    private int myMaxPingCounter;

    private int myCounter;

    private bool myLongPingEnabled;

    public LongRunnerPingCommandWrapper(string hostname, string ip) : base()
    {
        var timerInterval = 2;// myPropertyAccessor.GetNumberValueForKey("longpinginterval");

        if (timerInterval > 3)
        {
            //((PropertyListAccessor)myPropertyAccessor).PropertyListAccessorErrorOutput.Invoke($"Value for 'longpinginterval' should not be too large. Recommended 1-3s.");
        }

        myTimer = new System.Timers.Timer
        {
            Interval = timerInterval * 1000,
            AutoReset = true
        };

        myTimer.Elapsed += MyTimer_Elapsed;

        myMaxPingCounter = 10000; // myPropertyAccessor.GetNumberValueForKey("maxpingcounter");
        myLongPingEnabled = false;//  myPropertyAccessor.GetBoolValueForKey("enablelongping");

        base.IsInitialized = true;
    }

    internal override void Start()
    {
        myTimer.Enabled = true;

        base.Start();
    }

    private void MyTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        var ping = new Ping();

        byte[] packageSize = new byte[64];

        var sentDateTime = DateTime.Now;
        var pingResult = ping.Send(IpAddress, 10000, packageSize);

        if (base.IsRunning)
        {
            base.InvokeResult(pingResult.Status == IPStatus.Success);
            Roundtrip.Invoke(sentDateTime, pingResult);
        }

        if (!myLongPingEnabled)
        {
            if (++myCounter == myMaxPingCounter)
            {
                myTimer.Stop();
                myTimer.Enabled = false;
            }
        }
    }
}