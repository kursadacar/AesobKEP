using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.ServiceModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Aesob.KEP.Model;
using Aesob.Web.Core.Public;
using Aesob.Web.Library;
using Aesob.Web.Library.Service;
using Tr.Com.Eimza.EYazisma;

namespace Aesob.KEP.Services
{
    public class PackageListenerService : IAesobService
    {
        private const float _loginTrialInterval = 5f;
        private const float _checkInterval = 10f;//Check every 10 seconds

        private IAesobService _thisAsInterface;

        private bool _isLoggedIn;
        private float _checkTimer;

        private HttpClient _httpClient;
        private EMailService _emailService;
        private EYazismaApi _eYazisma;

        public PackageListenerService(EMailService emailService)
        {
            _httpClient = new HttpClient();
            _thisAsInterface = this;
            _emailService = emailService;
        }

        void IAesobService.Start()
        {
            var configs = GetWebServiceConfigs();

            var account = _thisAsInterface.GetConfig("Hesap");
            var id = _thisAsInterface.GetConfig("TC");
            var password = _thisAsInterface.GetConfig("Parola");
            var passCode = _thisAsInterface.GetConfig("Sifre");

            _eYazisma = new EYazismaApi(account, id, password, passCode, configs);

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

        private List<PackageData> CheckForPackages()
        {
            Debug.Print("Checking for packages...");

            var curDate = DateTime.Now;

            //var package = _eYazisma.PaketSorgula(curDate.AddMinutes(-_checkInterval), curDate, "Inbox");
            var package = CreateTestResult();

            if (package == null || (package.Durum.Length == 1 && package.HataAciklama.Length == 1 && package.Durum[0] != 0))
            {
                Debug.Print("Error while getting packages: " + package?.HataAciklama[0] ?? "Package is null");
            }

            return PackageData.CreateFrom(package);
        }

        private void AddPackageToEDYS(PackageData packageData)
        {
            Debug.Print("Adding package to EDYS");

            //TODO_Kursad: Add packages
        }

        private void MailPackageToReceivers(PackageData packageData)
        {
            string subject = packageData.Konu;
            string content = packageData.Konu;

            //var mailData = new EMailService.MailData("Yeni KEP Mesajı", subject, content, new string[]
            //{
            //    //"yaziisleri@aesob.org.tr",
            //    //"aesobmuhasebe@gmail.com",
            //    //"adlihandere@hotmail.com",
            //});
            //_emailService.SendMail(mailData);

            var targetAddresses = new string[]
            {
                "kursadacarr@hotmail.com",
                "kursad.fb.96@hotmail.com",
                "fatihh@msn.com"
            };

            string mailXML = GetXMLStringFromMailData("Kep Yönlendirme", subject, content, targetAddresses);

            var httpContent = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"ver", "AesobEmailEncryiptionProtocol123456789!*"},
                {"mail", mailXML }
            });

            var postTask = _httpClient.PostAsync("https://aesob.org.tr/Index?handler=RedirectEMail", httpContent);
            postTask.Wait();
            var postResult = postTask.Result;

            var resultStream = postResult.Content.ReadAsStringAsync().Result;
        }

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

        private string GetXMLStringFromMailData(string senderAlias, string subject, string content, string[] targetAddresses)
        {
            XmlDocument document = new XmlDocument();

            var rootNode = document.AppendChild(document.CreateElement("EmailData"));

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
            basicHttpBinding.AllowCookies = true;
            basicHttpBinding.BypassProxyOnLocal = true;
            basicHttpBinding.MaxBufferPoolSize = 33554432;
            basicHttpBinding.MaxReceivedMessageSize = 33554432;
            basicHttpBinding.MaxBufferSize = 33554432;
            basicHttpBinding.UseDefaultWebProxy = true;
            basicHttpBinding.MessageEncoding = WSMessageEncoding.Mtom;

            XmlDictionaryReaderQuotas readerQuotas = new XmlDictionaryReaderQuotas();
            readerQuotas.MaxDepth = 32;
            readerQuotas.MaxStringContentLength = 33554432;
            readerQuotas.MaxArrayLength = 16384;
            readerQuotas.MaxBytesPerRead = 4096;
            readerQuotas.MaxNameTableCharCount = 16384;

            basicHttpBinding.ReaderQuotas = readerQuotas;

            return basicHttpBinding;
        }
    }
}
