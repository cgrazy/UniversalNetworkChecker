// See https://aka.ms/new-console-template for more information
using System.Net.NetworkInformation;

internal class PingWorker
{
    private IPingWrapper ping;

    internal PingWorker(IPingWrapper pingWrapper)
    {
        ping = pingWrapper;
    }

    internal PingWorker()
    {
        ping = new PingWrapper();
    }

    internal IPingReplyWrapper DoPing(string ip)
    {
        return ping.Send(ip);
    }
}

public interface IPingWrapper
{
    public IPingReplyWrapper Send(string ip);
}

internal class PingWrapper : IPingWrapper
{
    internal int TimeOut { get; set; }

    private byte[] packageSize = new byte[64];

    internal PingWrapper()
    {
        TimeOut = 1000;
    }

    public virtual IPingReplyWrapper Send(string ip)
    {
        var ping = new Ping();

        PingReplyWrapper pingReply = new PingReplyWrapper(ping.Send(ip, TimeOut, packageSize));

        return pingReply;
    }

    public virtual IPingReplyWrapper Send(string ip, PingOptionsWrapper pow)
    {
        var ping = new Ping();

        PingReplyWrapper pingReply = new PingReplyWrapper(ping.Send(ip, TimeOut, packageSize, pow.PingOptions));

        return pingReply;
    }
}

public interface IPingReplyWrapper
{
    public bool? IsPingSuccessful { get; set; }
    public PingReply PingReply { get; set; }
}


public class PingReplyWrapper : IPingReplyWrapper
{
    public bool? IsPingSuccessful { get; set; }

    public PingReply PingReply { get; set; }

    public PingReplyWrapper(PingReply pingReply)
    {
        IsPingSuccessful = pingReply.Status == IPStatus.Success;

        PingReply = pingReply;
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string? ToString()
    {
        return base.ToString();
    }
}