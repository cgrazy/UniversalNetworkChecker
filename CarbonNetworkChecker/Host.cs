// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;


public class Hosts
{

    public string Hostname { get; set; }

    public string IP { get; set; }
}

/*
string name;
string ip;



var longRunningPingCommand = new LongRunnerPingCommandWrapper()
{
    IpAddress = myLongRunPingIp,
    Result = (color) => SetPingStatusColor(color),
    Roundtrip = (datetime, pingReply) => EvaluteLongRunningPingCommand(datetime, pingReply)
};
*/