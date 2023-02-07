using Aesob.Web.Library;
using Aesob.Web.Library.Path;
using Aesob.Web.Library.Service;
using Aesob.Web.Library.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Aesob.Web.Core.Internal.Services
{
    internal class ServiceData : IServiceData
    {
        private static Dictionary<IAesobService, Dictionary<string, IServiceData>> _serviceDatas;
        private static Dictionary<IAesobService, string> _serviceDataPaths;

        public static ServiceData Empty { get; } = new ServiceData();

        public bool IsEmpty => this == Empty;

        private bool _isDirty;
        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if(value != _name)
                {
                    _name = value;
                    IsDirty = true;
                }
            }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                if(value != _value)
                {
                    _value = value;
                    IsDirty = true;
                }
            }
        }

        private List<IServiceData> _subData;
        public IReadOnlyCollection<IServiceData> SubData { get; }

        public ServiceData()
        {
            Name = string.Empty;
            Value = string.Empty;

            _subData = new List<IServiceData>();
            SubData = new ReadOnlyCollection<IServiceData>(_subData);
        }

        public ServiceData(string name, string value)
        {
            Name = name;
            Value = value;

            _subData = new List<IServiceData>();
            SubData = new ReadOnlyCollection<IServiceData>(_subData);
        }

        public IServiceData AddSubData(IServiceData data)
        {
            _subData.Add(data);
            IsDirty = true;
            return data;
        }

        public IServiceData RemoveSubData(IServiceData data)
        {
            _subData.Remove(data);
            IsDirty = true;
            return data;
        }

        internal static IServiceData GetServiceData(IAesobService service, string key)
        {
            if (_serviceDatas.TryGetValue(service, out var config))
            {
                if (config.TryGetValue(key, out var value))
                {
                    return value;
                }
            }
            else
            {
                Debug.FailedAssert($"Could not find service in data dictionary: {service.GetType().Name}");
            }

            return null;
        }

        internal static void SetServiceData(IAesobService service, string key, IServiceData value)
        {
            _serviceDatas[service][key] = value;
        }

        internal static void SaveServiceData(IAesobService service)
        {
            if(_serviceDatas.TryGetValue(service, out var serviceDatas))
            {
                bool isAnyDataDirty = false;
                foreach(var dataKvp in serviceDatas)
                {
                    if (GetIsDataDirty(dataKvp.Value))
                    {
                        isAnyDataDirty = true;
                        break;
                    }
                }

                //Do not process if there is no new data
                if (!isAnyDataDirty)
                {
                    return;
                }

                string dataPath;
                if(!_serviceDataPaths.TryGetValue(service, out dataPath))
                {
                    dataPath = ApplicationPath.DataFolder.Append(service.GetType().Name + ".xml").ToString();
                    _serviceDataPaths[service] = dataPath;
                }

                var document = new XmlDocument();

                var documentRoot = document.AppendChild(document.CreateElement("ServiceData"));
                var serviceNameAttr = document.CreateAttribute("ServiceName");
                serviceNameAttr.Value = service.GetType().Name;
                documentRoot.Attributes.Append(serviceNameAttr);

                var rootNode = documentRoot.AppendChild(document.CreateElement("DataCollection"));

                foreach(var data in serviceDatas)
                {
                    if (!data.Value.IsEmpty)
                    {
                        AppendNestedData(rootNode, data.Value);
                    }
                }

                document.Save(dataPath);

                foreach(var data in serviceDatas)
                {
                    SetAllDataDirty(data.Value, false);
                }
            }
        }

        private static bool GetIsDataDirty(IServiceData serviceData)
        {
            if (serviceData.IsDirty)
            {
                return true;
            }

            foreach(var subData in serviceData.SubData)
            {
                if (GetIsDataDirty(subData))
                {
                    return true;
                }
            }

            return false;
        }

        private static void SetAllDataDirty(IServiceData serviceData, bool value)
        {
            serviceData.IsDirty = value;

            foreach(var subData in serviceData.SubData)
            {
                SetAllDataDirty(subData, value);
            }
        }

        private static void AppendNestedData(XmlNode node, IServiceData data)
        {
            var listParent = node.AppendElementWithNameAndValue("Data", data.Name, data.Value);

            foreach (var subData in data.SubData)
            {
                AppendNestedData(listParent, subData);
            }
        }

        internal static void ClearServiceData(IAesobService service)
        {
            if(_serviceDatas.TryGetValue(service, out var serviceData))
            {
                serviceData.Clear();
            }
            else
            {
                _serviceDatas[service] = new Dictionary<string, IServiceData>();
            }
        }

        internal static void LoadServiceData(IAesobService service)
        {
            if(_serviceDataPaths.TryGetValue(service, out var path))
            {
                var document = XMLUtility.LoadDocumentAtPath(path);
                var firstChild = document?.FirstChild;
                if(firstChild != null)
                {
                    _serviceDatas[service].Clear();
                    SetDatasFromDocument(service, firstChild);
                }
            }
        }

        internal static void LoadServiceDatas(IEnumerable<IAesobService> services)
        {
            _serviceDataPaths = new Dictionary<IAesobService, string>();
            _serviceDatas = new Dictionary<IAesobService, Dictionary<string, IServiceData>>();

            foreach (var service in services)
            {
                _serviceDatas.Add(service, new Dictionary<string, IServiceData>());
            }

            var datas = XMLUtility.LoadAllDocumentsAtDirectory(ApplicationPath.DataFolder.ToString());

            Dictionary<string, IAesobService> serviceNames = new Dictionary<string, IAesobService>();
            foreach (var service in services)
            {
                serviceNames.Add(service.GetType().Name, service);
            }

            for (int i = 0; i < datas.Count; i++)
            {
                var data = datas[i];

                var document = data.Document;

                if (document == null)
                {
                    Debug.FailedAssert("Data file is null");
                    continue;
                }

                if (document.FirstChild.Name != "ServiceData")
                {
                    Debug.FailedAssert($"Config file does not have root element: \"ServiceConfig\"");
                    continue;
                }

                if (serviceNames.TryGetValue(document.FirstChild.Attributes["ServiceName"].Value, out var service))
                {
                    _serviceDataPaths[service] = data.DocumentPath;
                    SetDatasFromDocument(service, document.FirstChild);
                }
                //else
                //{
                //    Debug.FailedAssert($"Can't find service: {document.FirstChild.Name} from the config file: {document.Name}");
                //}
            }
        }

        private static void SetDatasFromDocument(IAesobService service, XmlNode document)
        {
            XmlNode parametersParentNode = document.FindElementWithId("DataCollection");

            if (parametersParentNode != null)
            {
                var datas = GetDatasFromNode(parametersParentNode);
                foreach (var data in datas)
                {
                    SetServiceData(service, data.Name, data);
                }
            }
        }

        private static List<IServiceData> GetDatasFromNode(XmlNode parentNode)
        {
            List<IServiceData> datas = new List<IServiceData>();

            for (int i = 0; i < parentNode.ChildNodes.Count; i++)
            {
                var parameterNode = parentNode.ChildNodes.Item(i);
                ServiceData data = GetNestedDataFromNode(parameterNode);

                datas.Add(data);
            }

            return datas;
        }

        private static ServiceData GetNestedDataFromNode(XmlNode node)
        {
            ServiceData data = null;

            if (node.Name == "Data")
            {
                data = new ServiceData(node.Attributes["Name"].Value, node.Attributes["Value"].Value);
            }

            if(data != null && node.ChildNodes.Count > 0)
            {
                foreach(var childNode in node.ChildNodes)
                {
                    var subData = GetNestedDataFromNode((XmlNode)childNode);
                    data.AddSubData(subData);
                }
            }

            return data;
        }
    }
}
