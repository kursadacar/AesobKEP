using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyKontorSonuc
	{
		private bool miktarSpecified;

		public decimal Miktar { get; set; }

		public string Cins { get; set; }

		internal static EyKontorSonuc GetKontorSonuc(kontorSonuc eySonuc)
		{
			return new EyKontorSonuc
			{
				Cins = eySonuc.cins,
				Miktar = eySonuc.miktar,
				miktarSpecified = eySonuc.miktarSpecified
			};
		}

		public static kontorSonuc ConvertToWebServiceSonuc(EyKontorSonuc eySonuc)
		{
			return new kontorSonuc
			{
				cins = eySonuc.Cins,
				miktar = eySonuc.Miktar,
				miktarSpecified = eySonuc.miktarSpecified
			};
		}
	}
}
