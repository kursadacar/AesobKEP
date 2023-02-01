using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tools
{
	internal class NtpClient
	{
		private struct SystemTime
		{
			public ushort year;

			public ushort month;

			public ushort dayOfWeek;

			public ushort day;

			public ushort hour;

			public ushort minute;

			public ushort second;

			public ushort milliseconds;
		}

		public const string ULAKNET_1_IPV4 = "ntp1.ulak.net.tr";

		public const string ULAKNET_2_IPV4 = "ntp2.ulak.net.tr";

		public const string ULAKNET_3_IPV4 = "ntp3.ulak.net.tr";

		public const string ULAKNET_1_IPV6 = "v6ntp1.ulak.net.tr";

		public const string ULAKNET_2_IPV6 = "v6ntp2.ulak.net.tr";

		private readonly string TimeServer;

		private const byte NTPDataLength = 48;

		private readonly byte[] NTPData = new byte[48];

		private const byte offReferenceID = 12;

		private const byte offReferenceTimestamp = 16;

		private const byte offOriginateTimestamp = 24;

		private const byte offReceiveTimestamp = 32;

		private const byte offTransmitTimestamp = 40;

		public DateTime DestinationTimestamp;

		public LeapIndicator LeapIndicator
		{
			get
			{
				switch ((byte)(NTPData[0] >> 6))
				{
				case 0:
					return LeapIndicator.NoWarning;
				case 1:
					return LeapIndicator.LastMinute61;
				case 2:
					return LeapIndicator.LastMinute59;
				default:
					return LeapIndicator.Alarm;
				}
			}
		}

		public byte VersionNumber
		{
			get
			{
				return (byte)((NTPData[0] & 0x38) >> 3);
			}
		}

		public Mode Mode
		{
			get
			{
				switch ((byte)(NTPData[0] & 7))
				{
				default:
					return Mode.Unknown;
				case 1:
					return Mode.SymmetricActive;
				case 2:
					return Mode.SymmetricPassive;
				case 3:
					return Mode.Client;
				case 4:
					return Mode.Server;
				case 5:
					return Mode.Broadcast;
				}
			}
		}

		public Stratum Stratum
		{
			get
			{
				byte b = NTPData[1];
				if (b == 0)
				{
					return Stratum.Unspecified;
				}
				if (b == 1)
				{
					return Stratum.PrimaryReference;
				}
				if (b <= 15)
				{
					return Stratum.SecondaryReference;
				}
				return Stratum.Reserved;
			}
		}

		public double PollInterval
		{
			get
			{
				return System.Math.Round(System.Math.Pow(2.0, (int)NTPData[2]));
			}
		}

		public double Precision
		{
			get
			{
				return 1000.0 * System.Math.Pow(2.0, (int)NTPData[3]);
			}
		}

		public double RootDelay
		{
			get
			{
				int num = 256 * (256 * (256 * NTPData[4] + NTPData[5]) + NTPData[6]) + NTPData[7];
				return 1000.0 * ((double)num / 65536.0);
			}
		}

		public double RootDispersion
		{
			get
			{
				int num = 256 * (256 * (256 * NTPData[8] + NTPData[9]) + NTPData[10]) + NTPData[11];
				return 1000.0 * ((double)num / 65536.0);
			}
		}

		public string ReferenceID
		{
			get
			{
				string text = "";
				switch (Stratum)
				{
				case Stratum.Unspecified:
				case Stratum.PrimaryReference:
				{
					string text3 = text;
					char c = (char)NTPData[12];
					text = text3 + c;
					string text4 = text;
					c = (char)NTPData[13];
					text = text4 + c;
					string text5 = text;
					c = (char)NTPData[14];
					text = text5 + c;
					string text6 = text;
					c = (char)NTPData[15];
					text = text6 + c;
					break;
				}
				case Stratum.SecondaryReference:
					switch (VersionNumber)
					{
					case 3:
					{
						string text2 = NTPData[12] + "." + NTPData[13] + "." + NTPData[14] + "." + NTPData[15];
						try
						{
							return Dns.GetHostEntry(text2).HostName + " (" + text2 + ")";
						}
						catch (Exception)
						{
							return "N/A";
						}
					}
					case 4:
					{
						DateTime dateTime = ComputeDate(GetMilliSeconds(12));
						TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
						text = (dateTime + utcOffset).ToString();
						break;
					}
					default:
						text = "N/A";
						break;
					}
					break;
				}
				return text;
			}
		}

		public DateTime ReferenceTimestamp
		{
			get
			{
				DateTime dateTime = ComputeDate(GetMilliSeconds(16));
				TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
				return dateTime + utcOffset;
			}
		}

		public DateTime OriginateTimestamp
		{
			get
			{
				return ComputeDate(GetMilliSeconds(24));
			}
		}

		public DateTime ReceiveTimestamp
		{
			get
			{
				DateTime dateTime = ComputeDate(GetMilliSeconds(32));
				TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
				return dateTime + utcOffset;
			}
		}

		public DateTime CurrentTimestamp
		{
			get
			{
				return ComputeDate(GetMilliSeconds(32)).ToLocalTime();
			}
		}

		public DateTime TransmitTimestamp
		{
			get
			{
				DateTime dateTime = ComputeDate(GetMilliSeconds(40));
				TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
				return dateTime + utcOffset;
			}
			set
			{
				SetDate(40, value);
			}
		}

		public int RoundTripDelay
		{
			get
			{
				return (int)(ReceiveTimestamp - OriginateTimestamp + (DestinationTimestamp - TransmitTimestamp)).TotalMilliseconds;
			}
		}

		public int LocalClockOffset
		{
			get
			{
				return (int)((ReceiveTimestamp - OriginateTimestamp - (DestinationTimestamp - TransmitTimestamp)).TotalMilliseconds / 2.0);
			}
		}

		private DateTime ComputeDate(ulong milliseconds)
		{
			TimeSpan timeSpan = TimeSpan.FromMilliseconds(milliseconds);
			return new DateTime(1900, 1, 1) + timeSpan;
		}

		private ulong GetMilliSeconds(byte offset)
		{
			ulong num = 0uL;
			ulong num2 = 0uL;
			for (int i = 0; i <= 3; i++)
			{
				num = 256 * num + NTPData[offset + i];
			}
			for (int j = 4; j <= 7; j++)
			{
				num2 = 256 * num2 + NTPData[offset + j];
			}
			return num * 1000 + num2 * 1000 / 4294967296uL;
		}

		private void SetDate(byte offset, DateTime date)
		{
			DateTime dateTime = new DateTime(1900, 1, 1, 0, 0, 0);
			ulong num = (ulong)(date - dateTime).TotalMilliseconds;
			ulong num2 = num / 1000uL;
			ulong num3 = num % 1000uL * 4294967296L / 1000uL;
			ulong num4 = num2;
			for (int num5 = 3; num5 >= 0; num5--)
			{
				NTPData[offset + num5] = (byte)(num4 % 256uL);
				num4 /= 256uL;
			}
			num4 = num3;
			for (int num6 = 7; num6 >= 4; num6--)
			{
				NTPData[offset + num6] = (byte)(num4 % 256uL);
				num4 /= 256uL;
			}
		}

		private void Initialize()
		{
			NTPData[0] = 27;
			for (int i = 1; i < 48; i++)
			{
				NTPData[i] = 0;
			}
			TransmitTimestamp = DateTime.Now;
		}

		public NtpClient(string TimeServer = "time.windows.com")
		{
			this.TimeServer = TimeServer;
		}

		public void Connect(bool UpdateSystemTime = false)
		{
			try
			{
				IPAddress[] addressList = Dns.GetHostEntry(TimeServer).AddressList;
				if (addressList == null || addressList.Length == 0)
				{
					throw new ArgumentException("Could not resolve ip address from '" + TimeServer + "'.", "ntpServer");
				}
				IPEndPoint remoteEP = new IPEndPoint(addressList[0], 123);
				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				socket.Connect(remoteEP);
				Initialize();
				socket.Send(NTPData);
				socket.Receive(NTPData);
				socket.Close();
				if (!IsResponseValid())
				{
					throw new Exception("Invalid response from " + TimeServer);
				}
				DestinationTimestamp = DateTime.Now;
			}
			catch (SocketException ex)
			{
				throw new Exception(ex.Message);
			}
			try
			{
				if (UpdateSystemTime && LeapIndicator == LeapIndicator.NoWarning)
				{
					SetTime();
				}
			}
			catch
			{
			}
		}

		public bool IsResponseValid()
		{
			if (NTPData.Length < 48 || Mode != Mode.Server)
			{
				return false;
			}
			return true;
		}

		public override string ToString()
		{
			string text = "Leap Indicator : ";
			switch (LeapIndicator)
			{
			case LeapIndicator.NoWarning:
				text += "No Warning";
				break;
			case LeapIndicator.LastMinute61:
				text += "Last minute has 61 seconds";
				break;
			case LeapIndicator.LastMinute59:
				text += "Last minute has 59 seconds";
				break;
			case LeapIndicator.Alarm:
				text += "Alarm Condition (clock not synchronized)";
				break;
			}
			text = text + "\r\nVersion number: " + VersionNumber + "\r\n";
			text += "Mode: ";
			switch (Mode)
			{
			case Mode.Unknown:
				text += "Unknown";
				break;
			case Mode.SymmetricActive:
				text += "Symmetric Active";
				break;
			case Mode.SymmetricPassive:
				text += "Symmetric Pasive";
				break;
			case Mode.Client:
				text += "Client";
				break;
			case Mode.Server:
				text += "Server";
				break;
			case Mode.Broadcast:
				text += "Broadcast";
				break;
			}
			text += "\r\nStratum: ";
			switch (Stratum)
			{
			case Stratum.Unspecified:
			case Stratum.Reserved:
				text += "Unspecified";
				break;
			case Stratum.PrimaryReference:
				text += "Primary Reference";
				break;
			case Stratum.SecondaryReference:
				text += "Secondary Reference";
				break;
			}
			text = text + "\r\nLocal time: " + TransmitTimestamp.ToString();
			text = text + "\r\nPrecision: " + Precision + " ms";
			text = text + "\r\nPoll Interval: " + PollInterval + " s";
			text = text + "\r\nReference ID: " + ReferenceID;
			text = text + "\r\nRoot Dispersion: " + RootDispersion + " ms";
			text = text + "\r\nRound Trip Delay: " + RoundTripDelay + " ms";
			text = text + "\r\nLocal Clock Offset: " + LocalClockOffset + " ms";
			return text + "\r\n";
		}

		[DllImport("kernel32.dll")]
		private static extern bool SetLocalTime(ref SystemTime time);

		private void SetTime()
		{
			try
			{
				DateTime dateTime = DateTime.Now.AddMilliseconds(LocalClockOffset);
				SystemTime time = default(SystemTime);
				time.year = (ushort)dateTime.Year;
				time.month = (ushort)dateTime.Month;
				time.dayOfWeek = (ushort)dateTime.DayOfWeek;
				time.day = (ushort)dateTime.Day;
				time.hour = (ushort)dateTime.Hour;
				time.minute = (ushort)dateTime.Minute;
				time.second = (ushort)dateTime.Second;
				time.milliseconds = (ushort)dateTime.Millisecond;
				SetLocalTime(ref time);
			}
			catch
			{
			}
		}
	}
}
