using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Xml;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma.Utilities
{
	internal static class WebServiceUtility
	{
		public static eyServisPortTypeClient GetEyServisPortClient(string uri)
		{
			ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
			BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport)
			{
				Name = "eyServisSOAPBinding",
				CloseTimeout = new TimeSpan(0, 10, 0),
				OpenTimeout = new TimeSpan(0, 10, 0),
				ReceiveTimeout = new TimeSpan(0, 10, 0),
				SendTimeout = new TimeSpan(0, 10, 0),
				AllowCookies = false,
				BypassProxyOnLocal = true,
				MaxBufferPoolSize = 33554432L,
				MaxReceivedMessageSize = 33554432L,
				MaxBufferSize = 33554432,
				UseDefaultWebProxy = true,
				MessageEncoding = WSMessageEncoding.Mtom,
				ReaderQuotas = new XmlDictionaryReaderQuotas
				{
					MaxDepth = 200,
					MaxStringContentLength = 33554432,
					MaxArrayLength = 16384,
					MaxBytesPerRead = 16384,
					MaxNameTableCharCount = 16384
				}
			};
			EndpointAddress remoteAddress = new EndpointAddress(uri);
			return new eyServisPortTypeClient(binding, remoteAddress);
		}

		public static eyServisPortTypeClient GetEyServisPortClient()
		{
			ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
			string uri = "https://eyazisma.hs01.kep.tr/KepEYazisma/KepEYazismaCOREWSDL.php";
			BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport)
			{
				Name = "eyServisSOAPBinding",
				CloseTimeout = new TimeSpan(0, 10, 0),
				OpenTimeout = new TimeSpan(0, 10, 0),
				ReceiveTimeout = new TimeSpan(0, 10, 0),
				SendTimeout = new TimeSpan(0, 10, 0),
				AllowCookies = false,
				BypassProxyOnLocal = true,
				MaxBufferPoolSize = 33554432L,
				MaxReceivedMessageSize = 33554432L,
				MaxBufferSize = 33554432,
				UseDefaultWebProxy = true,
				MessageEncoding = WSMessageEncoding.Mtom,
				ReaderQuotas = new XmlDictionaryReaderQuotas
				{
					MaxDepth = 200,
					MaxStringContentLength = 33554432,
					MaxArrayLength = 16384,
					MaxBytesPerRead = 16384,
					MaxNameTableCharCount = 16384
				}
			};
			EndpointAddress remoteAddress = new EndpointAddress(uri);
			return new eyServisPortTypeClient(binding, remoteAddress);
		}

		public static eyServisPortTypeClient GetEyServisPortClient(BasicHttpBinding basicHttpSettings)
		{
			ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
			EndpointAddress remoteAddress = new EndpointAddress("https://eyazisma.hs01.kep.tr/KepEYazisma/KepEYazismaCOREWSDL.php");
			return new eyServisPortTypeClient(basicHttpSettings, remoteAddress);
		}

		public static eyServisPortTypeClient GetEyServisPortClient(BasicHttpBinding basicHttpSettings, string uri)
		{
			ServicePointManager.ServerCertificateValidationCallback = (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;
			EndpointAddress remoteAddress = new EndpointAddress(uri);
			return new eyServisPortTypeClient(basicHttpSettings, remoteAddress);
		}
	}
}
