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
        private const string _rootNodeName = "ServiceConfig";
        private const string _parameterNodeParentName = "Parameters";
        private const string _parameterNodeName = "Parameter";

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

                if(config.FirstChild.Name != _rootNodeName)
                {
                    Debug.FailedAssert($"Config file does not have root element: {_rootNodeName}");
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
            XmlNode parametersParentNode = null;

            for(int i = 0; i < document.ChildNodes.Count; i++)
            {
                var item = document.ChildNodes.Item(i);

                if(item.Name == _parameterNodeParentName)
                {
                    parametersParentNode = item;
                    break;
                }
            }

            if(parametersParentNode == null)
            {
                Debug.FailedAssert($"No {_parameterNodeParentName} node in config file: {document.Name}");
                return;
            }

            for(int i = 0; i < parametersParentNode.ChildNodes.Count; i++)
            {
                var parameterNode = parametersParentNode.ChildNodes.Item(i);
                if(parameterNode.Name != _parameterNodeName)
                {
                    Debug.FailedAssert($"Parameters node should only contain {_parameterNodeName} node in config file: {_parameterNodeParentName}");
                    continue;
                }

                ServiceManager.Instance.SetServiceConfig(service, parameterNode.Attributes["Name"].InnerText, parameterNode.Attributes["Value"].InnerText);
            }
        }
    }
}
