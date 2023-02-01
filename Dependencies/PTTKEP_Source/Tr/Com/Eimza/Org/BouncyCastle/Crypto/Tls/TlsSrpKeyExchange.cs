using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Agreement.Srp;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class TlsSrpKeyExchange : AbstractTlsKeyExchange
	{
		protected TlsSigner mTlsSigner;

		protected byte[] mIdentity;

		protected byte[] mPassword;

		protected AsymmetricKeyParameter mServerPublicKey;

		protected byte[] mS;

		protected BigInteger mB;

		protected Srp6Client mSrpClient = new Srp6Client();

		public override bool RequiresServerKeyExchange
		{
			get
			{
				return true;
			}
		}

		public TlsSrpKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, byte[] identity, byte[] password)
			: base(keyExchange, supportedSignatureAlgorithms)
		{
			switch (keyExchange)
			{
			case 21:
				mTlsSigner = null;
				break;
			case 23:
				mTlsSigner = new TlsRsaSigner();
				break;
			case 22:
				mTlsSigner = new TlsDssSigner();
				break;
			default:
				throw new InvalidOperationException("unsupported key exchange algorithm");
			}
			mIdentity = identity;
			mPassword = password;
		}

		public override void Init(TlsContext context)
		{
			base.Init(context);
			if (mTlsSigner != null)
			{
				mTlsSigner.Init(context);
			}
		}

		public override void SkipServerCredentials()
		{
			if (mTlsSigner != null)
			{
				throw new TlsFatalAlert(10);
			}
		}

		public override void ProcessServerCertificate(Certificate serverCertificate)
		{
			if (mTlsSigner == null)
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
			if (!mTlsSigner.IsValidPublicKey(mServerPublicKey))
			{
				throw new TlsFatalAlert(46);
			}
			TlsUtilities.ValidateKeyUsage(certificateAt, 128);
			base.ProcessServerCertificate(serverCertificate);
		}

		public override void ProcessServerKeyExchange(Stream input)
		{
			SecurityParameters securityParameters = context.SecurityParameters;
			SignerInputBuffer signerInputBuffer = null;
			Stream input2 = input;
			if (mTlsSigner != null)
			{
				signerInputBuffer = new SignerInputBuffer();
				input2 = new TeeInputStream(input, signerInputBuffer);
			}
			byte[] bytes = TlsUtilities.ReadOpaque16(input2);
			byte[] bytes2 = TlsUtilities.ReadOpaque16(input2);
			byte[] array = TlsUtilities.ReadOpaque8(input2);
			byte[] bytes3 = TlsUtilities.ReadOpaque16(input2);
			if (signerInputBuffer != null)
			{
				DigitallySigned digitallySigned = DigitallySigned.Parse(context, input);
				ISigner signer = InitVerifyer(mTlsSigner, digitallySigned.Algorithm, securityParameters);
				signerInputBuffer.UpdateSigner(signer);
				if (!signer.VerifySignature(digitallySigned.Signature))
				{
					throw new TlsFatalAlert(51);
				}
			}
			BigInteger n = new BigInteger(1, bytes);
			BigInteger g = new BigInteger(1, bytes2);
			mS = array;
			try
			{
				mB = Srp6Utilities.ValidatePublicValue(n, new BigInteger(1, bytes3));
			}
			catch (CryptoException alertCause)
			{
				throw new TlsFatalAlert(47, alertCause);
			}
			mSrpClient.Init(n, g, TlsUtilities.CreateHash(2), context.SecureRandom);
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
			TlsUtilities.WriteOpaque16(BigIntegers.AsUnsignedByteArray(mSrpClient.GenerateClientCredentials(mS, mIdentity, mPassword)), output);
		}

		public override byte[] GeneratePremasterSecret()
		{
			try
			{
				return BigIntegers.AsUnsignedByteArray(mSrpClient.CalculateSecret(mB));
			}
			catch (CryptoException alertCause)
			{
				throw new TlsFatalAlert(47, alertCause);
			}
		}

		protected virtual ISigner InitVerifyer(TlsSigner tlsSigner, SignatureAndHashAlgorithm algorithm, SecurityParameters securityParameters)
		{
			ISigner signer = tlsSigner.CreateVerifyer(algorithm, mServerPublicKey);
			signer.BlockUpdate(securityParameters.clientRandom, 0, securityParameters.clientRandom.Length);
			signer.BlockUpdate(securityParameters.serverRandom, 0, securityParameters.serverRandom.Length);
			return signer;
		}
	}
}
