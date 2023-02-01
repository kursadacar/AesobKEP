using System;
using System.IO;
using System.ServiceModel;
using System.Xml;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;

namespace Tr.Com.Eimza.EYazisma.Config
{
	public class EYazismaApiConfig
	{
		private string _WsdlUrl;

		private Boyut _MaxMailDosyaBoyutu;

		private BasicHttpBinding _WebServiceAyarlari;

		private eyServisPortTypeClient _EyServisPortClient;

		private EndpointAddress _EndpointAddress;

		private string EYAZISMA_API_LOG;

		private string EY_LOGLARI;

		private string LOG_CONFIG_FILE;

		public EYazismaApiConfig Default
		{
			get
			{
				return new EYazismaApiConfig();
			}
		}

		public eyServisPortTypeClient EyServisPortClient
		{
			get
			{
				return _EyServisPortClient;
			}
		}

		public BasicHttpBinding WebServiceAyarlari
		{
			get
			{
				return _WebServiceAyarlari;
			}
			set
			{
				_WebServiceAyarlari = value;
				_EyServisPortClient = new eyServisPortTypeClient(_WebServiceAyarlari, _EndpointAddress);
			}
		}

		public string WsdlUrl
		{
			get
			{
				return _WsdlUrl;
			}
			set
			{
				_WsdlUrl = value;
				_EndpointAddress = CreateEndPoint(_WsdlUrl);
				_EyServisPortClient = new eyServisPortTypeClient(_WebServiceAyarlari, _EndpointAddress);
			}
		}

		public Boyut MaxMailDosyaBoyutu
		{
			get
			{
				return _MaxMailDosyaBoyutu;
			}
			set
			{
				_MaxMailDosyaBoyutu = value;
			}
		}

		public EndpointAddress Address
		{
			get
			{
				return _EndpointAddress;
			}
		}

		public string EyazismaApiLog
		{
			get
			{
				return EYAZISMA_API_LOG;
			}
			set
			{
				EYAZISMA_API_LOG = value;
			}
		}

		public string EyLoglari
		{
			get
			{
				return EY_LOGLARI;
			}
			set
			{
				EY_LOGLARI = value;
			}
		}

		public string LogConfigFile
		{
			get
			{
				return LOG_CONFIG_FILE;
			}
			set
			{
				LOG_CONFIG_FILE = value;
			}
		}

		public EYazismaApiConfig()
		{
			_WsdlUrl = "https://eyazisma.hs01.kep.tr/KepEYazismaV1.1/KepEYazismaCOREWSDL.php";
			_MaxMailDosyaBoyutu = Boyut.Default;
			_WebServiceAyarlari = GetDefaultWebServiceAyarlari();
			_EndpointAddress = GetDefaultEndPoint();
			_EyServisPortClient = new eyServisPortTypeClient(_WebServiceAyarlari, _EndpointAddress);
			Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
			EYAZISMA_API_LOG = Path.Combine(Directory.GetCurrentDirectory(), "EYazismaApi.log");
			EY_LOGLARI = Path.Combine(Directory.GetCurrentDirectory(), "EyLoglari.log");
			LOG_CONFIG_FILE = Path.Combine(Directory.GetCurrentDirectory(), "log4net.xml");
		}

		private BasicHttpBinding GetDefaultWebServiceAyarlari()
		{
			return new BasicHttpBinding(BasicHttpSecurityMode.Transport)
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
		}

		private EndpointAddress GetDefaultEndPoint()
		{
			return new EndpointAddress(_WsdlUrl);
		}

		private EndpointAddress CreateEndPoint(string Uri)
		{
			return new EndpointAddress(Uri);
		}
	}
}
