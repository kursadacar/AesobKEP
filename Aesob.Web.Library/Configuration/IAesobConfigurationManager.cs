using Aesob.Web.Library.Service;

namespace Aesob.Web.Library.Configuration
{
    public interface IAesobConfigurationManager
    {
        void ConfigureServices(IEnumerable<IAesobService> services);
    }
}
