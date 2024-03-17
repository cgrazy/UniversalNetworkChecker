// See https://aka.ms/new-console-template for more information
internal class PingCommandOption : ArgsPars
{
    internal PingCommandOption(List<string> args) : base(args) { }

    internal string OutFile = string.Empty;

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

            }
        }
    }
    // not yet happy about the syntax
    internal override void Run(UniversalNetworkCheckerResultContainer resultsContainer, IJsonFileWrapper jFW)
    {
        base.Run(resultsContainer, jFW);

        if (OutFile != string.Empty)
        {
            var fileOutput = new FileOutput(OutFile);

            for (int i = 0; i < jFW.HostsToCheck?.Count; i++)
            {
                fileOutput.Print(resultsContainer.Results[jFW.HostsToCheck[i].Hostname].GetFullReport());
            }
        }
    }
}