using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	[Serializable]
	internal class CmsException : Exception
	{
		public CmsException()
		{
		}

		public CmsException(Exception e)
			: base(null, e)
		{
		}

		public CmsException(string msg)
			: base(msg)
		{
		}

		public CmsException(string msg, Exception e)
			: base(msg, e)
		{
		}
	}
}
