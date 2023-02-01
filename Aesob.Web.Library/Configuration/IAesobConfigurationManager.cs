using Aesob.Web.Library.Service;

namespace Aesob.Web.Library.Configuration
{
    public interface IAesobConfigurationManager
    {
        void ConfigureServices<T>(IEnumerable<T> services) where T : IAesobService;
    }
}
