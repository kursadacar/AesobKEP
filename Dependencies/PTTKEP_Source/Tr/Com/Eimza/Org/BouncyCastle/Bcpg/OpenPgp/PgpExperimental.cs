namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	internal class PgpExperimental : PgpObject
	{
		private readonly ExperimentalPacket p;

		public PgpExperimental(BcpgInputStream bcpgIn)
		{
			p = (ExperimentalPacket)bcpgIn.ReadPacket();
		}
	}
}
