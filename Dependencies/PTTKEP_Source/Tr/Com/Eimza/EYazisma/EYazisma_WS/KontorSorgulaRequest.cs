using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.Xml.Serialization;

namespace Tr.Com.Eimza.EYazisma.EYazisma_WS
{
	[DebuggerStepThrough]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[MessageContract(WrapperName = "KontorSorgula", WrapperNamespace = "http://ws.apache.org/axis2", IsWrapped = true)]
	public class KontorSorgulaRequest
	{
		[MessageBodyMember(Namespace = "http://ws.apache.org/axis2", Order = 0)]
		[XmlElement(IsNullable = true)]
		public eyKepHesapGirisP param0;

		public KontorSorgulaRequest()
		{
		}

		public KontorSorgulaRequest(eyKepHesapGirisP param0)
		{
			this.param0 = param0;
		}
	}
}
