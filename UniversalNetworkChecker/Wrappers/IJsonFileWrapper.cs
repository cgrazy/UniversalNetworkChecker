
// See https://aka.ms/new-console-template for more information

public interface IJsonFileWrapper
{
    public Action<string> OutputAction { get; set; }

    public void LoadJson(string fileName);

    public List<Host> HostsToCheck { get; set; }
}
