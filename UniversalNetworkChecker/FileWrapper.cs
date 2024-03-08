// See https://aka.ms/new-console-template for more information

public interface IFileWrapper
{
    bool Exists(string fileName);
}

internal class FileWrapper : IFileWrapper
{
    public virtual bool Exists(string fileName)
    {
        return File.Exists(fileName);
    }
}