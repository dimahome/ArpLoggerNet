using System;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ArpLoggerNet
{
	
	class Program
	{
		static string LoadTableEntries(string textread)
		{
			ProcessStartInfo psi = new ProcessStartInfo("arp", "-a");
			psi.UseShellExecute = false;
			psi.RedirectStandardOutput = true;
			Process p = Process.Start(psi);
			string arpOutputStrMulti = p.StandardOutput.ReadToEnd();
			char[] charsToTrim = { ' ', '\r', '\n' };
			string text = textread;
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
							
							string strIPandMAC = array_IP_MAC_Type[0] + ' ' + array_IP_MAC_Type[1];

							if (!text.Contains(strIPandMAC))
								{
								string strTypeChange = "New";
								string strDataTime = DateTime.Now.ToString();
								if (!text.Contains(array_IP_MAC_Type[1]) & !text.Contains(array_IP_MAC_Type[0]))
								{
									strTypeChange +=  "IP+MAC";
								}
								else if (!text.Contains(array_IP_MAC_Type[1]))
								{
									strTypeChange +=  "MAC";
								}
								else
                                {
									strTypeChange +=  "IP";
								}

								Console.WriteLine(strDataTime + " " + strIPandMAC + " "+ strTypeChange);
								text +=  (strDataTime + " " + strIPandMAC + " " + strTypeChange + System.Environment.NewLine);

							}
						}
						}

					}
				}
			 catch (ArgumentException e)
				{
				Console.WriteLine(e.Message);
				}
			return text;
		}



		static void Main(string[] args)
		{
			string PathLocalLog = Directory.GetCurrentDirectory() + "\\ARP.log";
			string text = "";
			if (File.Exists(PathLocalLog))
			{
				using (StreamReader sr = new StreamReader(PathLocalLog))
				{
					text = sr.ReadToEnd();
				}
			}
			string textToWrite = LoadTableEntries(text);
			using (StreamWriter sw = new StreamWriter(PathLocalLog, false, Encoding.UTF8))
			{
				sw.Write(textToWrite);
			}
			
		}
	}
}
