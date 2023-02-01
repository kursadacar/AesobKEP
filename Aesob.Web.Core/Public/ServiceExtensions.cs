using Aesob.Web.Core.Internal;
using Aesob.Web.Library.Service;

namespace Aesob.Web.Core.Public
{
    public static class ServiceExtensions
    {
        public static string GetConfig(this IAesobService service, string key)
        {
            return ServiceManager.Instance.GetServiceConfig(service, key);
        }
    }
}
