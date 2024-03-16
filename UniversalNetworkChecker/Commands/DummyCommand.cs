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

    public void Execute()
    {
        OutputAction?.Invoke($"Usage: {Environment.NewLine}");
        OutputAction?.Invoke($"dotnet UniversalNetworkChecker.dll <file> [-ping [-out <outputFile>] ] | [-nslookup | -nslu ]");
        OutputAction?.Invoke($"   <file>            : json file containg the hosts to check.");
        OutputAction?.Invoke($"   -ping             : uses the ping to check the hosts configured in <file>.");
        OutputAction?.Invoke($"   -nslookup | -nslu : do a nslookup for all ip address configured in <file>.");
        OutputAction?.Invoke($"   -out <outputFile> : output file where whole opuput is written to.");
    }
}

