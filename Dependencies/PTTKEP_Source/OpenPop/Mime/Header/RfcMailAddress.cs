using System;
using System.Collections.Generic;
using System.Net.Mail;
using OpenPop.Common.Logging;
using OpenPop.Mime.Decode;

namespace OpenPop.Mime.Header
{
	public class RfcMailAddress
	{
		public string Address { get; private set; }

		public string DisplayName { get; private set; }

		public string Raw { get; private set; }

		public MailAddress MailAddress { get; private set; }

		public bool HasValidMailAddress
		{
			get
			{
				return MailAddress != null;
			}
		}

		private RfcMailAddress(MailAddress mailAddress, string raw)
		{
			if (mailAddress == null)
			{
				throw new ArgumentNullException("mailAddress");
			}
			if (raw == null)
			{
				throw new ArgumentNullException("raw");
			}
			MailAddress = mailAddress;
			Address = mailAddress.Address;
			DisplayName = mailAddress.DisplayName;
			Raw = raw;
		}

		private RfcMailAddress(string raw)
		{
			if (raw == null)
			{
				throw new ArgumentNullException("raw");
			}
			MailAddress = null;
			Address = string.Empty;
			DisplayName = raw;
			Raw = raw;
		}

		public override string ToString()
		{
			if (HasValidMailAddress)
			{
				return MailAddress.ToString();
			}
			return Raw;
		}

		internal static RfcMailAddress ParseMailAddress(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			input = EncodedWord.Decode(input.Trim());
			int num = input.LastIndexOf('<');
			int num2 = input.LastIndexOf('>');
			try
			{
				if (num >= 0 && num2 >= 0)
				{
					string displayName = ((num <= 0) ? string.Empty : input.Substring(0, num).Trim());
					num++;
					int length = num2 - num;
					string text = input.Substring(num, length).Trim();
					if (!string.IsNullOrEmpty(text))
					{
						return new RfcMailAddress(new MailAddress(text, displayName), input);
					}
				}
				if (input.Contains("@"))
				{
					return new RfcMailAddress(new MailAddress(input), input);
				}
			}
			catch (FormatException)
			{
				DefaultLogger.Log.LogError("RfcMailAddress: Improper mail address: \"" + input + "\"");
			}
			return new RfcMailAddress(input);
		}

		internal static List<RfcMailAddress> ParseMailAddresses(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			List<RfcMailAddress> list = new List<RfcMailAddress>();
			foreach (string item in (IEnumerable<string>)Utility.SplitStringWithCharNotInsideQuotes(input, ','))
			{
				list.Add(ParseMailAddress(item));
			}
			return list;
		}
	}
}
