using Aesob.Web.Library.Service;

namespace Aesob.Web.Library.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsAesobService(this Type type)
        {
            var serviceType = typeof(IAesobService);
            return type != serviceType && serviceType.IsAssignableFrom(type);
        }
    }
}
