using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Xml;
using Aesob.KEP.Model;
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
        private const float _checkInterval = 10f;//Check every 10 seconds

        private IAesobService _thisAsInterface;

        private bool _isLoggedIn;
        private float _checkTimer;

        private HttpClient _httpClient;
        private EYazismaApi _eYazisma;

        public PackageListenerService()
        {
            _httpClient = new HttpClient();
            _thisAsInterface = this;
        }

        void IAesobService.Start()
        {
            var configs = GetWebServiceConfigs();

            var account = _thisAsInterface.GetConfig("Hesap");
            var id = _thisAsInterface.GetConfig("TC");
            var password = _thisAsInterface.GetConfig("Parola");
            var passCode = _thisAsInterface.GetConfig("Sifre");
            var endpointAddress = _thisAsInterface.GetConfig("EndPoint");

            _eYazisma = new EYazismaApi(account, id, password, passCode, configs, endpointAddress);

            _isLoggedIn = true;
            //var loginTask = TryLogin();

            _checkTimer = _checkInterval;
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

            if (_checkTimer > _checkInterval)
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

        private List<PackageDownloadResult> CheckForPackages()
        {
            Debug.Print("Checking for packages...");

            List<PackageDownloadResult> downloadResults = new List<PackageDownloadResult>();

            var curDate = DateTime.Now;

            //var package = CreateTestResult();
            var foundPackagesData = _eYazisma.PaketSorgula(curDate.AddSeconds(-_checkInterval), curDate);

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
                var packageResults = PackageData.CreateFrom(foundPackagesData);
                foreach(var result in packageResults)
                {
                    if(result.KepSiraNo != null)
                    {
                        int siraNo = result.KepSiraNo ?? 0;

                        var downloadResult = _eYazisma.PaketIndir(siraNo, "", EYazismaPart.ALL);

                        downloadResults.Add(PackageDownloadResult.CreateFrom(downloadResult, result));
                    }
                }
            }

            return downloadResults;
        }

        private void AddPackageToEDYS(PackageDownloadResult packageData)
        {
            Debug.Print("Adding package to EDYS");

            //TODO_Kursad: Add packages
        }

        private void MailPackageToReceivers(PackageDownloadResult downloadedPackage)
        {
            PackageData packageData = downloadedPackage.OriginalPackage;
            string subject = packageData.Konu;
            string content = "Bulunan içerik sayısı: " + downloadedPackage.EyazismaPaketi.Length.ToString();

            var targetAddresses = new string[]
            {
                "kursad.fb.96@hotmail.com",
                //"fatihh@msn.com",
                //"yaziisleri@aesob.org.tr",
                //"aesobmuhasebe@gmail.com",
                //"adlihandere@hotmail.com",
            };

            string mailXML = GetXMLStringFromMailData("AesobEmailEncryiptionProtocol123456789", "Kep Yönlendirme", subject, content, targetAddresses);

            var encodedString = Base64UrlEncoder.Encode(mailXML);

            var jsonContent = JsonContent.Create(encodedString, new MediaTypeHeaderValue("application/json"));

            var postTask = _httpClient.PostAsync("https://aesob.org.tr/api/Email/Redirect", jsonContent);
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

        private string GetXMLStringFromMailData(string authKeyword, string senderAlias, string subject, string content, string[] targetAddresses)
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

            var contentNode = rootNode.AppendChild(document.CreateElement("Content"));
            contentNode.InnerText = content;

            return document.InnerXml;
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
