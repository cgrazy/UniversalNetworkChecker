using System.Security.Cryptography.X509Certificates;
using System.Text;

internal class UniversalNetworkCheckerResult
{
    internal string Hostname { private get; set; }
    internal string IP { private get; set; }

    internal UniversalNetworkCheckerResult(string hostname, string ip)
    {
        Hostname = hostname;
        IP = ip;
        Results = new List<PingResultWrapper>();

        PingFailureTimes = new List<DateTime>();
    }

    internal List<PingResultWrapper> Results { get; set; }

    internal List<DateTime> PingFailureTimes { get; set; }


    internal string GetOutputSting()
    {
        Console.WriteLine("                                                                         ");
        Console.SetCursorPosition(0, Console.CursorTop - 1);

        string lastTwentyResults = string.Format("{0:s20}", GetPingSuccessString());

        return string.Format($"Hostname: {Hostname}, IP: {IP},  avg: {string.Format("{0:F2}", GetAvarage())} ms , Result: {lastTwentyResults}");

    }

    private double GetAvarage()
    {
        if (Results.Count == 0)
            return 0.0;

        double sum = Results.Sum(r => r.RoundTripTime);
        return sum / Results.Count;
    }

    private double GetMin()
    {
        if (Results.Count == 0)
            return 0.0;

        return Results.Min(r => r.RoundTripTime);
    }

    private double GetMax()
    {
        if (Results.Count == 0)
            return 0.0;

        return Results.Max(r => r.RoundTripTime);
    }
    
    private List<long> GetPingRoundtripTimes()
    {
        return Results.Select(r => (long)r.RoundTripTime).ToList();
    }

    internal string GetFullReport()
    {
        //Console.WriteLine(new StringBuilder("", 200).ToString());
        //Console.SetCursorPosition(0, Console.CursorTop - Results.Count);

        string lastTwentyResults = string.Format("{0:s20}", GetPingSuccessString());

        string failureTimes = string.Empty;
        if (PingFailureTimes.Count > 0)
        {
            failureTimes = string.Format($"{Environment.NewLine} Failure times: {GetFailureTimes()}");
        }

        double percentageLoss = DataCalculator.GetPercentageValueOf(PingFailureTimes.Count, Results.Count);

        double avg = GetAvarage();
        double min = GetMin();
        double max = GetMax();
        double median = DataCalculator.GetMedianOfList(GetPingRoundtripTimes());

        string minMedianMax = string.Format("(min/median/max: {0:F2} ms/{1:F2} ms/{2:F2} ms)", min, median, max);
                                
        return string.Format($"Hostname: {Hostname}, IP: {IP},  avg: {$"{avg:F2}"} ms {minMedianMax}, Result: {lastTwentyResults} ({$"{percentageLoss:F2}"} % loss) {failureTimes}");
    }

    private string GetFailureTimes()
    {
        return string.Join(", ", PingFailureTimes.Select(i => i.ToString("T")));
    }


    private string GetPingSuccessString()
    {
        string resultString = string.Empty;

        if (Results.Count > 20)
        {
            Results.GetRange(Results.Count - 20, 20).ForEach(i => resultString += (i.IsPingSuccessful) ? "_" : "|");
        }
        else
        {
            Results.GetRange(0, Results.Count - 1).ForEach(i => resultString += (i.IsPingSuccessful) ? "_" : "|");
        }

        return resultString;
    }
}

internal class PingResultWrapper
{
    internal long RoundTripTime { get; set; }
    internal bool IsPingSuccessful { get; set; }

    internal PingResultWrapper(long roundTripTime, bool isSuccessful)
    {
        RoundTripTime = roundTripTime;
        IsPingSuccessful = isSuccessful;
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