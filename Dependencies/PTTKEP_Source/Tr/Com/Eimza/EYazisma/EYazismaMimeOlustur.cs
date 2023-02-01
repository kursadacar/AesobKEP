using System.Collections.Generic;
using Tr.Com.Eimza.EYazisma.Config;
using Tr.Com.Eimza.Smime;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaMimeOlustur
	{
		internal static EyMimeOlusturSonuc MimeYap(string from, List<string> to, List<string> cc, List<string> bcc, string subject, string content, List<Ek> attachments, EYazismaPaketTur mailType, string mailTypeId, OzetAlg hashAlg, EYazismaApiConfig config)
		{
			EyMimeMessage mimeMesaj = new EyMimeMessage
			{
				Kimden = from,
				Kime = to,
				Cc = cc,
				Bcc = bcc,
				Konu = subject,
				Icerik = content,
				MailTipi = mailType,
				MailTipId = mailTypeId
			};
			return new MimeCreater().Create(mimeMesaj, attachments, hashAlg, config);
		}

		internal static EyMimeOlusturSonuc MimeYap(string from, List<string> to, List<string> cc, List<string> bcc, string subject, string content, List<string> attachments, EYazismaPaketTur mailType, string mailTypeId, OzetAlg hashAlg, EYazismaApiConfig config)
		{
			EyMimeMessage mimeMesaj = new EyMimeMessage
			{
				Kimden = from,
				Kime = to,
				Cc = cc,
				Bcc = bcc,
				Konu = subject,
				Icerik = content,
				MailTipi = mailType,
				MailTipId = mailTypeId
			};
			return new MimeCreater().Create(mimeMesaj, attachments, hashAlg, config);
		}

		internal static EyMimeOlusturSonuc MimeYap(string from, EyMimeMessage mimeMesaj, List<Ek> attachments, OzetAlg hashAlg, EYazismaApiConfig config)
		{
			MimeCreater mimeCreater = new MimeCreater();
			mimeMesaj.Kimden = from;
			return mimeCreater.Create(mimeMesaj, attachments, hashAlg, config);
		}

		internal static EyMimeOlusturSonuc MimeYap(string from, EyMimeMessage mimeMesaj, List<string> attachments, OzetAlg hashAlg, EYazismaApiConfig config)
		{
			MimeCreater mimeCreater = new MimeCreater();
			mimeMesaj.Kimden = from;
			return mimeCreater.Create(mimeMesaj, attachments, hashAlg, config);
		}
	}
}
