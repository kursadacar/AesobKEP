namespace Tr.Com.Eimza.Org.BouncyCastle.Math.EC.Multiplier
{
	internal interface ECMultiplier
	{
		ECPoint Multiply(ECPoint p, BigInteger k);
	}
}
