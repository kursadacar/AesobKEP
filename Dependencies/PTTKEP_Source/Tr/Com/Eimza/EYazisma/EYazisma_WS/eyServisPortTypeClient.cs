using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Tr.Com.Eimza.EYazisma.EYazisma_WS
{
	[DebuggerStepThrough]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	public class eyServisPortTypeClient : ClientBase<eyServisPortType>, eyServisPortType
	{
		public eyServisPortTypeClient()
		{
		}

		public eyServisPortTypeClient(string endpointConfigurationName)
			: base(endpointConfigurationName)
		{
		}

		public eyServisPortTypeClient(string endpointConfigurationName, string remoteAddress)
			: base(endpointConfigurationName, remoteAddress)
		{
		}

		public eyServisPortTypeClient(string endpointConfigurationName, EndpointAddress remoteAddress)
			: base(endpointConfigurationName, remoteAddress)
		{
		}

		public eyServisPortTypeClient(Binding binding, EndpointAddress remoteAddress)
			: base(binding, remoteAddress)
		{
		}

		public eyYukleSonuc Yukle(eyYukle param0)
		{
			return base.Channel.Yukle(param0);
		}

		public eyYukleTebligatSonuc YukleTebligat(eyYukleTebligat param0)
		{
			return base.Channel.YukleTebligat(param0);
		}

		public eyTevdiListesiSonuc TevdiListesiOlustur(eyTevdiListesi param0)
		{
			return base.Channel.TevdiListesiOlustur(param0);
		}

		public eyDurumSonuc TevdiListesiSil(eyTevdiListesiSil param0)
		{
			return base.Channel.TevdiListesiSil(param0);
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		KontorSorgulaResponse eyServisPortType.KontorSorgula(KontorSorgulaRequest request)
		{
			return base.Channel.KontorSorgula(request);
		}

		public kontorSonuc KontorSorgula(eyKepHesapGirisP param0)
		{
			KontorSorgulaRequest kontorSorgulaRequest = new KontorSorgulaRequest();
			kontorSorgulaRequest.param0 = param0;
			return ((eyServisPortType)this).KontorSorgula(kontorSorgulaRequest).@return;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		KotaSorgulaResponse eyServisPortType.KotaSorgula(KotaSorgulaRequest request)
		{
			return base.Channel.KotaSorgula(request);
		}

		public int? KotaSorgula(eyKepHesapGirisP param0)
		{
			KotaSorgulaRequest kotaSorgulaRequest = new KotaSorgulaRequest();
			kotaSorgulaRequest.param0 = param0;
			return ((eyServisPortType)this).KotaSorgula(kotaSorgulaRequest).@return;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		DizinSorgulaResponse eyServisPortType.DizinSorgula(DizinSorgulaRequest request)
		{
			return base.Channel.DizinSorgula(request);
		}

		public string[] DizinSorgula(eyKepHesapGirisP param0)
		{
			DizinSorgulaRequest dizinSorgulaRequest = new DizinSorgulaRequest();
			dizinSorgulaRequest.param0 = param0;
			return ((eyServisPortType)this).DizinSorgula(dizinSorgulaRequest).@return;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		PaketDelilSorgulaResponse eyServisPortType.PaketDelilSorgula(PaketDelilSorgulaRequest request)
		{
			return base.Channel.PaketDelilSorgula(request);
		}

		public eyPaketDelilSonuc PaketDelilSorgula(eyPaketKepId param0)
		{
			PaketDelilSorgulaRequest paketDelilSorgulaRequest = new PaketDelilSorgulaRequest();
			paketDelilSorgulaRequest.param0 = param0;
			return ((eyServisPortType)this).PaketDelilSorgula(paketDelilSorgulaRequest).@return;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		PaketDelilIndirResponse eyServisPortType.PaketDelilIndir(PaketDelilIndirRequest request)
		{
			return base.Channel.PaketDelilIndir(request);
		}

		public eyPaketDelilIndirSonuc PaketDelilIndir(eyPaketDelilId param0)
		{
			PaketDelilIndirRequest paketDelilIndirRequest = new PaketDelilIndirRequest();
			paketDelilIndirRequest.param0 = param0;
			return ((eyServisPortType)this).PaketDelilIndir(paketDelilIndirRequest).@return;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		PaketSorgulaResponse eyServisPortType.PaketSorgula(PaketSorgulaRequest request)
		{
			return base.Channel.PaketSorgula(request);
		}

		public eyPaketSonuc PaketSorgula(eyPaketSorgula param0)
		{
			PaketSorgulaRequest paketSorgulaRequest = new PaketSorgulaRequest();
			paketSorgulaRequest.param0 = param0;
			return ((eyServisPortType)this).PaketSorgula(paketSorgulaRequest).@return;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		IndirResponse eyServisPortType.Indir(IndirRequest request)
		{
			return base.Channel.Indir(request);
		}

		public eyIndirSonuc Indir(eyIndir param0)
		{
			IndirRequest indirRequest = new IndirRequest();
			indirRequest.param0 = param0;
			return ((eyServisPortType)this).Indir(indirRequest).@return;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		AlindiOnayResponse eyServisPortType.AlindiOnay(AlindiOnayRequest request)
		{
			return base.Channel.AlindiOnay(request);
		}

		public eyDurumSonuc AlindiOnay(eyPaketKepId param0)
		{
			AlindiOnayRequest alindiOnayRequest = new AlindiOnayRequest();
			alindiOnayRequest.param0 = param0;
			return ((eyServisPortType)this).AlindiOnay(alindiOnayRequest).@return;
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		PaketSilResponse eyServisPortType.PaketSil(PaketSilRequest request)
		{
			return base.Channel.PaketSil(request);
		}

		public eyDurumSonuc PaketSil(eyPaketKepId param0)
		{
			PaketSilRequest paketSilRequest = new PaketSilRequest();
			paketSilRequest.param0 = param0;
			return ((eyServisPortType)this).PaketSil(paketSilRequest).@return;
		}

		public eyGirisSonuc Giris(eyGiris param0)
		{
			return base.Channel.Giris(param0);
		}

		public eyGuvenliGirisSonuc GuvenliGiris(eyGuvenliGiris param0)
		{
			return base.Channel.GuvenliGiris(param0);
		}

		public eyYetkiliKayitSonuc YetkiliKayit(eyYetkiliKayit param0)
		{
			return base.Channel.YetkiliKayit(param0);
		}

		public eyDurumSonuc YetkiliSil(eyYetkiliSil param0)
		{
			return base.Channel.YetkiliSil(param0);
		}

		public eyMimeYapSonuc MimeYap(eyMimeYap param0)
		{
			return base.Channel.MimeYap(param0);
		}

		public eyDurumSonuc SmimeGonder(eySmimeGonder param0)
		{
			return base.Channel.SmimeGonder(param0);
		}
	}
}
