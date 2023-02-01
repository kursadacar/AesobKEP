using System.Collections.Generic;

namespace Tr.Com.Eimza.SmartCard.SmartCardTemplates
{
	internal class KeyCorpTemplate : CardTemplate
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

		static KeyCorpTemplate()
		{
			_ATR_HASHES = new List<string>();
			ATR_HASHES.Add("3BB79400C03E31FE6553504B32339000AE");
		}

		public override List<string> GetATRHashes()
		{
			return _ATR_HASHES;
		}
	}
}
