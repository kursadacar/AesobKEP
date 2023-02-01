using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyYetkiliKayitSonuc
	{
		private bool durumSpecified;

		public int? Durum { get; set; }

		public string HataAciklama { get; set; }

		public string Parola { get; set; }

		public string Sifre { get; set; }

		internal static EyYetkiliKayitSonuc GetEyYetkiliKayitSonuc(eyYetkiliKayitSonuc eySonuc)
		{
			return new EyYetkiliKayitSonuc
			{
				Durum = eySonuc.durum,
				durumSpecified = eySonuc.durumSpecified,
				HataAciklama = eySonuc.hataaciklama
			};
		}

		public static eyYetkiliKayitSonuc ConvertToWebServiceSonuc(EyYetkiliKayitSonuc eySonuc)
		{
			return new eyYetkiliKayitSonuc
			{
				durum = eySonuc.Durum,
				durumSpecified = eySonuc.durumSpecified,
				hataaciklama = eySonuc.HataAciklama
			};
		}
	}
}
