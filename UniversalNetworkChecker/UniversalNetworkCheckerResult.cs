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

        double percentageLoss = DataCalculator.GetPercentageValueOf(PingFailureTimes.Count, PingSuccess.Count);

        double avg = PingRoundtripTime.Average();
        double min = PingRoundtripTime.Min<long>();
        double max = PingRoundtripTime.Max<long>();
        double median = DataCalculator.GetMedianOfList(PingRoundtripTime);

        string minMedianMax = string.Format("(min/median/max: {0:F2} ms/{1:F2} ms/{2:F2} ms)", min, median, max);

        return string.Format($"hostname: {Hostname}, IP: {IP},  avg: {string.Format("{0:F2}", avg)} ms {minMedianMax}, Result: {lastTwentyResults} ({string.Format("{0:F2}", percentageLoss)} % loss) {failureTimes}");
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

internal class DataCalculator
{
    internal static double GetPercentageValueOf(int failure, int all)
    {
        return (double)failure / (double)all * 100;
    }

    internal static double GetMedianOfList(List<long> values)
    {
        values.Sort();

        int size = values.Count;
        int mid = size / 2;

        double median = (size % 2 != 0) ? (double)values[mid] : ((double)values[mid] + (double)values[mid - 1]) / 2;

        return median;
    }
}