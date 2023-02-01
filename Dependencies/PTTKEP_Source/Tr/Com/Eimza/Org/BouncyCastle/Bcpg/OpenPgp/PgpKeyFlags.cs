namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	internal abstract class PgpKeyFlags
	{
		public const int CanCertify = 1;

		public const int CanSign = 2;

		public const int CanEncryptCommunications = 4;

		public const int CanEncryptStorage = 8;

		public const int MaybeSplit = 16;

		public const int MaybeShared = 128;
	}
}
