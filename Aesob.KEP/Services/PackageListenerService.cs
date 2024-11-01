﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.ServiceModel;
using System.Text;
using System.Xml;
using Aesob.Docs.Data;
using Aesob.Docs.Services;
using Aesob.KEP.Services;
using Aesob.Web.Core.Public;
using Aesob.Web.Library;
using Aesob.Web.Library.Email;
using Aesob.Web.Library.Encyrption;
using Aesob.Web.Library.Service;
using Microsoft.IdentityModel.Tokens;
using Tr.Com.Eimza.EYazisma;

namespace KepStandalone
{
    public class PackageListenerService : IAesobService
    {
        private float _checkIntervalInSeconds = 10f;//Check every 10 seconds

        private IAesobService _thisAsInterface;

        private float _checkTimer;

        private HttpClient _httpClient;

        private EYazismaApi _eYazisma;

        private List<string> _targetEmails = new List<string>();

        public PackageListenerService()
        {
            _httpClient = new HttpClient();
            _thisAsInterface = this;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        async Task IAesobService.Start()
        {
            var configs = GetWebServiceConfigs();

            var account = _thisAsInterface.GetConfig("Hesap").Value;
            var id = _thisAsInterface.GetConfig("TC").Value;
            var password = _thisAsInterface.GetConfig("Parola").Value;
            var passCode = _thisAsInterface.GetConfig("Sifre").Value;
            var endpointAddress = _thisAsInterface.GetConfig("EndPoint").Value;

            _checkIntervalInSeconds = 60f;
            if(float.TryParse(_thisAsInterface.GetConfig("PackageCheckInterval").Value, out var checkInterval))
            {
                _checkIntervalInSeconds = checkInterval;
            }

            var targetEmailsConfig = _thisAsInterface.GetConfig("TargetEmails");
            if(targetEmailsConfig != null && !targetEmailsConfig.IsEmpty)
            {
                foreach (var targetEmailConfig in targetEmailsConfig.SubConfigs)
                {
                    _targetEmails.Add(targetEmailConfig.Value);
                }
            }

            Debug.Print("Paket Tarama Aralığı: " + new TimeSpan(0, 0, (int)_checkIntervalInSeconds));

            _eYazisma = new EYazismaApi(account, id, password, passCode, configs, endpointAddress);
            _checkTimer = _checkIntervalInSeconds;

            await LoginToDocsService();
		}

        async Task IAesobService.Update(float dt)
        {
            await PackageTick(dt);

            await Task.Delay(1);
        }

        async Task IAesobService.Stop()
        {
            _eYazisma = null;
            _checkTimer = 0f;

            await Task.Delay(1);
        }

        private async Task LoginToDocsService()
        {
			var aesobDocsLoginUsername = _thisAsInterface.GetConfig("AesobDocsLoginUsername").Value;
			var aesobDocsLoginPassword = _thisAsInterface.GetConfig("AesobDocsLoginPassword").Value;

			var loginResult = await UserService.TryLogin(aesobDocsLoginUsername, aesobDocsLoginPassword);
			if (loginResult?.IsSuccess == true)
			{
				Debug.Print($"Logged in as : {aesobDocsLoginUsername}");
				User.Current = loginResult.User;
			}
			else
			{
				Debug.Print($"Faile to login as: {aesobDocsLoginUsername}\n{loginResult?.ErrorMessage}");
			}
		}

        private IServiceData GetDailyEmailData(DateTime dateOfDay)
        {
            var dayString = dateOfDay.Day < 10 ? "0" + dateOfDay.Day.ToString() : dateOfDay.Day.ToString();
            var monthString = dateOfDay.Month < 10 ? "0" + dateOfDay.Month.ToString() : dateOfDay.Month.ToString();
            var yearString = dateOfDay.Year < 10 ? "0" + dateOfDay.Year.ToString() : dateOfDay.Year.ToString();

            var dataName = "RedirectedKepEmails_" + dayString + monthString + yearString;

            var data = _thisAsInterface.GetData(dataName);
            if (data == null)
            {
                data = _thisAsInterface.CreateAndRegisterServiceData(dataName, string.Empty);
            }

            return data;
        }

        private async Task PackageTick(float dt)
        {
            _checkTimer += dt;

            if (_checkTimer > _checkIntervalInSeconds)
            {
                await TryRemoveOldPackages();
                await TryRegisterNewPackages();
                _checkTimer = 0f;
            }
        }

        private async Task TryRemoveOldPackages()
        {
            try
            {
                await Task.Delay(10);

                var startDate = DateTime.Now.AddYears(-10);
                var endDate = DateTime.Now.AddMonths(-3);

				var foundPackagesData = _eYazisma.PaketSorgula(startDate, endDate);
				if (foundPackagesData != null && foundPackagesData.Durum.Length > 0 && foundPackagesData.Durum[0] == 0)
				{
					Debug.Print($"Found ({foundPackagesData.KepId.Length}) old packages to remove. ({startDate - endDate})");
					for (int i = 0; i < foundPackagesData.KepId.Length; i++)
					{
                        var kepId = foundPackagesData.KepId[i];
                        var kepSiraNo = foundPackagesData.KepSiraNo[i];

						Debug.Print($"Removing package with no: {kepSiraNo}");
						try
						{
							var removeResult = _eYazisma.PaketSil(kepId);
                            if(removeResult?.Durum == 0)
                            {
                                Debug.Print($"Package removed: {kepSiraNo}");
                            }
                            else
                            {
                                Debug.Print($"Failed to remove package: {kepSiraNo}. {removeResult?.HataAciklama}");
                            }
						}
                        catch (Exception e)
                        {
                            Debug.Print($"Failed to remove package: {kepSiraNo} {e.Message}");
                        }
					}
				}
			}
            catch (Exception e)
            {
                Debug.Print($"Failed to remove old packages: {e.Message}");
            }
        }

		private async Task TryRegisterNewPackages()
        {
            var packages = CheckForPackages();

            if (packages.Count > 0)
            {
                Debug.Print("Found " + packages.Count + " new packages");

                for (int i = 0; i < packages.Count; i++)
                {
                    try
                    {
                        Debug.Print($"Sending package emails: {packages[i].KepSıraNo}");
                        await MailPackageToReceivers(packages[i]);
                    }
                    catch (Exception e)
                    {
                        Debug.Print($"Failed to send package to receivers: {e.Message}");
                        continue;
                    }

                    try
                    {
                        Debug.Print($"Adding package to document system: {packages[i].KepSıraNo}");
						await AddPackageToEDYS(packages[i]);
					}
					catch (Exception e)
                    {
						Debug.Print($"Failed to add package to document system: {e.Message}");
					}
				}
            }
        }

        private List<PackageMailContent> CheckForPackages()
        {
            var today = DateTime.Now;
            var yesterday = today.AddDays(-1);

            var packages = new List<PackageMailContent>();

            //Debug.Print($"Paketler Taranıyor...");

            var foundPackagesData = _eYazisma.PaketSorgula(yesterday, today.AddDays(1));

            if (foundPackagesData == null)
            {
                Debug.Print("Error while getting packages: " + foundPackagesData?.HataAciklama[0] ?? "Package data is null");
            }
            else if (foundPackagesData.Durum[0] == 0)
            {
                var todaysData = GetDailyEmailData(today);
                var yesterdaysData = GetDailyEmailData(yesterday);

                for(int i = 0; i < foundPackagesData.KepId.Length; i++)
                {
                    var kepId = foundPackagesData.KepId[i];
                    var kepSiraNo = foundPackagesData.KepSiraNo[i].ToString();

					try
					{
						var todaysSentMailLookup = todaysData?.SubData.Select(s => s.Value);
						var yesterdaysSentMailLookup = yesterdaysData?.SubData.Select(s => s.Value);

						if (todaysSentMailLookup?.Contains(kepSiraNo) != true
							&& yesterdaysSentMailLookup?.Contains(kepSiraNo) != true)
						{
							var downloadResult = _eYazisma.PaketIndir(kepId, "", EYazismaPart.ALL);
							var package = GetPackagesFrom(downloadResult);
							package.ForEach(p => p.KepSıraNo = kepSiraNo);

							packages.AddRange(package);
						}
					}
					catch (Exception e)
					{
						Debug.Print($"{kepSiraNo} numaralı paketi ayıklarken hata:\n{e.Message}");
					}
				}
            }

            if(packages.Count == 0)
            {
                Debug.Print("Paket verisi alındı, yeni paket bulunmadı...");
            }

            return packages;
        }

        private List<PackageMailContent> GetPackagesFrom(EyPaketIndirSonuc downloadResult)
        {
            List<PackageMailContent> packages = new List<PackageMailContent>();

            if (downloadResult?.EyazismaPaketi != null)
            {
                foreach (var mailPckg in downloadResult.EyazismaPaketi)
                {
                    var base64 = Convert.ToBase64String(mailPckg.Value);
                    var smime = _eYazisma.SmimeParcala(base64);

                    foreach(var mimeAttachment in smime.Ekler)
                    {
                        var mimeValue = _eYazisma.SmimeParcala(Convert.ToBase64String(mimeAttachment.Degeri));

                        List<MailAttachment> attachments = new List<MailAttachment>();

                        foreach(var ek in mimeValue.Ekler)
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
                                        if(ustVeriEkler != null)
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
                                    catch(Exception ex)
                                    {
                                        Debug.Print($"Ek işlenirken bir hata oluştu: {ex}");
                                    }
                                }

                                if(ustYaziStream != null)
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

                        if(mimeValue != null && mimeValue.Durum == 0)
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

                            packages.Add(mailContent);
                        }
                    }
                }
            }

