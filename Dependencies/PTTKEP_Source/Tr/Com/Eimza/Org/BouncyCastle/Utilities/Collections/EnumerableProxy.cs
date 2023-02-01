using System;
using System.Collections;

namespace Tr.Com.Eimza.Org.BouncyCastle.Utilities.Collections
{
	internal sealed class EnumerableProxy : IEnumerable
	{
		private readonly IEnumerable inner;

		public EnumerableProxy(IEnumerable inner)
		{
			if (inner == null)
			{
				throw new ArgumentNullException("inner");
			}
			this.inner = inner;
		}

		public IEnumerator GetEnumerator()
		{
			return inner.GetEnumerator();
		}
	}
}
