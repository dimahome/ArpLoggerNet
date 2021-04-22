# ArpLoggerNet
ARP log for windows (Console application)

Creates an ARP.log file in the application launch directory.

example ARP.log:

22.04.2021 19:25:51 192.168.7.127 78-5d-c8-9b-70-5f NewIP+MAC

At each launch, it analyzes the output of "arp.exe -a" application and, 
if there is new information about ip or mac, it displays messages on the screen and adds entries to the log file.

Tested on windows server 2012 / windows 10
