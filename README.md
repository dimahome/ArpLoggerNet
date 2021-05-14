# ArpLoggerNet
ARP log for windows (Console application)

Creates an ARP.log file in the application launch directory.

example ARP.log file:

2021-05-14 09:33:37 192.168.7.1 b8-69-f4-09-e7-ae IP+MAC

2021-05-14 09:33:37 192.168.7.202 00-17-c8-67-93-06 IP+MAC KM679306


At each launch, it analyzes the output of "arp.exe -a" application and, 
if there is new information about ip or mac, it displays messages on the screen and adds entries to the log file.

Tested on windows server 2012 / windows 10

ARPlogger.exe /?

>>ARPlogger. Ver 2.0
>>
>>Parses the ARP table and maintains a log file (ARP.log) with a list of new entries.
>>
>>Parameters:
>>
>>[/NotUpdateLog] | [-n]: New entries of APR table are displayed but not saved to the log file (APR.log).
>>                        Not added log entries will be displayed as new each time the program is started.
>>                        
>>[/ReturnAlways] | [-r]  If this parameter is specified, then in the absence of data, the program will return the '-' character. Otherwise, it outputs nothing.

