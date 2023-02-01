namespace Tr.Com.Eimza.SmartCard
{
	internal class SmartCardParameters
	{
		public string DigestAlgorithm { get; set; }

		public string SignatureAlgorithm { get; set; }

		public byte[] HashToBeSigned { get; set; }

		public int SelectedCertIndex { get; set; }

		public SmartCard SmartCard { get; set; }

		public SmartCardType SmartCardType { get; set; }

		public int SelectedSmartCard { get; set; }
	}
}
