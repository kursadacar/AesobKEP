using System;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class CmsContentInfoParser
	{
		protected ContentInfoParser contentInfo;

		protected Stream data;

		protected CmsContentInfoParser(Stream data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this.data = data;
			try
			{
				Asn1StreamParser asn1StreamParser = new Asn1StreamParser(data);
				contentInfo = new ContentInfoParser((Asn1SequenceParser)asn1StreamParser.ReadObject());
			}
			catch (IOException e)
			{
				throw new CmsException("IOException reading content.", e);
			}
			catch (InvalidCastException e2)
			{
				throw new CmsException("Unexpected object reading content.", e2);
			}
		}

		public void Close()
		{
			data.Close();
		}
	}
}
