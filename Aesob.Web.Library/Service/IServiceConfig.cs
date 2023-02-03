namespace Aesob.Web.Library.Service
{
    public interface IServiceConfig 
    {
        public bool IsEmpty { get; }

        public string Name { get; set; }

        public string Value { get; set; }

        public IReadOnlyCollection<IServiceConfig> SubConfigs { get; }
    }
}
