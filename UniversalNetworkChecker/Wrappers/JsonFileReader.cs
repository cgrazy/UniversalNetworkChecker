// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

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
