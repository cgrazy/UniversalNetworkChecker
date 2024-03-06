// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

internal class JsonFileWrapper
{
    internal List<Hosts> HostsToCheck;

    internal async void LoadJson(string fileName)
    {
        if (!File.Exists(fileName))
        {
            Console.WriteLine($"The file {fileName} doesn't exist.");
        }
        else
        {
            List<Hosts> items = JsonFileReader.Read(fileName);

            HostsToCheck = items;
        }
    }
}

internal class JsonFileReader
{
    internal static List<Hosts> Read(string file)
    {
        string text = File.ReadAllText(file);

        return JsonConvert.DeserializeObject<List<Hosts>>(text);
    }
}
