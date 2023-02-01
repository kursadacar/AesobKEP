using System;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyPaketDelilSonuc
	{
		public int?[] Durum { get; set; }

		public string[] DelilId { get; set; }

		public string[] DelilAciklama { get; set; }

		public string[] HataAciklama { get; set; }

		public int?[] DelilTurId { get; set; }

		private DateTime?[] Tarih { get; set; }

		private string[] KepHs { get; set; }

		internal static EyPaketDelilSonuc GetEyPaketDelilSonuc(eyPaketDelilSonuc eySonuc)
		{
			return new EyPaketDelilSonuc
			{
				DelilAciklama = eySonuc.delilaciklama,
				DelilId = eySonuc.delilId,
				Durum = eySonuc.durum,
				HataAciklama = eySonuc.hataaciklama,
				KepHs = eySonuc.kephs,
				Tarih = eySonuc.tarih,
				DelilTurId = eySonuc.delilTurId
			};
		}

		public static eyPaketDelilSonuc ConvertToWebServiceSonuc(EyPaketDelilSonuc eySonuc)
		{
			return new eyPaketDelilSonuc
			{
				delilaciklama = eySonuc.DelilAciklama,
				delilId = eySonuc.DelilId,
				durum = eySonuc.Durum,
				hataaciklama = eySonuc.HataAciklama,
				kephs = eySonuc.KepHs,
				tarih = eySonuc.Tarih,
				delilTurId = eySonuc.DelilTurId
			};
		}
	}
}
