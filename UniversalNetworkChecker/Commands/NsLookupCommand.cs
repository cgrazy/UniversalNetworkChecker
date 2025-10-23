// See https://aka.ms/new-console-template for more information

internal class NsLookupCommand : BaseCommand, ICommand
{
    internal const string CommandName = "-nslookup";
    internal const string CommandNameAlias = "-nslu";

    private IDnsWrapper myDnsWrapper;

    internal bool WasRun { get; private set; }

    internal string? Hostname { get; private set; }

    internal NsLookupCommand():base()
    {
        myDnsWrapper = new DnsWrapper();
    }

    internal NsLookupCommand(List<string> args, IDnsWrapper dnsWrapperTestable) : base(args)
    {
        myDnsWrapper = dnsWrapperTestable;
    }

    internal NsLookupCommand(List<string> args) : this(args, new DnsWrapper(), null)
    {

    }

    internal NsLookupCommand(List<string> args, IDnsWrapper dnsWrapperTestable, List<Host> hostsToCheck) : base(args)
    {
        myDnsWrapper = dnsWrapperTestable;

        myHostsToCheck = hostsToCheck ?? base.JFW.HostsToCheck; 
    }

    private List<Host> myHostsToCheck;

    public override void Parse()
    {
        base.Parse();
    }

    public override string Usage()
    {
        return "( -nslookup | -nslu )";
    }

    public override void Help()
    {
        OutputAction?.Invoke($"   -nslookup | -nslu : do a nslookup for all ip address configured in <file>.");
    }

    internal virtual bool IsNsLookupCommandInitialized()
    {
        return base.IsInitialized;
    }

    internal override async Task Execute()
    {
        await base.Execute();

        if (!IsNsLookupCommandInitialized()) return;

        base.PrintHeader();

        await DoExecution();
    }

    internal async Task DoExecution()
    {
        myHostsToCheck?.ForEach(h =>
        {
#pragma warning disable CS8604 // Possible null reference argument.
            string hostname = RetrieveHostnameFromIP(h.IP);
#pragma warning restore CS8604 // Possible null reference argument.

            base.OutputAction?.Invoke($"{h.IP} => {hostname}");

            Hostname = hostname;
        });
    }

    internal string RetrieveHostnameFromIP(string ip)
    {
        string hostname = string.Empty;
        try
        {
            hostname = myDnsWrapper.GetHostEntry(ip);
        }
        catch (System.Net.Sockets.SocketException e)
        {
            OutputAction?.Invoke($"Unable to retrieve hostname for ip {ip}.");
            OutputAction?.Invoke($"{e.Message}");
        }

        return hostname;
    }

}
