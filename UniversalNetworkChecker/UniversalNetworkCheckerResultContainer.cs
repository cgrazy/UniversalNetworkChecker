using System.Net.NetworkInformation;

internal class UniversalNetworkCheckerResultContainer
{
    internal Dictionary<string, UniversalNetworkCheckerResult> Results { get; private set; }

    internal UniversalNetworkCheckerResultContainer()
    {
        Results = new Dictionary<string, UniversalNetworkCheckerResult>();
    }

    internal void AddResult(string hostname, PingReplyWrapper reply)
    {
        var success = reply.IsPingSuccessful.GetValueOrDefault();
        var roundtrip = reply.PingReply?.RoundtripTime ?? 0L;
        var result = new PingResult(roundtrip, success);

        Results[hostname].Results.Add(result);
        if (!success)
            Results[hostname].PingFailureTimes.Add(DateTime.Now);
    }
}