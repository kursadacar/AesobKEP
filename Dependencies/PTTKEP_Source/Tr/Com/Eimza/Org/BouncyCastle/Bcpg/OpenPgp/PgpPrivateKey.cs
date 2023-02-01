using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	internal class PgpPrivateKey
	{
		private readonly long keyId;

		private readonly AsymmetricKeyParameter privateKey;

		public long KeyId
		{
			get
			{
				return keyId;
			}
		}

		public AsymmetricKeyParameter Key
		{
			get
			{
				return privateKey;
			}
		}

		public PgpPrivateKey(AsymmetricKeyParameter privateKey, long keyId)
		{
			if (!privateKey.IsPrivate)
			{
				throw new ArgumentException("Expected a private key", "privateKey");
			}
			this.privateKey = privateKey;
			this.keyId = keyId;
		}
	}
}
