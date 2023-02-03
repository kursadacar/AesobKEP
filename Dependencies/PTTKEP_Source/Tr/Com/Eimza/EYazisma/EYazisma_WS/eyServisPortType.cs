using System.CodeDom.Compiler;
using System.ServiceModel;

namespace Tr.Com.Eimza.EYazisma.EYazisma_WS
{
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[ServiceContract(Namespace = "http://ws.apache.org/axis2", ConfigurationName = "EYazisma_WS.eyServisPortType", Name = "eyServisSOAPport_http")]
	public interface eyServisPortType
	{
		[OperationContract(Action = "urn:Yukle", ReplyAction = "urn:YukleResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		eyYukleSonuc Yukle(eyYukle param0);

		[OperationContract(Action = "urn:YukleTebligat", ReplyAction = "urn:YukleTebligatResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		eyYukleTebligatSonuc YukleTebligat(eyYukleTebligat param0);

		[OperationContract(Action = "urn:TevdiListesiOlustur", ReplyAction = "urn:TevdiListesiOlusturResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		eyTevdiListesiSonuc TevdiListesiOlustur(eyTevdiListesi param0);

		[OperationContract(Action = "urn:TevdiListesiSil", ReplyAction = "urn:TevdiListesiSilResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		eyDurumSonuc TevdiListesiSil(eyTevdiListesiSil param0);

		[OperationContract(Action = "urn:KontorSorgula", ReplyAction = "urn:KontorSorgulaResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		KontorSorgulaResponse KontorSorgula(KontorSorgulaRequest request);

		[OperationContract(Action = "urn:KotaSorgula", ReplyAction = "urn:KotaSorgulaResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		KotaSorgulaResponse KotaSorgula(KotaSorgulaRequest request);

		[OperationContract(Action = "urn:DizinSorgula", ReplyAction = "urn:DizinSorgulaResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		DizinSorgulaResponse DizinSorgula(DizinSorgulaRequest request);

		[OperationContract(Action = "urn:PaketDelilSorgula", ReplyAction = "urn:PaketDelilSorgulaResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		PaketDelilSorgulaResponse PaketDelilSorgula(PaketDelilSorgulaRequest request);

		[OperationContract(Action = "urn:PaketDelilIndir", ReplyAction = "urn:PaketDelilIndirResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		PaketDelilIndirResponse PaketDelilIndir(PaketDelilIndirRequest request);

		[OperationContract(Action = "urn:PaketSorgula", ReplyAction = "urn:PaketSorgulaResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		PaketSorgulaResponse PaketSorgula(PaketSorgulaRequest request);

		[OperationContract(Action = "urn:Indir", ReplyAction = "urn:IndirResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		IndirResponse Indir(IndirRequest request);

		[OperationContract(Action = "urn:AlindiOnay", ReplyAction = "urn:AlindiOnayResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		AlindiOnayResponse AlindiOnay(AlindiOnayRequest request);

		[OperationContract(Action = "urn:PaketSil", ReplyAction = "urn:PaketSilResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		PaketSilResponse PaketSil(PaketSilRequest request);

		[OperationContract(Action = "urn:Giris", ReplyAction = "urn:GirisResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		eyGirisSonuc Giris(eyGiris param0);

		[OperationContract(Action = "urn:GuvenliGiris", ReplyAction = "urn:GuvenliGirisResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		eyGuvenliGirisSonuc GuvenliGiris(eyGuvenliGiris param0);

		[OperationContract(Action = "urn:YetkiliKayit", ReplyAction = "urn:YetkiliKayitResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		eyYetkiliKayitSonuc YetkiliKayit(eyYetkiliKayit param0);

		[OperationContract(Action = "urn:YetkiliSil", ReplyAction = "urn:YetkiliSilResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		eyDurumSonuc YetkiliSil(eyYetkiliSil param0);

		[OperationContract(Action = "urn:MimeYap", ReplyAction = "urn:MimeYapResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		eyMimeYapSonuc MimeYap(eyMimeYap param0);

		[OperationContract(Action = "urn:SmimeGonder", ReplyAction = "urn:SmimeGonderResponse")]
		[XmlSerializerFormat(SupportFaults = true)]
		[return: MessageParameter(Name = "return")]
		eyDurumSonuc SmimeGonder(eySmimeGonder param0);
	}
}
