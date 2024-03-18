
// See https://aka.ms/new-console-template for more information


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
