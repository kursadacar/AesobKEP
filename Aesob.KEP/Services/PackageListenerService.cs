using Aesob.KEP.Data;
using Aesob.KEP.Utility;
using Aesob.Web.Library;
using Aesob.Web.Library.Service;
using PTTKEP;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.Xml;
//using Tr.Com.Eimza.EYazisma;

namespace Aesob.KEP.Services
{
    public class PackageListenerService : IAesobService
    {
        private const float _loginTrialInterval = 5f;
        private const float _checkInterval = 10f;//Check every 10 seconds

        private bool _isLoggedIn;
        private float _checkTimer;

        private PttKep _pttKep;
        private const string _wsdlUrl = "https://eyazisma.hs01.kep.tr/KepEYazismaV1.1/KepEYazismaCOREWSDL.php";
        //private EYazismaApi _eYazisma;

        void IAesobService.Start()
        {
            var configs = GetWebServiceConfigs();
            var endPoint = new EndpointAddress(_wsdlUrl);

            _pttKep = new PttKep(configs, endPoint);

            //var credentials = CredentialsHelper.GetCredentials();
            //_eYazisma = new EYazismaApi(credentials.AccountName, credentials.IdNumber, credentials.Password, credentials.PassCode, configs);

            var loginTask = TryLogin();

            _checkTimer = _checkInterval;
        }

        private async Task TryLogin()
        {
            var loginTrialInterval = (int)(_loginTrialInterval * 1000);
            while (!_isLoggedIn)
            {
                string loginInfo = $"Giriş Deneniyor...";
                Debug.Print(loginInfo);

                //var regularLoginResult = _eYazisma.Giris(EYazismaGirisTur.OTP);
                var loginParams = new eyGiris()
                {
                    girisTur = eyGirisTur.BASE,
                    girisTurSpecified = false,
                    kepHesap = GetCurAccount()
                };

                var loginTask = _pttKep.API.GirisAsync(new PTTKEP.GirisRequest(loginParams));
                loginTask.Wait();
                var regularLoginResult = loginTask.Result?.@return;

                bool isSuccess = regularLoginResult?.durum == "0";
                string status = isSuccess ? "Başarılı" : "Başarısız";
                Debug.Print($"Giriş Sonuç: {status}. Mesaj: {regularLoginResult?.hataaciklama}");

                if (!isSuccess)
                {
                    await Task.Delay(loginTrialInterval);
                    continue;
                }

                //var secureLoginResult =_eYazisma.GuvenliEGiris(regularLoginResult.GuvenlikId, p7s);
                //isSuccess = secureLoginResult.Durum == "0";
                //status = isSuccess ? "Başarılı" : "Başarısız";
                //Debug.Print($"Güvenli Giriş Sonuç: {status}. Mesaj: {secureLoginResult.HataAciklama}");

                //if (!isSuccess)
                //{
                //    await Task.Delay(loginTrialInterval);
                //    continue;
                //}

                Debug.Print("Giriş başarılı!");

                _isLoggedIn = true;
            }
        }

        void IAesobService.Update(float dt)
        {
            if (_isLoggedIn)
            {
                PackageTick(dt);
            }
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

        void IAesobService.Stop()
        {
            //_eYazisma = null;
            _checkTimer = 0f;
        }

        private void TryRegisterNewPackages()
        {
            var packages = CheckForPackages();

            if (packages.Count > 0)
            {
                Debug.Print("Found " + packages.Count + " new packages");

                for (int i = 0; i < packages.Count; i++)
                {
                    AddPackageToEDYS(packages[i]);
                }
            }
        }

        private List<PackageData> CheckForPackages()
        {
            Debug.Print("Checking for packages...");

            var curDate = DateTime.Now;

            //var package = _eYazisma.PaketSorgula(curDate.AddMinutes(-_checkInterval), curDate, "Inbox");
            var parameters = new eyPaketSorgula()
            {
                ilktarih = curDate.AddMinutes(_checkInterval),
                ilktarihSpecified = true,
                sontarih = curDate,
                sontarihSpecified = true,
                dizin = "INBOX",
                kepHesap = GetCurAccount()
            };

            var package = _pttKep.API.PaketSorgulaAsync(new PaketSorgulaRequest(parameters))?.Result?.@return;

            if(package == null || (package.durum.Length == 1 && package.hataaciklama.Length == 1 && package.durum[0] != 0))
            {
                Debug.Print("Error while getting packages: " + package?.hataaciklama[0] ?? "Package is null");
            }

            return PackageData.CreateFrom(package);
        }

        private eyKepHesapGirisP GetCurAccount()
        {
            var credentials = CredentialsHelper.GetCredentials();

            return new eyKepHesapGirisP()
            {
                kepHesap = credentials.AccountName,
                parola = credentials.Password,
                sifre = credentials.PassCode,
                tcno = credentials.IdNumber
            };
        }

        private void AddPackageToEDYS(PackageData packageData)
        {
            Debug.Print("Adding package to EDYS");

            //TODO_Kursad: Add packages
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
            readerQuotas.MaxStringContentLength = 33554432;
            readerQuotas.MaxArrayLength = 16384;
            readerQuotas.MaxBytesPerRead = 4096;
            readerQuotas.MaxNameTableCharCount = 16384;

            basicHttpBinding.ReaderQuotas = readerQuotas;

            return basicHttpBinding;
        }
    }
}
