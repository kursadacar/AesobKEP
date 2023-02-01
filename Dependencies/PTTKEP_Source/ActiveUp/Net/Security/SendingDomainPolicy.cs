using System.Text.RegularExpressions;
using ActiveUp.Net.Mail;

namespace ActiveUp.Net.Security
{
	public class SendingDomainPolicy
	{
		private string _n;

		private OutboundSigningPolicy _o = OutboundSigningPolicy.Some;

		private bool _t;

		private Address _r = new Address();

		public string Notes
		{
			get
			{
				return _n;
			}
			set
			{
				_n = value;
			}
		}

		public OutboundSigningPolicy OutboundSigningPolicy
		{
			get
			{
				return _o;
			}
			set
			{
				_o = value;
			}
		}

		public Address ReportTo
		{
			get
			{
				return _r;
			}
			set
			{
				_r = value;
			}
		}

		public static SendingDomainPolicy Parse(string input)
		{
			SendingDomainPolicy sendingDomainPolicy = new SendingDomainPolicy();
			foreach (Match item in Regex.Matches(input, "[a-zA-Z]+=[^;]+(?=(;|\\Z))"))
			{
				string text = item.Value.Substring(0, item.Value.IndexOf('='));
				string text2 = item.Value.Substring(item.Value.IndexOf('=') + 1);
				if (text.Equals("n"))
				{
					sendingDomainPolicy._n = text2;
				}
				else if (text.Equals("r"))
				{
					sendingDomainPolicy._r = Parser.ParseAddress(text2);
				}
				else if (text.Equals("o"))
				{
					if (text2.Equals("~"))
					{
						sendingDomainPolicy._o = OutboundSigningPolicy.Some;
					}
					else if (text2.Equals("-"))
					{
						sendingDomainPolicy._o = OutboundSigningPolicy.All;
					}
					else
					{
						sendingDomainPolicy._o = OutboundSigningPolicy.OtherOrNoPolicy;
					}
				}
				else if (text.Equals("t"))
				{
					sendingDomainPolicy._t = text2.Equals("y");
				}
			}
			return sendingDomainPolicy;
		}
	}
}
