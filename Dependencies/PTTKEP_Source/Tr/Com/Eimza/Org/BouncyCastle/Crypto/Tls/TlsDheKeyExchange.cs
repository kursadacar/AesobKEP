using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class TlsDheKeyExchange : TlsDHKeyExchange
	{
		protected TlsSignerCredentials mServerCredentials;

		public TlsDheKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, DHParameters dhParameters)
			: base(keyExchange, supportedSignatureAlgorithms, dhParameters)
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
			if (mDHParameters == null)
			{
				throw new TlsFatalAlert(80);
			}
			DigestInputBuffer digestInputBuffer = new DigestInputBuffer();
			mDHAgreeServerPrivateKey = TlsDHUtilities.GenerateEphemeralServerKeyExchange(context.SecureRandom, mDHParameters, digestInputBuffer);
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
			ServerDHParams serverDHParams = ServerDHParams.Parse(new TeeInputStream(input, signerInputBuffer));
			DigitallySigned digitallySigned = DigitallySigned.Parse(context, input);
			ISigner signer = InitVerifyer(mTlsSigner, digitallySigned.Algorithm, securityParameters);
			signerInputBuffer.UpdateSigner(signer);
			if (!signer.VerifySignature(digitallySigned.Signature))
			{
				throw new TlsFatalAlert(51);
			}
			mDHAgreeServerPublicKey = TlsDHUtilities.ValidateDHPublicKey(serverDHParams.PublicKey);
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
