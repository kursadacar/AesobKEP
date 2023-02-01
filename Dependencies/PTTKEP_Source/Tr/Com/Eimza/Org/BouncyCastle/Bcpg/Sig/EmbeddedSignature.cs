namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.Sig
{
	internal class EmbeddedSignature : SignatureSubpacket
	{
		public EmbeddedSignature(bool critical, byte[] data)
			: base(SignatureSubpacketTag.EmbeddedSignature, critical, data)
		{
		}
	}
}
