using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class CmsAuthenticatedGenerator : CmsEnvelopedGenerator
	{
		public CmsAuthenticatedGenerator()
		{
		}

		public CmsAuthenticatedGenerator(SecureRandom rand)
			: base(rand)
		{
		}
	}
}
