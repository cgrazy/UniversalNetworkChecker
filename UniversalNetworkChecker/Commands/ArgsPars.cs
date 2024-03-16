// See https://aka.ms/new-console-template for more information
internal abstract class ArgsPars
{
    internal const string ParaOutFile = "-out";

    internal virtual void Run() { }

    internal Queue<string> myArguments;

    public ArgsPars(List<string> args)
    {
        myArguments = new Queue<string>(args);
    }

    internal abstract void Parse();
}
