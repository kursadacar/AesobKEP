using System.Collections.Generic;

namespace Tr.Com.Eimza.SmartCard.SmartCardTemplates
{
	internal class OmniKeyTemplate : CardTemplate
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

		static OmniKeyTemplate()
		{
			_ATR_HASHES = new List<string>();
			ATR_HASHES.Add("3BF2180002C10A31FE58C80874");
		}

		public override List<string> GetATRHashes()
		{
			return _ATR_HASHES;
		}
	}
}
