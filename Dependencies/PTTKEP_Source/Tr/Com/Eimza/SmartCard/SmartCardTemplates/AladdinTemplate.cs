using System.Collections.Generic;

namespace Tr.Com.Eimza.SmartCard.SmartCardTemplates
{
	internal class AladdinTemplate : CardTemplate
	{
		protected static List<string> _ATR_HASHES;

		internal static List<string> ATR_HASHES
		{
			get
			{
				return _ATR_HASHES;
			}
			set
			{
				_ATR_HASHES = value;
			}
		}

		static AladdinTemplate()
		{
			_ATR_HASHES = new List<string>();
			_ATR_HASHES.Add("3BD5180081313A7D8073C8211030");
			_ATR_HASHES.Add("3BD518008131FE7D8073C82110F4");
		}

		public override List<string> GetATRHashes()
		{
			return _ATR_HASHES;
		}
	}
}
