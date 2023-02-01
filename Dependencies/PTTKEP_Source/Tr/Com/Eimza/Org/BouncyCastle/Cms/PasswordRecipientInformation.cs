using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class PasswordRecipientInformation : RecipientInformation
	{
		private readonly PasswordRecipientInfo info;

		public virtual AlgorithmIdentifier KeyDerivationAlgorithm
		{
			get
			{
				return info.KeyDerivationAlgorithm;
			}
		}

		internal PasswordRecipientInformation(PasswordRecipientInfo info, CmsSecureReadable secureReadable)
			: base(info.KeyEncryptionAlgorithm, secureReadable)
		{
			this.info = info;
			rid = new RecipientID();
		}

		protected override CmsTypedStream GetContentStream(ICipherParameters key)
		{
			try
			{
				Asn1Sequence obj = (Asn1Sequence)AlgorithmIdentifier.GetInstance(info.KeyEncryptionAlgorithm).Parameters;
				byte[] octets = info.EncryptedKey.GetOctets();
				string id = DerObjectIdentifier.GetInstance(obj[0]).Id;
				IWrapper wrapper = WrapperUtilities.GetWrapper(CmsEnvelopedHelper.Instance.GetRfc3211WrapperName(id));
				byte[] octets2 = Asn1OctetString.GetInstance(obj[1]).GetOctets();
				ICipherParameters encoded = ((CmsPbeKey)key).GetEncoded(id);
				encoded = new ParametersWithIV(encoded, octets2);
				wrapper.Init(false, encoded);
				KeyParameter sKey = ParameterUtilities.CreateKeyParameter(GetContentAlgorithmName(), wrapper.Unwrap(octets, 0, octets.Length));
				return GetContentFromSessionKey(sKey);
			}
			catch (SecurityUtilityException e)
			{
				throw new CmsException("couldn't create cipher.", e);
			}
			catch (InvalidKeyException e2)
			{
				throw new CmsException("key invalid in message.", e2);
			}
		}
	}
}
