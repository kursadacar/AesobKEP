using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.ServiceModel;
using System.Text;
using System.Xml;
using Aesob.KEP.Services;
using Aesob.Web.Core.Public;
using Aesob.Web.Library;
using Aesob.Web.Library.Service;
using Microsoft.IdentityModel.Tokens;
using Tr.Com.Eimza.EYazisma;

namespace KepStandalone
{
    public class PackageListenerService : IAesobService
    {
        private const float _loginTrialInterval = 5f;
        private float _checkIntervalInSeconds = 10f;//Check every 10 seconds

        private IAesobService _thisAsInterface;

        private bool _isLoggedIn;
        private float _checkTimer;

        private HttpClient _httpClient;
        private EYazismaApi _eYazisma;
        private DateTime _lastCheckedDate;

        private List<string> _targetEmails = new List<string>();

        public PackageListenerService()
        {
            _httpClient = new HttpClient();
            _thisAsInterface = this;
            _lastCheckedDate = DateTime.Now;
        }

        void IAesobService.Start()
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

            _isLoggedIn = true;
            //var loginTask = TryLogin();

            _checkTimer = 0;
        }

        void IAesobService.Update(float dt)
        {
            if (_isLoggedIn)
            {
                PackageTick(dt);
            }
        }

        void IAesobService.Stop()
        {
            _eYazisma = null;
            _checkTimer = 0f;
        }

        private void PackageTick(float dt)
        {
            _checkTimer += dt;

            if (_checkTimer > _checkIntervalInSeconds)
            {
                TryRegisterNewPackages();
                _checkTimer = 0f;
            }
        }

        private async Task TryLogin()
        {
            var loginTrialInterval = (int)(_loginTrialInterval * 1000);
            while (!_isLoggedIn)
            {
                string loginInfo = $"Giriş Deneniyor...";
                Debug.Print(loginInfo);

                var regularLoginResult = _eYazisma.Giris(EYazismaGirisTur.OTP);

                bool isSuccess = regularLoginResult?.Durum == "0";
                string status = isSuccess ? "Başarılı" : "Başarısız";
                Debug.Print($"Giriş Sonuç: {status}. Mesaj: {regularLoginResult?.HataAciklama}");

                if (!isSuccess)
                {
                    await Task.Delay(loginTrialInterval);
                    continue;
                }

                Debug.Print($"Lütfen telefonunuza gelen SMS şifresini giriniz...");
                var smsKey = Console.ReadLine();

                var secureLoginResult = _eYazisma.GuvenliGiris(regularLoginResult.GuvenlikId, smsKey);

                isSuccess = secureLoginResult?.Durum == "0";
                status = isSuccess ? "Başarılı" : "Başarısız";
                Debug.Print($"Güvenli Giriş Sonuç: {status}. Mesaj: {secureLoginResult?.HataAciklama}");

                if (!isSuccess)
                {
                    await Task.Delay(loginTrialInterval);
                    continue;
                }

                Debug.Print("Giriş başarılı!");

                _isLoggedIn = true;
            }
        }

        private void TryRegisterNewPackages()
        {
            var packages = CheckForPackages();

            if (packages.Count > 0)
            {
                Debug.Print("Found " + packages.Count + " new packages");

                for (int i = 0; i < packages.Count; i++)
                {
                    MailPackageToReceivers(packages[i]);
                    AddPackageToEDYS(packages[i]);
                }
            }
        }

