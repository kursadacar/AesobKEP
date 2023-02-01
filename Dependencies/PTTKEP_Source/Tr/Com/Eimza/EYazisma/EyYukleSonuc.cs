using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyYukleSonuc
	{
		private bool durumSpecified;

		public int Durum { get; set; }

		public string KepId { get; set; }

		public string HataAciklama { get; set; }

		public static EyYukleSonuc GetEyYukleSonuc(eyYukleSonuc eySonuc)
		{
			return new EyYukleSonuc
			{
				Durum = eySonuc.durum,
				durumSpecified = eySonuc.durumSpecified,
				HataAciklama = eySonuc.hataaciklama,
				KepId = eySonuc.kepId
			};
		}

		public static eyYukleSonuc ConvertToWebServiceSonuc(EyYukleSonuc eySonuc)
		{
			return new eyYukleSonuc
			{
				durum = eySonuc.Durum,
				durumSpecified = eySonuc.durumSpecified,
				hataaciklama = eySonuc.HataAciklama,
				kepId = eySonuc.KepId
			};
		}
	}
}
