namespace Tr.Com.Eimza.Org.BouncyCastle.OID
{
	internal class OID
	{
		private int[] oid;

		private readonly string oidStr;

		public string Id
		{
			get
			{
				return GetValueStr();
			}
		}

		public int[] Oid
		{
			get
			{
				return GetValue();
			}
		}

		public OID(int[] aValue)
		{
			oid = aValue;
			oidStr = OIDUtil.Concat(oid);
		}

		public OID(string aValue)
		{
			oid = OIDUtil.Parse(aValue);
			oidStr = aValue;
		}

		public override bool Equals(object obj)
		{
			if (obj != null && obj is OID)
			{
				return oidStr.Equals(((OID)obj).ToString());
			}
			return Equals(obj);
		}

		public static OID FromURN(string str)
		{
			return new OID(OIDUtil.FromURN(str));
		}

		public override int GetHashCode()
		{
			return oidStr.GetHashCode();
		}

		public int[] GetValue()
		{
			return oid;
		}

		public string GetValueStr()
		{
			return oidStr;
		}

		public static OID Parse(string str)
		{
			return new OID(OIDUtil.Parse(str));
		}

		public override string ToString()
		{
			return oidStr;
		}
	}
}
