using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Digests
{
	internal class NullDigest : IDigest
	{
		private readonly MemoryStream bOut = new MemoryStream();

		public string AlgorithmName
		{
			get
			{
				return "NULL";
			}
		}

		public int GetByteLength()
		{
			return 0;
		}

		public int GetDigestSize()
		{
			return (int)bOut.Length;
		}

		public void Update(byte b)
		{
			bOut.WriteByte(b);
		}

		public void BlockUpdate(byte[] inBytes, int inOff, int len)
		{
			bOut.Write(inBytes, inOff, len);
		}

		public int DoFinal(byte[] outBytes, int outOff)
		{
			byte[] array = bOut.ToArray();
			array.CopyTo(outBytes, outOff);
			Reset();
			return array.Length;
		}

		public void Reset()
		{
			bOut.SetLength(0L);
		}
	}
}
