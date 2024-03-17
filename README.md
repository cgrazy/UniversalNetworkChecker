# UniversalNetworkChecker

Tool to check accessability of different nodes in a network.

A list of hosts are given in a json formated file which are checked.

In a first version UniversalNetworkChecker just pings these hosts periodically until any key is pressed.

For every success a '_' is printed, in case of failure '|'. Up to 20 pings are reported in that way. After any key has been pressed, all times of failure are printed.

### Usage

````
dotnet UniversalNetworkChecker.dll <file> [-ping [-out <outputFile> | -append ] ] | [-nslookup | -nslu ]

Universal Network Checker
Usage:

   <file>            : json file containg the hosts to check.
   -ping             : uses the ping to check the hosts configured in <file>.
      -out <outputFile> : output file where whole opuput is written to.
      -append           : if not set a new file will be created, otherwise output will be appended.
   -nslookup | -nslu : do a nslookup for all ip address configured in <file>.

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
Hostname: C, IP: 192.168.10.200
--------------------------
Hostname: A, IP: 192.168.10.3, Ping: Success, 0 ms
hostname: A, IP: 1192.168.10.3,  avg: 0,00 ms , Result: _____
Hostname: B, IP: 192.168.10.100, Ping: TimedOut, 0 ms
hostname: B, IP: 192.168.10.100,  avg: 0,00 ms , Result: |||||
Hostname: C, IP: 192.168.10.200, Ping: TimedOut, 0 ms
hostname: C, IP: 192.168.10.200,  avg: 0,00 ms , Result: ||||
````


After pressing any key ...

````
dotnet UniversalNetworkChecker.dll UniversalNetworkCheckerConfig.json
Universal Network Checker
Hostname: A, IP: 192.168.10.3
Hostname: B, IP: 10.81.76.94
Hostname: C, IP: 192.168.10.200
--------------------------
hostname: A, IP: 192.168.10.3,  avg: 0,00 ms , Result: ____________________
hostname: B, IP: 192.168.10.100,  avg: 0,00 ms , Result: |||||||||||||||||||| _
 Failure times: 13:02:57, 13:02:59, 13:03:01, 13:03:03, 13:03:05, 13:03:07, 13:03:09, 13:03:11, 13:03:13, 13:03:15, 13:03:17, 13:03:19, 13:03:21, 13:03:23, 13:03:25, 13:03:27, 13:03:29, 13:03:31, 13:03:33, 13:03:35, 13:03:37
hostname: C, IP: 192.168.10.200,  avg: 0,00 ms , Result: ||||||||||||||||||||
 Failure times: 13:02:58, 13:03:00, 13:03:02, 13:03:04, 13:03:06, 13:03:08, 13:03:10, 13:03:12, 13:03:14, 13:03:16, 13:03:18, 13:03:20, 13:03:22, 13:03:24, 13:03:26, 13:03:28, 13:03:30, 13:03:32, 13:03:34, 13:03:36, 13:03:38
````

### Change Log
**Version history**

| **Version**  | **Comment**  |
|:-----|:-----|  
| 0.0.1 | first draft | 

### Status
[![build_n_test](https://github.com/cgrazy/UniversalNetworkChecker/actions/workflows/build_n_test.yml/badge.svg)](https://github.com/cgrazy/UniversalNetworkChecker/actions/workflows/build_n_test.yml)