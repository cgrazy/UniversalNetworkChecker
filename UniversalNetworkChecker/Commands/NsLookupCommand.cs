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

    internal NsLookupCommand(List<string> args) : base(args)
    {
        myDnsWrapper = new DnsWrapper();
    }

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

    internal override async Task Execute()
    {
        await base.Execute();

        if (!base.IsInitialized) return;

        base.PrintHeader();

        base.JFW.HostsToCheck?.ForEach(h =>
        {
#pragma warning disable CS8604 // Possible null reference argument.
            string hostname = RetrieveHostnameFromIP(h.IP);
#pragma warning restore CS8604 // Possible null reference argument.

            base.OutputAction?.Invoke($"{h.IP} => {hostname}");

            Hostname = hostname;
        });

        
    }

    internal virtual string RetrieveHostnameFromIP(string ip)
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
