// See https://aka.ms/new-console-template for more information


internal class FileOutput : IOutput
{
    internal string OutputFileName { get; set; }

    FileWrapper myFileWrapper;

    internal bool Append { get; set; }

    internal FileOutput(string fileName)
    {
        OutputFileName = fileName;
        myFileWrapper = new FileWrapper();
    
    }

    internal void EnsureOutputFile()
    {
        if(!myFileWrapper.Exists(OutputFileName))
        {
            myFileWrapper.Open(OutputFileName);
        }
        else
        {
            if(!Append)
            {
                myFileWrapper.Delete(OutputFileName);
                myFileWrapper.Open(OutputFileName);
            }
        }
    }

    public void Print(string outputToPrint)
    {
        myFileWrapper.WriteLine(OutputFileName, outputToPrint);
    }

    public void SetCurserPosition(int x, int y)
    {
        throw new NotImplementedException("This method will never be implemented.");
    }
}