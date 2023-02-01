namespace Tr.Com.Eimza.Org.BouncyCastle.Utilities.Encoders
{
	internal class UrlBase64Encoder : Base64Encoder
	{
		public UrlBase64Encoder()
		{
			encodingTable[encodingTable.Length - 2] = 45;
			encodingTable[encodingTable.Length - 1] = 95;
			padding = 46;
			InitialiseDecodingTable();
		}
	}
}
