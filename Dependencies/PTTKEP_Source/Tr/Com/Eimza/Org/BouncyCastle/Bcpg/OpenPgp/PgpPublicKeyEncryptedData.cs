using System;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	internal class PgpPublicKeyEncryptedData : PgpEncryptedData
	{
		private PublicKeyEncSessionPacket keyData;

		public long KeyId
		{
			get
			{
				return keyData.KeyId;
			}
		}

		internal PgpPublicKeyEncryptedData(PublicKeyEncSessionPacket keyData, InputStreamPacket encData)
			: base(encData)
		{
			this.keyData = keyData;
		}

		private static IBufferedCipher GetKeyCipher(PublicKeyAlgorithmTag algorithm)
		{
			try
			{
				switch (algorithm)
				{
				case PublicKeyAlgorithmTag.RsaGeneral:
				case PublicKeyAlgorithmTag.RsaEncrypt:
					return CipherUtilities.GetCipher("RSA//PKCS1Padding");
				case PublicKeyAlgorithmTag.ElGamalEncrypt:
				case PublicKeyAlgorithmTag.ElGamalGeneral:
					return CipherUtilities.GetCipher("ElGamal/ECB/PKCS1Padding");
				default:
					throw new PgpException("unknown asymmetric algorithm: " + algorithm);
				}
			}
			catch (PgpException ex)
			{
				throw ex;
			}
			catch (Exception exception)
			{
				throw new PgpException("Exception creating cipher", exception);
			}
		}

		private bool ConfirmCheckSum(byte[] sessionInfo)
		{
			int num = 0;
			for (int i = 1; i != sessionInfo.Length - 2; i++)
			{
				num += sessionInfo[i] & 0xFF;
			}
			if (sessionInfo[sessionInfo.Length - 2] == (byte)(num >> 8))
			{
				return sessionInfo[sessionInfo.Length - 1] == (byte)num;
			}
			return false;
		}

		public SymmetricKeyAlgorithmTag GetSymmetricAlgorithm(PgpPrivateKey privKey)
		{
			return (SymmetricKeyAlgorithmTag)fetchSymmetricKeyData(privKey)[0];
		}

		public Stream GetDataStream(PgpPrivateKey privKey)
		{
			byte[] array = fetchSymmetricKeyData(privKey);
			string symmetricCipherName = PgpUtilities.GetSymmetricCipherName((SymmetricKeyAlgorithmTag)array[0]);
			string text = symmetricCipherName;
			IBufferedCipher cipher;
			try
			{
				text = ((!(encData is SymmetricEncIntegrityPacket)) ? (text + "/OpenPGPCFB/NoPadding") : (text + "/CFB/NoPadding"));
				cipher = CipherUtilities.GetCipher(text);
			}
			catch (PgpException ex)
			{
				throw ex;
			}
			catch (Exception exception)
			{
				throw new PgpException("exception creating cipher", exception);
			}
			if (cipher == null)
			{
				return encData.GetInputStream();
			}
			try
			{
				KeyParameter parameters = ParameterUtilities.CreateKeyParameter(symmetricCipherName, array, 1, array.Length - 3);
				byte[] array2 = new byte[cipher.GetBlockSize()];
				cipher.Init(false, new ParametersWithIV(parameters, array2));
				encStream = BcpgInputStream.Wrap(new CipherStream(encData.GetInputStream(), cipher, null));
				if (encData is SymmetricEncIntegrityPacket)
				{
					truncStream = new TruncatedStream(encStream);
					IDigest digest = DigestUtilities.GetDigest(PgpUtilities.GetDigestName(HashAlgorithmTag.Sha1));
					encStream = new DigestStream(truncStream, digest, null);
				}
				if (Streams.ReadFully(encStream, array2, 0, array2.Length) < array2.Length)
				{
					throw new EndOfStreamException("unexpected end of stream.");
				}
				int num = encStream.ReadByte();
				int num2 = encStream.ReadByte();
				if (num < 0 || num2 < 0)
				{
					throw new EndOfStreamException("unexpected end of stream.");
				}
				return encStream;
			}
			catch (PgpException ex2)
			{
				throw ex2;
			}
			catch (Exception exception2)
			{
				throw new PgpException("Exception starting decryption", exception2);
			}
		}

		private byte[] fetchSymmetricKeyData(PgpPrivateKey privKey)
		{
			IBufferedCipher keyCipher = GetKeyCipher(keyData.Algorithm);
			try
			{
				keyCipher.Init(false, privKey.Key);
			}
			catch (InvalidKeyException exception)
			{
				throw new PgpException("error setting asymmetric cipher", exception);
			}
			BigInteger[] encSessionKey = keyData.GetEncSessionKey();
			if (keyData.Algorithm == PublicKeyAlgorithmTag.RsaEncrypt || keyData.Algorithm == PublicKeyAlgorithmTag.RsaGeneral)
			{
				keyCipher.ProcessBytes(encSessionKey[0].ToByteArrayUnsigned());
			}
			else
			{
				int num = (((ElGamalPrivateKeyParameters)privKey.Key).Parameters.P.BitLength + 7) / 8;
				byte[] array = encSessionKey[0].ToByteArray();
				int num2 = array.Length - num;
				if (num2 >= 0)
				{
					keyCipher.ProcessBytes(array, num2, num);
				}
				else
				{
					byte[] input = new byte[-num2];
					keyCipher.ProcessBytes(input);
					keyCipher.ProcessBytes(array);
				}
				array = encSessionKey[1].ToByteArray();
				num2 = array.Length - num;
				if (num2 >= 0)
				{
					keyCipher.ProcessBytes(array, num2, num);
				}
				else
				{
					byte[] input2 = new byte[-num2];
					keyCipher.ProcessBytes(input2);
					keyCipher.ProcessBytes(array);
				}
			}
			byte[] array2;
			try
			{
				array2 = keyCipher.DoFinal();
			}
			catch (Exception exception2)
			{
				throw new PgpException("exception decrypting secret key", exception2);
			}
			if (!ConfirmCheckSum(array2))
			{
				throw new PgpKeyValidationException("key checksum failed");
			}
			return array2;
		}
	}
}
