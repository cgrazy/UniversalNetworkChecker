// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

internal class JsonFileWrapper
{
    internal List<Host> HostsToCheck;

    internal async void LoadJson(string fileName)
    {
        if (!File.Exists(fileName))
        {
            Console.WriteLine($"The file {fileName} doesn't exist.");
        }
        else
        {
            List<Host> items = JsonFileReader.Read(fileName);

            HostsToCheck = items;
        }
    }
}

internal class JsonFileReader
{
    internal static List<Host> Read(string file)
    {
        string text = File.ReadAllText(file);

        return JsonConvert.DeserializeObject<List<Host>>(text);
    }
}
