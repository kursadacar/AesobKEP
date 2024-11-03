using Aesob.Web.Library;
using Aesob.Web.Library.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tr.Com.Eimza.EYazisma;

namespace Aesob.KEP.Library
{
	public static class PackageUtilities
	{
		public static KepPackage[] CreatePackageFor(EyPaketIndirSonuc downloadResult)
		{
			if (downloadResult?.EyazismaPaketi == null)
			{
				return null;
			}

			KepPackage[] packageContent = new KepPackage[downloadResult.EyazismaPaketi.Length];
			for (int i = 0; i < packageContent.Length; i++)
			{
				var downloadedData = downloadResult.EyazismaPaketi[i];
				KepPackage package = new KepPackage(downloadedData.contentType, downloadedData.fileName, downloadedData.Value);
				packageContent[i] = package;
			}
			return packageContent;
		}

		public static List<PackageMailContent> GetMailContentFor(EYazismaApi eYazisma, KepPackage[] packages)
		{
			List<PackageMailContent> result = new List<PackageMailContent>();

			foreach (var package in packages)
			{
				var base64 = Convert.ToBase64String(package.Value);
				var smime = eYazisma.SmimeParcala(base64);

				foreach (var mimeAttachment in smime.Ekler)
				{
					var mimeValue = eYazisma.SmimeParcala(Convert.ToBase64String(mimeAttachment.Degeri));

					List<MailAttachment> attachments = new List<MailAttachment>();

					foreach (var ek in mimeValue.Ekler)
					{
						if (ek.Adi.EndsWith(".eyp"))
						{
							var memoryStream = new MemoryStream(ek.Degeri);

							List<Ek> ekler = new List<Ek>();
							Stream ustYaziStream = null;
							string dosyaAdi = ek.Adi;

							try
							{
								var paket = Cbddo.eYazisma.Tipler.Paket.Ac(memoryStream, Cbddo.eYazisma.Tipler.PaketModu.Ac);

								var ustVeriEkler = paket.Ustveri?.EkleriAl();
								if (ustVeriEkler != null)
								{
									foreach (var ustVeriEk in ustVeriEkler)
									{
										var ekPaket = paket.EkAl(new Guid(ustVeriEk.Id.Value));
										var ekPaketMs = new MemoryStream();
										ekPaket.CopyTo(ekPaketMs);
										var ekPaketBuffer = ekPaketMs.ToArray();
										ekPaketMs.Dispose();

										ekler.Add(new Ek(ustVeriEk.DosyaAdi, ekPaketBuffer));
									}
								}

								ustYaziStream = paket.UstYaziAl();
								dosyaAdi = paket.Ustveri.DosyaAdiAl();
							}
							catch
							{
								try
								{
									var paket = Dpt.eYazisma.Tipler.Paket.Ac(memoryStream, Dpt.eYazisma.Tipler.PaketModu.Ac);

									var ustVeriEkler = paket.Ustveri?.EkleriAl();
									if (ustVeriEkler != null)
									{
										foreach (var ustVeriEk in ustVeriEkler)
										{
											var ekPaket = paket.EkAl(new Guid(ustVeriEk.Id.Value));

											var ekPaketMs = new MemoryStream();
											ekPaket.CopyTo(ekPaketMs);
											var ekPaketBuffer = ekPaketMs.ToArray();
											ekPaketMs.Dispose();

											ekler.Add(new Ek(ustVeriEk.DosyaAdi, ekPaketBuffer));
										}
									}

									ustYaziStream = paket.UstYaziAl();
									dosyaAdi = paket.Ustveri.DosyaAdiAl();
								}
								catch (Exception ex)
								{
									Debug.Print($"Ek işlenirken bir hata oluştu: {ex}");
								}
							}

							if (ustYaziStream != null)
							{
								MemoryStream ms = new MemoryStream();
								ustYaziStream.CopyTo(ms);
								var bytes = ms.ToArray();

								var ustVeriAttachments = CreateAttachmentFromMultipleEks(ekler);
								var attachment = CreateAttachmentFromEk(new Ek(dosyaAdi, bytes));

								attachments.Add(attachment);
								attachments.AddRange(ustVeriAttachments);
							}
						}
						else
						{
							attachments.Add(CreateAttachmentFromEk(ek));
						}
					}

					if (mimeValue != null && mimeValue.Durum == 0)
					{
						var mailContent = new PackageMailContent()
						{
							Cc = mimeValue.Cc,
							Bcc = mimeValue.Bcc,
							Attachments = attachments,
							Content = string.IsNullOrEmpty(mimeValue.Icerik) ? smime.Icerik : mimeValue.Icerik,
							From = mimeValue.Kimden,
							To = mimeValue.Kime,
							ImzaP7s = CreateAttachmentFromEk(mimeValue.ImzaP7s),
							MailType = mimeValue.MailTipi,
							MailTypeId = mimeValue.MailTipId,
							Subject = mimeValue.Konu
						};

						result.Add(mailContent);
					}
				}
			}

			return result;
		}

		private static MailAttachment CreateAttachmentFromEk(Ek ek)
		{
			return new MailAttachment(ek.Adi, ek.Degeri);
		}

		private static List<MailAttachment> CreateAttachmentFromMultipleEks(List<Ek> eks)
		{
			var result = new List<MailAttachment>();

			foreach (var ek in eks)
			{
				result.Add(CreateAttachmentFromEk(ek));
			}

			return result;
		}
	}
}
