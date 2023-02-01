using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyPaketSonuc
	{
		public int?[] Durum { get; set; }

		public string[] KepId { get; set; }

		public int?[] KepSiraNo { get; set; }

		public string[] From { get; set; }

		public string[] FromKep { get; set; }

		public string[] Konu { get; set; }

		public string[] Tur { get; set; }

		public string[] HataAciklama { get; set; }

		public string[] OrjinalMesajId { get; set; }

		internal static EyPaketSonuc GetEyPaketSonuc(eyPaketSonuc eySonuc)
		{
			return new EyPaketSonuc
			{
				Durum = eySonuc.durum,
				From = eySonuc.from,
				FromKep = eySonuc.fromKep,
				HataAciklama = eySonuc.hataaciklama,
				KepId = eySonuc.kepId,
				KepSiraNo = eySonuc.kepSiraNo,
				Konu = eySonuc.konu,
				Tur = eySonuc.tur,
				OrjinalMesajId = eySonuc.OrgMesajId
			};
		}

		public static eyPaketSonuc ConvertToWebServiceSonuc(EyPaketSonuc eySonuc)
		{
			return new eyPaketSonuc
			{
				durum = eySonuc.Durum,
				from = eySonuc.From,
				fromKep = eySonuc.FromKep,
				hataaciklama = eySonuc.HataAciklama,
				kepId = eySonuc.KepId,
				kepSiraNo = eySonuc.KepSiraNo,
				konu = eySonuc.Konu,
				tur = eySonuc.Tur,
				OrgMesajId = eySonuc.OrjinalMesajId
			};
		}
	}
}
