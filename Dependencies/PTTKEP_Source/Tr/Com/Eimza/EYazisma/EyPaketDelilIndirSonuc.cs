using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyPaketDelilIndirSonuc
	{
		private bool durumSpecified;

		public int Durum { get; set; }

		public base64Binary EyazismaDelil { get; set; }

		public string HataAciklama { get; set; }

		internal static EyPaketDelilIndirSonuc GetEyPaketDelilIndirSonuc(eyPaketDelilIndirSonuc eySonuc)
		{
			return new EyPaketDelilIndirSonuc
			{
				Durum = eySonuc.durum,
				durumSpecified = eySonuc.durumSpecified,
				EyazismaDelil = eySonuc.eDelil,
				HataAciklama = eySonuc.hataaciklama
			};
		}

		public static eyPaketDelilIndirSonuc ConvertToWebServiceSonuc(EyPaketDelilIndirSonuc eySonuc)
		{
			return new eyPaketDelilIndirSonuc
			{
				durum = eySonuc.Durum,
				durumSpecified = eySonuc.durumSpecified,
				eDelil = eySonuc.EyazismaDelil,
				hataaciklama = eySonuc.HataAciklama
			};
		}
	}
}
