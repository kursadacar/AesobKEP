using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg
{
	internal abstract class ContainedPacket : Packet
	{
		public byte[] GetEncoded()
		{
			MemoryStream memoryStream = new MemoryStream();
			new BcpgOutputStream(memoryStream).WritePacket(this);
			return memoryStream.ToArray();
		}

		public abstract void Encode(BcpgOutputStream bcpgOut);
	}
}
