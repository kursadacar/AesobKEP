using Aesob.Web.Core.Internal;
using Aesob.Web.Core.Internal.Services;
using Aesob.Web.Library.Service;

namespace Aesob.Web.Core.Public
{
    public static class ServiceExtensions
    {
        public static IServiceConfig GetConfig(this IAesobService service, string key)
        {
            return ServiceConfig.GetServiceConfig(service, key);
        }

        public static IServiceData GetData(this IAesobService service, string key)
        {
            return ServiceData.GetServiceData(service, key);
        }

        public static void SetData(this IAesobService service, string key, IServiceData value)
        {
            ServiceData.SetServiceData(service, key, value);
        }

        public static IServiceData CreateServiceData(this IAesobService service, string key, string value)
        {
            return new ServiceData(key, value);
        }

        public static IServiceData CreateAndRegisterServiceData(this IAesobService service, string key, string value)
        {
            var data = CreateServiceData(service, key, value);
            SetData(service, key, data);
            return data;
        }

        public static void SaveData(this IAesobService service)
        {
            ServiceData.SaveServiceData(service);
        }

        public static void ClearData(this IAesobService service)
        {
            ServiceData.ClearServiceData(service);
        }

        public static void LoadDataFromFile(this IAesobService service)
        {
            ServiceData.LoadServiceData(service);
        }
    }
}
