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
	public class eyYukle : INotifyPropertyChanged
	{
		private eyKepHesapGirisP kepHesapField;

		private base64Binary ePaketField;

		private eyPaketTur ePaketTurField;

		private bool ePaketTurFieldSpecified;

		[XmlElement(Order = 0)]
		public eyKepHesapGirisP kepHesap
		{
			get
			{
				return kepHesapField;
			}
			set
			{
				kepHesapField = value;
				RaisePropertyChanged("kepHesap");
			}
		}

		[XmlElement(Order = 1)]
		public base64Binary ePaket
		{
			get
			{
				return ePaketField;
			}
			set
			{
				ePaketField = value;
				RaisePropertyChanged("ePaket");
			}
		}

		[XmlElement(Order = 2)]
		public eyPaketTur ePaketTur
		{
			get
			{
				return ePaketTurField;
			}
			set
			{
				ePaketTurField = value;
				RaisePropertyChanged("ePaketTur");
			}
		}

		[XmlIgnore]
		public bool ePaketTurSpecified
		{
			get
			{
				return ePaketTurFieldSpecified;
			}
			set
			{
				ePaketTurFieldSpecified = value;
				RaisePropertyChanged("ePaketTurSpecified");
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
