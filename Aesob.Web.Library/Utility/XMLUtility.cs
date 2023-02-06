using System.Xml;

namespace Aesob.Web.Library.Utility
{
    public static class XMLUtility
    {
        public static XmlDocument LoadDocumentAtPath(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            XmlDocument document = new XmlDocument();

            var xmlFile = File.Open(path, FileMode.Open);
            document.Load(xmlFile);
            xmlFile.Close();

            return document;
        }

        public static List<(string DocumentPath, XmlDocument Document)> LoadAllDocumentsAtDirectory(string directory)
        {
            List<(string,XmlDocument)> documents = new List<(string, XmlDocument)>();

            if (!Directory.Exists(directory))
            {
                return documents;
            }

            var allFiles = Directory.GetFiles(directory);
            for(int i = 0; i < allFiles.Length; i++)
            {
                var document = LoadDocumentAtPath(allFiles[i]);
                documents.Add((allFiles[i], document));
            }

            return documents;
        }

        public static XmlElement FindElementWithId(this XmlNode document, string id)
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

        public static XmlElement AppendElementWithNameAndValue(this XmlNode element, string elementName, string nameValue, string valueValue)
        {
            var document = element.OwnerDocument;
            var childNode = element.AppendChild(document.CreateElement(elementName));

            var nameAttr = document.CreateAttribute("Name");
            nameAttr.Value = nameValue;

            childNode.Attributes.Append(nameAttr);

            var valueAttr = document.CreateAttribute("Value");
            valueAttr.Value = valueValue;

            childNode.Attributes.Append(valueAttr);

            return (XmlElement)childNode;
        }
    }
}
