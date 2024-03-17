// See https://aka.ms/new-console-template for more information

internal class PingCommand : BaseCommand, ICommand
{
    internal const string CommandName = "-ping";
    private const string Obj = "                                                                         ";
    PingCommandOption myPingCommandOption;

    internal PingCommand(List<string> args):base(args)
    {
        myPingCommandOption = new PingCommandOption(args);
    }

    public new void Parse()
    {
        base.Parse();
        myPingCommandOption.Parse();
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

        myPingCommandOption.Run(resultsContainer, JFW);
    }
}
