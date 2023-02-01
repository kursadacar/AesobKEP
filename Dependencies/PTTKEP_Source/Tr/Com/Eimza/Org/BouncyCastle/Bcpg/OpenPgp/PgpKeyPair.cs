using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	internal class PgpKeyPair
	{
		private readonly PgpPublicKey pub;

		private readonly PgpPrivateKey priv;

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

		public PgpPrivateKey PrivateKey
		{
			get
			{
				return priv;
			}
		}

		public PgpKeyPair(PublicKeyAlgorithmTag algorithm, AsymmetricCipherKeyPair keyPair, DateTime time)
			: this(algorithm, keyPair.Public, keyPair.Private, time)
		{
		}

		public PgpKeyPair(PublicKeyAlgorithmTag algorithm, AsymmetricKeyParameter pubKey, AsymmetricKeyParameter privKey, DateTime time)
		{
			pub = new PgpPublicKey(algorithm, pubKey, time);
			priv = new PgpPrivateKey(privKey, pub.KeyId);
		}

		public PgpKeyPair(PgpPublicKey pub, PgpPrivateKey priv)
		{
			this.pub = pub;
			this.priv = priv;
		}
	}
}
