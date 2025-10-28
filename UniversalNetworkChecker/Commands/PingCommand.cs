// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

internal class PingCommand : BaseCommand, ICommand
{
    internal const string CommandName = "-ping";

    private List<Host>? myHostsToCheck;

    private string Obj;
    private PingCommandOption myPingCommandOption;

    internal UniversalNetworkCheckerResultContainer ResultContainer { get; private set; }

    internal PingCommand() : this(null, null, null) { }
    
    private PingWorker myPingWorker;

    internal PingCommand(List<string> args) : this(args, new PingWorker(), null)
    {
        Obj = new StringBuilder("", 500).ToString();

        myPingCommandOption = new PingCommandOption(args);
    }
    
    internal PingCommand(List<string> args, PingWorker pingWorker, List<Host> hostsToCheck) : base(args)
    {
        Obj = new StringBuilder("", 700).ToString();

        myPingWorker = pingWorker;

        myHostsToCheck = hostsToCheck ?? base.JFW.HostsToCheck;

        myPingCommandOption = new PingCommandOption(args);

        ResultContainer = new UniversalNetworkCheckerResultContainer();
    }


    public override string Usage()
    {
        return "( -ping [-out <outputFile> ] [ -append ] )";
    }

    public override void Help()
    {
        OutputAction?.Invoke($"   -ping             : uses the ping to check the hosts configured in <file>.");
        OutputAction?.Invoke($"      -out <outputFile> : output file where whole opuput is written to.");
        OutputAction?.Invoke($"      -append : if not set a new file will be created, otherwise output will be appended.");
    }

    public override void Parse()
    {
        base.Parse();
        myPingCommandOption.Parse();
        myPingCommandOption.StartTime = DateTime.Now;
    }

    internal async Task DoExecution()
    {
        myHostsToCheck?.ForEach(h =>
        {
            UniversalNetworkCheckerResult? tmp;
            if (!ResultContainer.Results.TryGetValue(h.Hostname, out tmp))
            {
#pragma warning disable CS8604 // Possible null reference argument.
                ResultContainer.Results.Add(h.Hostname, new UniversalNetworkCheckerResult(h.Hostname, h.IP));
#pragma warning restore CS8604 // Possible null reference argument.
            }
            //else
            {
#pragma warning disable CS8604 // Possible null reference argument.
                var task = new Task<IPingReplyWrapper>(() => myPingWorker.DoPing(h.IP));
#pragma warning restore CS8604 // Possible null reference argument.

                task.Start();

                task.WaitAsync(TimeSpan.FromSeconds(2));
                PingReplyWrapper reply = (PingReplyWrapper)task.Result;

                ResultContainer.AddResult(h.Hostname, reply);

                OutputAction?.Invoke(ResultContainer.Results[h.Hostname].GetOutputSting());
                OutputAction?.Invoke($"Hostname: {h.Hostname}, IP: {h.IP}, Ping: {reply.PingReply.Status}, {reply.PingReply.RoundtripTime} ms.   ");
            }
        });

        await Delay();

        if (++myCounter > 0)
        {
            int s = (myHostsToCheck != null) ? myHostsToCheck.Count : 0;
            int offset = Console.CursorTop - s * 2;

            CurserAction?.Invoke(0, offset);
        }
    }

    int myCounter = 0;

    internal override async Task Execute()
    {
        OutputAction?.Invoke("Executing Ping Command...");
        await base.Execute();
        if (!base.IsInitialized) return;

        PrintHeader();

        OutputAction?.Invoke("Press any key to stop...");

        myCounter = 0;

        while (!Console.KeyAvailable)
        {
            await DoExecution();
        }

        myHostsToCheck?.ForEach(h =>
        {
           CurserAction?.Invoke(0, Console.CursorTop - 1);
           OutputAction?.Invoke(Obj);

           UniversalNetworkCheckerResult? tmp;
           if (ResultContainer.Results.TryGetValue(h.Hostname, out tmp))
           {
               OutputAction?.Invoke(tmp.GetFullReport());
           }
        });

        myPingCommandOption.Result = ResultContainer;
        myPingCommandOption.JFW = JFW;
        myPingCommandOption.Run();
    }

    private async Task Delay()
    {
        Task t = Task.Delay(1000);
       await t;       
    }
}
