# UniversalNetworkChecker

Tool to check accessability of different nodes in a network.

A list of hosts are given in a json formated file which are checked.

In a first version UniversalNetworkChecker just pings these hosts periodically until any key is pressed.

For every success a '_' is printed, in case of failure '|'. Up to 20 pings are reported in that way. After any key has been pressed, all times of failure are printed.

### Usage

````
Usage:

dotnet UniversalNetworkChecker.dll <file>  ( -ping [-out <outputFile> ] [ -append ] ) | ( -nslookup | -nslu ) | ( -traceroute | -tr [ -long ] )
   <file>            : json file containg the hosts to check.
   -ping             : uses the ping to check the hosts configured in <file>.
      -out <outputFile> : output file where whole opuput is written to.
      -append : if not set a new file will be created, otherwise output will be appended.
   -nslookup | -nslu : do a nslookup for all ip address configured in <file>.
   -traceroute | -tr : do a traceroute for all hostnames configured in <file>.
     -long : Also tries to do a nslookup for the route ip determined.
````

**Example**

````
[
  {
    "Hostname": "A",
    "IP": "192.168.10.3"
  },
  {
    "Hostname": "B",
    "IP": "192.168.10.100"
  },
  {
    "Hostname": "C",
    "IP": "192.168.10.200"
  }
]

````

````
dotnet UniversalNetworkChecker.dll UniversalNetworkCheckerConfig.json
Universal Network Checker
Hostname: A, IP: 192.168.10.3
Hostname: B, IP: 192.168.10.100
Hostname: www.google.de, IP: 8.8.8.8
--------------------------
hostname: A, IP: 192.168.10.3, Ping: Success, 0 ms
hostname: A, IP: 192.168.10.3,  avg: 0,00 ms , Result: ________|_
Hostname: B, IP: 192.168.10.100, Ping: TimedOut, 0 ms
hostname: B, IP: 192.168.10.100,  avg: 0,00 ms , Result: ________||
Hostname: www.google.de, IP: 8.8.8.8, Ping: TimedOut, 0 ms
hostname: www.google.de, IP: 8.8.8.8,  avg: 0,00 ms , Result: __________
````


After pressing any key ...

````
dotnet UniversalNetworkChecker.dll UniversalNetworkCheckerConfig.json
Hostname: A, IP: 192.168.10.3
Hostname: B, IP: 192.168.10.100
Hostname: www.google.de, IP: 8.8.8.8
--------------------------
hostname: A, IP: 192.168.10.3,  avg: 135,87 ms (min/median/max: 0,00 ms/2,00 ms/1005,00 ms), Result: ________|_|_|| (26,67 % loss)
 Failure times: 20:00:55, 20:01:03, 20:01:11, 20:01:15ssms
hostname: B, IP: 192.168.10.100,  avg: 68,47 ms (min/median/max: 0,00 ms/2,00 ms/1005,00 ms), Result: ________||_||_ (26,67 % loss)
 Failure times: 20:00:56, 20:01:00, 20:01:08, 20:01:12 mssms
hostname: www.google.de, IP: 8.8.8.8,  avg: 219,93 ms (min/median/max: 0,00 ms/23,00 ms/1030,00 ms), Result: __________|_|_ (13,33 % loss)
 Failure times: 20:01:05, 20:01:13.8, Ping: Success, 18 msms
````

### Change Log
**Version history**

| **Version**  | **Comment**  |
|:-----|:-----|  
| 0.0.1 | first draft | 

### Status
[![build_n_test](https://github.com/cgrazy/UniversalNetworkChecker/actions/workflows/build_n_test.yml/badge.svg)](https://github.com/cgrazy/UniversalNetworkChecker/actions/workflows/build_n_test.yml)