using System.Collections.Generic;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyYukleTebligatSonuc
	{
		public int Durum { get; set; }

		private bool DurumSpecified { get; set; }

		public string KepId { get; set; }

		public List<EyDonusBarkod> DonusBarkod { get; set; }

		public string HataAciklama { get; set; }

		internal static EyYukleTebligatSonuc GetEyYukleTebligatSonuc(eyYukleTebligatSonuc eySonuc)
		{
			return new EyYukleTebligatSonuc
			{
				Durum = eySonuc.durum,
				DurumSpecified = eySonuc.durumSpecified,
				KepId = eySonuc.kepId,
				DonusBarkod = EyDonusBarkod.GetEyDonusBarkod(eySonuc.DonusBarkod),
				HataAciklama = eySonuc.hataaciklama
			};
		}

		public static eyYukleTebligatSonuc ConvertToWebServiceSonuc(EyYukleTebligatSonuc eySonuc)
		{
			return new eyYukleTebligatSonuc
			{
				durum = eySonuc.Durum,
				durumSpecified = eySonuc.DurumSpecified,
				kepId = eySonuc.KepId,
				DonusBarkod = EyDonusBarkod.ConvertToWebServiceSonuc(eySonuc.DonusBarkod).ToArray(),
				hataaciklama = eySonuc.HataAciklama
			};
		}
	}
}
