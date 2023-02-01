using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class KekRecipientInformation : RecipientInformation
	{
		private readonly KekRecipientInfo info;

		internal KekRecipientInformation(KekRecipientInfo info, CmsSecureReadable secureReadable)
			: base(info.KeyEncryptionAlgorithm, secureReadable)
		{
			this.info = info;
			rid = new RecipientID();
			KekIdentifier kekID = info.KekID;
			rid.KeyIdentifier = kekID.KeyIdentifier.GetOctets();
		}

		protected override CmsTypedStream GetContentStream(ICipherParameters key)
		{
			try
			{
				byte[] octets = info.EncryptedKey.GetOctets();
				IWrapper wrapper = WrapperUtilities.GetWrapper(keyEncAlg.ObjectID.Id);
				wrapper.Init(false, key);
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
