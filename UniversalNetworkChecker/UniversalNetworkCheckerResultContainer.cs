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
        var result=new PingResultWrapper(reply.PingReply.RoundtripTime, reply.IsPingSuccessful.Value);

        Results[hostname].Results.Add(result);
        if (!reply.IsPingSuccessful.Value)
            Results[hostname].PingFailureTimes.Add(DateTime.Now);
    }
}