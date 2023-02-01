namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.Sig
{
	internal class Exportable : SignatureSubpacket
	{
		private static byte[] BooleanToByteArray(bool val)
		{
			byte[] array = new byte[1];
			if (val)
			{
				array[0] = 1;
				return array;
			}
			return array;
		}

		public Exportable(bool critical, byte[] data)
			: base(SignatureSubpacketTag.Exportable, critical, data)
		{
		}

		public Exportable(bool critical, bool isExportable)
			: base(SignatureSubpacketTag.Exportable, critical, BooleanToByteArray(isExportable))
		{
		}

		public bool IsExportable()
		{
			return data[0] != 0;
		}
	}
}
