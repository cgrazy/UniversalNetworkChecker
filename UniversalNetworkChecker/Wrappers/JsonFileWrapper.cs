
// See https://aka.ms/new-console-template for more information



internal class JsonFileWrapper : IJsonFileWrapper
{
    public List<Host> HostsToCheck { get; set; }

    internal IFileWrapper myFileWrapper;

    internal IJsonFileReader myJsonFileReader;

    public Action<string>? OutputAction { get; set; }

    //testability
    internal JsonFileWrapper(IFileWrapper fileWrapper, IJsonFileReader jsonFileReader)
    {
        HostsToCheck = new List<Host>();

        myFileWrapper = fileWrapper;
        myJsonFileReader = jsonFileReader;
    }

    internal JsonFileWrapper()
    {
        myFileWrapper = new FileWrapper();
        myJsonFileReader = new JsonFileReader();
    }

    public void LoadJson(string fileName)
    {
        if(!myFileWrapper.Exists(fileName))
        {
            OutputAction?.Invoke($"The file {fileName} doesn't exist.");
        }
        else
        {
            List<Host> items = myJsonFileReader.Read(fileName);

            HostsToCheck = items;
        }
    }
}
