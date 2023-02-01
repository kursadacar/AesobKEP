namespace Tr.Com.Eimza.Org.BouncyCastle.Shared
{
	internal class Pair<T1, T2>
	{
		private T1 t1;

		private T2 t2;

		public T1 First
		{
			get
			{
				return t1;
			}
			set
			{
				t1 = value;
			}
		}

		public T2 Second
		{
			get
			{
				return t2;
			}
			set
			{
				t2 = value;
			}
		}

		public Pair()
		{
		}

		public Pair(T1 aNesne1, T2 aNesne2)
		{
			t1 = aNesne1;
			t2 = aNesne2;
		}

		public override string ToString()
		{
			return string.Concat("[", t1, ",", t2, "]");
		}
	}
}
