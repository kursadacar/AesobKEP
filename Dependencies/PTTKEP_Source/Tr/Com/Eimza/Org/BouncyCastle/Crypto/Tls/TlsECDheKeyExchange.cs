using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class TlsECDheKeyExchange : TlsECDHKeyExchange
	{
		protected TlsSignerCredentials mServerCredentials;

		public TlsECDheKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, int[] namedCurves, byte[] clientECPointFormats, byte[] serverECPointFormats)
			: base(keyExchange, supportedSignatureAlgorithms, namedCurves, clientECPointFormats, serverECPointFormats)
		{
		}

		public override void ProcessServerCredentials(TlsCredentials serverCredentials)
		{
			if (!(serverCredentials is TlsSignerCredentials))
			{
				throw new TlsFatalAlert(80);
			}
			ProcessServerCertificate(serverCredentials.Certificate);
			mServerCredentials = (TlsSignerCredentials)serverCredentials;
		}

		public override byte[] GenerateServerKeyExchange()
		{
			int num = -1;
			if (mNamedCurves == null)
			{
				num = 23;
			}
			else
			{
				for (int i = 0; i < mNamedCurves.Length; i++)
				{
					int num2 = mNamedCurves[i];
					if (NamedCurve.IsValid(num2) && TlsEccUtilities.IsSupportedNamedCurve(num2))
					{
						num = num2;
						break;
					}
				}
			}
			ECDomainParameters eCDomainParameters = null;
			if (num >= 0)
			{
				eCDomainParameters = TlsEccUtilities.GetParametersForNamedCurve(num);
			}
			else if (Arrays.Contains(mNamedCurves, 65281))
			{
				eCDomainParameters = TlsEccUtilities.GetParametersForNamedCurve(23);
			}
			else if (Arrays.Contains(mNamedCurves, 65282))
			{
				eCDomainParameters = TlsEccUtilities.GetParametersForNamedCurve(10);
			}
			if (eCDomainParameters == null)
			{
				throw new TlsFatalAlert(80);
			}
			AsymmetricCipherKeyPair asymmetricCipherKeyPair = TlsEccUtilities.GenerateECKeyPair(context.SecureRandom, eCDomainParameters);
			mECAgreePrivateKey = (ECPrivateKeyParameters)asymmetricCipherKeyPair.Private;
			DigestInputBuffer digestInputBuffer = new DigestInputBuffer();
			if (num < 0)
			{
				TlsEccUtilities.WriteExplicitECParameters(mClientECPointFormats, eCDomainParameters, digestInputBuffer);
			}
			else
			{
				TlsEccUtilities.WriteNamedECParameters(num, digestInputBuffer);
			}
			ECPublicKeyParameters eCPublicKeyParameters = (ECPublicKeyParameters)asymmetricCipherKeyPair.Public;
			TlsEccUtilities.WriteECPoint(mClientECPointFormats, eCPublicKeyParameters.Q, digestInputBuffer);
			SignatureAndHashAlgorithm signatureAndHashAlgorithm;
			IDigest digest;
			if (TlsUtilities.IsTlsV12(context))
			{
				signatureAndHashAlgorithm = mServerCredentials.SignatureAndHashAlgorithm;
				if (signatureAndHashAlgorithm == null)
				{
					throw new TlsFatalAlert(80);
				}
				digest = TlsUtilities.CreateHash(signatureAndHashAlgorithm.Hash);
			}
			else
			{
				signatureAndHashAlgorithm = null;
				digest = new CombinedHash();
			}
			SecurityParameters securityParameters = context.SecurityParameters;
			digest.BlockUpdate(securityParameters.clientRandom, 0, securityParameters.clientRandom.Length);
			digest.BlockUpdate(securityParameters.serverRandom, 0, securityParameters.serverRandom.Length);
			digestInputBuffer.UpdateDigest(digest);
			byte[] hash = DigestUtilities.DoFinal(digest);
			byte[] signature = mServerCredentials.GenerateCertificateSignature(hash);
			new DigitallySigned(signatureAndHashAlgorithm, signature).Encode(digestInputBuffer);
			return digestInputBuffer.ToArray();
		}

		public override void ProcessServerKeyExchange(Stream input)
		{
			SecurityParameters securityParameters = context.SecurityParameters;
			SignerInputBuffer signerInputBuffer = new SignerInputBuffer();
			Stream input2 = new TeeInputStream(input, signerInputBuffer);
			ECDomainParameters curve_params = TlsEccUtilities.ReadECParameters(mNamedCurves, mClientECPointFormats, input2);
			byte[] encoding = TlsUtilities.ReadOpaque8(input2);
			DigitallySigned digitallySigned = DigitallySigned.Parse(context, input);
			ISigner signer = InitVerifyer(mTlsSigner, digitallySigned.Algorithm, securityParameters);
			signerInputBuffer.UpdateSigner(signer);
			if (!signer.VerifySignature(digitallySigned.Signature))
			{
				throw new TlsFatalAlert(51);
			}
			mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(mClientECPointFormats, curve_params, encoding));
		}

		public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
		{
			byte[] certificateTypes = certificateRequest.CertificateTypes;
			foreach (byte b in certificateTypes)
			{
				if ((uint)(b - 1) > 1u && b != 64)
				{
					throw new TlsFatalAlert(47);
				}
			}
		}

		public override void ProcessClientCredentials(TlsCredentials clientCredentials)
		{
			if (!(clientCredentials is TlsSignerCredentials))
			{
				throw new TlsFatalAlert(80);
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
