// See https://aka.ms/new-console-template for more information
using System.Net.NetworkInformation;
using System.Reflection;

ConsoleOutput consoleOutput = new();
Action<string> OutputAction = (value) => consoleOutput.Print(value);
Action<int, int> CuserAction = (x, y) => consoleOutput.SetCurserPosition(x, y);

OutputAction.Invoke("Universal Network Checker");
JsonFileWrapper jfw = new();
jfw.OutputAction = OutputAction;

PingWrapper pingWrapper = new();

if(args.Length < 1)
{
    Usage();
    return;
}

string outputFileName = string.Empty;

if(args.Length == 3)
{
    if (string.Compare(args[1], "-out", StringComparison.InvariantCultureIgnoreCase)==0)
    {
        outputFileName = args[2];

        OutputAction.Invoke($"Save output to file {outputFileName}.");
    }
}

string fileToLoad = string.Empty;
if (Path.IsPathRooted(args[0]))
{
    fileToLoad = args[0];
}
else
{
#pragma warning disable CS8604 // Possible null reference argument.
    fileToLoad = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), args[0]);
#pragma warning restore CS8604 // Possible null reference argument.
}

if (!File.Exists(fileToLoad))
{
    OutputAction.Invoke($"The given file {fileToLoad} doesn't exists.");
    return;
}

OutputAction.Invoke($"{fileToLoad}{Environment.NewLine}");

jfw.LoadJson(fileToLoad);

jfw.HostsToCheck.ForEach(h =>
{
    OutputAction.Invoke($"Hostname: {h.Hostname}, IP: {h.IP}");
});

OutputAction.Invoke("--------------------------");

int cnt = 0;

var resultsContainer = new UniversalNetworkCheckerResultContainer();

while (!Console.KeyAvailable)
{
    jfw.HostsToCheck.ForEach(h =>
    {
        UniversalNetworkCheckerResult? tmp;
        if(!resultsContainer.Results.TryGetValue(h.Hostname, out tmp))
        {
#pragma warning disable CS8604 // Possible null reference argument.
            resultsContainer.Results.Add(h.Hostname, new UniversalNetworkCheckerResult(h.Hostname, h.IP));
#pragma warning restore CS8604 // Possible null reference argument.
        }
        else
        {
            var task =  new Task<PingReply>(()=>pingWrapper.DoPing(h.IP));

            task.Start();

            task.WaitAsync(TimeSpan.FromSeconds(2));
            PingReply reply = task.Result;

            OutputAction.Invoke($"Hostname: {h.Hostname}, IP: {h.IP}, Ping: {reply.Status}, {reply.RoundtripTime} ms");

            resultsContainer.AddResult(h.Hostname, reply);

            OutputAction.Invoke(resultsContainer.Results[h.Hostname].GetOutputSting());
        }
    });

    if(++cnt>1) CuserAction.Invoke(0, Console.CursorTop - jfw.HostsToCheck.Count *2);
}

jfw.HostsToCheck.ForEach(h =>
{
    OutputAction.Invoke("                                                                         ");
    CuserAction.Invoke(0, Console.CursorTop - 1);

    OutputAction.Invoke(resultsContainer.Results[h.Hostname].GetFullReport());
});

if (outputFileName != string.Empty)
{
    var fileOutput = new FileOutput(outputFileName);

    object lockObject = new object();

    for (int i = 0; i < jfw.HostsToCheck.Count; i++)
    {
        lock (lockObject)
        {
            fileOutput.Print(resultsContainer.Results[jfw.HostsToCheck[i].Hostname].GetFullReport());
        }
    }
}

void Usage()
{
    OutputAction.Invoke($"Usage: {Environment.NewLine}");
    OutputAction.Invoke($"dotnet UniversalNetworkChecker.dll <file> [-out <outputFile>]");
    OutputAction.Invoke($"   <file>            : json file containg the hosts to check.");
    OutputAction.Invoke($"   -out <outputFile> : output file where whole opuput is written to.");
}
