using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal interface CmsProcessable
	{
		void Write(Stream outStream);

		object GetContent();
	}
}
