using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Utilities.Encoders
{
	internal interface IEncoder
	{
		int Encode(byte[] data, int off, int length, Stream outStream);

		int Decode(byte[] data, int off, int length, Stream outStream);

		int DecodeString(string data, Stream outStream);
	}
}
