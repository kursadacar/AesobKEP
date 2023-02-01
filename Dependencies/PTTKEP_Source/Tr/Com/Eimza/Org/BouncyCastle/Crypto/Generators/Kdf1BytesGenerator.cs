namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Generators
{
	internal class Kdf1BytesGenerator : BaseKdfBytesGenerator
	{
		public Kdf1BytesGenerator(IDigest digest)
			: base(0, digest)
		{
		}
	}
}
