using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyMimeYapSonuc
	{
		public string Durum { get; set; }

		public string HataAciklama { get; set; }

		public string Hash { get; set; }

		public string MesajId { get; set; }

		internal static EyMimeYapSonuc GetEyMimeYapSonuc(eyMimeYapSonuc eySonuc)
		{
			return new EyMimeYapSonuc
			{
				Durum = eySonuc.durum,
				HataAciklama = eySonuc.hataaciklama,
				Hash = eySonuc.eHash,
				MesajId = eySonuc.mesajid
			};
		}

		public static eyMimeYapSonuc ConvertToWebServiceSonuc(EyMimeYapSonuc eySonuc)
		{
			return new eyMimeYapSonuc
			{
				durum = eySonuc.Durum,
				hataaciklama = eySonuc.HataAciklama,
				eHash = eySonuc.Hash,
				mesajid = eySonuc.MesajId
			};
		}
	}
}
