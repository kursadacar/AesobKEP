using Aesob.Web.Core.Internal.Services;
using Aesob.Web.Library;
using Aesob.Web.Library.Configuration;
using Aesob.Web.Library.Service;

namespace Aesob.Web.Core.Internal.Configuration
{
    internal class AesobConfigurationManager : IAesobConfigurationManager
    {
        void IAesobConfigurationManager.ConfigureServices(IEnumerable<IAesobService> services)
        {
            ServiceConfig.LoadConfigurations(services);
            ServiceData.LoadServiceDatas(services);
        }
    }
}
