using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Cms;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tools
{
    internal static class BouncyCastleUtil
	{
		public static Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.SignedData GetSignedData(CmsSignedData cmsSignedData)
		{
			return Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.SignedData.GetInstance(cmsSignedData.ContentInfo.Content);
		}

		public static Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.SignedData GetSignedData(byte[] cmsSignedDataValue)
		{
			return Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.SignedData.GetInstance(new CmsSignedData(cmsSignedDataValue).ContentInfo.Content);
		}

		public static Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.ContentInfo GetEncapContentInfo(CmsSignedData cmsSignedData)
		{
			return GetSignedData(cmsSignedData).EncapContentInfo;
		}

		public static Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.ContentInfo GetEncapContentInfo(Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.SignedData signedData)
		{
			return signedData.EncapContentInfo;
		}

		public static Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.ContentInfo GetEncapContentInfoOctets(CmsSignedData cmsSignedData)
		{
			DerOctetString derOctetString = (DerOctetString)GetSignedData(cmsSignedData).EncapContentInfo.Content;
			return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.ContentInfo(PkcsObjectIdentifiers.Data, new BerOctetString(derOctetString.GetOctets()));
		}

		public static Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.ContentInfo GetEncapContentInfoOctets(Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.SignedData signedData)
		{
			DerOctetString derOctetString = (DerOctetString)signedData.EncapContentInfo.Content;
			return new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.ContentInfo(PkcsObjectIdentifiers.Data, new BerOctetString(derOctetString.GetOctets()));
		}
	}
}
