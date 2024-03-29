namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp
{
	internal class OcspResponseStatus : DerEnumerated
	{
		public const int Successful = 0;

		public const int MalformedRequest = 1;

		public const int InternalError = 2;

		public const int TryLater = 3;

		public const int SignatureRequired = 5;

		public const int Unauthorized = 6;

		public int StatusId { get; set; }

		public OcspResponseStatus(int value)
			: base(value)
		{
			StatusId = value;
		}

		public OcspResponseStatus(DerEnumerated value)
			: base(value.Value.IntValue)
		{
			StatusId = value.Value.IntValue;
		}
	}
}
