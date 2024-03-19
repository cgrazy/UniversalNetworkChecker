// See https://aka.ms/new-console-template for more information
internal class TraceRouteCommandOption : ArgsPars
{
    internal const string LongParameter = "-long";

    internal bool LongOutput { get; private set; }

    internal string LongParmaterOutput { get; private set; }
    internal string LongParameterValue { get; set; }

    internal TraceRouteCommandOption(List<string> args) : base(args) { }

    internal override void Parse()
    {
        while (myArguments.Count > 0)
        {
            string arg = myArguments.Dequeue();
            switch (arg)
            {
                case LongParameter:
                    LongOutput = true;
                    break;
            }
        }
    }

    internal override void Run()
    {
        base.Run();

        if (LongOutput)
        {
            NsLookupCommand nlc = new();

            LongParmaterOutput = string.Format($" - {nlc.RetrieveHostnameFromIP(LongParameterValue)}");
        }
    }
}

