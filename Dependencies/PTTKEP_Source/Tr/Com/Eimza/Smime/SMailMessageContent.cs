using System;
using System.Text;

namespace Tr.Com.Eimza.Smime
{
	internal class SMailMessageContent
	{
		public SMailContentType ContentType { get; private set; }

		public SMailTransferEncoding TransferEncoding { get; private set; }

		public byte[] Body { get; private set; }

		public SMailMessageContent(byte[] body, SMailContentType contentType, SMailTransferEncoding transferEncoding, bool encodeBody)
		{
			if (encodeBody)
			{
				switch (transferEncoding)
				{
				case SMailTransferEncoding.QuotedPrintable:
					Body = Encoding.ASCII.GetBytes(SMailTransferEncoder.ToQuotedPrintable(body, false));
					break;
				case SMailTransferEncoding.Base64:
					Body = Encoding.ASCII.GetBytes(SMailTransferEncoder.ToBase64(body));
					break;
				case SMailTransferEncoding.SevenBit:
					Body = body;
					break;
				default:
					throw new ArgumentOutOfRangeException("transferEncoding", "Invalid Transfer Encoding");
				}
			}
			else
			{
				Body = body;
			}
			TransferEncoding = transferEncoding;
			ContentType = contentType;
		}
	}
}
