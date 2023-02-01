using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Zlib;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class CmsCompressedData
	{
		internal ContentInfo contentInfo;

		public ContentInfo ContentInfo
		{
			get
			{
				return contentInfo;
			}
		}

		public CmsCompressedData(byte[] compressedData)
			: this(CmsUtilities.ReadContentInfo(compressedData))
		{
		}

		public CmsCompressedData(Stream compressedDataStream)
			: this(CmsUtilities.ReadContentInfo(compressedDataStream))
		{
		}

		public CmsCompressedData(ContentInfo contentInfo)
		{
			this.contentInfo = contentInfo;
		}

		public byte[] GetContent()
		{
			ZInputStream zInputStream = new ZInputStream(((Asn1OctetString)CompressedData.GetInstance(contentInfo.Content).EncapContentInfo.Content).GetOctetStream());
			try
			{
				return CmsUtilities.StreamToByteArray(zInputStream);
			}
			catch (IOException e)
			{
				throw new CmsException("exception reading compressed stream.", e);
			}
			finally
			{
				zInputStream.Close();
			}
		}

		public byte[] GetContent(int limit)
		{
			ZInputStream inStream = new ZInputStream(new MemoryStream(((Asn1OctetString)CompressedData.GetInstance(contentInfo.Content).EncapContentInfo.Content).GetOctets(), false));
			try
			{
				return CmsUtilities.StreamToByteArray(inStream, limit);
			}
			catch (IOException e)
			{
				throw new CmsException("exception reading compressed stream.", e);
			}
		}

		public byte[] GetEncoded()
		{
			return contentInfo.GetEncoded();
		}
	}
}
