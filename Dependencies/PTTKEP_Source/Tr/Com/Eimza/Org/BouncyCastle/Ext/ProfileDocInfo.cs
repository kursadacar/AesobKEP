using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
	internal interface ProfileDocInfo
	{
		byte[] GetDigestOfProfile(string oid);

		byte[] GetDigestOfProfile(DerObjectIdentifier oid);

		Stream GetProfile();

		string GetURI();
	}
}
