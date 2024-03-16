// See https://aka.ms/new-console-template for more information

public interface IFileWrapper
{
    bool Exists(string fileName);

    void Open(string fileName);

    void WriteLine(string fileName, string output);
}

internal class FileWrapper : IFileWrapper
{
    public virtual bool Exists(string fileName)
    {
        return File.Exists(fileName);
    }

    public virtual void Open(string fileName)
    {
        File.Open( string.Format($"""{fileName}"""), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
    }

    public virtual void WriteLine(string fileName, string output)
    {
        object lockObject = new object();
        lock (lockObject)
        {
            File.AppendAllText(fileName, output + Environment.NewLine);
        }
    }
}