namespace Aesob.Web.Library.Service
{
    public interface IServiceConfig 
    {
        bool IsEmpty { get; }

        string Name { get; set; }

        string Value { get; set; }

        IReadOnlyCollection<IServiceConfig> SubConfigs { get; }
    }
}
