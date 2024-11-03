using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.ServiceModel;
using System.Text;
using System.Xml;
using Aesob.Docs.Data;
using Aesob.Docs.Services;
using Aesob.KEP.Library;
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

        private string _docsUsername;
        private string _docsPassword;

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

			_docsUsername = _thisAsInterface.GetConfig("AesobDocsLoginUsername").Value;
			_docsPassword = _thisAsInterface.GetConfig("AesobDocsLoginPassword").Value;

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


            await DocumentUtilities.LoginToDocsService(_docsUsername, _docsPassword);
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
            await CheckAndRegisterPackagesAux(true);
            await CheckAndRegisterPackagesAux(false);
        }

        private async Task CheckAndRegisterPackagesAux(bool isInbox)
        {
            var foundPackages = CheckForPackages(isInbox);

            if (foundPackages.Count > 0)
            {
                Debug.Print("Found " + foundPackages.Count + " new incoming packages");

                for (int i = 0; i < foundPackages.Count; i++)
                {
                    try
                    {
                        Debug.Print($"Sending package emails: {foundPackages[i].KepSıraNo}");
                        await MailPackageToReceivers(foundPackages[i], isInbox);
                    }
                    catch (Exception e)
                    {
                        Debug.Print($"Failed to send package to receivers: {e.Message}");
                        continue;
                    }

                    try
                    {
                        Debug.Print($"Adding package to document system: {foundPackages[i].KepSıraNo}");

                        await DocumentUtilities.AddPackageToEDYS(foundPackages[i], _docsUsername, _docsPassword);
                    }
                    catch (Exception e)
                    {
                        Debug.Print($"Failed to add package to document system: {e.Message}");
                    }

					var todaysData = GetDailyEmailData(DateTime.Now);

					var dataName = isInbox ? "RedirectedKepMail" : "Out_RedirectedKepMail";
					todaysData.AddSubData(_thisAsInterface.CreateServiceData(dataName, foundPackages[i].KepSıraNo));
					_thisAsInterface.SaveData();
				}
            }
        }

        private List<PackageMailContent> CheckForPackages(bool checkInbox)
        {
            var today = DateTime.Now;
            var yesterday = today.AddDays(-1);

            var mailContent = new List<PackageMailContent>();

            //Debug.Print($"Paketler Taranıyor...");

			var foundPackagesData = _eYazisma.PaketSorgula(yesterday, today.AddDays(1), checkInbox ? "INBOX" : "OUTBOX");

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
						var todaysSentMailLookup = checkInbox ? todaysData?.SubData.Where(s => s.Name == "RedirectedKepMail") : todaysData?.SubData.Where(s => s.Name == "Out_RedirectedKepMail");
						var yesterdaysSentMailLookup = checkInbox ? yesterdaysData?.SubData.Where(s => s.Name == "RedirectedKepMail") : yesterdaysData?.SubData.Where(s => s.Name == "Out_RedirectedKepMail");

                        if (todaysSentMailLookup?.Any(m => m.Value == kepSiraNo) != true
							&& yesterdaysSentMailLookup?.Any(m => m.Value == kepSiraNo) != true)
						{
							var downloadResult = _eYazisma.PaketIndir(kepId, "", EYazismaPart.ALL);
                            var packages = PackageUtilities.CreatePackageFor(downloadResult);
                            var package = PackageUtilities.GetMailContentFor(_eYazisma, packages);

							package.ForEach(p => p.KepSıraNo = kepSiraNo);

							mailContent.AddRange(package);
						}
					}
					catch (Exception e)
					{
						Debug.Print($"{kepSiraNo} numaralı paketi ayıklarken hata:\n{e.Message}");
					}
				}
            }

            if(mailContent.Count == 0)
            {
                Debug.Print("Paket verisi alındı, yeni paket bulunmadı...");
            }

            return mailContent;
        }

        private async Task MailPackageToReceivers(PackageMailContent downloadedPackage, bool isInbox)
        {
#if DEBUG
            if(downloadedPackage != null)
            {
                Debug.Print("Skipping mail redirection in debug mode...");

                var todaysData = GetDailyEmailData(DateTime.Now);
                var dataName = isInbox ? "RedirectedKepMail" : "Out_RedirectedKepMail";
                todaysData.AddSubData(_thisAsInterface.CreateServiceData(dataName, downloadedPackage.KepSıraNo));
                _thisAsInterface.SaveData();
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
            const int sizeMultiplier = 2;

            BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            basicHttpBinding.Name = "eyServisSOAPBinding";
            basicHttpBinding.CloseTimeout = new TimeSpan(0, 10, 0);
            basicHttpBinding.OpenTimeout = new TimeSpan(0, 10, 0);
            basicHttpBinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            basicHttpBinding.SendTimeout = new TimeSpan(0, 10, 0);
            basicHttpBinding.AllowCookies = false;
            basicHttpBinding.BypassProxyOnLocal = true;
            basicHttpBinding.MaxBufferPoolSize = 33554432 * sizeMultiplier;
            basicHttpBinding.MaxReceivedMessageSize = 33554432 * sizeMultiplier;
            basicHttpBinding.MaxBufferSize = 33554432 * sizeMultiplier;
            basicHttpBinding.UseDefaultWebProxy = true;
            basicHttpBinding.MessageEncoding = WSMessageEncoding.Mtom;

            XmlDictionaryReaderQuotas readerQuotas = new XmlDictionaryReaderQuotas();
            readerQuotas.MaxDepth = 32;
            readerQuotas.MaxStringContentLength = 41943040 * sizeMultiplier;
            readerQuotas.MaxArrayLength = 16384 * sizeMultiplier;
            readerQuotas.MaxBytesPerRead = 4096 * sizeMultiplier;
            readerQuotas.MaxNameTableCharCount = 16384 * sizeMultiplier;

            basicHttpBinding.ReaderQuotas = readerQuotas;
            basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;

            return basicHttpBinding;
        }
    }
}