        private List<PackageMailContent> CheckForPackages()
        {
            var curDate = DateTime.Now;

            var packages = new List<PackageMailContent>();

            Debug.Print($"Paketler Taranıyor: {_lastCheckedDate:dd/MM/yyyy HH:mm:ss}  -  {curDate:dd/MM/yyyy HH:mm:ss}");

            var foundPackagesData = _eYazisma.PaketSorgula(_lastCheckedDate, curDate);
            _lastCheckedDate = curDate;

            if (foundPackagesData == null || foundPackagesData.Durum[0] != 0)
            {
                if (foundPackagesData?.Durum[0] == -1)
                {
                    Debug.Print("Paket verisi alındı, yeni paket bulunmadı...");
                }
                else
                {
                    Debug.Print("Error while getting packages: " + foundPackagesData?.HataAciklama[0] ?? "Package data is null");
                }
            }
            else
            {
                foreach(var kepSiraNo in foundPackagesData.KepSiraNo)
                {
                    var siraNo = kepSiraNo ?? -1;

                    var downloadResult = _eYazisma.PaketIndir(siraNo, "", EYazismaPart.ALL);

                    packages = GetPackagesFrom(downloadResult);
                }
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

                        if(mimeValue != null && mimeValue.Durum == 0)
                        {
                            var mailContent = new PackageMailContent()
                            {
                                Cc = mimeValue.Cc,
                                Bcc = mimeValue.Bcc,
                                Attachments = MailAttachment.FromMultipleEk(mimeValue.Ekler),
                                Content = mimeValue.Icerik,
                                From = mimeValue.Kimden,
                                To = mimeValue.Kime,
                                ImzaP7s = MailAttachment.FromEk(mimeValue.ImzaP7s),
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

        private void AddPackageToEDYS(PackageMailContent packageData)
        {
            Debug.Print("Adding package to EDYS");

            //TODO_Kursad: Add packages
        }

        private void MailPackageToReceivers(PackageMailContent downloadedPackage)
        {
            string mailXML = GetXMLStringFromMailData("AesobEmailEncryiptionProtocol123456789",
                "Kep Yönlendirme",
                downloadedPackage.Subject,
                downloadedPackage.Content,
                downloadedPackage.Attachments,
                downloadedPackage.From,
                downloadedPackage.To,
                downloadedPackage.Cc,
                downloadedPackage.Bcc,
                _targetEmails.ToArray());

            var encodedString = Base64UrlEncoder.Encode(mailXML);

            var jsonContent = JsonContent.Create(encodedString, new MediaTypeHeaderValue("application/json"));

            var postTask = _httpClient.PostAsync("https://aesob.org.tr/api/Email/Redirect", jsonContent);
            //var postTask = _httpClient.PostAsync("https://localhost:44397/api/Email/Redirect", jsonContent);
            postTask.Wait();
            var postResult = postTask.Result;

            var resultStream = postResult.Content.ReadAsStringAsync().Result;
            Debug.Print("EMail Yönlendirme Sonucu: " + resultStream);
        }

        //REMARK_Kursad: This is for debug purposes only
        private EyPaketSonuc CreateTestResult()
        {
            return new EyPaketSonuc()
            {
                Durum = new int?[] { 0 },
                From = new string[] { "KEP Uygulama TEST" },
                FromKep = new string[] { "KEP Uygulama TEST" },
                HataAciklama = new string[] { "KEP Uygulama Test Başarılı" },
                KepId = new string[] { "KEP TEST ID" },
                KepSiraNo = new int?[] { 0 },
                Konu = new string[] { "KEP TEST MESAJ KONU" },
                OrjinalMesajId = new string[] { "KEP TEST MESAJ ID" },
                Tur = new string[] { "KEP TEST MESAJ TUR" }
            };
        }

        private EyPaketIndirSonuc CreateTestPackage()
        {
            var api = _eYazisma;

            EyPaketIndirSonuc indirSonuc = api.PaketIndir("<64964.1085840022.37.1639395387226.e8866ba0-5c08-11ec-aae7-e1b7dd8706e4.pttkepmail@hs01.kep.tr>", null,  EYazismaPart.ALL);

            if(indirSonuc != null && indirSonuc.EyazismaPaketi.Length > 0)
            {
                return indirSonuc;
            }

            return null;
        }

        private string GetXMLStringFromMailData(string authKeyword, string senderAlias, string subject, string content, List<MailAttachment> attachments, string from, List<string> to, List<string> cc, List<string> bcc, string[] targetAddresses)
        {
            XmlDocument document = new XmlDocument();

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
            contentNode.InnerText = CreateContentFrom(from, to, cc, bcc, content);

            return document.InnerXml;
        }

        private string CreateContentFrom(string from, List<string> to, List<string> cc, List<string> bcc, string originalContent)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(originalContent);

            sb.AppendLine();
            sb.AppendLine();

            sb.AppendLine("Kimden: " + from);
            sb.AppendLine();

            if(to.Count > 0)
            {
                sb.AppendLine("Kime: ");

                foreach(var _to in to)
                {
                    sb.Append(_to);
                    sb.Append(';');
                }

                sb.Remove(sb.Length - 1, 1);
            }

            sb.AppendLine();

            if (cc.Count > 0)
            {
                sb.AppendLine("CC: ");

                foreach (var c in cc)
                {
                    sb.Append(c);
                    sb.Append(';');
                }

                sb.Remove(sb.Length - 1, 1);
            }
            sb.AppendLine();

            if (bcc.Count > 0)
            {
                sb.AppendLine("Bcc: ");

                foreach (var _bcc in bcc)
                {
                    sb.Append(_bcc);
                    sb.Append(';');
                }

                sb.Remove(sb.Length - 1, 1);
            }

            sb.AppendLine();
            sb.AppendLine();

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
