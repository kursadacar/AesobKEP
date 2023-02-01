using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Zlib;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class CmsCompressedDataGenerator
	{
		public const string ZLib = "1.2.840.113549.1.9.16.3.8";

		public CmsCompressedData Generate(CmsProcessable content, string compressionOid)
		{
			AlgorithmIdentifier compressionAlgorithm;
			Asn1OctetString content2;
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				ZOutputStream zOutputStream = new ZOutputStream(memoryStream, -1);
				content.Write(zOutputStream);
				zOutputStream.Close();
				compressionAlgorithm = new AlgorithmIdentifier(new DerObjectIdentifier(compressionOid));
				content2 = new BerOctetString(memoryStream.ToArray());
			}
			catch (IOException e)
			{
				throw new CmsException("exception encoding data.", e);
			}
			ContentInfo encapContentInfo = new ContentInfo(CmsObjectIdentifiers.Data, content2);
			return new CmsCompressedData(new ContentInfo(CmsObjectIdentifiers.CompressedData, new CompressedData(compressionAlgorithm, encapContentInfo)));
		}
	}
}
