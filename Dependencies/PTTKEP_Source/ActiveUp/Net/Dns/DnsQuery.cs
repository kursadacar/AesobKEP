using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace ActiveUp.Net.Dns
{
	public class DnsQuery
	{
		private const int DNS_PORT = 53;

		private const int MAX_TRIES = 1;

		private const byte IN_CLASS = 1;

		private byte[] query;

		private string domain;

		private IPEndPoint dnsServer;

		private bool recursiveQuery = true;

		private Socket reqSocket;

		private int numTries;

		private int reqId;

		public byte[] Query
		{
			get
			{
				return query;
			}
			set
			{
				query = value;
			}
		}

		public string Domain
		{
			get
			{
				return domain;
			}
			set
			{
				if (value.Length == 0 || value.Length > 255 || !Regex.IsMatch(value, "^[a-z|A-Z|0-9|\\-|_]{1,63}(\\.[a-z|A-Z|0-9|\\-]{1,63})+$"))
				{
					throw new DnsQueryException("Invalid Domain Name", null);
				}
				domain = value;
			}
		}

		public IPEndPoint DnsServer
		{
			get
			{
				return dnsServer;
			}
			set
			{
				dnsServer = value;
			}
		}

		public bool RecursiveQuery
		{
			get
			{
				return recursiveQuery;
			}
			set
			{
				recursiveQuery = value;
			}
		}

		public DnsQuery()
		{
		}

		public DnsQuery(string serverUrl)
		{
			IPHostEntry hostEntry = System.Net.Dns.GetHostEntry(serverUrl);
			if (hostEntry.AddressList.Length != 0)
			{
				dnsServer = new IPEndPoint(hostEntry.AddressList[0], 53);
				return;
			}
			throw new DnsQueryException("Invalid DNS Server Name Specified", null);
		}

		public DnsQuery(IPAddress dnsAddress)
		{
			dnsServer = new IPEndPoint(dnsAddress, 53);
		}

		public DnsAnswer QueryServer(RecordType recType, int timeout)
		{
			if (dnsServer == null)
			{
				throw new DnsQueryException("There is no Dns server set in Dns Query Component", null);
			}
			if (!ValidRecordType(recType))
			{
				throw new DnsQueryException("Invalid Record Type submitted to Dns Query Component", null);
			}
			DnsAnswer dnsAnswer = null;
			numTries = 0;
			byte[] array = new byte[512];
			Exception[] array2 = new Exception[1];
			while (numTries < 1)
			{
				try
				{
					CreateDnsQuery(recType);
					reqSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
					reqSocket.ReceiveTimeout = timeout;
					reqSocket.SendTo(query, query.Length, SocketFlags.None, dnsServer);
					reqSocket.Receive(array);
					if (array[0] == query[0] && array[1] == query[1])
					{
						dnsAnswer = new DnsAnswer(array);
					}
					numTries++;
					if (dnsAnswer.ReturnCode == ReturnCode.Success)
					{
						return dnsAnswer;
					}
				}
				catch (SocketException ex)
				{
					array2[numTries] = ex;
					numTries++;
					reqId++;
					if (numTries > 1)
					{
						throw new DnsQueryException("Failure Querying DNS Server", array2);
					}
				}
				finally
				{
					reqId++;
					reqSocket.Close();
					Query = null;
				}
			}
			return dnsAnswer;
		}

		public DnsAnswer QueryServer(RecordType recType)
		{
			return QueryServer(recType, 5000);
		}

		private void CreateDnsQuery(RecordType recType)
		{
			List<byte> list = new List<byte>();
			list.Add((byte)(reqId >> 8));
			list.Add((byte)reqId);
			list.Add((byte)(0u | (RecursiveQuery ? 1u : 0u)));
			list.Add(0);
			list.Add(0);
			list.Add(1);
			for (int i = 0; i < 6; i++)
			{
				list.Add(0);
			}
			InsertDomainName(list, domain);
			list.Add(0);
			list.Add((byte)recType);
			list.Add(0);
			list.Add(1);
			Query = list.ToArray();
		}

		private void InsertDomainName(List<byte> data, string domain)
		{
			int num = 0;
			int num2;
			for (num2 = 0; num2 < domain.Length; num2++)
			{
				int num3 = num2;
				num2 = domain.IndexOf('.', num2);
				num = num2 - num3;
				if (num < 0)
				{
					num = domain.Length - num3;
				}
				data.Add((byte)num);
				for (int i = 0; i < num; i++)
				{
					data.Add((byte)domain[num3++]);
				}
				num2 = num3;
			}
			data.Add(0);
		}

		private bool ValidRecordType(RecordType t)
		{
			if (!Enum.IsDefined(typeof(RecordType), t))
			{
				return t == RecordType.All;
			}
			return true;
		}

		public static List<IPAddress> GetMachineDnsServers()
		{
			List<IPAddress> list = new List<IPAddress>();
			IPGlobalProperties.GetIPGlobalProperties();
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			for (int i = 0; i < allNetworkInterfaces.Length; i++)
			{
				foreach (IPAddress dnsAddress in allNetworkInterfaces[i].GetIPProperties().DnsAddresses)
				{
					list.Add(dnsAddress);
				}
			}
			return list;
		}
	}
}
