using Aesob.Web.Library.Service;
using System.Collections.ObjectModel;

namespace Aesob.Web.Core.Internal.Services
{
    internal class ServiceConfig : IServiceConfig
    {
        public static ServiceConfig Empty = new ServiceConfig();

        public bool IsEmpty => this == Empty;

        public string Name { get; set; }

        public string Value { get; set; }

        private List<IServiceConfig> _subConfigs { get; set; }

        public IReadOnlyCollection<IServiceConfig> SubConfigs { get; private set; }

        public ServiceConfig()
        {
            Name = string.Empty;
            Value = string.Empty;

            _subConfigs = new List<IServiceConfig>();
            SubConfigs = new ReadOnlyCollection<IServiceConfig>(_subConfigs);
        }

        public ServiceConfig(string name, string value)
        {
            Name = name;
            Value = value;

            _subConfigs = new List<IServiceConfig>();
            SubConfigs = new ReadOnlyCollection<IServiceConfig>(_subConfigs);
        }

        public void AddSubConfig(IServiceConfig config)
        {
            _subConfigs.Add(config);
        }
    }
}
