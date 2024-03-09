// See https://aka.ms/new-console-template for more information

public interface IOutput
{
    public void Print(string outputToPrint);

    public void SetCurserPosition(int x, int y);
}

internal class ConsoleOutput : IOutput
{
    public void Print(string outputToPrint)
    {
        Console.WriteLine(outputToPrint);
    }

    public void SetCurserPosition(int x, int y)
    {
        Console.SetCursorPosition(x, y);
    }
}
