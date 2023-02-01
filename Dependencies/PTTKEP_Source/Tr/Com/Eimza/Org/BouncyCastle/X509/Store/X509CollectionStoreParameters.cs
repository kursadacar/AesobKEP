using System;
using System.Collections;
using System.Text;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.X509.Store
{
	internal class X509CollectionStoreParameters : IX509StoreParameters
	{
		private readonly IList collection;

		public X509CollectionStoreParameters(ICollection collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("collection");
			}
			this.collection = Platform.CreateArrayList(collection);
		}

		public ICollection GetCollection()
		{
			return Platform.CreateArrayList(collection);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("X509CollectionStoreParameters: [\n");
			IList list = collection;
			stringBuilder.Append("  collection: " + ((list != null) ? list.ToString() : null) + "\n");
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}
	}
}
