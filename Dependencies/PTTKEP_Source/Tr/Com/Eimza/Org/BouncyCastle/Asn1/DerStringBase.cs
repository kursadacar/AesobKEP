namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1
{
	internal abstract class DerStringBase : Asn1Object, IAsn1String
	{
		public abstract string GetString();

		public override string ToString()
		{
			return GetString();
		}

		protected override int Asn1GetHashCode()
		{
			return GetString().GetHashCode();
		}
	}
}
