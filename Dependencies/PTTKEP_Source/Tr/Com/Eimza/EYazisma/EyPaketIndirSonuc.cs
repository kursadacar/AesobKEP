using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyPaketIndirSonuc
	{
		private bool durumSpecified;

		public int Durum { get; set; }

		public base64Binary[] EyazismaPaketi { get; set; }

		public string HataAciklama { get; set; }

		internal static EyPaketIndirSonuc GetEyIndirSonuc(eyIndirSonuc eySonuc)
		{
			return new EyPaketIndirSonuc
			{
				Durum = eySonuc.durum,
				durumSpecified = eySonuc.durumSpecified,
				EyazismaPaketi = eySonuc.ePaket,
				HataAciklama = eySonuc.hataaciklama
			};
		}

		public static eyIndirSonuc ConvertToWebServiceSonuc(EyPaketIndirSonuc eySonuc)
		{
			return new eyIndirSonuc
			{
				durum = eySonuc.Durum,
				durumSpecified = eySonuc.durumSpecified,
				ePaket = eySonuc.EyazismaPaketi,
				hataaciklama = eySonuc.HataAciklama
			};
		}
	}
}
