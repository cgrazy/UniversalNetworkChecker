// See https://aka.ms/new-console-template for more information
using System.Net.NetworkInformation;

internal class PingWrapper
{
    private byte[] packageSize = new byte[64];
    private int timeout = 1000;

    private Ping ping = new();

    internal PingReply DoPing(string ip)
    {
        return ping.Send(ip, timeout, packageSize);
    }
}