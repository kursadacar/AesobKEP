using System;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	internal class PgpPbeEncryptedData : PgpEncryptedData
	{
		private readonly SymmetricKeyEncSessionPacket keyData;

		internal PgpPbeEncryptedData(SymmetricKeyEncSessionPacket keyData, InputStreamPacket encData)
			: base(encData)
		{
			this.keyData = keyData;
		}

		public override Stream GetInputStream()
		{
			return encData.GetInputStream();
		}

		public Stream GetDataStream(char[] passPhrase)
		{
			try
			{
				SymmetricKeyAlgorithmTag symmetricKeyAlgorithmTag = keyData.EncAlgorithm;
				KeyParameter parameters = PgpUtilities.MakeKeyFromPassPhrase(symmetricKeyAlgorithmTag, keyData.S2k, passPhrase);
				byte[] secKeyData = keyData.GetSecKeyData();
				if (secKeyData != null && secKeyData.Length != 0)
				{
					IBufferedCipher cipher = CipherUtilities.GetCipher(PgpUtilities.GetSymmetricCipherName(symmetricKeyAlgorithmTag) + "/CFB/NoPadding");
					cipher.Init(false, new ParametersWithIV(parameters, new byte[cipher.GetBlockSize()]));
					byte[] array = cipher.DoFinal(secKeyData);
					symmetricKeyAlgorithmTag = (SymmetricKeyAlgorithmTag)array[0];
					parameters = ParameterUtilities.CreateKeyParameter(PgpUtilities.GetSymmetricCipherName(symmetricKeyAlgorithmTag), array, 1, array.Length - 1);
				}
				IBufferedCipher bufferedCipher = CreateStreamCipher(symmetricKeyAlgorithmTag);
				byte[] array2 = new byte[bufferedCipher.GetBlockSize()];
				bufferedCipher.Init(false, new ParametersWithIV(parameters, array2));
				encStream = BcpgInputStream.Wrap(new CipherStream(encData.GetInputStream(), bufferedCipher, null));
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
				bool num3 = array2[array2.Length - 2] == (byte)num && array2[array2.Length - 1] == (byte)num2;
				bool flag = num == 0 && num2 == 0;
				if (!num3 && !flag)
				{
					throw new PgpDataValidationException("quick check failed.");
				}
				return encStream;
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

		private IBufferedCipher CreateStreamCipher(SymmetricKeyAlgorithmTag keyAlgorithm)
		{
			string text = ((encData is SymmetricEncIntegrityPacket) ? "CFB" : "OpenPGPCFB");
			return CipherUtilities.GetCipher(PgpUtilities.GetSymmetricCipherName(keyAlgorithm) + "/" + text + "/NoPadding");
		}
	}
}
