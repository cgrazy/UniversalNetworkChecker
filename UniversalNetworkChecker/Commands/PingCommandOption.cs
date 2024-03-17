// See https://aka.ms/new-console-template for more information
internal class PingCommandOption : ArgsPars
{
    internal const string AppendParameter = "-append";

    internal PingCommandOption(List<string> args) : base(args) { }

    internal string OutFile = string.Empty;
    internal bool myAppendOutFile = false;

    internal UniversalNetworkCheckerResultContainer Result { get; set; }
    internal IJsonFileWrapper JFW { get; set; }
    internal DateTime StartTime { get; set; }

    internal override void Parse()
    {
        while(myArguments.Count > 0)
        {
            string arg = myArguments.Dequeue();
            switch(arg)
            {
                case ParaOutFile:
                    OutFile = myArguments.Dequeue();
                    break;
                case AppendParameter:
                    myAppendOutFile = true;
                    break;
            }
        }
    }

    internal override void Run()
    {
        base.Run();

        if (OutFile != string.Empty)
        {
            var fileOutput = new FileOutput(OutFile);
            fileOutput.Append = myAppendOutFile;
            fileOutput.EnsureOutputFile();

            fileOutput.Print($"Start: {StartTime}");
            fileOutput.Print($"End  : {DateTime.Now}");
            fileOutput.Print($"-----------------------");

            for (int i = 0; i < JFW.HostsToCheck?.Count; i++)
            {
                fileOutput.Print(Result.Results[JFW.HostsToCheck[i].Hostname].GetFullReport());
            }
        }
    }
}