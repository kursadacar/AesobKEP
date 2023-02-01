using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg
{
	internal class UserAttributePacket : ContainedPacket
	{
		private readonly UserAttributeSubpacket[] subpackets;

		public UserAttributePacket(BcpgInputStream bcpgIn)
		{
			UserAttributeSubpacketsParser userAttributeSubpacketsParser = new UserAttributeSubpacketsParser(bcpgIn);
			IList list = Platform.CreateArrayList();
			UserAttributeSubpacket value;
			while ((value = userAttributeSubpacketsParser.ReadPacket()) != null)
			{
				list.Add(value);
			}
			subpackets = new UserAttributeSubpacket[list.Count];
			for (int i = 0; i != subpackets.Length; i++)
			{
				subpackets[i] = (UserAttributeSubpacket)list[i];
			}
		}

		public UserAttributePacket(UserAttributeSubpacket[] subpackets)
		{
			this.subpackets = subpackets;
		}

		public UserAttributeSubpacket[] GetSubpackets()
		{
			return subpackets;
		}

		public override void Encode(BcpgOutputStream bcpgOut)
		{
			MemoryStream memoryStream = new MemoryStream();
			for (int i = 0; i != subpackets.Length; i++)
			{
				subpackets[i].Encode(memoryStream);
			}
			bcpgOut.WritePacket(PacketTag.UserAttribute, memoryStream.ToArray(), false);
		}
	}
}
