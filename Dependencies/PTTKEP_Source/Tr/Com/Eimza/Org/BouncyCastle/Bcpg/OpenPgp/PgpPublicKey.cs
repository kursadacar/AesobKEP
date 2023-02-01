using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Collections;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	internal class PgpPublicKey
	{
		private static readonly int[] MasterKeyCertificationTypes = new int[4] { 19, 18, 17, 16 };

		private long keyId;

		private byte[] fingerprint;

		private int keyStrength;

		internal PublicKeyPacket publicPk;

		internal TrustPacket trustPk;

		internal IList keySigs = Platform.CreateArrayList();

		internal IList ids = Platform.CreateArrayList();

		internal IList idTrusts = Platform.CreateArrayList();

		internal IList idSigs = Platform.CreateArrayList();

		internal IList subSigs;

		public int Version
		{
			get
			{
				return publicPk.Version;
			}
		}

		public DateTime CreationTime
		{
			get
			{
				return publicPk.GetTime();
			}
		}

		public int ValidDays
		{
			get
			{
				if (publicPk.Version > 3)
				{
					return (int)(GetValidSeconds() / 86400);
				}
				return publicPk.ValidDays;
			}
		}

		public long KeyId
		{
			get
			{
				return keyId;
			}
		}

		public bool IsEncryptionKey
		{
			get
			{
				PublicKeyAlgorithmTag algorithm = publicPk.Algorithm;
				if ((uint)(algorithm - 1) <= 1u || algorithm == PublicKeyAlgorithmTag.ElGamalEncrypt || algorithm == PublicKeyAlgorithmTag.ElGamalGeneral)
				{
					return true;
				}
				return false;
			}
		}

		public bool IsMasterKey
		{
			get
			{
				return subSigs == null;
			}
		}

		public PublicKeyAlgorithmTag Algorithm
		{
			get
			{
				return publicPk.Algorithm;
			}
		}

		public int BitStrength
		{
			get
			{
				return keyStrength;
			}
		}

		private void Init()
		{
			IBcpgKey key = publicPk.Key;
			if (publicPk.Version <= 3)
			{
				RsaPublicBcpgKey rsaPublicBcpgKey = (RsaPublicBcpgKey)key;
				keyId = rsaPublicBcpgKey.Modulus.LongValue;
				try
				{
					IDigest digest = DigestUtilities.GetDigest("MD5");
					byte[] array = rsaPublicBcpgKey.Modulus.ToByteArrayUnsigned();
					digest.BlockUpdate(array, 0, array.Length);
					array = rsaPublicBcpgKey.PublicExponent.ToByteArrayUnsigned();
					digest.BlockUpdate(array, 0, array.Length);
					fingerprint = DigestUtilities.DoFinal(digest);
				}
				catch (Exception innerException)
				{
					throw new IOException("can't find MD5", innerException);
				}
				keyStrength = rsaPublicBcpgKey.Modulus.BitLength;
				return;
			}
			byte[] encodedContents = publicPk.GetEncodedContents();
			try
			{
				IDigest digest2 = DigestUtilities.GetDigest("SHA1");
				digest2.Update(153);
				digest2.Update((byte)(encodedContents.Length >> 8));
				digest2.Update((byte)encodedContents.Length);
				digest2.BlockUpdate(encodedContents, 0, encodedContents.Length);
				fingerprint = DigestUtilities.DoFinal(digest2);
			}
			catch (Exception innerException2)
			{
				throw new IOException("can't find SHA1", innerException2);
			}
			keyId = (long)(((ulong)fingerprint[fingerprint.Length - 8] << 56) | ((ulong)fingerprint[fingerprint.Length - 7] << 48) | ((ulong)fingerprint[fingerprint.Length - 6] << 40) | ((ulong)fingerprint[fingerprint.Length - 5] << 32) | ((ulong)fingerprint[fingerprint.Length - 4] << 24) | ((ulong)fingerprint[fingerprint.Length - 3] << 16) | ((ulong)fingerprint[fingerprint.Length - 2] << 8) | fingerprint[fingerprint.Length - 1]);
			if (key is RsaPublicBcpgKey)
			{
				keyStrength = ((RsaPublicBcpgKey)key).Modulus.BitLength;
			}
			else if (key is DsaPublicBcpgKey)
			{
				keyStrength = ((DsaPublicBcpgKey)key).P.BitLength;
			}
			else if (key is ElGamalPublicBcpgKey)
			{
				keyStrength = ((ElGamalPublicBcpgKey)key).P.BitLength;
			}
		}

		public PgpPublicKey(PublicKeyAlgorithmTag algorithm, AsymmetricKeyParameter pubKey, DateTime time)
		{
			if (pubKey.IsPrivate)
			{
				throw new ArgumentException("Expected a public key", "pubKey");
			}
			IBcpgKey key;
			if (pubKey is RsaKeyParameters)
			{
				RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)pubKey;
				key = new RsaPublicBcpgKey(rsaKeyParameters.Modulus, rsaKeyParameters.Exponent);
			}
			else if (pubKey is DsaPublicKeyParameters)
			{
				DsaPublicKeyParameters dsaPublicKeyParameters = (DsaPublicKeyParameters)pubKey;
				DsaParameters parameters = dsaPublicKeyParameters.Parameters;
				key = new DsaPublicBcpgKey(parameters.P, parameters.Q, parameters.G, dsaPublicKeyParameters.Y);
			}
			else
			{
				if (!(pubKey is ElGamalPublicKeyParameters))
				{
					throw new PgpException("unknown key class");
				}
				ElGamalPublicKeyParameters elGamalPublicKeyParameters = (ElGamalPublicKeyParameters)pubKey;
				ElGamalParameters parameters2 = elGamalPublicKeyParameters.Parameters;
				key = new ElGamalPublicBcpgKey(parameters2.P, parameters2.G, elGamalPublicKeyParameters.Y);
			}
			publicPk = new PublicKeyPacket(algorithm, time, key);
			ids = Platform.CreateArrayList();
			idSigs = Platform.CreateArrayList();
			try
			{
				Init();
			}
			catch (IOException exception)
			{
				throw new PgpException("exception calculating keyId", exception);
			}
		}

		internal PgpPublicKey(PublicKeyPacket publicPk, TrustPacket trustPk, IList sigs)
		{
			this.publicPk = publicPk;
			this.trustPk = trustPk;
			subSigs = sigs;
			Init();
		}

		internal PgpPublicKey(PgpPublicKey key, TrustPacket trust, IList subSigs)
		{
			publicPk = key.publicPk;
			trustPk = trust;
			this.subSigs = subSigs;
			fingerprint = key.fingerprint;
			keyId = key.keyId;
			keyStrength = key.keyStrength;
		}

		internal PgpPublicKey(PgpPublicKey pubKey)
		{
			publicPk = pubKey.publicPk;
			keySigs = Platform.CreateArrayList(pubKey.keySigs);
			ids = Platform.CreateArrayList(pubKey.ids);
			idTrusts = Platform.CreateArrayList(pubKey.idTrusts);
			idSigs = Platform.CreateArrayList(pubKey.idSigs.Count);
			for (int i = 0; i != pubKey.idSigs.Count; i++)
			{
				idSigs.Add(Platform.CreateArrayList((IList)pubKey.idSigs[i]));
			}
			if (pubKey.subSigs != null)
			{
				subSigs = Platform.CreateArrayList(pubKey.subSigs.Count);
				for (int j = 0; j != pubKey.subSigs.Count; j++)
				{
					subSigs.Add(pubKey.subSigs[j]);
				}
			}
			fingerprint = pubKey.fingerprint;
			keyId = pubKey.keyId;
			keyStrength = pubKey.keyStrength;
		}

		internal PgpPublicKey(PublicKeyPacket publicPk, TrustPacket trustPk, IList keySigs, IList ids, IList idTrusts, IList idSigs)
		{
			this.publicPk = publicPk;
			this.trustPk = trustPk;
			this.keySigs = keySigs;
			this.ids = ids;
			this.idTrusts = idTrusts;
			this.idSigs = idSigs;
			Init();
		}

		internal PgpPublicKey(PublicKeyPacket publicPk, IList ids, IList idSigs)
		{
			this.publicPk = publicPk;
			this.ids = ids;
			this.idSigs = idSigs;
			Init();
		}

		public byte[] GetTrustData()
		{
			if (trustPk == null)
			{
				return null;
			}
			return trustPk.GetLevelAndTrustAmount();
		}

		public long GetValidSeconds()
		{
			if (publicPk.Version > 3)
			{
				if (IsMasterKey)
				{
					for (int i = 0; i != MasterKeyCertificationTypes.Length; i++)
					{
						long expirationTimeFromSig = GetExpirationTimeFromSig(true, MasterKeyCertificationTypes[i]);
						if (expirationTimeFromSig >= 0)
						{
							return expirationTimeFromSig;
						}
					}
				}
				else
				{
					long expirationTimeFromSig2 = GetExpirationTimeFromSig(false, 24);
					if (expirationTimeFromSig2 >= 0)
					{
						return expirationTimeFromSig2;
					}
				}
				return 0L;
			}
			return (long)publicPk.ValidDays * 24L * 60 * 60;
		}

		private long GetExpirationTimeFromSig(bool selfSigned, int signatureType)
		{
			foreach (PgpSignature item in GetSignaturesOfType(signatureType))
			{
				if (!selfSigned || item.KeyId == KeyId)
				{
					PgpSignatureSubpacketVector hashedSubPackets = item.GetHashedSubPackets();
					if (hashedSubPackets != null)
					{
						return hashedSubPackets.GetKeyExpirationTime();
					}
					return 0L;
				}
			}
			return -1L;
		}

		public byte[] GetFingerprint()
		{
			return (byte[])fingerprint.Clone();
		}

		public AsymmetricKeyParameter GetKey()
		{
			try
			{
				switch (publicPk.Algorithm)
				{
				case PublicKeyAlgorithmTag.RsaGeneral:
				case PublicKeyAlgorithmTag.RsaEncrypt:
				case PublicKeyAlgorithmTag.RsaSign:
				{
					RsaPublicBcpgKey rsaPublicBcpgKey = (RsaPublicBcpgKey)publicPk.Key;
					return new RsaKeyParameters(false, rsaPublicBcpgKey.Modulus, rsaPublicBcpgKey.PublicExponent);
				}
				case PublicKeyAlgorithmTag.Dsa:
				{
					DsaPublicBcpgKey dsaPublicBcpgKey = (DsaPublicBcpgKey)publicPk.Key;
					return new DsaPublicKeyParameters(dsaPublicBcpgKey.Y, new DsaParameters(dsaPublicBcpgKey.P, dsaPublicBcpgKey.Q, dsaPublicBcpgKey.G));
				}
				case PublicKeyAlgorithmTag.ElGamalEncrypt:
				case PublicKeyAlgorithmTag.ElGamalGeneral:
				{
					ElGamalPublicBcpgKey elGamalPublicBcpgKey = (ElGamalPublicBcpgKey)publicPk.Key;
					return new ElGamalPublicKeyParameters(elGamalPublicBcpgKey.Y, new ElGamalParameters(elGamalPublicBcpgKey.P, elGamalPublicBcpgKey.G));
				}
				default:
					throw new PgpException("unknown public key algorithm encountered");
				}
			}
			catch (PgpException ex)
			{
				throw ex;
			}
			catch (Exception exception)
			{
				throw new PgpException("exception constructing public key", exception);
			}
		}

		public IEnumerable GetUserIds()
		{
			IList list = Platform.CreateArrayList();
			foreach (object id in ids)
			{
				if (id is string)
				{
					list.Add(id);
				}
			}
			return new EnumerableProxy(list);
		}

		public IEnumerable GetUserAttributes()
		{
			IList list = Platform.CreateArrayList();
			foreach (object id in ids)
			{
				if (id is PgpUserAttributeSubpacketVector)
				{
					list.Add(id);
				}
			}
			return new EnumerableProxy(list);
		}

		public IEnumerable GetSignaturesForId(string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			for (int i = 0; i != ids.Count; i++)
			{
				if (id.Equals(ids[i]))
				{
					return new EnumerableProxy((IList)idSigs[i]);
				}
			}
			return null;
		}

		public IEnumerable GetSignaturesForUserAttribute(PgpUserAttributeSubpacketVector userAttributes)
		{
			for (int i = 0; i != ids.Count; i++)
			{
				if (userAttributes.Equals(ids[i]))
				{
					return new EnumerableProxy((IList)idSigs[i]);
				}
			}
			return null;
		}

		public IEnumerable GetSignaturesOfType(int signatureType)
		{
			IList list = Platform.CreateArrayList();
			foreach (PgpSignature signature in GetSignatures())
			{
				if (signature.SignatureType == signatureType)
				{
					list.Add(signature);
				}
			}
			return new EnumerableProxy(list);
		}

		public IEnumerable GetSignatures()
		{
			IList list;
			if (subSigs != null)
			{
				list = subSigs;
			}
			else
			{
				list = Platform.CreateArrayList(keySigs);
				foreach (ICollection idSig in idSigs)
				{
					CollectionUtilities.AddRange(list, idSig);
				}
			}
			return new EnumerableProxy(list);
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
			bcpgOutputStream.WritePacket(publicPk);
			if (trustPk != null)
			{
				bcpgOutputStream.WritePacket(trustPk);
			}
			if (subSigs == null)
			{
				foreach (PgpSignature keySig in keySigs)
				{
					keySig.Encode(bcpgOutputStream);
				}
				for (int i = 0; i != ids.Count; i++)
				{
					if (ids[i] is string)
					{
						string id = (string)ids[i];
						bcpgOutputStream.WritePacket(new UserIdPacket(id));
					}
					else
					{
						PgpUserAttributeSubpacketVector pgpUserAttributeSubpacketVector = (PgpUserAttributeSubpacketVector)ids[i];
						bcpgOutputStream.WritePacket(new UserAttributePacket(pgpUserAttributeSubpacketVector.ToSubpacketArray()));
					}
					if (idTrusts[i] != null)
					{
						bcpgOutputStream.WritePacket((ContainedPacket)idTrusts[i]);
					}
					foreach (PgpSignature item in (IList)idSigs[i])
					{
						item.Encode(bcpgOutputStream);
					}
				}
				return;
			}
			foreach (PgpSignature subSig in subSigs)
			{
				subSig.Encode(bcpgOutputStream);
			}
		}

		public bool IsRevoked()
		{
			int num = 0;
			bool flag = false;
			if (IsMasterKey)
			{
				while (!flag && num < keySigs.Count)
				{
					if (((PgpSignature)keySigs[num++]).SignatureType == 32)
					{
						flag = true;
					}
				}
			}
			else
			{
				while (!flag && num < subSigs.Count)
				{
					if (((PgpSignature)subSigs[num++]).SignatureType == 40)
					{
						flag = true;
					}
				}
			}
			return flag;
		}

		public static PgpPublicKey AddCertification(PgpPublicKey key, string id, PgpSignature certification)
		{
			return AddCert(key, id, certification);
		}

		public static PgpPublicKey AddCertification(PgpPublicKey key, PgpUserAttributeSubpacketVector userAttributes, PgpSignature certification)
		{
			return AddCert(key, userAttributes, certification);
		}

		private static PgpPublicKey AddCert(PgpPublicKey key, object id, PgpSignature certification)
		{
			PgpPublicKey pgpPublicKey = new PgpPublicKey(key);
			IList list = null;
			for (int i = 0; i != pgpPublicKey.ids.Count; i++)
			{
				if (id.Equals(pgpPublicKey.ids[i]))
				{
					list = (IList)pgpPublicKey.idSigs[i];
				}
			}
			if (list != null)
			{
				list.Add(certification);
			}
			else
			{
				list = Platform.CreateArrayList();
				list.Add(certification);
				pgpPublicKey.ids.Add(id);
				pgpPublicKey.idTrusts.Add(null);
				pgpPublicKey.idSigs.Add(list);
			}
			return pgpPublicKey;
		}

		public static PgpPublicKey RemoveCertification(PgpPublicKey key, PgpUserAttributeSubpacketVector userAttributes)
		{
			return RemoveCert(key, userAttributes);
		}

		public static PgpPublicKey RemoveCertification(PgpPublicKey key, string id)
		{
			return RemoveCert(key, id);
		}

		private static PgpPublicKey RemoveCert(PgpPublicKey key, object id)
		{
			PgpPublicKey pgpPublicKey = new PgpPublicKey(key);
			bool flag = false;
			for (int i = 0; i < pgpPublicKey.ids.Count; i++)
			{
				if (id.Equals(pgpPublicKey.ids[i]))
				{
					flag = true;
					pgpPublicKey.ids.RemoveAt(i);
					pgpPublicKey.idTrusts.RemoveAt(i);
					pgpPublicKey.idSigs.RemoveAt(i);
				}
			}
			if (!flag)
			{
				return null;
			}
			return pgpPublicKey;
		}

		public static PgpPublicKey RemoveCertification(PgpPublicKey key, string id, PgpSignature certification)
		{
			return RemoveCert(key, id, certification);
		}

		public static PgpPublicKey RemoveCertification(PgpPublicKey key, PgpUserAttributeSubpacketVector userAttributes, PgpSignature certification)
		{
			return RemoveCert(key, userAttributes, certification);
		}

		private static PgpPublicKey RemoveCert(PgpPublicKey key, object id, PgpSignature certification)
		{
			PgpPublicKey pgpPublicKey = new PgpPublicKey(key);
			bool flag = false;
			for (int i = 0; i < pgpPublicKey.ids.Count; i++)
			{
				if (id.Equals(pgpPublicKey.ids[i]))
				{
					IList list = (IList)pgpPublicKey.idSigs[i];
					flag = list.Contains(certification);
					if (flag)
					{
						list.Remove(certification);
					}
				}
			}
			if (!flag)
			{
				return null;
			}
			return pgpPublicKey;
		}

		public static PgpPublicKey AddCertification(PgpPublicKey key, PgpSignature certification)
		{
			if (key.IsMasterKey)
			{
				if (certification.SignatureType == 40)
				{
					throw new ArgumentException("signature type incorrect for master key revocation.");
				}
			}
			else if (certification.SignatureType == 32)
			{
				throw new ArgumentException("signature type incorrect for sub-key revocation.");
			}
			PgpPublicKey pgpPublicKey = new PgpPublicKey(key);
			if (pgpPublicKey.subSigs != null)
			{
				pgpPublicKey.subSigs.Add(certification);
			}
			else
			{
				pgpPublicKey.keySigs.Add(certification);
			}
			return pgpPublicKey;
		}

		public static PgpPublicKey RemoveCertification(PgpPublicKey key, PgpSignature certification)
		{
			PgpPublicKey pgpPublicKey = new PgpPublicKey(key);
			IList list = ((pgpPublicKey.subSigs != null) ? pgpPublicKey.subSigs : pgpPublicKey.keySigs);
			int num = list.IndexOf(certification);
			bool flag = num >= 0;
			if (flag)
			{
				list.RemoveAt(num);
			}
			else
			{
				foreach (string userId in key.GetUserIds())
				{
					foreach (object item in key.GetSignaturesForId(userId))
					{
						if (certification == item)
						{
							flag = true;
							pgpPublicKey = RemoveCertification(pgpPublicKey, userId, certification);
						}
					}
				}
				if (!flag)
				{
					foreach (PgpUserAttributeSubpacketVector userAttribute in key.GetUserAttributes())
					{
						foreach (object item2 in key.GetSignaturesForUserAttribute(userAttribute))
						{
							if (certification == item2)
							{
								flag = true;
								pgpPublicKey = RemoveCertification(pgpPublicKey, userAttribute, certification);
							}
						}
					}
					return pgpPublicKey;
				}
			}
			return pgpPublicKey;
		}
	}
}
