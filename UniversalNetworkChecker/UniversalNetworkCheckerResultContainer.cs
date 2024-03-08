using System.Net.NetworkInformation;

internal class UniversalNetworkCheckerResultContainer
{
    internal Dictionary<string, UniversalNetworkCheckerResult> Results { get; private set; }

    internal UniversalNetworkCheckerResultContainer()
    {
        Results = new Dictionary<string, UniversalNetworkCheckerResult>();
    }

    internal void AddResult(string hostname, PingReply reply)
    {
        Results[hostname].PingSuccess.Add((reply.Status == IPStatus.Success));
        Results[hostname].PingRoundtripTime.Add(reply.RoundtripTime);
        if (reply.Status != IPStatus.Success)
            Results[hostname].PingFailureTimes.Add(DateTime.Now);
    }
}