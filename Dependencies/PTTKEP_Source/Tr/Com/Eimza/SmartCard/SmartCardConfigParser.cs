using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.SmartCard
{
	internal class SmartCardConfigParser
	{
		private static readonly string ARCH_32_BIT = "32";

		private static readonly string ARCH_64_BIT = "64";

		private static readonly string ATTRIBUTE_ARCH = "arch";

		private static readonly string ATTRIBUTE_NAME = "name";

		private static readonly string ATTRIBUTE_VALUE = "value";

		private static readonly string ELEMENT_ATR = "atr";

		private static readonly string ELEMENT_CARD_TYPE = "card-type";

		private static readonly string ELEMENT_LIB = "lib";

		private static string CONFIG_FILE_NAME = "Config\\smartcard-config.xml";

		private static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public List<CardTypeConfig> ReadConfig()
		{
			List<CardTypeConfig> list = new List<CardTypeConfig>();
			try
			{
				return ReadConfig(new FileStream(CONFIG_FILE_NAME, FileMode.Open, FileAccess.Read));
			}
			catch (Exception exception)
			{
				logger.Error("Akıllı Kart Ayar Dosyası Okunamadı.", exception);
				return null;
			}
		}

		public List<CardTypeConfig> ReadConfig(Stream istrm)
		{
			List<CardTypeConfig> list = new List<CardTypeConfig>();
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.PreserveWhitespace = true;
				xmlDocument.Load(istrm);
				foreach (XmlElement item2 in xmlDocument.GetElementsByTagName(ELEMENT_CARD_TYPE))
				{
					string attribute = item2.GetAttribute(ATTRIBUTE_NAME);
					string aLib = null;
					string aLib2 = null;
					string aLib3 = null;
					List<string> list2 = new List<string>();
					foreach (XmlElement item3 in item2.GetElementsByTagName(ELEMENT_LIB))
					{
						string attribute2 = item3.GetAttribute(ATTRIBUTE_NAME);
						string attribute3 = item3.GetAttribute(ATTRIBUTE_ARCH);
						if (attribute3 == null)
						{
							aLib = attribute2;
						}
						else if (attribute3.Equals(ARCH_32_BIT))
						{
							aLib2 = attribute2;
						}
						else if (attribute3.Equals(ARCH_64_BIT))
						{
							aLib3 = attribute2;
						}
						else
						{
							aLib = attribute2;
						}
					}
					foreach (XmlElement item4 in item2.GetElementsByTagName(ELEMENT_ATR))
					{
						string attribute4 = item4.GetAttribute(ATTRIBUTE_VALUE);
						if (attribute4 != null && attribute4.Length > 0)
						{
							list2.Add(attribute4);
						}
					}
					CardTypeConfig item = new CardTypeConfig(attribute, aLib, aLib2, aLib3, list2);
					list.Add(item);
				}
				return list;
			}
			catch (Exception exception)
			{
				logger.Error("Akıllı Kart Ayar Dosyası Okunamadı.", exception);
				return list;
			}
		}
	}
}
