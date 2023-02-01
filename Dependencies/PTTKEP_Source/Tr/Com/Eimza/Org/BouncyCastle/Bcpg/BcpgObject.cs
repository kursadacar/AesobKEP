using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg
{
	internal abstract class BcpgObject
	{
		public virtual byte[] GetEncoded()
		{
			MemoryStream memoryStream = new MemoryStream();
			new BcpgOutputStream(memoryStream).WriteObject(this);
			return memoryStream.ToArray();
		}

		public abstract void Encode(BcpgOutputStream bcpgOut);
	}
}
