internal class UniversalNetworkCheckerResult
{
    internal string Hostname { private get; set; } 
    internal string IP { private get; set; }

    internal UniversalNetworkCheckerResult(string hostname, string ip)
    {
        Hostname= hostname;
        IP= ip;

        PingSuccess = new List<bool>();

        PingRoundtripTime = new List<long>();

        PingFailureTimes = new List<DateTime>();
    }

    internal List<bool> PingSuccess { get; set; }

    internal List<long> PingRoundtripTime { get; set; }

    internal List<DateTime> PingFailureTimes { get; set; }


    internal string GetOutputSting()
    {
        Console.WriteLine("                                                                         ");
        Console.SetCursorPosition(0, Console.CursorTop - 1);

        string lastTwentyResults = string.Format("{0:s20}", GetPingSuccessString());
        
        return string.Format($"hostname: {Hostname}, IP: {IP},  avg: {string.Format("{0:F2}", PingRoundtripTime.Average())} ms , Result: {lastTwentyResults}");
    }
    internal string GetFullReport()
    {
        Console.WriteLine("                                                                         ");
        Console.SetCursorPosition(0, Console.CursorTop - 1);

        string lastTwentyResults = string.Format("{0:s20}", GetPingSuccessString());

        string failureTimes = string.Empty;
        if (PingFailureTimes.Count > 0)
        {
            failureTimes = string.Format($"{Environment.NewLine} Failure times: {GetFailureTimes()}");
        }


        return string.Format($"hostname: {Hostname}, IP: {IP},  avg: {string.Format("{0:F2}", PingRoundtripTime.Average())} ms , Result: {lastTwentyResults} {failureTimes}");
    }

    private string GetFailureTimes()
    {
        return string.Join(", ", PingFailureTimes.Select(i=>i.ToString("T")));
    }


    private string GetPingSuccessString()
    {
        string resultString = string.Empty;

        if (PingSuccess.Count > 20)
        {
            PingSuccess.GetRange(PingSuccess.Count - 20, 20).ForEach(i => resultString += (i) ? "_" : "|");
        }
        else
        {
            PingSuccess.GetRange(0, PingSuccess.Count-1).ForEach(i => resultString += (i) ? "_" : "|");
        }

        return resultString;
    }
}