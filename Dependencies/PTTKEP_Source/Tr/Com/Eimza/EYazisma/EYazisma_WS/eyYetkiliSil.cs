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
	public class eyYetkiliSil : INotifyPropertyChanged
	{
		private eyKepHesapGirisP kepHesapField;

		private string kepYetkiliTCNOField;

		private string kepYetkiliAdiField;

		private string kepYetkiliSoyadiField;

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
		public string kepYetkiliTCNO
		{
			get
			{
				return kepYetkiliTCNOField;
			}
			set
			{
				kepYetkiliTCNOField = value;
				RaisePropertyChanged("kepYetkiliTCNO");
			}
		}

		[XmlElement(Order = 2)]
		public string kepYetkiliAdi
		{
			get
			{
				return kepYetkiliAdiField;
			}
			set
			{
				kepYetkiliAdiField = value;
				RaisePropertyChanged("kepYetkiliAdi");
			}
		}

		[XmlElement(Order = 3)]
		public string kepYetkiliSoyadi
		{
			get
			{
				return kepYetkiliSoyadiField;
			}
			set
			{
				kepYetkiliSoyadiField = value;
				RaisePropertyChanged("kepYetkiliSoyadi");
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
