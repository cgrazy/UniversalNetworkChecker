# UniversalNetworkChecker


````text
dotnet UniversalNetworkChecker.dll UniversalNetworkCheckerConfig.json
Universal Network Checker
Hostname: A, IP: 10.176.23.233
Hostname: B, IP: 10.81.76.94
Hostname: C, IP: 192.168.10.200
--------------------------
Hostname: A, IP: 10.176.23.233, Ping: Success, 0 ms
hostname: A, IP: 10.176.23.233,  avg: 0,00 ms , Result: _____
Hostname: B, IP: 10.81.76.94, Ping: TimedOut, 0 ms
hostname: B, IP: 10.81.76.94,  avg: 0,00 ms , Result: |||||
Hostname: C, IP: 192.168.10.200, Ping: TimedOut, 0 ms
hostname: C, IP: 192.168.10.200,  avg: 0,00 ms , Result: ||||
````


After pressing any key ...

````text
dotnet UniversalNetworkChecker.dll UniversalNetworkCheckerConfig.json
Universal Network Checker
Hostname: A, IP: 10.176.23.233
Hostname: B, IP: 10.81.76.94
Hostname: C, IP: 192.168.10.200
--------------------------
hostname: A, IP: 10.176.23.233,  avg: 0,00 ms , Result: ____________________
hostname: B, IP: 10.81.76.94,  avg: 0,00 ms , Result: |||||||||||||||||||| _
 Failure times: 13:02:57, 13:02:59, 13:03:01, 13:03:03, 13:03:05, 13:03:07, 13:03:09, 13:03:11, 13:03:13, 13:03:15, 13:03:17, 13:03:19, 13:03:21, 13:03:23, 13:03:25, 13:03:27, 13:03:29, 13:03:31, 13:03:33, 13:03:35, 13:03:37
hostname: C, IP: 192.168.10.200,  avg: 0,00 ms , Result: ||||||||||||||||||||
 Failure times: 13:02:58, 13:03:00, 13:03:02, 13:03:04, 13:03:06, 13:03:08, 13:03:10, 13:03:12, 13:03:14, 13:03:16, 13:03:18, 13:03:20, 13:03:22, 13:03:24, 13:03:26, 13:03:28, 13:03:30, 13:03:32, 13:03:34, 13:03:36, 13:03:38
````
