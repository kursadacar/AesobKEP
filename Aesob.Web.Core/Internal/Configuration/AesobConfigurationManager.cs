using Aesob.Web.Core.Internal.Services;
using Aesob.Web.Library;
using Aesob.Web.Library.Configuration;
using Aesob.Web.Library.Path;
using Aesob.Web.Library.Service;
using Aesob.Web.Library.Utility;
using System.Xml;

namespace Aesob.Web.Core.Internal.Configuration
{
    internal class AesobConfigurationManager : IAesobConfigurationManager
    {
        void IAesobConfigurationManager.ConfigureServices(IEnumerable<IAesobService> services)
        {
            SetConfigsFromFiles(services);
        }

        private void SetConfigsFromFiles(IEnumerable<IAesobService> services)
        {
            var configs = XMLUtility.LoadAllDocumentsAtDirectory(DataPaths.ConfigFolder.ToString());

            Dictionary<string, IAesobService> serviceNames = new Dictionary<string, IAesobService>();
            foreach(var service in services)
            {
                serviceNames.Add(service.GetType().Name, service);
            }

            for(int i = 0; i < configs.Count; i++)
            {
                var config = configs[i];

                if(config == null)
                {
                    Debug.FailedAssert("Config file is null");
                    continue;
                }

                if(config.FirstChild.Name != "ServiceConfig")
                {
                    Debug.FailedAssert($"Config file does not have root element: \"ServiceConfig\"");
                    continue;
                }

                if (serviceNames.TryGetValue(config.FirstChild.Attributes["ServiceName"].Value, out var service))
                {
                    SetConfigsFromDocument(service, config.FirstChild);
                }
                else
                {
                    Debug.FailedAssert($"Can't find service: {config.FirstChild.Name} from the config file: {config.Name}");
                }
            }
        }

        private void SetConfigsFromDocument(IAesobService service, XmlNode document)
        {
            XmlNode parametersParentNode = FindElementWithId(document, "Parameters");

            if(parametersParentNode != null)
            {
                var configs = GetConfigsFromNode(parametersParentNode);
                foreach(var config in configs)
                {
                    ServiceManager.Instance.SetServiceConfig(service, config.Name, config);
                }
            }
        }

        private static XmlElement FindElementWithId(XmlNode document, string id)
        {
            XmlElement element = null;

            for (int i = 0; i < document.ChildNodes.Count; i++)
            {
                var item = document.ChildNodes.Item(i);

                if (item.Name == id && item is XmlElement elementItem)
                {
                    element = elementItem;
                    break;
                }
            }

            return element;
        }

        private List<IServiceConfig> GetConfigsFromNode(XmlNode parentNode)
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
                else if(parameterNode.Name == "ListParameter")
                {
                    config = new ServiceConfig(parameterNode.Attributes["Name"].Value, string.Empty);
                }
                else if(parameterNode.Name == "SubParameter")
                {
                    config = new ServiceConfig(string.Empty, parameterNode.Attributes["Value"].Value);
                }

                configs.Add(config);

                if (config != null && parameterNode.ChildNodes.Count > 0)
                {
                    var subConfigs = GetConfigsFromNode(parameterNode);

                    foreach (var subConfig in subConfigs)
                    {
                        config.AddSubConfig(subConfig);
                    }
                }
            }

            return configs;
        }
    }
}
