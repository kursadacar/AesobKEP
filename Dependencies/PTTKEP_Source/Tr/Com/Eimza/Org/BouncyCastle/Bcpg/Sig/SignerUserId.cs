namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.Sig
{
	internal class SignerUserId : SignatureSubpacket
	{
		private static byte[] UserIdToBytes(string id)
		{
			byte[] array = new byte[id.Length];
			for (int i = 0; i != id.Length; i++)
			{
				array[i] = (byte)id[i];
			}
			return array;
		}

		public SignerUserId(bool critical, byte[] data)
			: base(SignatureSubpacketTag.SignerUserId, critical, data)
		{
		}

		public SignerUserId(bool critical, string userId)
			: base(SignatureSubpacketTag.SignerUserId, critical, UserIdToBytes(userId))
		{
		}

		public string GetId()
		{
			char[] array = new char[data.Length];
			for (int i = 0; i != array.Length; i++)
			{
				array[i] = (char)(data[i] & 0xFFu);
			}
			return new string(array);
		}
	}
}
