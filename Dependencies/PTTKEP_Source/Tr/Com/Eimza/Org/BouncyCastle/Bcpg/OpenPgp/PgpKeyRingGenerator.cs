using System;
using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	internal class PgpKeyRingGenerator
	{
		private IList keys = Platform.CreateArrayList();

		private string id;

		private SymmetricKeyAlgorithmTag encAlgorithm;

		private int certificationLevel;

		private char[] passPhrase;

		private bool useSha1;

		private PgpKeyPair masterKey;

		private PgpSignatureSubpacketVector hashedPacketVector;

		private PgpSignatureSubpacketVector unhashedPacketVector;

		private SecureRandom rand;

		public PgpKeyRingGenerator(int certificationLevel, PgpKeyPair masterKey, string id, SymmetricKeyAlgorithmTag encAlgorithm, char[] passPhrase, PgpSignatureSubpacketVector hashedPackets, PgpSignatureSubpacketVector unhashedPackets, SecureRandom rand)
			: this(certificationLevel, masterKey, id, encAlgorithm, passPhrase, false, hashedPackets, unhashedPackets, rand)
		{
		}

		public PgpKeyRingGenerator(int certificationLevel, PgpKeyPair masterKey, string id, SymmetricKeyAlgorithmTag encAlgorithm, char[] passPhrase, bool useSha1, PgpSignatureSubpacketVector hashedPackets, PgpSignatureSubpacketVector unhashedPackets, SecureRandom rand)
		{
			this.certificationLevel = certificationLevel;
			this.masterKey = masterKey;
			this.id = id;
			this.encAlgorithm = encAlgorithm;
			this.passPhrase = passPhrase;
			this.useSha1 = useSha1;
			hashedPacketVector = hashedPackets;
			unhashedPacketVector = unhashedPackets;
			this.rand = rand;
			keys.Add(new PgpSecretKey(certificationLevel, masterKey, id, encAlgorithm, passPhrase, useSha1, hashedPackets, unhashedPackets, rand));
		}

		public void AddSubKey(PgpKeyPair keyPair)
		{
			AddSubKey(keyPair, hashedPacketVector, unhashedPacketVector);
		}

		public void AddSubKey(PgpKeyPair keyPair, PgpSignatureSubpacketVector hashedPackets, PgpSignatureSubpacketVector unhashedPackets)
		{
			try
			{
				PgpSignatureGenerator pgpSignatureGenerator = new PgpSignatureGenerator(masterKey.PublicKey.Algorithm, HashAlgorithmTag.Sha1);
				pgpSignatureGenerator.InitSign(24, masterKey.PrivateKey);
				pgpSignatureGenerator.SetHashedSubpackets(hashedPackets);
				pgpSignatureGenerator.SetUnhashedSubpackets(unhashedPackets);
				IList list = Platform.CreateArrayList();
				list.Add(pgpSignatureGenerator.GenerateCertification(masterKey.PublicKey, keyPair.PublicKey));
				keys.Add(new PgpSecretKey(keyPair.PrivateKey, new PgpPublicKey(keyPair.PublicKey, null, list), encAlgorithm, passPhrase, useSha1, rand));
			}
			catch (PgpException ex)
			{
				throw ex;
			}
			catch (Exception exception)
			{
				throw new PgpException("exception adding subkey: ", exception);
			}
		}

		public PgpSecretKeyRing GenerateSecretKeyRing()
		{
			return new PgpSecretKeyRing(keys);
		}

		public PgpPublicKeyRing GeneratePublicKeyRing()
		{
			IList list = Platform.CreateArrayList();
			IEnumerator enumerator = keys.GetEnumerator();
			enumerator.MoveNext();
			PgpSecretKey pgpSecretKey = (PgpSecretKey)enumerator.Current;
			list.Add(pgpSecretKey.PublicKey);
			while (enumerator.MoveNext())
			{
				pgpSecretKey = (PgpSecretKey)enumerator.Current;
				PgpPublicKey pgpPublicKey = new PgpPublicKey(pgpSecretKey.PublicKey);
				pgpPublicKey.publicPk = new PublicSubkeyPacket(pgpPublicKey.Algorithm, pgpPublicKey.CreationTime, pgpPublicKey.publicPk.Key);
				list.Add(pgpPublicKey);
			}
			return new PgpPublicKeyRing(list);
		}
	}
}
