using System;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Tr.Com.Eimza.EYazisma.Utilities
{
	public static class MimeUtilities
	{
		private static readonly Random Random = new Random();

		private static readonly object SyncLock = new object();

		private static int id = 0;

		public static string GetUniqueMessageIDValue()
		{
			Version version = Assembly.GetCallingAssembly().GetName().Version;
			string value = "NetApi_v" + (((object)version != null) ? version.ToString() : null) + "@hs01.kep.tr";
			string text = Dns.GetHostName();
			if (string.IsNullOrEmpty(text))
			{
				text = "EYazismaApi@localhost";
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = Math.Abs(text.GetHashCode());
			stringBuilder.Append("<").Append(num.ToString()).Append('.')
				.Append(GetRandomNumber().ToString())
				.Append('.')
				.Append(GetUniqueId())
				.Append('.')
				.Append(GetCurrentMiliSecond().ToString())
				.Append('.')
				.Append("EYazisma.")
				.Append(value)
				.Append(">");
			return stringBuilder.ToString();
		}

		private static int GetUniqueId()
		{
			return ++id;
		}

		private static int GetRandomNumber()
		{
			lock (SyncLock)
			{
				return Math.Abs(Random.Next(100, int.MaxValue));
			}
		}

		public static bool EmailKontrol(string email)
		{
			if (!string.IsNullOrEmpty(email))
			{
				return Regex.IsMatch(email, "^(([\\w-]+\\.)+[\\w-]+|([a-zA-Z]{1}|[\\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z]+[\\w-]+\\.)+[a-zA-Z]{2,4})$");
			}
			return false;
		}

		public static long GetCurrentMiliSecond()
		{
			return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
		}
	}
}
