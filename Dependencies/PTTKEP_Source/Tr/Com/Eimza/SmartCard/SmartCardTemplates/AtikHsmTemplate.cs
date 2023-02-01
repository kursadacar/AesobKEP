using System.Collections.Generic;

namespace Tr.Com.Eimza.SmartCard.SmartCardTemplates
{
	internal class AtikHsmTemplate : CardTemplate
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

		static AtikHsmTemplate()
		{
			_ATR_HASHES = new List<string>();
		}

		public override List<string> GetATRHashes()
		{
			return _ATR_HASHES;
		}
	}
}
