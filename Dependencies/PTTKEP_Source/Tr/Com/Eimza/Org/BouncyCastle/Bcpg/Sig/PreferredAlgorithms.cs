namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.Sig
{
	internal class PreferredAlgorithms : SignatureSubpacket
	{
		private static byte[] IntToByteArray(int[] v)
		{
			byte[] array = new byte[v.Length];
			for (int i = 0; i != v.Length; i++)
			{
				array[i] = (byte)v[i];
			}
			return array;
		}

		public PreferredAlgorithms(SignatureSubpacketTag type, bool critical, byte[] data)
			: base(type, critical, data)
		{
		}

		public PreferredAlgorithms(SignatureSubpacketTag type, bool critical, int[] preferences)
			: base(type, critical, IntToByteArray(preferences))
		{
		}

		public int[] GetPreferences()
		{
			int[] array = new int[data.Length];
			for (int i = 0; i != array.Length; i++)
			{
				array[i] = data[i] & 0xFF;
			}
			return array;
		}
	}
}
