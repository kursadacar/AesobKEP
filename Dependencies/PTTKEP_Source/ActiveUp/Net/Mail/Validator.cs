using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using ActiveUp.Net.Dns;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class Validator
	{
		private ServerCollection _dnsServers = new ServerCollection();

		public static bool ValidateSyntax(string address)
		{
			return Regex.IsMatch(address, "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
		}

		public static bool ValidateSyntax(Address address)
		{
			int.Parse("20", NumberStyles.HexNumber);
			return ValidateSyntax(address.Email);
		}

		public static AddressCollection ValidateSyntax(AddressCollection addresses)
		{
			AddressCollection addressCollection = new AddressCollection();
			foreach (Address address in addresses)
			{
				if (!ValidateSyntax(address.Email))
				{
					addressCollection.Add(address);
				}
			}
			return addressCollection;
		}

		public static MxRecordCollection GetMxRecords(string address)
		{
			return GetMxRecords(address, 5000);
		}

		public static MxRecordCollection GetMxRecords(string address, int timeout)
		{
			ArrayList listNameServers = GetListNameServers();
			if (listNameServers.Count > 0)
			{
				Logger.AddEntry("Name servers found : " + listNameServers.Count, 0);
				foreach (string item in listNameServers)
				{
					if (item.Length > 3)
					{
						try
						{
							Logger.AddEntry("Ask " + item + ":53 for MX records.", 0);
							return GetMxRecords(address, item, 53, timeout);
						}
						catch
						{
							Logger.AddEntry("Can't connect to " + item + ":53", 0);
						}
					}
				}
			}
			Logger.AddEntry("Can't connect to any of the specified DNS servers.", 0);
			return null;
		}

		public static byte[] GetTxtRecords(string address)
		{
			return GetTxtRecords(address, 5000);
		}

		public static byte[] GetTxtRecords(string address, int timeout)
		{
			ArrayList listNameServers = GetListNameServers();
			if (listNameServers.Count > 0)
			{
				Logger.AddEntry("Name servers found : " + listNameServers.Count, 0);
				foreach (string item in listNameServers)
				{
					if (item.Length > 3)
					{
						Logger.AddEntry("Ask " + item + ":53 for TXT records.", 0);
						return GetTxtRecords(address, item, 53);
					}
				}
			}
			Logger.AddEntry("Can't connect to any of the specified DNS servers.", 0);
			return null;
		}

		public static MxRecordCollection GetMxRecords(string address, ServerCollection dnsServers)
		{
			return GetMxRecords(address, dnsServers, 5000);
		}

		public static MxRecordCollection GetMxRecords(string address, ServerCollection dnsServers, int timeout)
		{
			if (dnsServers == null)
			{
				dnsServers = new ServerCollection();
			}
			if (dnsServers.Count == 0)
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				for (int i = 0; i < allNetworkInterfaces.Length; i++)
				{
					IPInterfaceProperties iPProperties = allNetworkInterfaces[i].GetIPProperties();
					if (iPProperties.DnsAddresses.Count <= 0)
					{
						continue;
					}
					foreach (IPAddress dnsAddress in iPProperties.DnsAddresses)
					{
						dnsServers.Add(dnsAddress.ToString(), 53);
					}
				}
			}
			foreach (Server dnsServer in dnsServers)
			{
				try
				{
					return GetMxRecords(address, dnsServer.Host, dnsServer.Port, timeout);
				}
				catch
				{
					Logger.AddEntry("Can't connect to " + dnsServer.Host + ":" + dnsServer.Port, 0);
				}
			}
			return GetMxRecords(address);
		}

		public static MxRecordCollection GetMxRecords(string address, string host, int port)
		{
			return GetMxRecords(address, host, port, 5000);
		}

		public static MxRecordCollection GetMxRecords(string address, string host, int port, int timeout)
		{
			MxRecordCollection mxRecordCollection = new MxRecordCollection();
			foreach (Answer answer in new DnsQuery(IPAddress.Parse(host))
			{
				RecursiveQuery = true,
				DnsServer = 
				{
					Port = port
				},
				Domain = address
			}.QueryServer(RecordType.MX, timeout).Answers)
			{
				MXRecord mXRecord = (MXRecord)answer.Data;
				mxRecordCollection.Add(mXRecord.Domain, mXRecord.Preference);
			}
			return mxRecordCollection;
		}

		public static byte[] GetTxtRecords(string address, string host, int port)
		{
			byte[] array = new byte[12]
			{
				0, 0, 1, 0, 0, 1, 0, 0, 0, 0,
				0, 0
			};
			string[] array2 = address.Split('.');
			byte[] array3 = new byte[address.Length + 2];
			int num = 0;
			string[] array4 = array2;
			foreach (string text in array4)
			{
				array3[num++] = Convert.ToByte(text.Length);
				string text2 = text;
				foreach (char value in text2)
				{
					array3[num++] = Convert.ToByte(value);
				}
				array3[num] = 0;
			}
			byte[] array5 = new byte[4] { 0, 16, 0, 1 };
			byte[] array6 = new byte[array.Length + array3.Length + array5.Length];
			array.CopyTo(array6, 0);
			array3.CopyTo(array6, array.Length);
			array5.CopyTo(array6, array.Length + array3.Length);
			IPEndPoint remote = new IPEndPoint(IPAddress.Parse(host), 53);
			TimedUdpClient timedUdpClient = new TimedUdpClient();
			timedUdpClient.Connect(remote);
			timedUdpClient.Send(array6, array6.Length);
			byte[] array7;
			try
			{
				array7 = timedUdpClient.Receive(ref remote);
			}
			catch (Exception)
			{
				timedUdpClient.Close();
				throw new Exception("Can't connect to DNS server.");
			}
			num = array6.Length;
			GetLabelsByPos(array7, ref num);
			num += 7;
			byte b = array7[num];
			byte[] array8 = new byte[array7.Length - num - 4];
			Array.Copy(array7, num + 4, array8, 0, array7.Length - num - 4);
			return array8;
		}

		private static string GetLabelsByPos(byte[] streamData, ref int pos)
		{
			int num = pos;
			bool flag = false;
			string text = string.Empty;
			string @string = Encoding.ASCII.GetString(streamData, 0, streamData.Length);
			byte b = streamData[num];
			while (b != 0 && !flag)
			{
				if ((b & 0xC0) == 192)
				{
					int pos2 = ((streamData[num] != 192) ? streamData[num + 1] : ((streamData[num] - 192) * 256 + streamData[num + 1]));
					text += GetLabelsByPos(streamData, ref pos2);
					text += ".";
					num += 2;
					flag = true;
				}
				else
				{
					text = text + @string.Substring(num + 1, b) + ".";
					num = num + b + 1;
					b = streamData[num];
				}
			}
			if (flag)
			{
				pos = num;
			}
			else
			{
				pos = num + 1;
			}
			if (text.Length > 0)
			{
				return text.TrimEnd('.');
			}
			return text;
		}

		public static ArrayList GetListNameServers()
		{
			ArrayList arrayList = new ArrayList();
			foreach (IPAddress item in (IEnumerable<IPAddress>)DnsQuery.GetMachineDnsServers())
			{
				arrayList.Add(item.ToString());
			}
			return arrayList;
		}

		private static bool IsPresent(ArrayList list, string valueToTest)
		{
			return list.Cast<string>().Any((string valueList) => valueToTest == valueList);
		}
	}
}
