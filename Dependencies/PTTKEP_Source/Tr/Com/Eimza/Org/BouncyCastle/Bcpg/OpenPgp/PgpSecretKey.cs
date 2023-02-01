using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	internal class PgpSecretKey
	{
		private readonly SecretKeyPacket secret;

		private readonly PgpPublicKey pub;

		public bool IsSigningKey
		{
			get
			{
				switch (pub.Algorithm)
				{
				case PublicKeyAlgorithmTag.RsaGeneral:
				case PublicKeyAlgorithmTag.RsaSign:
				case PublicKeyAlgorithmTag.Dsa:
				case PublicKeyAlgorithmTag.ECDsa:
				case PublicKeyAlgorithmTag.ElGamalGeneral:
					return true;
				default:
					return false;
				}
			}
		}

		public bool IsMasterKey
		{
			get
			{
				return pub.IsMasterKey;
			}
		}

		public bool IsPrivateKeyEmpty
		{
			get
			{
				byte[] secretKeyData = secret.GetSecretKeyData();
				if (secretKeyData != null)
				{
					return secretKeyData.Length < 1;
				}
				return true;
			}
		}

		public SymmetricKeyAlgorithmTag KeyEncryptionAlgorithm
		{
			get
			{
				return secret.EncAlgorithm;
			}
		}

		public long KeyId
		{
			get
			{
				return pub.KeyId;
			}
		}

		public PgpPublicKey PublicKey
		{
			get
			{
				return pub;
			}
		}

		public IEnumerable UserIds
		{
			get
			{
				return pub.GetUserIds();
			}
		}

		public IEnumerable UserAttributes
		{
			get
			{
				return pub.GetUserAttributes();
			}
		}

		internal PgpSecretKey(SecretKeyPacket secret, PgpPublicKey pub)
		{
			this.secret = secret;
			this.pub = pub;
		}

		internal PgpSecretKey(PgpPrivateKey privKey, PgpPublicKey pubKey, SymmetricKeyAlgorithmTag encAlgorithm, char[] passPhrase, bool useSha1, SecureRandom rand)
			: this(privKey, pubKey, encAlgorithm, passPhrase, useSha1, rand, false)
		{
		}

		internal PgpSecretKey(PgpPrivateKey privKey, PgpPublicKey pubKey, SymmetricKeyAlgorithmTag encAlgorithm, char[] passPhrase, bool useSha1, SecureRandom rand, bool isMasterKey)
		{
			pub = pubKey;
			BcpgObject bcpgObject;
			switch (pubKey.Algorithm)
			{
			case PublicKeyAlgorithmTag.RsaGeneral:
			case PublicKeyAlgorithmTag.RsaEncrypt:
			case PublicKeyAlgorithmTag.RsaSign:
			{
				RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters = (RsaPrivateCrtKeyParameters)privKey.Key;
				bcpgObject = new RsaSecretBcpgKey(rsaPrivateCrtKeyParameters.Exponent, rsaPrivateCrtKeyParameters.P, rsaPrivateCrtKeyParameters.Q);
				break;
			}
			case PublicKeyAlgorithmTag.Dsa:
				bcpgObject = new DsaSecretBcpgKey(((DsaPrivateKeyParameters)privKey.Key).X);
				break;
			case PublicKeyAlgorithmTag.ElGamalEncrypt:
			case PublicKeyAlgorithmTag.ElGamalGeneral:
				bcpgObject = new ElGamalSecretBcpgKey(((ElGamalPrivateKeyParameters)privKey.Key).X);
				break;
			default:
				throw new PgpException("unknown key class");
			}
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				new BcpgOutputStream(memoryStream).WriteObject(bcpgObject);
				byte[] array = memoryStream.ToArray();
				byte[] b = Checksum(useSha1, array, array.Length);
				array = Arrays.Concatenate(array, b);
				if (encAlgorithm == SymmetricKeyAlgorithmTag.Null)
				{
					if (isMasterKey)
					{
						secret = new SecretKeyPacket(pub.publicPk, encAlgorithm, null, null, array);
					}
					else
					{
						secret = new SecretSubkeyPacket(pub.publicPk, encAlgorithm, null, null, array);
					}
					return;
				}
				if (pub.Version >= 4)
				{
					S2k s2k;
					byte[] iv;
					byte[] secKeyData = EncryptKeyData(array, encAlgorithm, passPhrase, rand, out s2k, out iv);
					int s2kUsage = (useSha1 ? 254 : 255);
					if (isMasterKey)
					{
						secret = new SecretKeyPacket(pub.publicPk, encAlgorithm, s2kUsage, s2k, iv, secKeyData);
					}
					else
					{
						secret = new SecretSubkeyPacket(pub.publicPk, encAlgorithm, s2kUsage, s2k, iv, secKeyData);
					}
					return;
				}
				throw Platform.CreateNotImplementedException("v3 RSA");
			}
			catch (PgpException ex)
			{
				throw ex;
			}
			catch (Exception exception)
			{
				throw new PgpException("Exception encrypting key", exception);
			}
		}

		public PgpSecretKey(int certificationLevel, PgpKeyPair keyPair, string id, SymmetricKeyAlgorithmTag encAlgorithm, char[] passPhrase, PgpSignatureSubpacketVector hashedPackets, PgpSignatureSubpacketVector unhashedPackets, SecureRandom rand)
			: this(certificationLevel, keyPair, id, encAlgorithm, passPhrase, false, hashedPackets, unhashedPackets, rand)
		{
		}

		public PgpSecretKey(int certificationLevel, PgpKeyPair keyPair, string id, SymmetricKeyAlgorithmTag encAlgorithm, char[] passPhrase, bool useSha1, PgpSignatureSubpacketVector hashedPackets, PgpSignatureSubpacketVector unhashedPackets, SecureRandom rand)
			: this(keyPair.PrivateKey, CertifiedPublicKey(certificationLevel, keyPair, id, hashedPackets, unhashedPackets), encAlgorithm, passPhrase, useSha1, rand, true)
		{
		}

		private static PgpPublicKey CertifiedPublicKey(int certificationLevel, PgpKeyPair keyPair, string id, PgpSignatureSubpacketVector hashedPackets, PgpSignatureSubpacketVector unhashedPackets)
		{
			PgpSignatureGenerator pgpSignatureGenerator;
			try
			{
				pgpSignatureGenerator = new PgpSignatureGenerator(keyPair.PublicKey.Algorithm, HashAlgorithmTag.Sha1);
			}
			catch (Exception ex)
			{
				throw new PgpException("Creating signature generator: " + ex.Message, ex);
			}
			pgpSignatureGenerator.InitSign(certificationLevel, keyPair.PrivateKey);
			pgpSignatureGenerator.SetHashedSubpackets(hashedPackets);
			pgpSignatureGenerator.SetUnhashedSubpackets(unhashedPackets);
			try
			{
				PgpSignature certification = pgpSignatureGenerator.GenerateCertification(id, keyPair.PublicKey);
				return PgpPublicKey.AddCertification(keyPair.PublicKey, id, certification);
			}
			catch (Exception ex2)
			{
				throw new PgpException("Exception doing certification: " + ex2.Message, ex2);
			}
		}

		public PgpSecretKey(int certificationLevel, PublicKeyAlgorithmTag algorithm, AsymmetricKeyParameter pubKey, AsymmetricKeyParameter privKey, DateTime time, string id, SymmetricKeyAlgorithmTag encAlgorithm, char[] passPhrase, PgpSignatureSubpacketVector hashedPackets, PgpSignatureSubpacketVector unhashedPackets, SecureRandom rand)
			: this(certificationLevel, new PgpKeyPair(algorithm, pubKey, privKey, time), id, encAlgorithm, passPhrase, hashedPackets, unhashedPackets, rand)
		{
		}

		public PgpSecretKey(int certificationLevel, PublicKeyAlgorithmTag algorithm, AsymmetricKeyParameter pubKey, AsymmetricKeyParameter privKey, DateTime time, string id, SymmetricKeyAlgorithmTag encAlgorithm, char[] passPhrase, bool useSha1, PgpSignatureSubpacketVector hashedPackets, PgpSignatureSubpacketVector unhashedPackets, SecureRandom rand)
			: this(certificationLevel, new PgpKeyPair(algorithm, pubKey, privKey, time), id, encAlgorithm, passPhrase, useSha1, hashedPackets, unhashedPackets, rand)
		{
		}

		private byte[] ExtractKeyData(char[] passPhrase)
		{
			SymmetricKeyAlgorithmTag encAlgorithm = secret.EncAlgorithm;
			byte[] secretKeyData = secret.GetSecretKeyData();
			if (encAlgorithm == SymmetricKeyAlgorithmTag.Null)
			{
				return secretKeyData;
			}
			IBufferedCipher bufferedCipher = null;
			try
			{
				bufferedCipher = CipherUtilities.GetCipher(PgpUtilities.GetSymmetricCipherName(encAlgorithm) + "/CFB/NoPadding");
			}
			catch (Exception exception)
			{
				throw new PgpException("Exception creating cipher", exception);
			}
			try
			{
				KeyParameter parameters = PgpUtilities.MakeKeyFromPassPhrase(secret.EncAlgorithm, secret.S2k, passPhrase);
				byte[] iV = secret.GetIV();
				byte[] array;
				if (secret.PublicKeyPacket.Version >= 4)
				{
					bufferedCipher.Init(false, new ParametersWithIV(parameters, iV));
					array = bufferedCipher.DoFinal(secretKeyData);
					bool flag = secret.S2kUsage == 254;
					byte[] array2 = Checksum(flag, array, flag ? (array.Length - 20) : (array.Length - 2));
					for (int i = 0; i != array2.Length; i++)
					{
						if (array2[i] != array[array.Length - array2.Length + i])
						{
							throw new PgpException("Checksum mismatch at " + i + " of " + array2.Length);
						}
					}
				}
				else
				{
					array = new byte[secretKeyData.Length];
					iV = Arrays.Clone(iV);
					int num = 0;
					for (int j = 0; j != 4; j++)
					{
						bufferedCipher.Init(false, new ParametersWithIV(parameters, iV));
						int num2 = (((secretKeyData[num] << 8) | (secretKeyData[num + 1] & 0xFF)) + 7) / 8;
						array[num] = secretKeyData[num];
						array[num + 1] = secretKeyData[num + 1];
						num += 2;
						bufferedCipher.DoFinal(secretKeyData, num, num2, array, num);
						num += num2;
						if (j != 3)
						{
							Array.Copy(secretKeyData, num - iV.Length, iV, 0, iV.Length);
						}
					}
					array[num] = secretKeyData[num];
					array[num + 1] = secretKeyData[num + 1];
					int num3 = ((secretKeyData[num] << 8) & 0xFF00) | (secretKeyData[num + 1] & 0xFF);
					int num4 = 0;
					for (int k = 0; k < num; k++)
					{
						num4 += array[k] & 0xFF;
					}
					num4 &= 0xFFFF;
					if (num4 != num3)
					{
						throw new PgpException("Checksum mismatch: passphrase wrong, expected " + num3.ToString("X") + " found " + num4.ToString("X"));
					}
				}
				return array;
			}
			catch (PgpException ex)
			{
				throw ex;
			}
			catch (Exception exception2)
			{
				throw new PgpException("Exception decrypting key", exception2);
			}
		}

		public PgpPrivateKey ExtractPrivateKey(char[] passPhrase)
		{
			if (IsPrivateKeyEmpty)
			{
				return null;
			}
			PublicKeyPacket publicKeyPacket = secret.PublicKeyPacket;
			try
			{
				BcpgInputStream bcpgIn = BcpgInputStream.Wrap(new MemoryStream(ExtractKeyData(passPhrase), false));
				AsymmetricKeyParameter privateKey;
				switch (publicKeyPacket.Algorithm)
				{
				case PublicKeyAlgorithmTag.RsaGeneral:
				case PublicKeyAlgorithmTag.RsaEncrypt:
				case PublicKeyAlgorithmTag.RsaSign:
				{
					RsaPublicBcpgKey rsaPublicBcpgKey = (RsaPublicBcpgKey)publicKeyPacket.Key;
					RsaSecretBcpgKey rsaSecretBcpgKey = new RsaSecretBcpgKey(bcpgIn);
					privateKey = new RsaPrivateCrtKeyParameters(rsaSecretBcpgKey.Modulus, rsaPublicBcpgKey.PublicExponent, rsaSecretBcpgKey.PrivateExponent, rsaSecretBcpgKey.PrimeP, rsaSecretBcpgKey.PrimeQ, rsaSecretBcpgKey.PrimeExponentP, rsaSecretBcpgKey.PrimeExponentQ, rsaSecretBcpgKey.CrtCoefficient);
					break;
				}
				case PublicKeyAlgorithmTag.Dsa:
				{
					DsaPublicBcpgKey dsaPublicBcpgKey = (DsaPublicBcpgKey)publicKeyPacket.Key;
					DsaSecretBcpgKey dsaSecretBcpgKey = new DsaSecretBcpgKey(bcpgIn);
					privateKey = new DsaPrivateKeyParameters(parameters: new DsaParameters(dsaPublicBcpgKey.P, dsaPublicBcpgKey.Q, dsaPublicBcpgKey.G), x: dsaSecretBcpgKey.X);
					break;
				}
				case PublicKeyAlgorithmTag.ElGamalEncrypt:
				case PublicKeyAlgorithmTag.ElGamalGeneral:
				{
					ElGamalPublicBcpgKey elGamalPublicBcpgKey = (ElGamalPublicBcpgKey)publicKeyPacket.Key;
					ElGamalSecretBcpgKey elGamalSecretBcpgKey = new ElGamalSecretBcpgKey(bcpgIn);
					privateKey = new ElGamalPrivateKeyParameters(parameters: new ElGamalParameters(elGamalPublicBcpgKey.P, elGamalPublicBcpgKey.G), x: elGamalSecretBcpgKey.X);
					break;
				}
				default:
					throw new PgpException("unknown public key algorithm encountered");
				}
				return new PgpPrivateKey(privateKey, KeyId);
			}
			catch (PgpException ex)
			{
				throw ex;
			}
			catch (Exception exception)
			{
				throw new PgpException("Exception constructing key", exception);
			}
		}

		private static byte[] Checksum(bool useSha1, byte[] bytes, int length)
		{
			if (useSha1)
			{
				try
				{
					IDigest digest = DigestUtilities.GetDigest("SHA1");
					digest.BlockUpdate(bytes, 0, length);
					return DigestUtilities.DoFinal(digest);
				}
				catch (Exception exception)
				{
					throw new PgpException("Can't find SHA-1", exception);
				}
			}
			int num = 0;
			for (int i = 0; i != length; i++)
			{
				num += bytes[i];
			}
			return new byte[2]
			{
				(byte)(num >> 8),
				(byte)num
			};
		}

		public byte[] GetEncoded()
		{
			MemoryStream memoryStream = new MemoryStream();
			Encode(memoryStream);
			return memoryStream.ToArray();
		}

		public void Encode(Stream outStr)
		{
			BcpgOutputStream bcpgOutputStream = BcpgOutputStream.Wrap(outStr);
			bcpgOutputStream.WritePacket(secret);
			if (pub.trustPk != null)
			{
				bcpgOutputStream.WritePacket(pub.trustPk);
			}
			if (pub.subSigs == null)
			{
				foreach (PgpSignature keySig in pub.keySigs)
				{
					keySig.Encode(bcpgOutputStream);
				}
				for (int i = 0; i != pub.ids.Count; i++)
				{
					object obj = pub.ids[i];
					if (obj is string)
					{
						string id = (string)obj;
						bcpgOutputStream.WritePacket(new UserIdPacket(id));
					}
					else
					{
						PgpUserAttributeSubpacketVector pgpUserAttributeSubpacketVector = (PgpUserAttributeSubpacketVector)obj;
						bcpgOutputStream.WritePacket(new UserAttributePacket(pgpUserAttributeSubpacketVector.ToSubpacketArray()));
					}
					if (pub.idTrusts[i] != null)
					{
						bcpgOutputStream.WritePacket((ContainedPacket)pub.idTrusts[i]);
					}
					foreach (PgpSignature item in (IList)pub.idSigs[i])
					{
						item.Encode(bcpgOutputStream);
					}
				}
				return;
			}
			foreach (PgpSignature subSig in pub.subSigs)
			{
				subSig.Encode(bcpgOutputStream);
			}
		}

		public static PgpSecretKey CopyWithNewPassword(PgpSecretKey key, char[] oldPassPhrase, char[] newPassPhrase, SymmetricKeyAlgorithmTag newEncAlgorithm, SecureRandom rand)
		{
			if (key.IsPrivateKeyEmpty)
			{
				throw new PgpException("no private key in this SecretKey - public key present only.");
			}
			byte[] array = key.ExtractKeyData(oldPassPhrase);
			int s2kUsage = key.secret.S2kUsage;
			byte[] iv = null;
			S2k s2k = null;
			PublicKeyPacket publicKeyPacket = key.secret.PublicKeyPacket;
			byte[] array2;
			if (newEncAlgorithm == SymmetricKeyAlgorithmTag.Null)
			{
				s2kUsage = 0;
				if (key.secret.S2kUsage == 254)
				{
					array2 = new byte[array.Length - 18];
					Array.Copy(array, 0, array2, 0, array2.Length - 2);
					byte[] array3 = Checksum(false, array2, array2.Length - 2);
					array2[array2.Length - 2] = array3[0];
					array2[array2.Length - 1] = array3[1];
				}
				else
				{
					array2 = array;
				}
			}
			else
			{
				try
				{
					if (publicKeyPacket.Version < 4)
					{
						throw Platform.CreateNotImplementedException("v3 RSA");
					}
					array2 = EncryptKeyData(array, newEncAlgorithm, newPassPhrase, rand, out s2k, out iv);
				}
				catch (PgpException ex)
				{
					throw ex;
				}
				catch (Exception exception)
				{
					throw new PgpException("Exception encrypting key", exception);
				}
			}
			SecretKeyPacket secretKeyPacket = ((!(key.secret is SecretSubkeyPacket)) ? new SecretKeyPacket(publicKeyPacket, newEncAlgorithm, s2kUsage, s2k, iv, array2) : new SecretSubkeyPacket(publicKeyPacket, newEncAlgorithm, s2kUsage, s2k, iv, array2));
			return new PgpSecretKey(secretKeyPacket, key.pub);
		}

		public static PgpSecretKey ReplacePublicKey(PgpSecretKey secretKey, PgpPublicKey publicKey)
		{
			if (publicKey.KeyId != secretKey.KeyId)
			{
				throw new ArgumentException("KeyId's do not match");
			}
			return new PgpSecretKey(secretKey.secret, publicKey);
		}

		private static byte[] EncryptKeyData(byte[] rawKeyData, SymmetricKeyAlgorithmTag encAlgorithm, char[] passPhrase, SecureRandom random, out S2k s2k, out byte[] iv)
		{
			IBufferedCipher cipher;
			try
			{
				cipher = CipherUtilities.GetCipher(PgpUtilities.GetSymmetricCipherName(encAlgorithm) + "/CFB/NoPadding");
			}
			catch (Exception exception)
			{
				throw new PgpException("Exception creating cipher", exception);
			}
			byte[] array = new byte[8];
			random.NextBytes(array);
			s2k = new S2k(HashAlgorithmTag.Sha1, array, 96);
			KeyParameter parameters = PgpUtilities.MakeKeyFromPassPhrase(encAlgorithm, s2k, passPhrase);
			iv = new byte[cipher.GetBlockSize()];
			random.NextBytes(iv);
			cipher.Init(true, new ParametersWithRandom(new ParametersWithIV(parameters, iv), random));
			return cipher.DoFinal(rawKeyData);
		}
	}
}
