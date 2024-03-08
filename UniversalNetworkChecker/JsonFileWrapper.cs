// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;



internal class JsonFileWrapper
{
    internal List<Host> HostsToCheck = new List<Host>();

    internal IFileWrapper myFileWrapper;

    internal IJsonFileReader myJsonFileReader;

    //testability
    internal JsonFileWrapper(IFileWrapper fileWrapper, IJsonFileReader jsonFileReader)
    {
        myFileWrapper = fileWrapper;
        myJsonFileReader = jsonFileReader;
    }

    internal JsonFileWrapper()
    {
        myFileWrapper = new FileWrapper();
        myJsonFileReader = new JsonFileReader();
    }

    internal void LoadJson(string fileName)
    {
        if(!myFileWrapper.Exists(fileName))
        {
            Console.WriteLine($"The file {fileName} doesn't exist.");
        }
        else
        {
            List<Host> items = myJsonFileReader.Read(fileName);

            HostsToCheck = items;
        }
    }
}

public interface IJsonFileReader
{
    public List<Host> Read(string file);
}

internal class JsonFileReader : IJsonFileReader
{
    public virtual List<Host> Read(string file)
    {
        string text = File.ReadAllText(file);

        return JsonConvert.DeserializeObject<List<Host>>(text);
    }
}
