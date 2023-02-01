using Aesob.Web.Library.Extensions;
using Aesob.Web.Library.Service;

namespace Aesob.Web.Core.Internal.Services
{
    internal class AesobServiceProvider : IAesobServiceProvider
    {
        IEnumerable<Type> IAesobServiceProvider.CollectAvailableServiceTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsAesobService())
                    {
                        yield return type;
                    }
                }
            }
        }

        IEnumerable<IAesobService> IAesobServiceProvider.GetRunningServices()
        {
            return null;
        }
    }
}