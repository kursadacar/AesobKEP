using System.Collections.Generic;

namespace Tr.Com.Eimza.EYazisma
{
	public class EySmimeParcalaSonuc
	{
		public int Durum { get; set; }

		public string HataAciklama { get; set; }

		public List<Ek> Ekler { get; set; }

		public Ek ImzaP7s { get; set; }

		public string Konu { get; set; }

		public string Icerik { get; set; }

		public string Kimden { get; set; }

		public List<string> Kime { get; set; }

		public List<string> Cc { get; set; }

		public List<string> Bcc { get; set; }

		public EYazismaPaketTur MailTipi { get; set; }

		public string MailTipId { get; set; }

		public EySmimeParcalaSonuc()
		{
			Ekler = new List<Ek>();
			Kime = new List<string>();
			Cc = new List<string>();
			Bcc = new List<string>();
		}
	}
}
