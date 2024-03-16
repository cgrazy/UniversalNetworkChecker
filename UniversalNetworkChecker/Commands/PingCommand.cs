// See https://aka.ms/new-console-template for more information

internal class PingCommand : BaseCommand, ICommand
{
    internal const string CommandName = "-ping";

    PingCommandOption myPingCommandOption;

    internal PingCommand(List<string> args):base(args)
    {
        myPingCommandOption = new PingCommandOption(args);
    }

    public  void Parse()
    {
        base.Parse();
        myPingCommandOption.Parse();
    }

    public void Execute()
    {
        base.Execute();
        if (!base.IsInitialized) return;

        PingWorker pingWrapper = new();

        string outputFileName = string.Empty;

        if (base.Args.Count == 3)
        {
            if (string.Compare(base.Args[1], "-out", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                outputFileName = base.Args[2];

                OutputAction?.Invoke($"Save output to file {outputFileName}.");
            }
        }

        PrintHeader();

        int cnt = 0;

        var resultsContainer = new UniversalNetworkCheckerResultContainer();

        while (!Console.KeyAvailable)
        {
            base.JFW.HostsToCheck.ForEach(h =>
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
                    var task = new Task<IPingReplyWrapper>(() => pingWrapper.DoPing(h.IP));

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
                int offset = (Console.CursorTop - base.JFW.HostsToCheck.Count * 2);

                CurserAction?.Invoke(0, offset);
            }
        }

        base.JFW.HostsToCheck.ForEach(h =>
        {
            OutputAction?.Invoke("                                                                         ");
            CurserAction?.Invoke(0, Console.CursorTop - 1);

            OutputAction?.Invoke(resultsContainer.Results[h.Hostname].GetFullReport());
        });

        OutputAction?.Invoke("                                                                         ");
        
        if(myPingCommandOption.OutFile != string.Empty)
        {
            var fileOutput = new FileOutput(myPingCommandOption.OutFile);

            for (int i = 0; i < base.JFW.HostsToCheck.Count; i++)
            {
                fileOutput.Print(resultsContainer.Results[base.JFW.HostsToCheck[i].Hostname].GetFullReport());
            }
        }
    }
}
