using System.Collections.Generic;

namespace Tr.Com.Eimza.EYazisma
{
	public class EyMimeMessage
	{
		public string Konu { get; set; }

		public string Icerik { get; set; }

		public string Kimden { get; set; }

		public List<string> Kime { get; set; }

		public List<string> Cc { get; set; }

		public List<string> Bcc { get; set; }

		public EYazismaPaketTur MailTipi { get; set; }

		public string MailTipId { get; set; }
	}
}
