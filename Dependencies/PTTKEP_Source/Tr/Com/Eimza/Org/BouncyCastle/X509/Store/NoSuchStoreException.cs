using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.X509.Store
{
	[Serializable]
	internal class NoSuchStoreException : X509StoreException
	{
		public NoSuchStoreException()
		{
		}

		public NoSuchStoreException(string message)
			: base(message)
		{
		}

		public NoSuchStoreException(string message, Exception e)
			: base(message, e)
		{
		}
	}
}
