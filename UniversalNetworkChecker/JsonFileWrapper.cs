// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;



internal class JsonFileWrapper
{
    internal List<Host> HostsToCheck = new List<Host>();

    internal IFileWrapper myFileWrapper;

    internal IJsonFileReader myJsonFileReader;

    internal Action<string> OutputAction { get; set; }

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
            OutputAction.Invoke($"The file {fileName} doesn't exist.");
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

        List<Host> returnedListOfHosts = new();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        returnedListOfHosts = JsonConvert.DeserializeObject<List<Host>>(text);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        return returnedListOfHosts??new List<Host>();
    }
}
