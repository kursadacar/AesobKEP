namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.Sig
{
	internal class TrustSignature : SignatureSubpacket
	{
		public int Depth
		{
			get
			{
				return data[0] & 0xFF;
			}
		}

		public int TrustAmount
		{
			get
			{
				return data[1] & 0xFF;
			}
		}

		private static byte[] IntToByteArray(int v1, int v2)
		{
			return new byte[2]
			{
				(byte)v1,
				(byte)v2
			};
		}

		public TrustSignature(bool critical, byte[] data)
			: base(SignatureSubpacketTag.TrustSig, critical, data)
		{
		}

		public TrustSignature(bool critical, int depth, int trustAmount)
			: base(SignatureSubpacketTag.TrustSig, critical, IntToByteArray(depth, trustAmount))
		{
		}
	}
}
