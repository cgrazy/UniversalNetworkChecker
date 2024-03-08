// See https://aka.ms/new-console-template for more information

using System.Diagnostics.CodeAnalysis;
using System.Net;

public class Host
{
    private string? myIP;

    [NotNull]
    public required string IP {
        get => myIP;
        set
        {
            IPAddress ip;
            if(IPAddress.TryParse(value, out ip))
            {
                myIP = value;
            }
            else
            {
                throw new ArgumentException($"No valid ip address given: {value}");
            }
        }
    }

    [NotNull]
    public required string Hostname { get; set; }
}
