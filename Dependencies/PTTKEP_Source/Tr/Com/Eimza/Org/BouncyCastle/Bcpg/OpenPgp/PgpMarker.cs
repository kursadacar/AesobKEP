namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	internal class PgpMarker : PgpObject
	{
		private readonly MarkerPacket p;

		public PgpMarker(BcpgInputStream bcpgIn)
		{
			p = (MarkerPacket)bcpgIn.ReadPacket();
		}
	}
}