            return packages;
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

        private async Task AddPackageToEDYS(PackageMailContent packageData)
        {
            Debug.Print("Adding package to EDYS");

            if(User.Current == null || User.Current.Id < 0)
            {
                Debug.Print($"User was invalid, trying to login again.");

                for(int i = 0; i < 5; i++)
                {
                    Debug.Print($"Login attempt: {i + 1}");
                    await LoginToDocsService();
                    if(User.Current?.Id >= 0)
                    {
                        break;
                    }
				}

                if(User.Current == null || User.Current.Id < 0)
                {
					Debug.Print($"Login attempts failed.");
				}
			}

            var sourcesResult = await DocumentService.GetDocumentSources();
            if(sourcesResult?.IsSuccessful != true)
            {
                Debug.Print($"Failed to retrieve sources from docs: {sourcesResult.ErrorMessage}");
                return;
            }

            var source = sourcesResult.Value?.FirstOrDefault(s => s.Id == 114);
            if(source == null)
            {
                Debug.Print($"Failed to retrieve KEP source");
                return;
            }

            var document = new Document()
            {
                Name = packageData.Subject,
                Description = packageData.Content,
                IsOutgoing = false,
                UploadDate = DateTime.Now,
                LastRevisionDate = DateTime.Now,
                Source = source,
                SourceId = source.Id,
                OwnerUserId = 10
            };

            var files = new List<DocumentFile>();
            foreach(var attachment in packageData.Attachments)
            {
                var aesobDocumentFile = new DocumentFile()
                {
                    Name = $"Kep İletisi Eki: {packageData.KepSıraNo} - {attachment.Name}",
                    Description = $"{packageData.KepSıraNo} no'lu kep iletisi eki",
                    FileContent = attachment.Value,
                    FileSize = attachment.Value.Length,
                    FileName = attachment.Name,
                    VisibleFileName = attachment.Name,
                    Document = document,
                    DocumentId = -1,
                };

                files.Add(aesobDocumentFile);
            }

            document.DocumentFiles = files.ToArray();

            var uploadResult = await DocumentService.UploadDocument(document);
            if (uploadResult?.IsSuccessful == true)
            {
                Debug.Print($"Document successfully added to document server. ({document.Name})");
            }
            else
            {
                Debug.Print($"Failed to add document to document server: {uploadResult?.ErrorMessage}");
            }
        }

