using System.Collections.Generic;
using System.Text;

namespace Tr.Com.Eimza.SmartCard
{
	internal class CardTypeConfig
	{
		private List<string> _atrs = new List<string>();

		private string _name;

		private string _lib;

		private string _lib32;

		private string _lib64;

		public List<string> Atrs
		{
			get
			{
				return _atrs;
			}
			set
			{
				_atrs = value;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public string Lib
		{
			get
			{
				return _lib;
			}
			set
			{
				_lib = value;
			}
		}

		public string Lib32
		{
			get
			{
				return _lib32;
			}
			set
			{
				_lib32 = value;
			}
		}

		public string Lib64
		{
			get
			{
				return _lib64;
			}
			set
			{
				_lib64 = value;
			}
		}

		public CardTypeConfig()
		{
		}

		public CardTypeConfig(string aName, string aLib, string aLib32, string aLib64, List<string> aAtrs)
		{
			_name = aName;
			_lib = aLib;
			_lib32 = aLib32;
			_lib64 = aLib64;
			if (aAtrs != null)
			{
				_atrs = aAtrs;
			}
		}

		public new string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder().Append("[ Card type: ").Append(_name).Append("\n")
				.Append("lib : ")
				.Append(_lib)
				.Append(", lib32 : ")
				.Append(_lib32)
				.Append(", lib64 : ")
				.Append(_lib64)
				.Append("\n");
			foreach (string atr in _atrs)
			{
				stringBuilder.Append("  atr : ").Append(atr).Append("\n");
			}
			stringBuilder.Append("]\n");
			return stringBuilder.ToString();
		}
	}
}
