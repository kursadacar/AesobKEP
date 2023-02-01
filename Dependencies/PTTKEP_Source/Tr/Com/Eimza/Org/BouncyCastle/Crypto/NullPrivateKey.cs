namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto
{
	internal class NullPrivateKey : AsymmetricKeyParameter
	{
		public NullPrivateKey()
			: base(true)
		{
		}

		public override string ToString()
		{
			return "NULL";
		}
	}
}
