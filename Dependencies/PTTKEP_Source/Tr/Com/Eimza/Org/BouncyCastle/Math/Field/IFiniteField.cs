namespace Tr.Com.Eimza.Org.BouncyCastle.Math.Field
{
	internal interface IFiniteField
	{
		BigInteger Characteristic { get; }

		int Dimension { get; }
	}
}
