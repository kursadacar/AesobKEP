namespace Tr.Com.Eimza.Org.BouncyCastle.Math.Field
{
	internal interface IPolynomialExtensionField : IExtensionField, IFiniteField
	{
		IPolynomial MinimalPolynomial { get; }
	}
}
