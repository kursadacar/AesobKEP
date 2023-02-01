using System;
using System.Collections;

namespace Tr.Com.Eimza.Log4Net.Util
{
	public sealed class NullEnumerator : IEnumerator
	{
		private static readonly NullEnumerator s_instance = new NullEnumerator();

		public static NullEnumerator Instance
		{
			get
			{
				return s_instance;
			}
		}

		public object Current
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		private NullEnumerator()
		{
		}

		public bool MoveNext()
		{
			return false;
		}

		public void Reset()
		{
		}
	}
}
