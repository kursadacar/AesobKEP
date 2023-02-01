namespace Tr.Com.Eimza.EYazisma
{
	public class EyMimeOlusturSonuc
	{
		public byte[] Icerik { get; set; }

		public string HashBase64 { get; set; }

		public byte[] Hash { get; set; }

		public byte[] KepIletisi { get; set; }

		public EyMimeMessage MailBilgileri { get; set; }

		public string HataAciklama { get; set; }

		public OzetAlg OzetAlgoritmasi { get; set; }

		public int Durum { get; set; }

		public EyMimeOlusturSonuc()
		{
			Durum = 0;
		}
	}
}
