using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Agreement;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class DefaultTlsAgreementCredentials : AbstractTlsAgreementCredentials
	{
		protected readonly Certificate mCertificate;

		protected readonly AsymmetricKeyParameter mPrivateKey;

		protected readonly IBasicAgreement mBasicAgreement;

		protected readonly bool mTruncateAgreement;

		public override Certificate Certificate
		{
			get
			{
				return mCertificate;
			}
		}

		public DefaultTlsAgreementCredentials(Certificate certificate, AsymmetricKeyParameter privateKey)
		{
			if (certificate == null)
			{
				throw new ArgumentNullException("certificate");
			}
			if (certificate.IsEmpty)
			{
				throw new ArgumentException("cannot be empty", "certificate");
			}
			if (privateKey == null)
			{
				throw new ArgumentNullException("privateKey");
			}
			if (!privateKey.IsPrivate)
			{
				throw new ArgumentException("must be private", "privateKey");
			}
			if (privateKey is DHPrivateKeyParameters)
			{
				mBasicAgreement = new DHBasicAgreement();
				mTruncateAgreement = true;
			}
			else
			{
				if (!(privateKey is ECPrivateKeyParameters))
				{
					throw new ArgumentException("type not supported: " + privateKey.GetType().FullName, "privateKey");
				}
				mBasicAgreement = new ECDHBasicAgreement();
				mTruncateAgreement = false;
			}
			mCertificate = certificate;
			mPrivateKey = privateKey;
		}

		public override byte[] GenerateAgreement(AsymmetricKeyParameter peerPublicKey)
		{
			mBasicAgreement.Init(mPrivateKey);
			BigInteger n = mBasicAgreement.CalculateAgreement(peerPublicKey);
			if (mTruncateAgreement)
			{
				return BigIntegers.AsUnsignedByteArray(n);
			}
			return BigIntegers.AsUnsignedByteArray(mBasicAgreement.GetFieldSize(), n);
		}
	}
}
