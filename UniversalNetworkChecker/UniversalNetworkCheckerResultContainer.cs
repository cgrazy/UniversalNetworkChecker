﻿using System.Net.NetworkInformation;

internal class UniversalNetworkCheckerResultContainer
{
    internal Dictionary<string, UniversalNetworkCheckerResult> Results { get; private set; }

    internal UniversalNetworkCheckerResultContainer()
    {
        Results = new Dictionary<string, UniversalNetworkCheckerResult>();
    }

    internal void AddResult(string hostname, PingReplyWrapper reply)
    {
        Results[hostname].PingSuccess.Add(reply.IsPingSuccessful);
        Results[hostname].PingRoundtripTime.Add(reply.PingReply.RoundtripTime);
        if (!reply.IsPingSuccessful)
            Results[hostname].PingFailureTimes.Add(DateTime.Now);
    }
}