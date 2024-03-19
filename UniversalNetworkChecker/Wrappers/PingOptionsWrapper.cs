// See https://aka.ms/new-console-template for more information
using System.Net.NetworkInformation;

internal class PingOptionsWrapper
{
    internal PingOptions PingOptions { get; private set; }

    public PingOptionsWrapper(int ttl, bool dontFragement)
    {
        PingOptions = new PingOptions(ttl, dontFragement);
    }

}