using System.Collections.Generic;

namespace Tr.Com.Eimza.SmartCard.SmartCardTemplates
{
	internal class SafeSignTemplate : CardTemplate
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

		static SafeSignTemplate()
		{
			_ATR_HASHES = new List<string>();
			ATR_HASHES.Add("3BBB1800C01031FE4580670412B00303000081053C");
			ATR_HASHES.Add("3BFA1800FF8131FE454A434F5032315632333165");
			ATR_HASHES.Add("3BB79400C03E31FE6553504B32339000AE");
			ATR_HASHES.Add("3BF81800FF8131FE454A434F507632343143");
		}

		public override List<string> GetATRHashes()
		{
			return _ATR_HASHES;
		}
	}
}