        private async Task MailPackageToReceivers(PackageMailContent downloadedPackage)
        {
#if DEBUG
            if(downloadedPackage != null)
            {
                Debug.Print("Skipping mail redirection in debug mode...");
                return;
            }
#endif
            string mailXML = GetXMLStringFromMailData("AesobEmailEncryptionProtocol123456789",
                "Kep Yönlendirme",
                downloadedPackage,
                _targetEmails.ToArray());

            var encodedString = EncryptionHelper.EncryptText(mailXML);

            var jsonContent = JsonContent.Create(encodedString, new MediaTypeHeaderValue("application/json"));

#if DEBUG
            var postResult = await _httpClient.PostAsync("https://localhost:44397/api/Email/Redirect", jsonContent);
#else
            var postResult = await _httpClient.PostAsync("https://aesob.org.tr/api/Email/Redirect", jsonContent);
#endif

            if(postResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var todaysData = GetDailyEmailData(DateTime.Now);
                todaysData.AddSubData(_thisAsInterface.CreateServiceData("RedirectedKepMail", downloadedPackage.KepSıraNo));
                _thisAsInterface.SaveData();
            }

            var resultStream = postResult.Content.ReadAsStringAsync().Result;
            Debug.Print("EMail Yönlendirme Sonucu: " + resultStream);
        }

