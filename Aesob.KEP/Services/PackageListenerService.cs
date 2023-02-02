using System;
using System.Collections.Generic;
using System.ServiceModel;
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

        private EYazismaApi _eYazisma;

        void IAesobService.Start()
        {
            var configs = GetWebServiceConfigs();

            _thisAsInterface = this;

            var account = _thisAsInterface.GetConfig("Hesap");
            var id = _thisAsInterface.GetConfig("TC");
            var password = _thisAsInterface.GetConfig("Parola");
            var passCode = _thisAsInterface.GetConfig("Sifre");

            _eYazisma = new EYazismaApi(account, id, password, passCode, configs);

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

                var regularLoginResult = _eYazisma.Giris(EYazismaGirisTur.OTP);

                bool isSuccess = regularLoginResult?.Durum== "0";
                string status = isSuccess ? "Başarılı" : "Başarısız";
                Debug.Print($"Giriş Sonuç: {status}. Mesaj: {regularLoginResult?.HataAciklama}");

                if (!isSuccess)
                {
                    await Task.Delay(loginTrialInterval);
                    continue;
                }

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

            var package = _eYazisma.PaketSorgula(curDate.AddMinutes(-_checkInterval), curDate, "Inbox");

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
