using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class TlsPskKeyExchange : AbstractTlsKeyExchange
	{
		protected TlsPskIdentity mPskIdentity;

		protected DHParameters mDHParameters;

		protected int[] mNamedCurves;

		protected byte[] mClientECPointFormats;

		protected byte[] mServerECPointFormats;

		protected byte[] mPskIdentityHint;

		protected DHPrivateKeyParameters mDHAgreePrivateKey;

		protected DHPublicKeyParameters mDHAgreePublicKey;

		protected AsymmetricKeyParameter mServerPublicKey;

		protected RsaKeyParameters mRsaServerPublicKey;

		protected TlsEncryptionCredentials mServerCredentials;

		protected byte[] mPremasterSecret;

		public override bool RequiresServerKeyExchange
		{
			get
			{
				int num = mKeyExchange;
				if (num == 14 || num == 24)
				{
					return true;
				}
				return false;
			}
		}

		public TlsPskKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, TlsPskIdentity pskIdentity, DHParameters dhParameters, int[] namedCurves, byte[] clientECPointFormats, byte[] serverECPointFormats)
			: base(keyExchange, supportedSignatureAlgorithms)
		{
			if ((uint)(keyExchange - 13) > 2u && keyExchange != 24)
			{
				throw new InvalidOperationException("unsupported key exchange algorithm");
			}
			mPskIdentity = pskIdentity;
			mDHParameters = dhParameters;
			mNamedCurves = namedCurves;
			mClientECPointFormats = clientECPointFormats;
			mServerECPointFormats = serverECPointFormats;
		}

		public override void SkipServerCredentials()
		{
			if (mKeyExchange == 15)
			{
				throw new TlsFatalAlert(10);
			}
		}

		public override void ProcessServerCredentials(TlsCredentials serverCredentials)
		{
			if (!(serverCredentials is TlsEncryptionCredentials))
			{
				throw new TlsFatalAlert(80);
			}
			ProcessServerCertificate(serverCredentials.Certificate);
			mServerCredentials = (TlsEncryptionCredentials)serverCredentials;
		}

		public override byte[] GenerateServerKeyExchange()
		{
			mPskIdentityHint = null;
			if (mPskIdentityHint == null && !RequiresServerKeyExchange)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			if (mPskIdentityHint == null)
			{
				TlsUtilities.WriteOpaque16(TlsUtilities.EmptyBytes, memoryStream);
			}
			else
			{
				TlsUtilities.WriteOpaque16(mPskIdentityHint, memoryStream);
			}
			if (mKeyExchange == 14)
			{
				if (mDHParameters == null)
				{
					throw new TlsFatalAlert(80);
				}
				mDHAgreePrivateKey = TlsDHUtilities.GenerateEphemeralServerKeyExchange(context.SecureRandom, mDHParameters, memoryStream);
			}
			else
			{
				int mKeyExchange2 = mKeyExchange;
				int num = 24;
			}
			return memoryStream.ToArray();
		}

		public override void ProcessServerCertificate(Certificate serverCertificate)
		{
			if (mKeyExchange != 15)
			{
				throw new TlsFatalAlert(10);
			}
			if (serverCertificate.IsEmpty)
			{
				throw new TlsFatalAlert(42);
			}
			X509CertificateStructure certificateAt = serverCertificate.GetCertificateAt(0);
			SubjectPublicKeyInfo subjectPublicKeyInfo = certificateAt.SubjectPublicKeyInfo;
			try
			{
				mServerPublicKey = PublicKeyFactory.CreateKey(subjectPublicKeyInfo);
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(43, alertCause);
			}
			if (mServerPublicKey.IsPrivate)
			{
				throw new TlsFatalAlert(80);
			}
			mRsaServerPublicKey = ValidateRsaPublicKey((RsaKeyParameters)mServerPublicKey);
			TlsUtilities.ValidateKeyUsage(certificateAt, 32);
			base.ProcessServerCertificate(serverCertificate);
		}

		public override void ProcessServerKeyExchange(Stream input)
		{
			mPskIdentityHint = TlsUtilities.ReadOpaque16(input);
			if (mKeyExchange == 14)
			{
				ServerDHParams serverDHParams = ServerDHParams.Parse(input);
				mDHAgreePublicKey = TlsDHUtilities.ValidateDHPublicKey(serverDHParams.PublicKey);
			}
			else
			{
				int mKeyExchange2 = mKeyExchange;
				int num = 24;
			}
		}

		public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
		{
			throw new TlsFatalAlert(10);
		}

		public override void ProcessClientCredentials(TlsCredentials clientCredentials)
		{
			throw new TlsFatalAlert(80);
		}

		public override void GenerateClientKeyExchange(Stream output)
		{
			if (mPskIdentityHint == null)
			{
				mPskIdentity.SkipIdentityHint();
			}
			else
			{
				mPskIdentity.NotifyIdentityHint(mPskIdentityHint);
			}
			TlsUtilities.WriteOpaque16(mPskIdentity.GetPskIdentity(), output);
			if (mKeyExchange == 14)
			{
				mDHAgreePrivateKey = TlsDHUtilities.GenerateEphemeralClientKeyExchange(context.SecureRandom, mDHAgreePublicKey.Parameters, output);
				return;
			}
			if (mKeyExchange == 24)
			{
				throw new TlsFatalAlert(80);
			}
			if (mKeyExchange == 15)
			{
				mPremasterSecret = TlsRsaUtilities.GenerateEncryptedPreMasterSecret(context, mRsaServerPublicKey, output);
			}
		}

		public override byte[] GeneratePremasterSecret()
		{
			byte[] psk = mPskIdentity.GetPsk();
			byte[] array = GenerateOtherSecret(psk.Length);
			MemoryStream memoryStream = new MemoryStream(4 + array.Length + psk.Length);
			TlsUtilities.WriteOpaque16(array, memoryStream);
			TlsUtilities.WriteOpaque16(psk, memoryStream);
			return memoryStream.ToArray();
		}

		protected virtual byte[] GenerateOtherSecret(int pskLength)
		{
			if (mKeyExchange == 14)
			{
				if (mDHAgreePrivateKey != null)
				{
					return TlsDHUtilities.CalculateDHBasicAgreement(mDHAgreePublicKey, mDHAgreePrivateKey);
				}
				throw new TlsFatalAlert(80);
			}
			if (mKeyExchange == 24)
			{
				throw new TlsFatalAlert(80);
			}
			if (mKeyExchange == 15)
			{
				return mPremasterSecret;
			}
			return new byte[pskLength];
		}

		protected virtual RsaKeyParameters ValidateRsaPublicKey(RsaKeyParameters key)
		{
			if (!key.Exponent.IsProbablePrime(2))
			{
				throw new TlsFatalAlert(47);
			}
			return key;
		}
	}
}
