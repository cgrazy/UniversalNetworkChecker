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

    internal override void Run()
    {
        base.Run();


    }
}