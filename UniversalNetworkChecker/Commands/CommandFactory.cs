// See https://aka.ms/new-console-template for more information
internal class CommandFactory
{
    private readonly List<string> myArgs;

    internal Action<string>? OutputAction { get; set; }
    internal Action<int, int>? CurserAction { get; set; }

    internal CommandFactory(List<string> arguments)
    {
        myArgs = arguments;
    }

    internal ICommand Parse()
    {
        ICommand usedCommand = new DummyCommand();
        foreach (var argument in myArgs.Select(a => a.ToLowerInvariant()))
        {
            switch (argument)
            {
                case PingCommand.CommandName:
                    usedCommand = new PingCommand(myArgs);
                    break;
                case NsLookupCommand.CommandName:
                case NsLookupCommand.CommandNameAlias:
                    usedCommand = new NsLookupCommand(myArgs);
                    break;
                case TraceRouteCommand.CommandName:
                case TraceRouteCommand.CommandNameAlias:
                    usedCommand = new TraceRouteCommand(myArgs);
                    break;
            }
            
        }

        usedCommand.OutputAction = OutputAction;
        usedCommand.CurserAction = CurserAction;

        return usedCommand;
    }

}
