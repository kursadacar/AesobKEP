using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyDurumSonuc
	{
		private bool durumSpecified;

		public int? Durum { get; set; }

		public string HataAciklama { get; set; }

		public static EyDurumSonuc GetEyDurumSonuc(eyDurumSonuc eySonuc)
		{
			return new EyDurumSonuc
			{
				Durum = eySonuc.durum,
				durumSpecified = eySonuc.durumSpecified,
				HataAciklama = eySonuc.hataaciklama
			};
		}

		public static eyDurumSonuc ConvertToWebServiceSonuc(EyDurumSonuc eySonuc)
		{
			return new eyDurumSonuc
			{
				durum = eySonuc.Durum,
				durumSpecified = eySonuc.durumSpecified,
				hataaciklama = eySonuc.HataAciklama
			};
		}
	}
}
