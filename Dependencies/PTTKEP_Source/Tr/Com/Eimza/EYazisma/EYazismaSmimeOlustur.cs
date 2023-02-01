using System.Collections.Generic;
using Tr.Com.Eimza.EYazisma.Config;
using Tr.Com.Eimza.Smime;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaSmimeOlustur
	{
		internal static EySmimeOlusturSonuc SmimeYap(string kimden, List<string> kime, List<string> cc, List<string> bcc, string konu, byte[] icerik, byte[] mimeP7sImzasi, EYazismaPaketTur mailTip, string mailTipId, EYazismaApiConfig config)
		{
			EyMimeMessage mimeMesaj = new EyMimeMessage
			{
				Kimden = kimden,
				Kime = kime,
				Cc = cc,
				Bcc = bcc,
				Konu = konu,
				MailTipi = mailTip,
				MailTipId = mailTipId
			};
			return new SmimeCreater().Create(mimeMesaj, icerik, mimeP7sImzasi, config);
		}

		internal static EySmimeOlusturSonuc SMimeYap(EyMimeMessage mesaj, byte[] icerik, byte[] mimeP7sImzasi, EYazismaApiConfig config)
		{
			return new SmimeCreater().Create(mesaj, icerik, mimeP7sImzasi, config);
		}
	}
}
