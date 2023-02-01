using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public enum ContentTransferEncoding
	{
		None,
		Unknown,
		Base64,
		QuotedPrintable,
		SevenBits,
		EightBits,
		Binary
	}
}
