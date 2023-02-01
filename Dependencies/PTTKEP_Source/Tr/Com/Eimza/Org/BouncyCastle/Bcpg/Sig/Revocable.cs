namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.Sig
{
	internal class Revocable : SignatureSubpacket
	{
		private static byte[] BooleanToByteArray(bool value)
		{
			byte[] array = new byte[1];
			if (value)
			{
				array[0] = 1;
				return array;
			}
			return array;
		}

		public Revocable(bool critical, byte[] data)
			: base(SignatureSubpacketTag.Revocable, critical, data)
		{
		}

		public Revocable(bool critical, bool isRevocable)
			: base(SignatureSubpacketTag.Revocable, critical, BooleanToByteArray(isRevocable))
		{
		}

		public bool IsRevocable()
		{
			return data[0] != 0;
		}
	}
}
