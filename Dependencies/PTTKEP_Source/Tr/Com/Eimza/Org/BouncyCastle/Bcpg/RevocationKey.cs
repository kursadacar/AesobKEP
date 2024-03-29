using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg
{
	internal class RevocationKey : SignatureSubpacket
	{
		public virtual RevocationKeyTag SignatureClass
		{
			get
			{
				return (RevocationKeyTag)GetData()[0];
			}
		}

		public virtual PublicKeyAlgorithmTag Algorithm
		{
			get
			{
				return (PublicKeyAlgorithmTag)GetData()[1];
			}
		}

		public RevocationKey(bool isCritical, byte[] data)
			: base(SignatureSubpacketTag.RevocationKey, isCritical, data)
		{
		}

		public RevocationKey(bool isCritical, RevocationKeyTag signatureClass, PublicKeyAlgorithmTag keyAlgorithm, byte[] fingerprint)
			: base(SignatureSubpacketTag.RevocationKey, isCritical, CreateData(signatureClass, keyAlgorithm, fingerprint))
		{
		}

		private static byte[] CreateData(RevocationKeyTag signatureClass, PublicKeyAlgorithmTag keyAlgorithm, byte[] fingerprint)
		{
			byte[] array = new byte[2 + fingerprint.Length];
			array[0] = (byte)signatureClass;
			array[1] = (byte)keyAlgorithm;
			Array.Copy(fingerprint, 0, array, 2, fingerprint.Length);
			return array;
		}

		public virtual byte[] GetFingerprint()
		{
			byte[] array = GetData();
			byte[] array2 = new byte[array.Length - 2];
			Array.Copy(array, 2, array2, 0, array2.Length);
			return array2;
		}
	}
}
