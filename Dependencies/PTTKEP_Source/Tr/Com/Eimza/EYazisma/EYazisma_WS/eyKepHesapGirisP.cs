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
	public class eyKepHesapGirisP : INotifyPropertyChanged
	{
		private string kepHesapField;

		private string tcnoField;

		private string parolaField;

		private string sifreField;

		[XmlElement(Order = 0)]
		public string kepHesap
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
		public string tcno
		{
			get
			{
				return tcnoField;
			}
			set
			{
				tcnoField = value;
				RaisePropertyChanged("tcno");
			}
		}

		[XmlElement(Order = 2)]
		public string parola
		{
			get
			{
				return parolaField;
			}
			set
			{
				parolaField = value;
				RaisePropertyChanged("parola");
			}
		}

		[XmlElement(Order = 3)]
		public string sifre
		{
			get
			{
				return sifreField;
			}
			set
			{
				sifreField = value;
				RaisePropertyChanged("sifre");
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
