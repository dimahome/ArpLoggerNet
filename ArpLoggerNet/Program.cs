using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;

namespace ArpLoggerNet
{
	class Program
	{

		static bool notUpdate = false;
		static bool returnAlways = false;

		static bool IgnoreMAC(String strMACForTest)
		{
            if (strMACForTest == "00-00-00-00-00-00" | 
				strMACForTest == "ff-ff-ff-ff-ff-ff")
			     { return true; }

			else { return false;  }

		}
	
		static bool IgnoreIP(String strIPForTest)
        {
			if (strIPForTest == "0.0.0.0" |
			strIPForTest == "127.0.0.0" |
			strIPForTest == "255.255.255.255" |
			strIPForTest == "192.168.255.255" |
			strIPForTest == "10.255.255.255" |
			strIPForTest.Substring(0, 4) == "224." |
			strIPForTest.Substring(0, 4) == "225." |
			strIPForTest.Substring(0, 4) == "226." |
			strIPForTest.Substring(0, 4) == "227." |
			strIPForTest.Substring(0, 4) == "228." |
			strIPForTest.Substring(0, 4) == "229." |
			strIPForTest.Substring(0, 4) == "230." |
			strIPForTest.Substring(0, 4) == "231." |
			strIPForTest.Substring(0, 4) == "232." |
			strIPForTest.Substring(0, 4) == "233." |
			strIPForTest.Substring(0, 4) == "234." |
			strIPForTest.Substring(0, 4) == "235." |
			strIPForTest.Substring(0, 4) == "236." |
			strIPForTest.Substring(0, 4) == "237." |
			strIPForTest.Substring(0, 4) == "238." |
			strIPForTest.Substring(0, 4) == "239.")
			     { return true; }
			else { return false;  }
        }
		static string LoadTableEntries(string textread)
		{
			ProcessStartInfo psi = new ProcessStartInfo("arp", "-a");
			psi.UseShellExecute = false;
			psi.RedirectStandardOutput = true;
			Process p = Process.Start(psi);
			string arpOutputStrMulti = p.StandardOutput.ReadToEnd();
			char[] charsToTrim = { ' ', '\r', '\n' };
			int countStringWrite = 0;
			string appendtext = "";
			try
				{
				foreach (string strFromARP in arpOutputStrMulti.Split(System.Environment.NewLine[1]))
					{
					string strFromARPForSplit = strFromARP.Trim(charsToTrim).Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace("\r\n", "");
					string[] array_IP_MAC_Type = strFromARPForSplit.Split(charsToTrim);
					if (array_IP_MAC_Type.Length == 3)  //only 3 elements: (0)-IP (1)-MAC (2)-Type
					{
						if (array_IP_MAC_Type[1].Length == 17 & array_IP_MAC_Type[1].Contains("-")) //MAC length string = 17  00-00-00-00-00-00
						{
							if (IgnoreMAC(array_IP_MAC_Type[1]) | IgnoreIP(array_IP_MAC_Type[0]))
							{
								continue;
							}
							string strIPandMAC = array_IP_MAC_Type[0] + ' ' + array_IP_MAC_Type[1];
							string strIPHostName = "";
							if (!textread.Contains(strIPandMAC))
							{
								if (IgnoreMAC(array_IP_MAC_Type[1]) | IgnoreIP(array_IP_MAC_Type[0]))
								{
									strIPHostName = "";
								}
								else
								{
									try
									{
										strIPHostName = Dns.GetHostEntry(array_IP_MAC_Type[0]).HostName.ToString();
									}
									catch
									{
										strIPHostName = "";
									}
								}
								string strTypeChange = "";
								string strDataTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
								if (!textread.Contains(array_IP_MAC_Type[1]) & !textread.Contains(array_IP_MAC_Type[0]))
								{
									strTypeChange = "IP+MAC";
								}
								else if (!textread.Contains(array_IP_MAC_Type[1]))
								{
									strTypeChange = "MAC";
								}
								else
								{
									strTypeChange = "IP";
								}

								Console.WriteLine(strDataTime + " " + strIPandMAC + " " + strTypeChange + " " + strIPHostName);
								countStringWrite += 1;
								appendtext += (strDataTime + " " + strIPandMAC + " " + strTypeChange + " " + strIPHostName + System.Environment.NewLine);

								}
						}
						}

					}
				}
			 catch (ArgumentException e)
				{
				Console.WriteLine(e.Message);
				countStringWrite += 1;
				}
			if (countStringWrite == 0 & returnAlways)
				{
			    Console.WriteLine("-"); 
				}
	


			return appendtext;
		}


		static void OutputHelpText()
		{
			Console.WriteLine("ARPlogger. Ver 2.0");
			Console.WriteLine("Parses the ARP table and maintains a log file (ARP.log) with a list of new entries.");
			Console.WriteLine("Parameters:");
			Console.WriteLine("[/NotUpdateLog] | [-n]: New entries of APR table are displayed but not saved to the log file (APR.log).");
			Console.WriteLine("                        Not added log entries will be displayed as new each time the program is started.");
			Console.WriteLine("[/ReturnAlways] | [-r]  If this parameter is specified, then in the absence of data, the program will return the '-' character. Otherwise, it outputs nothing.");

		}

		static void Main(string[] args)
		{
			foreach (string strArg in args)
			{
				if (strArg.ToLower() == "/?" | strArg.ToLower() == "-help" | strArg.ToLower() == "-?")
				{
					OutputHelpText();
					return;
				}
				if (strArg.ToLower() == "/notupdatelog" | strArg.ToLower() == "-n" )
				{
					notUpdate = true;
				}
				if (strArg.ToLower() == "/returnalways" | strArg.ToLower() == "-r")
				{
					returnAlways = true;
				}

			}

			string PathLocalLog = Directory.GetCurrentDirectory() + "\\ARP.log";
			string text = "";
			if (!File.Exists(PathLocalLog))
			{
				// Create emply a file to write to.
				File.WriteAllText(PathLocalLog, "", System.Text.Encoding.UTF8);
			}
			text = File.ReadAllText(PathLocalLog);
			string textToAppendWrite = LoadTableEntries(text);
			if (!notUpdate)
            {
				File.AppendAllText(PathLocalLog, textToAppendWrite, System.Text.Encoding.UTF8);
			}


		}
	}
}
