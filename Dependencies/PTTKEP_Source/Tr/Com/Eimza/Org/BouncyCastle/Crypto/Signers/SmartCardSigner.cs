using Tr.Com.Eimza.Org.BouncyCastle.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.X509;
using Tr.Com.Eimza.SmartCard;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Signers
{
	internal class SmartCardSigner : ISigner
	{
		public readonly SmartCardReader sCard;

		private readonly X509Certificate SelectedCertificate;

		private byte[] currentSignature;

		private readonly IDigest digest;

		private readonly string digestAlg;

		public string AlgorithmName
		{
			get
			{
				if (SelectedCertificate != null)
				{
					return CmsSignedHelper.Instance.GetEncryptionAlgName(SelectedCertificate.SigAlgOid);
				}
				return null;
			}
		}

		public SmartCardSigner(SmartCardReader SmartCard)
		{
			sCard = SmartCard;
			SelectedCertificate = SmartCard.CertList[SmartCard.SmartCardParams.SelectedCertIndex];
			digestAlg = CmsSignedHelper.Instance.GetDigestAlgFromSignatureAlg(SelectedCertificate.SigAlgOid);
			digest = DigestUtilities.GetDigest(digestAlg);
		}

		public SmartCardSigner(SmartCardReader SmartCard, string signAlgOid)
		{
			sCard = SmartCard;
			SelectedCertificate = SmartCard.CertList[SmartCard.SmartCardParams.SelectedCertIndex];
			digestAlg = CmsSignedHelper.Instance.GetDigestAlgFromSignatureAlg(signAlgOid);
			digest = DigestUtilities.GetDigest(digestAlg);
		}

		public void BlockUpdate(byte[] input, int inOff, int length)
		{
			digest.BlockUpdate(input, inOff, length);
		}

		public byte[] CurrentSignature()
		{
			return currentSignature;
		}

		public byte[] GenerateSignature()
		{
			byte[] array = new byte[digest.GetDigestSize()];
			digest.DoFinal(array, 0);
			currentSignature = sCard.GetHashSignature(array, digestAlg);
			return currentSignature;
		}

		public void Init(bool forSigning, ICipherParameters parameters)
		{
		}

		public void Reset()
		{
			digest.Reset();
			currentSignature = null;
		}

		public void Update(byte input)
		{
			digest.Update(input);
		}

		public bool VerifySignature(byte[] signature)
		{
			return true;
		}
	}
}
