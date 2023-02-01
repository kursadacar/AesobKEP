using System;
using System.Reflection;
using ActiveUp.Net.Mail;
using OpenPop.Mime;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma
{
	internal static class EYazismaSmimeParser
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		internal static EySmimeParcalaSonuc SmimeParcala(byte[] SmimeMail)
		{
			EySmimeParcalaSonuc eySmimeParcalaSonuc = new EySmimeParcalaSonuc();
			ActiveUp.Net.Mail.Message message = null;
			try
			{
				message = Parser.ParseMessage(SmimeMail);
			}
			catch (Exception exception)
			{
				LOG.Error("S/Mime Mail Parçalanırken Hata Meydana Geldi.", exception);
				eySmimeParcalaSonuc.Durum = 161;
				eySmimeParcalaSonuc.HataAciklama = "S/Mime Mail Parçalanırken Hata Meydana Geldi.";
				return eySmimeParcalaSonuc;
			}
			for (int i = 0; i < message.Bcc.Count; i++)
			{
				eySmimeParcalaSonuc.Bcc.Add(message.Bcc[i].Email);
			}
			for (int j = 0; j < message.Cc.Count; j++)
			{
				eySmimeParcalaSonuc.Cc.Add(message.Cc[j].Email);
			}
			for (int k = 0; k < message.Attachments.Count; k++)
			{
				if (message.Attachments[k].Filename.Equals("smime.p7s"))
				{
					eySmimeParcalaSonuc.ImzaP7s = new Ek(message.Attachments[k].Filename, message.Attachments[k].BinaryContent);
				}
				else
				{
					eySmimeParcalaSonuc.Ekler.Add(new Ek(message.Attachments[k].Filename, message.Attachments[k].BinaryContent));
				}
			}
			if (message.IsHtml)
			{
				eySmimeParcalaSonuc.Icerik = message.BodyHtml.Text;
			}
			else
			{
				eySmimeParcalaSonuc.Icerik = message.BodyText.Text;
			}
			eySmimeParcalaSonuc.Kimden = message.From.Email;
			for (int l = 0; l < message.To.Count; l++)
			{
				eySmimeParcalaSonuc.Kime.Add(message.To[l].Email);
			}
			eySmimeParcalaSonuc.Konu = message.Subject;
			string[] values = message.HeaderFields.GetValues("X-TR-REM-iletiTip".ToLower());
			if (values != null && values.Length != 0)
			{
				switch (values[0])
				{
				case "eYazisma":
					eySmimeParcalaSonuc.MailTipi = EYazismaPaketTur.EYazisma;
					break;
				case "eTebligat":
					eySmimeParcalaSonuc.MailTipi = EYazismaPaketTur.ETEbligat;
					break;
				case "standart":
					eySmimeParcalaSonuc.MailTipi = EYazismaPaketTur.Standart;
					break;
				default:
					eySmimeParcalaSonuc.MailTipi = EYazismaPaketTur.Standart;
					break;
				}
			}
			values = message.HeaderFields.GetValues("X-TR-REM-iletiID".ToLower());
			if (values != null && values.Length != 0)
			{
				eySmimeParcalaSonuc.MailTipId = values[0];
			}
			eySmimeParcalaSonuc.Durum = 0;
			eySmimeParcalaSonuc.HataAciklama = "S/Mime Parçalama Başarılı";
			return eySmimeParcalaSonuc;
		}

		internal static EySmimeParcalaSonuc SmimeParcala(string SmimeMail)
		{
			EySmimeParcalaSonuc eySmimeParcalaSonuc = new EySmimeParcalaSonuc();
			OpenPop.Mime.Message message = null;
			try
			{
				message = new OpenPop.Mime.Message(Convert.FromBase64String(SmimeMail));
			}
			catch (Exception exception)
			{
				LOG.Error("S/Mime Mail Parçalanırken Hata Meydana Geldi.", exception);
				eySmimeParcalaSonuc.Durum = 161;
				eySmimeParcalaSonuc.HataAciklama = "S/Mime Mail Parçalanırken Hata Meydana Geldi.";
				return eySmimeParcalaSonuc;
			}
			for (int i = 0; i < message.Headers.Bcc.Count; i++)
			{
				eySmimeParcalaSonuc.Bcc.Add(message.Headers.Bcc[i].MailAddress.Address);
			}
			for (int j = 0; j < message.Headers.Cc.Count; j++)
			{
				eySmimeParcalaSonuc.Bcc.Add(message.Headers.Cc[j].MailAddress.Address);
			}
			foreach (MessagePart item in message.FindAllAttachments())
			{
				if (item.FileName.Equals("smime.p7s"))
				{
					eySmimeParcalaSonuc.ImzaP7s = new Ek(item.FileName, item.Body);
				}
				else
				{
					eySmimeParcalaSonuc.Ekler.Add(new Ek(item.FileName, item.Body));
				}
			}
			if (message.FindFirstHtmlVersion() != null)
			{
				eySmimeParcalaSonuc.Icerik = message.FindFirstHtmlVersion().GetBodyAsText();
			}
			else
			{
				eySmimeParcalaSonuc.Icerik = message.FindFirstPlainTextVersion().GetBodyAsText();
			}
			eySmimeParcalaSonuc.Kimden = message.Headers.From.MailAddress.Address;
			for (int k = 0; k < message.Headers.To.Count; k++)
			{
				eySmimeParcalaSonuc.Kime.Add(message.Headers.To[k].MailAddress.Address);
			}
			eySmimeParcalaSonuc.Konu = message.Headers.Subject;
			string[] values = message.Headers.UnknownHeaders.GetValues("X-TR-REM-iletiTip".ToLower());
			if (values != null && values.Length != 0)
			{
				switch (values[0])
				{
				case "eYazisma":
					eySmimeParcalaSonuc.MailTipi = EYazismaPaketTur.EYazisma;
					break;
				case "eTebligat":
					eySmimeParcalaSonuc.MailTipi = EYazismaPaketTur.ETEbligat;
					break;
				case "standart":
					eySmimeParcalaSonuc.MailTipi = EYazismaPaketTur.Standart;
					break;
				default:
					eySmimeParcalaSonuc.MailTipi = EYazismaPaketTur.Standart;
					break;
				}
			}
			values = message.Headers.UnknownHeaders.GetValues("X-TR-REM-iletiID".ToLower());
			if (values != null && values.Length != 0)
			{
				eySmimeParcalaSonuc.MailTipId = values[0];
			}
			eySmimeParcalaSonuc.Durum = 0;
			eySmimeParcalaSonuc.HataAciklama = "S/Mime Parçalama Başarılı";
			return eySmimeParcalaSonuc;
		}
	}
}
