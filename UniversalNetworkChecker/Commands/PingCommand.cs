// See https://aka.ms/new-console-template for more information

internal class PingCommand : BaseCommand, ICommand
{
    internal const string CommandName = "-ping";

    private const string Obj = "                                                                         ";
    PingCommandOption myPingCommandOption;

    internal PingCommand():base() { }

    internal PingCommand(List<string> args):base(args)
    {
        myPingCommandOption = new PingCommandOption(args);
    }

    public string Usage()
    {
        return "[-ping [-out <outputFile> | -append ] ]";
    }

    public void Help()
    {
        OutputAction?.Invoke($"   -ping             : uses the ping to check the hosts configured in <file>.");
        OutputAction?.Invoke($"      -out <outputFile> : output file where whole opuput is written to.");
        OutputAction?.Invoke($"      -append : if not set a new file will be created, otherwise output will be appended.");
    }

    public new void Parse()
    {
        base.Parse();
        myPingCommandOption.Parse();
        myPingCommandOption.StartTime = DateTime.Now;
    }

    public new void Execute()
    {
        base.Execute();
        if (!base.IsInitialized) return;

        PingWorker pingWrapper = new();

        PrintHeader();

        int cnt = 0;

        var resultsContainer = new UniversalNetworkCheckerResultContainer();

        while (!Console.KeyAvailable)
        {
            base.JFW.HostsToCheck?.ForEach(h =>
            {
                UniversalNetworkCheckerResult? tmp;
                if (!resultsContainer.Results.TryGetValue(h.Hostname, out tmp))
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    resultsContainer.Results.Add(h.Hostname, new UniversalNetworkCheckerResult(h.Hostname, h.IP));
#pragma warning restore CS8604 // Possible null reference argument.
                }
                else
                {
#pragma warning disable CS8604 // Possible null reference argument.
                    var task = new Task<IPingReplyWrapper>(() => pingWrapper.DoPing(h.IP));
#pragma warning restore CS8604 // Possible null reference argument.

                    task.Start();

                    task.WaitAsync(TimeSpan.FromSeconds(2));
                    PingReplyWrapper reply = (PingReplyWrapper)task.Result;

                    resultsContainer.AddResult(h.Hostname, reply);

                    OutputAction?.Invoke(resultsContainer.Results[h.Hostname].GetOutputSting());
                    OutputAction?.Invoke($"Hostname: {h.Hostname}, IP: {h.IP}, Ping: {reply.PingReply.Status}, {reply.PingReply.RoundtripTime} ms");
                }
            });

            if (++cnt > 1)
            {
                int s = (JFW.HostsToCheck != null) ? JFW.HostsToCheck.Count : 0;
                int offset = Console.CursorTop - s * 2;

                CurserAction?.Invoke(0, offset);
            }
        }

        base.JFW.HostsToCheck?.ForEach(h =>
        {
            OutputAction?.Invoke(Obj);
            CurserAction?.Invoke(0, Console.CursorTop - 1);

            OutputAction?.Invoke(resultsContainer.Results[h.Hostname].GetFullReport());
        });

        OutputAction?.Invoke(Obj);


        myPingCommandOption.Result = resultsContainer;
        myPingCommandOption.JFW = JFW;
        myPingCommandOption.Run();
    }
}
