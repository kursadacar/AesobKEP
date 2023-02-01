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
	public class kontorSonuc : INotifyPropertyChanged
	{
		private decimal miktarField;

		private bool miktarFieldSpecified;

		private string cinsField;

		[XmlElement(Order = 0)]
		public decimal miktar
		{
			get
			{
				return miktarField;
			}
			set
			{
				miktarField = value;
				RaisePropertyChanged("miktar");
			}
		}

		[XmlIgnore]
		public bool miktarSpecified
		{
			get
			{
				return miktarFieldSpecified;
			}
			set
			{
				miktarFieldSpecified = value;
				RaisePropertyChanged("miktarSpecified");
			}
		}

		[XmlElement(Order = 1)]
		public string cins
		{
			get
			{
				return cinsField;
			}
			set
			{
				cinsField = value;
				RaisePropertyChanged("cins");
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
