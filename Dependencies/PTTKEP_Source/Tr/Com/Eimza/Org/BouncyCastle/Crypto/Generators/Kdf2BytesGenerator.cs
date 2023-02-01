namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Generators
{
	internal class Kdf2BytesGenerator : BaseKdfBytesGenerator
	{
		public Kdf2BytesGenerator(IDigest digest)
			: base(1, digest)
		{
		}
	}
}
