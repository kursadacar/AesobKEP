using Aesob.Web.Library;
using Aesob.Web.Library.Path;
using Aesob.Web.Library.Service;
using Aesob.Web.Library.Utility;
using System.Collections.ObjectModel;
using System.Xml;

namespace Aesob.Web.Core.Internal.Services
{
    internal class ServiceConfig : IServiceConfig
    {
        private static Dictionary<IAesobService, Dictionary<string, IServiceConfig>> _serviceConfigs;

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

        internal static IServiceConfig GetServiceConfig(IAesobService service, string key)
        {
            if (_serviceConfigs.TryGetValue(service, out var config))
            {
                if (config.TryGetValue(key, out var value))
                {
                    return value;
                }
                else
                {
                    Debug.FailedAssert($"Could not find config parameter: {key} in config for: {service.GetType().Name}");
                }
            }
            else
            {
                Debug.FailedAssert($"Could not find service in config dictionary: {service.GetType().Name}");
            }

            return ServiceConfig.Empty;
        }

        internal static void SetServiceConfig(IAesobService service, string key, IServiceConfig value)
        {
            _serviceConfigs[service][key] = value;
        }

        internal static void LoadConfigurations(IEnumerable<IAesobService> services)
        {
            _serviceConfigs = new Dictionary<IAesobService, Dictionary<string, IServiceConfig>>();

            foreach(var service in services)
            {
                _serviceConfigs.Add(service, new Dictionary<string, IServiceConfig>());
            }

            var configs = XMLUtility.LoadAllDocumentsAtDirectory(ApplicationPath.ConfigFolder.ToString());

            Dictionary<string, IAesobService> serviceNames = new Dictionary<string, IAesobService>();
            foreach (var service in services)
            {
                serviceNames.Add(service.GetType().Name, service);
            }

            for (int i = 0; i < configs.Count; i++)
            {
                var config = configs[i];

                var document = config.Document;

                if (document == null)
                {
                    Debug.FailedAssert("Config file is null");
                    continue;
                }

                if (document.FirstChild.Name != "ServiceConfig")
                {
                    Debug.FailedAssert($"Config file does not have root element: \"ServiceConfig\"");
                    continue;
                }

                if (serviceNames.TryGetValue(document.FirstChild.Attributes["ServiceName"].Value, out var service))
                {
                    SetConfigsFromDocument(service, document.FirstChild);
                }
                //else
                //{
                //    Debug.FailedAssert($"Can't find service: {document.FirstChild.Name} from the config file: {document.Name}");
                //}
            }
        }

        private static void SetConfigsFromDocument(IAesobService service, XmlNode document)
        {
            XmlNode parametersParentNode = document.FindElementWithId("Parameters");

            if (parametersParentNode != null)
            {
                var configs = GetConfigsFromNode(parametersParentNode);
                foreach (var config in configs)
                {
                    SetServiceConfig(service, config.Name, config);
                }
            }
        }

        private static List<IServiceConfig> GetConfigsFromNode(XmlNode parentNode)
        {
            List<IServiceConfig> configs = new List<IServiceConfig>();

            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
            {
                var parameterNode = parentNode.ChildNodes.Item(i);
                ServiceConfig config = null;

                if (parameterNode.Name == "Parameter")
                {
                    config = new ServiceConfig(parameterNode.Attributes["Name"].Value, parameterNode.Attributes["Value"].Value);
                }
                else if (parameterNode.Name == "ListParameter")
                {
                    config = new ServiceConfig(parameterNode.Attributes["Name"].Value, string.Empty);
                }
                else if (parameterNode.Name == "SubParameter")
                {
                    config = new ServiceConfig(string.Empty, parameterNode.Attributes["Value"].Value);
                }

                if(config != null)
                {
                    configs.Add(config);

                    if (parameterNode.ChildNodes.Count > 0)
                    {
                        var subConfigs = GetConfigsFromNode(parameterNode);

                        foreach (var subConfig in subConfigs)
                        {
                            config.AddSubConfig(subConfig);
                        }
                    }
                }

            }

            return configs;
        }
    }
}
