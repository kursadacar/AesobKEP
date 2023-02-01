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
	[MessageContract(WrapperName = "DizinSorgulaResponse", WrapperNamespace = "http://ws.apache.org/axis2", IsWrapped = true)]
	public class DizinSorgulaResponse
	{
		[MessageBodyMember(Namespace = "http://ws.apache.org/axis2", Order = 0)]
		[XmlElement("return", IsNullable = true)]
		public string[] @return;

		public DizinSorgulaResponse()
		{
		}

		public DizinSorgulaResponse(string[] @return)
		{
			this.@return = @return;
		}
	}
}
