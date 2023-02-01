namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
	internal class AddressType
	{
		public static readonly AddressType DN = new AddressType("dn");

		public static readonly AddressType HTTP = new AddressType("http");

		public static readonly AddressType LDAP = new AddressType("ldap");

		private string adres;

		private AddressType(string aValue)
		{
			adres = aValue;
		}

		public string asString()
		{
			return adres;
		}

		public bool Equals(AddressType aAddressType)
		{
			return adres == aAddressType.asString();
		}
	}
}
