using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal interface RecipientInfoGenerator
	{
		RecipientInfo Generate(KeyParameter contentEncryptionKey, SecureRandom random);
	}
}
