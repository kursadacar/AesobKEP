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

            return document;
        }

        public static List<XmlDocument> LoadAllDocumentsAtDirectory(string directory)
        {
            List<XmlDocument> documents = new List<XmlDocument>();

            if (!Directory.Exists(directory))
            {
                return documents;
            }

            var allFiles = Directory.GetFiles(directory);
            for(int i = 0; i < allFiles.Length; i++)
            {
                var document = LoadDocumentAtPath(allFiles[i]);
                documents.Add(document);
            }

            return documents;
        }
    }
}
