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
	public class eyGiris : INotifyPropertyChanged
	{
		private eyKepHesapGirisP kepHesapField;

		private eyGirisTur girisTurField;

		private bool girisTurFieldSpecified;

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
		public eyGirisTur girisTur
		{
			get
			{
				return girisTurField;
			}
			set
			{
				girisTurField = value;
				RaisePropertyChanged("girisTur");
			}
		}

		[XmlIgnore]
		public bool girisTurSpecified
		{
			get
			{
				return girisTurFieldSpecified;
			}
			set
			{
				girisTurFieldSpecified = value;
				RaisePropertyChanged("girisTurSpecified");
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
