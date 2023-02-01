using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Digests;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Signers;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal abstract class TlsDsaSigner : AbstractTlsSigner
	{
		protected abstract byte SignatureAlgorithm { get; }

		public override byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey, byte[] hash)
		{
			ISigner signer = MakeSigner(algorithm, true, true, new ParametersWithRandom(privateKey, mContext.SecureRandom));
			if (algorithm == null)
			{
				signer.BlockUpdate(hash, 16, 20);
			}
			else
			{
				signer.BlockUpdate(hash, 0, hash.Length);
			}
			return signer.GenerateSignature();
		}

		public override bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] hash)
		{
			ISigner signer = MakeSigner(algorithm, true, false, publicKey);
			if (algorithm == null)
			{
				signer.BlockUpdate(hash, 16, 20);
			}
			else
			{
				signer.BlockUpdate(hash, 0, hash.Length);
			}
			return signer.VerifySignature(sigBytes);
		}

		public override ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey)
		{
			return MakeSigner(algorithm, false, true, privateKey);
		}

		public override ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey)
		{
			return MakeSigner(algorithm, false, false, publicKey);
		}

		protected virtual ICipherParameters MakeInitParameters(bool forSigning, ICipherParameters cp)
		{
			return cp;
		}

		protected virtual ISigner MakeSigner(SignatureAndHashAlgorithm algorithm, bool raw, bool forSigning, ICipherParameters cp)
		{
			if (algorithm != null != TlsUtilities.IsTlsV12(mContext))
			{
				throw new InvalidOperationException();
			}
			if (algorithm != null && (algorithm.Hash != 2 || algorithm.Signature != SignatureAlgorithm))
			{
				throw new InvalidOperationException();
			}
			byte hashAlgorithm = (byte)((algorithm == null) ? 2 : algorithm.Hash);
			IDigest digest;
			if (!raw)
			{
				digest = TlsUtilities.CreateHash(hashAlgorithm);
			}
			else
			{
				IDigest digest2 = new NullDigest();
				digest = digest2;
			}
			IDigest digest3 = digest;
			DsaDigestSigner dsaDigestSigner = new DsaDigestSigner(CreateDsaImpl(hashAlgorithm), digest3);
			((ISigner)dsaDigestSigner).Init(forSigning, MakeInitParameters(forSigning, cp));
			return dsaDigestSigner;
		}

		protected abstract IDsa CreateDsaImpl(byte hashAlgorithm);
	}
}
