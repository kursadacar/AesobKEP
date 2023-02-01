using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Tr.Com.Eimza.EYazisma.EYazisma_WS
{
	[Serializable]
	[GeneratedCode("System.Xml", "4.6.81.0")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[XmlType(Namespace = "http://ws.apache.org/axis2/xsd")]
	public class eyDonusBarkod : INotifyPropertyChanged
	{
		private string aliciField;

		private string barkodField;

		[XmlElement(Order = 0)]
		public string Alici
		{
			get
			{
				return aliciField;
			}
			set
			{
				aliciField = value;
				RaisePropertyChanged("Alici");
			}
		}

		[XmlElement(Order = 1)]
		public string Barkod
		{
			get
			{
				return barkodField;
			}
			set
			{
				barkodField = value;
				RaisePropertyChanged("Barkod");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
