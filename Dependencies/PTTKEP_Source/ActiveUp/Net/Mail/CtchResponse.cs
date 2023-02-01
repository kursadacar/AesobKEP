using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace ActiveUp.Net.Mail
{
	public class CtchResponse
	{
		private string _version = string.Empty;

		private CtchSpam _spamClassification = CtchSpam.Unknown;

		private CtchVod _vodClassification = CtchVod.Unknown;

		private string _ctchFlag = string.Empty;

		private string _refID = string.Empty;

		private NameValueCollection _headers = new NameValueCollection();

		private string _fullResponse = string.Empty;

		public string Version
		{
			get
			{
				return _version;
			}
			set
			{
				_version = value;
			}
		}

		public CtchSpam SpamClassification
		{
			get
			{
				return _spamClassification;
			}
			set
			{
				_spamClassification = value;
			}
		}

		public CtchVod VodClassification
		{
			get
			{
				return _vodClassification;
			}
			set
			{
				_vodClassification = value;
			}
		}

		public string CtchFlag
		{
			get
			{
				return _ctchFlag;
			}
			set
			{
				_ctchFlag = value;
			}
		}

		public string RefID
		{
			get
			{
				return _refID;
			}
			set
			{
				_refID = value;
			}
		}

		public NameValueCollection Headers
		{
			get
			{
				return _headers;
			}
			set
			{
				_headers = value;
			}
		}

		public string FullResponse
		{
			get
			{
				return _fullResponse;
			}
			set
			{
				_fullResponse = value;
			}
		}

		private static string ReadHeaderName(string header)
		{
			return header.Split(':')[0].Trim();
		}

		private static string ReadHeaderValue(string header)
		{
			if (header.IndexOf(':') > -1)
			{
				return header.Split(':')[1].Trim();
			}
			return string.Empty;
		}

		public static CtchResponse ParseFromString(string response)
		{
			CtchResponse ctchResponse = new CtchResponse
			{
				FullResponse = response
			};
			try
			{
				using (StringReader stringReader = new StringReader(response))
				{
					string header;
					while ((header = stringReader.ReadLine()) != null)
					{
						string text = ReadHeaderName(header);
						switch (text.ToUpper())
						{
						case "X-CTCH-PVER":
							ctchResponse.Version = ReadHeaderValue(header);
							break;
						case "X-CTCH-SPAM":
							ctchResponse.SpamClassification = (CtchSpam)Enum.Parse(typeof(CtchSpam), ReadHeaderValue(header), true);
							break;
						case "X-CTCH-VOD":
							ctchResponse.VodClassification = (CtchVod)Enum.Parse(typeof(CtchVod), ReadHeaderValue(header), true);
							break;
						case "X-CTCH-FLAGS":
							ctchResponse.CtchFlag = ReadHeaderValue(header);
							break;
						case "X-CTCH-REFID":
							ctchResponse.RefID = ReadHeaderValue(header);
							break;
						default:
							ctchResponse.Headers.Add(text, ReadHeaderValue(header));
							break;
						}
					}
					return ctchResponse;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("The response could not be read:");
				Console.WriteLine(ex.Message);
				return ctchResponse;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("X-CTCH-Pver: ");
			stringBuilder.Append(Version);
			stringBuilder.Append("\r\n");
			stringBuilder.Append("X-CTCH-Spam: ");
			stringBuilder.Append(SpamClassification.ToString());
			stringBuilder.Append("\r\n");
			stringBuilder.Append("X-CTCH-VOD: ");
			stringBuilder.Append(VodClassification.ToString());
			stringBuilder.Append("\r\n");
			stringBuilder.Append("X-CTCH-Flags: ");
			stringBuilder.Append(CtchFlag);
			stringBuilder.Append("\r\n");
			stringBuilder.Append("X-CTCH-RefID: ");
			stringBuilder.Append(RefID);
			stringBuilder.Append("\r\n");
			string[] allKeys = Headers.AllKeys;
			foreach (string text in allKeys)
			{
				if (text.Trim() != string.Empty)
				{
					stringBuilder.Append(text);
					stringBuilder.Append(": ");
					stringBuilder.Append(Headers[text]);
					stringBuilder.Append("\r\n");
				}
			}
			return stringBuilder.ToString();
		}
	}
}
