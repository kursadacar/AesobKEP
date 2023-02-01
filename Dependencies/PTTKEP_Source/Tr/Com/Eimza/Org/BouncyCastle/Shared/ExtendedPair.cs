namespace Tr.Com.Eimza.Org.BouncyCastle.Shared
{
	internal class ExtendedPair<T1, T2, T3>
	{
		private T1 t1;

		private T2 t2;

		private T3 t3;

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

		public T3 Third
		{
			get
			{
				return t3;
			}
			set
			{
				t3 = value;
			}
		}

		public ExtendedPair(T1 aNesne1, T2 aNesne2, T3 aNesne3)
		{
			t1 = aNesne1;
			t2 = aNesne2;
			t3 = aNesne3;
		}

		public override string ToString()
		{
			return string.Concat("[", t1, ",", t2, ",", t3, "]");
		}
	}
}
