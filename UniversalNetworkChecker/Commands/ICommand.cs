// See https://aka.ms/new-console-template for more information

public interface ICommand
{
    public Action<string>? OutputAction { get; set; }
    public Action<int, int>? CurserAction { get; set; }

    public void Parse();

    public void Execute();

    public void PrintHeader();

    public void Help();

    public string Usage();
}
