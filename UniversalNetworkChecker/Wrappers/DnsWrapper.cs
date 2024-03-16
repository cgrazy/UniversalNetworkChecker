// See https://aka.ms/new-console-template for more information
using System.Net;


public interface IDnsWrapper
{
    public string GetHostEntry(string ip);
}


internal class DnsWrapper : IDnsWrapper
{
    public virtual string GetHostEntry(string ip)
    {
        string hostname = string.Empty;

        IPHostEntry ipHostEntry = Dns.GetHostEntry(ip);

        hostname = ipHostEntry.HostName;

        return hostname;
    }
}