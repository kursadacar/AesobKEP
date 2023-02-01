namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg
{
	internal class SymmetricEncIntegrityPacket : InputStreamPacket
	{
		internal readonly int version;

		internal SymmetricEncIntegrityPacket(BcpgInputStream bcpgIn)
			: base(bcpgIn)
		{
			version = bcpgIn.ReadByte();
		}
	}
}
