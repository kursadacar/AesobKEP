using System.Collections.Generic;

namespace Tr.Com.Eimza.SmartCard.SmartCardTemplates
{
	internal class GemPlusTemplate : CardTemplate
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

		static GemPlusTemplate()
		{
			_ATR_HASHES = new List<string>();
			ATR_HASHES.Add("3B7D94000080318065B08301029083009000");
			ATR_HASHES.Add("3B6D000080318065B08301029083009000");
			ATR_HASHES.Add("3B6D00008065B08301019083009000");
		}

		public override List<string> GetATRHashes()
		{
			return _ATR_HASHES;
		}
	}
}
