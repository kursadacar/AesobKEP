using System.Xml;

namespace Tr.Com.Eimza.Log4Net.Repository
{
	public interface IXmlRepositoryConfigurator
	{
		void Configure(XmlElement element);
	}
}
