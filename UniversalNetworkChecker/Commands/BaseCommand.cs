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

    public virtual void Help() {  }

    public virtual string Usage() { return ""; }

    public BaseCommand(List<string> args):this(args, new JsonFileWrapper())
    {
    }

    public BaseCommand(List<string> args, IJsonFileWrapper jsonFileWrapper)
    {
        JFW = jsonFileWrapper;
        Args = args;

        Initialize();
    }
    
    internal virtual void Initialize()
    {
        JFW.OutputAction = OutputAction;

        if (Args.Count < 1)
        {
            return;
        }

        string fileToLoad = Environment.ExpandEnvironmentVariables(Args[0]);

        OutputAction?.Invoke($"file to load: {fileToLoad}");

        if (Path.IsPathRooted(fileToLoad))
        {
            fileToLoad = Environment.ExpandEnvironmentVariables(Args[0]);
        }
       //else
       // {
#pragma warning disable CS8604 // Possible null reference argument.
        //    fileToLoad = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileToLoad);
#pragma warning restore CS8604 // Possible null reference argument.
        //}

        if (!File.Exists(fileToLoad))
        {
            OutputAction?.Invoke($"The given file {fileToLoad} doesn't exists.");
            return;
        }

        OutputAction?.Invoke($"{fileToLoad}{Environment.NewLine}");

        JFW.LoadJson(fileToLoad);

        this.IsInitialized = true;
    }

    public virtual void Parse() { }

    internal virtual async Task Execute()
    {
        
       
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
