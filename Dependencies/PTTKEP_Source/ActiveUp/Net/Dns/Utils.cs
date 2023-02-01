using System;
using System.Net;
using System.Net.NetworkInformation;

namespace ActiveUp.Net.Dns
{
	internal class Utils
	{
		public static string[] GetNetworkInterfaces()
		{
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			if (allNetworkInterfaces == null || allNetworkInterfaces.Length < 1)
			{
				throw new Exception("No network interfaces found");
			}
			string[] array = new string[allNetworkInterfaces.Length];
			int num = 0;
			NetworkInterface[] array2 = allNetworkInterfaces;
			foreach (NetworkInterface networkInterface in array2)
			{
				array[num] = networkInterface.Description;
				num++;
			}
			return array;
		}

		private static string[] GetAdapterIpAdresses(NetworkInterface adapter)
		{
			if (adapter == null)
			{
				throw new Exception("No network interfaces found");
			}
			IPInterfaceProperties iPProperties = adapter.GetIPProperties();
			string[] result = null;
			IPAddressCollection dnsAddresses = iPProperties.DnsAddresses;
			if (dnsAddresses != null)
			{
				result = new string[dnsAddresses.Count];
				int num = 0;
				{
					foreach (IPAddress item in dnsAddresses)
					{
						result[num] = item.ToString();
						num++;
					}
					return result;
				}
			}
			return result;
		}
	}
}
