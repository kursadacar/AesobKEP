namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Misc
{
	internal class NetscapeRevocationUrl : DerIA5String
	{
		public NetscapeRevocationUrl(DerIA5String str)
			: base(str.GetString())
		{
		}

		public override string ToString()
		{
			return "NetscapeRevocationUrl: " + GetString();
		}
	}
}
