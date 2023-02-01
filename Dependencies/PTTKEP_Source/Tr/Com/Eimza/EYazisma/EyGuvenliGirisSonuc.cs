using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyGuvenliGirisSonuc
	{
		public string Durum { get; set; }

		public string HataAciklama { get; set; }

		public string GuvenlikId { get; set; }

		internal static EyGuvenliGirisSonuc GetEyGuvenliGirisSonuc(eyGuvenliGirisSonuc eySonuc)
		{
			return new EyGuvenliGirisSonuc
			{
				Durum = eySonuc.durum,
				HataAciklama = eySonuc.hataaciklama,
				GuvenlikId = eySonuc.eGuvenlikId
			};
		}

		public static eyGuvenliGirisSonuc ConvertToWebServiceSonuc(EyGuvenliGirisSonuc eySonuc)
		{
			return new eyGuvenliGirisSonuc
			{
				durum = eySonuc.Durum,
				hataaciklama = eySonuc.HataAciklama,
				eGuvenlikId = eySonuc.GuvenlikId
			};
		}
	}
}
