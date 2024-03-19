// See https://aka.ms/new-console-template for more information
internal class DummyCommand: ICommand
{
    public DummyCommand()
    {

    }

    public Action<string>? OutputAction { get; set; }
    public Action<int, int>? CurserAction { get; set; }

    public void Parse() { }

    public void PrintHeader() { }

    public void Help()
    {
        OutputAction?.Invoke($"Usage: {Environment.NewLine}");
    }

    public string Usage() { return ""; }

    public void Execute()
    {
        var pc = new PingCommand() { OutputAction = OutputAction };
        var nc = new NsLookupCommand() { OutputAction = OutputAction };
        var tc = new TraceRouteCommand() { OutputAction = OutputAction };

        this.Help();

        OutputAction?.Invoke($"dotnet UniversalNetworkChecker.dll <file>  {pc.Usage()} | {nc.Usage()} | {tc.Usage()} ");
         
        OutputAction?.Invoke($"   <file>            : json file containg the hosts to check.");

        pc.Help();
        nc.Help();
        tc.Help();
    }
}

