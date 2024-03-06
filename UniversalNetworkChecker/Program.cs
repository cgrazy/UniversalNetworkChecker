// See https://aka.ms/new-console-template for more information
using System.Net.NetworkInformation;

Dictionary<string, CarbonNetworkCheckerResult> results = new Dictionary<string, CarbonNetworkCheckerResult>();

Console.WriteLine("Universal Network Checker");

JsonFileWrapper jfw = new JsonFileWrapper();

jfw.LoadJson("UniversalNetworkCheckerConfig.json");


jfw.HostsToCheck.ForEach(h =>
{
    Console.WriteLine($"Hostname: {h.Hostname}, IP: {h.IP}");
});

Console.WriteLine("--------------------------");

int cnt = 0;

while (!Console.KeyAvailable)
{
    jfw.HostsToCheck.ForEach(h =>
    {
        CarbonNetworkCheckerResult average;
        if(!results.TryGetValue(h.Hostname, out average))
        {
            results.Add(h.Hostname, new CarbonNetworkCheckerResult());
        }
        else
        {
            PingReply reply = DoPing(h);

            results[h.Hostname].PingSuccess.Add((reply.Status == IPStatus.Success));
            results[h.Hostname].PingRoundtripTime.Add(reply.RoundtripTime);

            DumpOutput(h);
        }
    });

    if(++cnt>1)     Console.SetCursorPosition(0, Console.CursorTop - jfw.HostsToCheck.Count *2);

    

    await Task.Delay(1000);
}

//Console.SetCursorPosition(0, Console.CursorTop - jfw.HostsToCheck.Count * 2);

jfw.HostsToCheck.ForEach(h =>
{
    DumpOutputDeep(h);
});

PingReply DoPing(Hosts host)
{
    var ping = new Ping();

    byte[] packageSize = new byte[64];

    var pingResult = ping.Send(host.IP, 1000, packageSize);

    Console.WriteLine($"Hostname: {host.Hostname}, IP: {host.IP}, Ping: {pingResult.Status}, {pingResult.RoundtripTime} ms");

    return pingResult;
}

void DumpOutputDeep(Hosts host)
{
    Console.WriteLine("                                                                         ");
    Console.SetCursorPosition(0, Console.CursorTop - 1);
    int fails = results[host.Hostname].PingSuccess.Count(i => !i);
    Double percentage = results[host.Hostname].PingSuccess.Count(i => !i) / results[host.Hostname].PingSuccess.Count *100;
    Console.WriteLine($"hostname: {host.Hostname}, IP: {host.IP}, avg: {string.Format("{0:F2}",results[host.Hostname].PingRoundtripTime.Average())} ms, failed {fails} times ({percentage} %).");

}

void DumpOutput(Hosts host)
{
    Console.WriteLine("                                                                         ");
    Console.SetCursorPosition(0, Console.CursorTop - 1);
    Console.WriteLine($"hostname: {host.Hostname}, IP: {host.IP}, avg: {string.Format("{0:F2}", results[host.Hostname].PingRoundtripTime.Average())} ms ");

}

internal class CarbonNetworkCheckerResult
{
    public CarbonNetworkCheckerResult()
    {
        PingSuccess = new List<bool>();

        PingRoundtripTime = new List<long>();
    }

    internal List<bool> PingSuccess { get; set; }

    internal List<long> PingRoundtripTime { get; set; }
}
