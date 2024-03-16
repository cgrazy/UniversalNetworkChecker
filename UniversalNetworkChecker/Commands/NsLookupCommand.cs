// See https://aka.ms/new-console-template for more information

internal class NsLookupCommand : BaseCommand, ICommand
{
    internal const string CommandName = "-nslookup";
    internal const string CommandNameAlias = "-nslu";

    private IDnsWrapper myDnsWrapper;

    internal bool WasRun { get; private set; }

    internal string Hostname { get; private set; }

    internal NsLookupCommand(List<string> args, IDnsWrapper dnsWrapperTestable): base(args)
    {
        myDnsWrapper = dnsWrapperTestable;
    }

    internal NsLookupCommand(List<string> args) : base(args)
    {
        myDnsWrapper = new DnsWrapper();
    }


    public void Execute()
    {
        base.Execute();

        if (!base.IsInitialized) return;

        base.PrintHeader();

        base.JFW.HostsToCheck.ForEach(h =>
        {
            string hostname = RetrieveHostnameFromIP(h.IP);

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

            OutputAction?.Invoke($"{hostname}");
        }
        catch (System.Net.Sockets.SocketException e)
        {
            OutputAction?.Invoke($"Unable to retrieve hostname for ip {ip}.");
            OutputAction?.Invoke($"{e.Message}");
        }

        return hostname;
    }

}
