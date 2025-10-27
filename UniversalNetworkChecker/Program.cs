// See https://aka.ms/new-console-template for more information

ConsoleOutput consoleOutput = new();
Action<string> OutputAction = (value) => consoleOutput.Print(value);
Action<int, int> CurserAction = (x, y) => consoleOutput.SetCurserPosition(x, y);

OutputAction.Invoke("Universal Network Checker");

var commandFactory = new CommandFactory(new List<string>(args));
commandFactory.OutputAction = OutputAction;
commandFactory.CurserAction = CurserAction;

ICommand commandToExecute = commandFactory.Parse();

commandToExecute.Parse();

if (commandToExecute is PingCommand)
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    await (commandToExecute as PingCommand).Execute();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}
else if (commandToExecute is NsLookupCommand)
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    await (commandToExecute as NsLookupCommand).Execute();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}
else if (commandToExecute is TraceRouteCommand)
{
#pragma warning disable CS8602 // Dereference of a possibly null reference.
    await (commandToExecute as TraceRouteCommand).Execute();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}


