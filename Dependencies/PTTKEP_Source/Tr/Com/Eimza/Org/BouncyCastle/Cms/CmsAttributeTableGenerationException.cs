using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	[Serializable]
	internal class CmsAttributeTableGenerationException : CmsException
	{
		public CmsAttributeTableGenerationException()
		{
		}

		public CmsAttributeTableGenerationException(string name)
			: base(name)
		{
		}

		public CmsAttributeTableGenerationException(string name, Exception e)
			: base(name, e)
		{
		}
	}
}
