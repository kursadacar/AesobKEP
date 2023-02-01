using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.EYazisma.Smime
{
	internal static class MailMessageExtentions
	{
		private static readonly ILog LOG = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static void ToEml(this MailMessage Message, string FileName)
		{
			try
			{
				SmtpClient smtpClient = new SmtpClient();
				using (TemporaryDirectory temporaryDirectory = new TemporaryDirectory())
				{
					smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
					smtpClient.PickupDirectoryLocation = temporaryDirectory.DirectoryPath;
					smtpClient.Send(Message);
					string text = Directory.GetFiles(smtpClient.PickupDirectoryLocation).FirstOrDefault();
					if (text != null)
					{
						File.Copy(text, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName));
					}
				}
			}
			catch (Exception exception)
			{
				LOG.Error("Mime Mesajı Dosya Olarak Kaydedilirken Hata Meydana Geldi.", exception);
			}
		}

		public static byte[] GetBytesOfMessage(this MailMessage Message)
		{
			try
			{
				SmtpClient smtpClient = new SmtpClient();
				using (TemporaryDirectory temporaryDirectory = new TemporaryDirectory())
				{
					smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
					smtpClient.PickupDirectoryLocation = temporaryDirectory.DirectoryPath;
					smtpClient.Send(Message);
					string text = Directory.GetFiles(smtpClient.PickupDirectoryLocation).FirstOrDefault();
					if (text != null)
					{
						return File.ReadAllBytes(text);
					}
					LOG.Error("MailMessage Verisi Okunamadı.");
					return null;
				}
			}
			catch (Exception exception)
			{
				LOG.Error("MailMessage Okunurken Hata Meydana Geldi.", exception);
				return null;
			}
		}
	}
}
