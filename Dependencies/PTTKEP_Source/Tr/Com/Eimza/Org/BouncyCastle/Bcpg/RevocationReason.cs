using System;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg
{
	internal class RevocationReason : SignatureSubpacket
	{
		public RevocationReason(bool isCritical, byte[] data)
			: base(SignatureSubpacketTag.RevocationReason, isCritical, data)
		{
		}

		public RevocationReason(bool isCritical, RevocationReasonTag reason, string description)
			: base(SignatureSubpacketTag.RevocationReason, isCritical, CreateData(reason, description))
		{
		}

		private static byte[] CreateData(RevocationReasonTag reason, string description)
		{
			byte[] array = Strings.ToUtf8ByteArray(description);
			byte[] array2 = new byte[1 + array.Length];
			array2[0] = (byte)reason;
			Array.Copy(array, 0, array2, 1, array.Length);
			return array2;
		}

		public virtual RevocationReasonTag GetRevocationReason()
		{
			return (RevocationReasonTag)GetData()[0];
		}

		public virtual string GetRevocationDescription()
		{
			byte[] array = GetData();
			if (array.Length == 1)
			{
				return string.Empty;
			}
			byte[] array2 = new byte[array.Length - 1];
			Array.Copy(array, 1, array2, 0, array2.Length);
			return Strings.FromUtf8ByteArray(array2);
		}
	}
}
