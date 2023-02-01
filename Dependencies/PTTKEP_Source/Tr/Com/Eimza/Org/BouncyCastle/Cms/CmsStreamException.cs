using System;
using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	[Serializable]
	internal class CmsStreamException : IOException
	{
		public CmsStreamException()
		{
		}

		public CmsStreamException(string name)
			: base(name)
		{
		}

		public CmsStreamException(string name, Exception e)
			: base(name, e)
		{
		}
	}
}
