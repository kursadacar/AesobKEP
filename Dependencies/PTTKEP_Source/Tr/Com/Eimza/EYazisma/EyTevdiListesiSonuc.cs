using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyTevdiListesiSonuc
	{
		public int Durum { get; set; }

		private bool DurumSpecified { get; set; }

		public string HataAciklama { get; set; }

		public string TevdiListeNo { get; set; }

		public decimal ToplamUcret { get; set; }

		private bool ToplamUcretSpecified { get; set; }

		public string[] Barkod { get; set; }

		internal static EyTevdiListesiSonuc GetEyTevdiListesiSonuc(eyTevdiListesiSonuc eySonuc)
		{
			return new EyTevdiListesiSonuc
			{
				Durum = eySonuc.durum,
				DurumSpecified = eySonuc.durumSpecified,
				Barkod = eySonuc.Barkod,
				TevdiListeNo = eySonuc.TevdiListeNo,
				ToplamUcret = eySonuc.ToplamUcret,
				ToplamUcretSpecified = eySonuc.ToplamUcretSpecified,
				HataAciklama = eySonuc.hataaciklama
			};
		}

		public static eyTevdiListesiSonuc ConvertToWebServiceSonuc(EyTevdiListesiSonuc eySonuc)
		{
			return new eyTevdiListesiSonuc
			{
				durum = eySonuc.Durum,
				durumSpecified = eySonuc.DurumSpecified,
				Barkod = eySonuc.Barkod,
				TevdiListeNo = eySonuc.TevdiListeNo,
				ToplamUcret = eySonuc.ToplamUcret,
				ToplamUcretSpecified = eySonuc.ToplamUcretSpecified,
				hataaciklama = eySonuc.HataAciklama
			};
		}
	}
}
