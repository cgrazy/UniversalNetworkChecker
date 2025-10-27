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
    await (commandToExecute as PingCommand).Execute();
}
else if (commandToExecute is NsLookupCommand)
{
    await (commandToExecute as NsLookupCommand).Execute();
}


