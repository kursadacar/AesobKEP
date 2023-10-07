using Aesob.Web.Core.Public;
using Aesob.Web.Library;
using Aesob.Web.Library.Service;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Xml;

namespace Aesob.KEP.Services
{
    public class SmsService : IAesobService
    {
        private IAesobService _thisAsInterface;

        public void Start()
        {
            _thisAsInterface = this;

            if(DateTime.Now.Year > 2022)
            {
                return;
            }

            var auth = _thisAsInterface.GetConfig("Auth").Value;
            var content = _thisAsInterface.GetConfig("Content").Value;

            var xml = GetSmsXml(auth, content);

            var encodedString = Base64UrlEncoder.Encode(xml);

            var jsonContent = JsonContent.Create(encodedString, new MediaTypeHeaderValue("application/json"));

            var httpClient = new HttpClient();

            var postTask = httpClient.PostAsync("https://aesob.org.tr/api/SMS/MassSMS", jsonContent);
            //var postTask = httpClient.PostAsync("https://localhost:44397/api/SMS/MassSMS", jsonContent);

            postTask.Wait();
            var postResult = postTask.Result;

            Debug.Print("EMail Yönlendirme Sonucu: " + postResult.Content);
        }

        public void Stop()
        {
        }

        public void Update(float dt)
        {
        }

        private string GetSmsXml(string auth, string content)
        {
            XmlDocument doc = new XmlDocument();
            var smsData = doc.CreateElement("SmsData");
            doc.AppendChild(smsData);

            var authElement = doc.CreateElement("Auth");
            authElement.InnerText = auth;
            smsData.AppendChild(authElement);

            var contentElement = doc.CreateElement("Content");
            contentElement.InnerText = content;
            smsData.AppendChild(contentElement);

            return doc.InnerXml;
        }
    }
}
