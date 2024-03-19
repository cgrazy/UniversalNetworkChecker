// See https://aka.ms/new-console-template for more information
using System.Net;
using System.Net.NetworkInformation;

internal class TraceRouteCommand : BaseCommand, ICommand
{
    internal const string CommandName = "-traceroute";
    internal const string CommandNameAlias = "-tr";

    TraceRouteCommandOption myTraceRouteCommandOption;

    private const int maxTTL = 30;

    internal TraceRouteCommand() : base()
    {

    }

    internal TraceRouteCommand(List<string> args) : base(args)
    {
        myTraceRouteCommandOption = new TraceRouteCommandOption(args);
    }

    public new void Help()
    {
        OutputAction?.Invoke($"   -traceroute | -tr : do a traceroute for all hostnames configured in <file>.");
        OutputAction?.Invoke($"     -long : Also tries to do a nslookup for the route ip determined.");
    }

    public new string Usage()
    {
        return "( -traceroute | -tr [ -long ] )";
    }

    public new void Parse()
    {
        base.Parse();
        myTraceRouteCommandOption.Parse();
    }

    public new void Execute()
    {
        base.Execute();

        PrintHeader();


        base.JFW.HostsToCheck?.ForEach(h =>
        {
            IEnumerable<IPAddress> route = GetTraceRouteForHost(h.Hostname);

            OutputAction?.Invoke($"Route for host: {h.Hostname}");

            int cnt = 0;
            IEnumerator<IPAddress> ipAIter = route.GetEnumerator();


            while (ipAIter.MoveNext())
            {
                IPAddress ipA = ipAIter.Current;

                myTraceRouteCommandOption.LongParameterValue = ipA.ToString();
                myTraceRouteCommandOption.Run();

                OutputAction?.Invoke($"{"",2}Route {++cnt}: {ipA} {myTraceRouteCommandOption.LongParmaterOutput}");
            }
        });
    }

    // https://stackoverflow.com/questions/142614/traceroute-and-ping-in-c-sharp
    private static IEnumerable<IPAddress> GetTraceRouteForHost(string hostnameOrIP)
    {
        PingWrapper pw = new PingWrapper();

        for (int ttl = 1; ttl < maxTTL; ttl++)
        {
            PingOptionsWrapper pow = new PingOptionsWrapper(ttl, true);
            PingReplyWrapper prw = (PingReplyWrapper)pw.Send(hostnameOrIP, pow);

            // we've found a route at this ttl
            if (prw.PingReply.Status == IPStatus.Success || prw.PingReply.Status == IPStatus.TtlExpired)
                yield return prw.PingReply.Address;

            // if we reach a status other than expired or timed out, we're done searching or there has been an error
            if (prw.PingReply.Status != IPStatus.TtlExpired && prw.PingReply.Status != IPStatus.TimedOut)
                break;
        }
    }
}

