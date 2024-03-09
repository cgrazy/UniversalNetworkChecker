// See https://aka.ms/new-console-template for more information


internal class FileOutput : IOutput
{
    internal string OutputFileName { get; set; }

    FileWrapper myFileWrapper;

    internal FileOutput(string fileName)
    {
        OutputFileName = fileName;
        myFileWrapper = new FileWrapper();
        EnsureOutputFile();
    }

    private void EnsureOutputFile()
    {
        if(!myFileWrapper.Exists(OutputFileName))
        {
            Console.WriteLine(OutputFileName);
            myFileWrapper.Open(OutputFileName);
        }
    }

    public void Print(string outputToPrint)
    {
        myFileWrapper.WriteLine(OutputFileName, outputToPrint);
    }

    public void SetCurserPosition(int x, int y)
    {
        //Console.SetCursorPosition(x, y);
    }
}