        private string GetXMLStringFromMailData(string authKeyword, string senderAlias, PackageMailContent mailContent, string[] targetAddresses)
        {
            XmlDocument document = new XmlDocument();

            var subject = mailContent.Subject;
            var content = mailContent.Content;
            var attachments = mailContent.Attachments;
            var from = mailContent.From;
            var to = mailContent.To;
            var cc = mailContent.Cc;
            var bcc = mailContent.Bcc;
            var mailId = mailContent.KepSıraNo;

            var actualContent = CreateContentFrom(mailId, from, to, cc, bcc, content);

            var rootNode = document.AppendChild(document.CreateElement("EmailData"));

            var authNode = rootNode.AppendChild(document.CreateElement("Auth"));
            authNode.InnerText = authKeyword;

            var targetEmailsNode = rootNode.AppendChild(document.CreateElement("TargetEmails"));
            foreach(var targetAddr in targetAddresses)
            {
                var targetEmailNode = targetEmailsNode.AppendChild(document.CreateElement("TargetEmail"));
                targetEmailNode.InnerText = targetAddr;
            }

            var subjectNode = rootNode.AppendChild(document.CreateElement("Subject"));
            subjectNode.InnerText = subject;

            var senderAliasNode = rootNode.AppendChild(document.CreateElement("SenderAlias"));
            senderAliasNode.InnerText = senderAlias;

            var attachmentsNode = rootNode.AppendChild(document.CreateElement("Attachments"));
            foreach(var attachment in attachments)
            {
                var attachementNode = attachmentsNode.AppendChild(document.CreateElement("Attachment")) as XmlElement;
                attachementNode.SetAttribute("Name", attachment.Name);
                attachementNode.InnerText = Base64UrlEncoder.Encode(attachment.Value);
            }

            var contentNode = rootNode.AppendChild(document.CreateElement("Content"));
            contentNode.InnerText = actualContent;

            return document.InnerXml;
        }

        private string CreateContentFrom(string mailId, string from, List<string> to, List<string> cc, List<string> bcc, string originalContent)
        {
            StringBuilder sb = new StringBuilder();

            const string newLineText = "{aesob_newline}";

            sb.AppendLine(originalContent);

            sb.AppendLine(newLineText);
            sb.AppendLine(newLineText);

            sb.AppendLine("Kimden: " + from);
            sb.AppendLine(newLineText);
            sb.AppendLine(newLineText);

            if(to.Count > 0)
            {
                sb.AppendLine("Kime: ");
                sb.Append(GetItemizedList(to, newLineText));

                sb.AppendLine(newLineText);
                sb.AppendLine(newLineText);
            }

            if (cc.Count > 0)
            {
                sb.AppendLine("CC: ");
                sb.Append(GetItemizedList(cc, newLineText));

                sb.AppendLine(newLineText);
                sb.AppendLine(newLineText);
            }

            if (bcc.Count > 0)
            {
                sb.AppendLine("Bcc: ");
                sb.Append(GetItemizedList(bcc, newLineText));

                sb.AppendLine(newLineText);
                sb.AppendLine(newLineText);
            }


            sb.AppendLine("Kep Sıra No: ");
            sb.Append(mailId);

            sb.AppendLine(newLineText);
            sb.AppendLine(newLineText);

            return sb.ToString();
        }

        private string GetItemizedList(List<string> strings, string newLineText)
        {
            if(strings == null || strings.Count == 0)
            {
                return string.Empty;
            }

            if(strings.Count == 1)
            {
                return strings[0];
            }

            StringBuilder sb = new StringBuilder();

            for(int i = 0; i < strings.Count; i++)
            {
                sb.Append(newLineText);
                sb.Append(" - ");
                sb.Append(strings[i]);
            }

            return sb.ToString();
        }

        private BasicHttpBinding GetWebServiceConfigs()
        {
            BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            basicHttpBinding.Name = "eyServisSOAPBinding";
            basicHttpBinding.CloseTimeout = new TimeSpan(0, 10, 0);
            basicHttpBinding.OpenTimeout = new TimeSpan(0, 10, 0);
            basicHttpBinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            basicHttpBinding.SendTimeout = new TimeSpan(0, 10, 0);
            basicHttpBinding.AllowCookies = false;
            basicHttpBinding.BypassProxyOnLocal = true;
            basicHttpBinding.MaxBufferPoolSize = 33554432;
            basicHttpBinding.MaxReceivedMessageSize = 33554432;
            basicHttpBinding.MaxBufferSize = 33554432;
            basicHttpBinding.UseDefaultWebProxy = true;
            basicHttpBinding.MessageEncoding = WSMessageEncoding.Mtom;

            XmlDictionaryReaderQuotas readerQuotas = new XmlDictionaryReaderQuotas();
            readerQuotas.MaxDepth = 32;
            readerQuotas.MaxStringContentLength = 41943040;
            readerQuotas.MaxArrayLength = 16384;
            readerQuotas.MaxBytesPerRead = 4096;
            readerQuotas.MaxNameTableCharCount = 16384;

            basicHttpBinding.ReaderQuotas = readerQuotas;
            basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;

            return basicHttpBinding;
        }
    }
}
