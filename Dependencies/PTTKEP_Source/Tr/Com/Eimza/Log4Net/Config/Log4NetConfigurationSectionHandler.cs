using System.Configuration;
using System.Xml;

namespace Tr.Com.Eimza.Log4Net.Config
{
	public class Log4NetConfigurationSectionHandler : IConfigurationSectionHandler
	{
		public object Create(object parent, object configContext, XmlNode section)
		{
			return section;
		}
	}
}
