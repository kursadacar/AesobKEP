using System;
using System.Text.RegularExpressions;

namespace ActiveUp.Net.Security
{
	public class PublicKeyRecord
	{
		private string _n;

		private string _p64;

		private string _g;

		private byte[] _p;

		private bool _t;

		private KeyType _k;

		public string Granularity
		{
			get
			{
				return _g;
			}
			set
			{
				_g = value;
			}
		}

		public KeyType KeyType
		{
			get
			{
				return _k;
			}
			set
			{
				_k = value;
			}
		}

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

		public string KeyDataBase64
		{
			get
			{
				return _p64;
			}
			set
			{
				_p64 = value;
			}
		}

		public byte[] KeyData
		{
			get
			{
				return _p;
			}
			set
			{
				_p = value;
			}
		}

		public bool InTestMode
		{
			get
			{
				return _t;
			}
			set
			{
				_t = value;
			}
		}

		public static PublicKeyRecord Parse(string input)
		{
			PublicKeyRecord publicKeyRecord = new PublicKeyRecord();
			foreach (Match item in Regex.Matches(input, "[a-zA-Z]+=[^;]+(?=(;|\\Z))"))
			{
				string text = item.Value.Substring(0, item.Value.IndexOf('='));
				string text2 = item.Value.Substring(item.Value.IndexOf('=') + 1);
				if (text.Equals("n"))
				{
					publicKeyRecord._n = text2;
				}
				else if (text.Equals("p"))
				{
					text2 = text2.Trim('\r', '\n').Replace(" ", "");
					while (text2.Length % 4 != 0)
					{
						text2 += "=";
					}
					publicKeyRecord._p64 = text2;
					publicKeyRecord._p = Convert.FromBase64String(publicKeyRecord._p64);
				}
				else if (text.Equals("k"))
				{
					if (text2.Equals("rsa"))
					{
						publicKeyRecord._k = KeyType.Rsa;
					}
				}
				else if (text.Equals("g"))
				{
					publicKeyRecord._g = text2;
				}
				else if (text.Equals("t"))
				{
					publicKeyRecord._t = text2.Equals("y");
				}
			}
			return publicKeyRecord;
		}
	}
}
