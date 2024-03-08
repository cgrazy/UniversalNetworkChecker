﻿// See https://aka.ms/new-console-template for more information
using System.Net.NetworkInformation;


Console.WriteLine("Universal Network Checker");

JsonFileWrapper jfw = new JsonFileWrapper();

string fileToLoad = args[0];
if (!File.Exists(fileToLoad))
{
    Console.WriteLine($"The given file {fileToLoad} doesn't exists.");
    return;
}

jfw.LoadJson(fileToLoad);

jfw.HostsToCheck.ForEach(h =>
{
    Console.WriteLine($"Hostname: {h.Hostname}, IP: {h.IP}");
});

Console.WriteLine("--------------------------");

int cnt = 0;

var resultsContainer = new UniversalNetworkCheckerResultContainer();

while (!Console.KeyAvailable)
{
    jfw.HostsToCheck.ForEach(h =>
    {
        UniversalNetworkCheckerResult tmp;
        if(!resultsContainer.Results.TryGetValue(h.Hostname, out tmp))
        {
            resultsContainer.Results.Add(h.Hostname, new UniversalNetworkCheckerResult(h.Hostname, h.IP));
        }
        else
        {
            Task<PingReply> task =  new Task<PingReply>(()=>DoPing(h));

            task.Start();

            task.WaitAsync(TimeSpan.FromSeconds(2));
            PingReply reply = task.Result;

            Console.WriteLine($"Hostname: {h.Hostname}, IP: {h.IP}, Ping: {reply.Status}, {reply.RoundtripTime} ms");

            resultsContainer.AddResult(h.Hostname, reply);

            Console.WriteLine(resultsContainer.Results[h.Hostname].GetOutputSting());
        }
    });

    if(++cnt>1)     Console.SetCursorPosition(0, Console.CursorTop - jfw.HostsToCheck.Count *2);
}

jfw.HostsToCheck.ForEach(h =>
{
    Console.WriteLine("                                                                         ");
    Console.SetCursorPosition(0, Console.CursorTop - 1);

    Console.WriteLine(resultsContainer.Results[h.Hostname].GetFullReport());

});

PingReply DoPing(Host host)
{
    var ping = new Ping();

    byte[] packageSize = new byte[64];

    var pingResult = ping.Send(host.IP, 1000, packageSize);
    
    return pingResult;
}