using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyGirisSonuc
	{
		public string Durum { get; set; }

		public string HataAciklama { get; set; }

		public string Metin { get; set; }

		public string Hash { get; set; }

		public string GuvenlikId { get; set; }

		internal static EyGirisSonuc GetEyGirisSonuc(eyGirisSonuc eySonuc)
		{
			return new EyGirisSonuc
			{
				Durum = eySonuc.durum,
				GuvenlikId = eySonuc.eGuvenlikId,
				Hash = eySonuc.eHash,
				Metin = eySonuc.eMetin,
				HataAciklama = eySonuc.hataaciklama
			};
		}

		public static eyGirisSonuc ConvertToWebServiceSonuc(EyGirisSonuc eySonuc)
		{
			return new eyGirisSonuc
			{
				durum = eySonuc.Durum,
				eGuvenlikId = eySonuc.GuvenlikId,
				eHash = eySonuc.Hash,
				eMetin = eySonuc.Metin,
				hataaciklama = eySonuc.HataAciklama
			};
		}
	}
}
