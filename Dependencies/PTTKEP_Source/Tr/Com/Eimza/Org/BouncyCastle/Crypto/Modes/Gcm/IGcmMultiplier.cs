namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Modes.Gcm
{
	internal interface IGcmMultiplier
	{
		void Init(byte[] H);

		void MultiplyH(byte[] x);
	}
}
