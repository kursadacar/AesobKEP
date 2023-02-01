namespace Aesob.Web.Library.Service
{
    public interface IAesobServiceProvider
    {
        IEnumerable<Type> CollectAvailableServiceTypes();
        IEnumerable<IAesobService> GetRunningServices();
    }
}