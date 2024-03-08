// See https://aka.ms/new-console-template for more information
internal class FileWrapper
{
    internal bool Exists(string fileName)
    {
        return File.Exists(fileName);
    }
}