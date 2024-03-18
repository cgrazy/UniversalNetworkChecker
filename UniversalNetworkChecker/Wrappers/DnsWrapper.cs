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
        return RetrieveHostNameOfIP(ip); ;
    }

    internal virtual string RetrieveHostNameOfIP(string ip)
    {
        IPHostEntry ipHostEntry = Dns.GetHostEntry(ip);

        return ipHostEntry.HostName;
    }
}