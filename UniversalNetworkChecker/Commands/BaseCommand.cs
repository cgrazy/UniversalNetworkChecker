// See https://aka.ms/new-console-template for more information
using System.Reflection;
using System.Runtime.CompilerServices;

internal class BaseCommand : ICommand
{
    public Action<string>? OutputAction { get; set; }
    public Action<int, int>? CurserAction { get; set; }

    internal IJsonFileWrapper JFW { get; set; }

    internal List<string> Args { get; private set; }

    internal bool IsInitialized { get;  set; }

    internal BaseCommand()
    {

    }

    public void Help() {  }

    public string Usage() { return ""; }

    public BaseCommand(List<string> args)
    {
        JFW = new JsonFileWrapper();
        Args = args;
    }

    public BaseCommand(List<string>args, IJsonFileWrapper jsonFileWrapper)
    {
        JFW = jsonFileWrapper;
        Args = args;
    }

    public void Parse() { }

    public async Task Execute()
    {
        
        JFW.OutputAction = OutputAction;

        if (Args.Count < 1)
        {
            return;
        }

        string fileToLoad = string.Empty;
        if (Path.IsPathRooted(Args[0]))
        {
            fileToLoad = Args[0];
        }
        else
        {
#pragma warning disable CS8604 // Possible null reference argument.
            fileToLoad = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Args[0]);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        if (!File.Exists(fileToLoad))
        {
            OutputAction?.Invoke($"The given file {fileToLoad} doesn't exists.");
            return;
        }

        OutputAction?.Invoke($"{fileToLoad}{Environment.NewLine}");

        JFW.LoadJson(fileToLoad);

        this.IsInitialized = true;
    }

    public void PrintHeader()
    {
        JFW.HostsToCheck?.ForEach(h =>
        {
            OutputAction?.Invoke($"Hostname: {h.Hostname}, IP: {h.IP}");
        });

        OutputAction?.Invoke("--------------------------");
    }

}
