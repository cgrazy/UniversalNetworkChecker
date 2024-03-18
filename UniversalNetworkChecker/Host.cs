// See https://aka.ms/new-console-template for more information
using System.Net;

public class Host
{
    public Host()
    {

    }

    public Host(string host, string ip)
    {
        Hostname = host;
        IP = ip;
    }

    private string? myIP;

    private string? myHostname;

    public required string? IP {
        get => myIP;
        set
        {
            IPAddress? ip;
            if (IPAddress.TryParse(value, out ip))
            {
                myIP = value;
            }
            else
            {
                throw new ArgumentException($"No valid ip address given: {value}");
            }
        }
    }

    public required string? Hostname
    {
        get => myHostname;
        set
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"No valid hostname giv en: {value}");
            }

            myHostname = value;
        }
    }
}
