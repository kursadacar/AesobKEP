namespace Tr.Com.Eimza.Org.BouncyCastle.Math.EC.Endo
{
	internal interface ECEndomorphism
	{
		ECPointMap PointMap { get; }

		bool HasEfficientPointMap { get; }
	}
}
