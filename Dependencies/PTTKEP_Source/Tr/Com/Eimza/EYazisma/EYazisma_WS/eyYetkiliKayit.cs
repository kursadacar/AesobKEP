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
	public class eyYetkiliKayit : INotifyPropertyChanged
	{
		private eyKepHesapGirisP kepHesapField;

		private string kepYetkiliTCNOField;

		private string kepYetkiliTelField;

		private string kepYetkiliEPostaField;

		private string kepYetkiliAdiField;

		private string kepYetkiliSoyadiField;

		private string islemTipField;

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
		public string kepYetkiliTel
		{
			get
			{
				return kepYetkiliTelField;
			}
			set
			{
				kepYetkiliTelField = value;
				RaisePropertyChanged("kepYetkiliTel");
			}
		}

		[XmlElement(Order = 3)]
		public string kepYetkiliEPosta
		{
			get
			{
				return kepYetkiliEPostaField;
			}
			set
			{
				kepYetkiliEPostaField = value;
				RaisePropertyChanged("kepYetkiliEPosta");
			}
		}

		[XmlElement(Order = 4)]
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

		[XmlElement(Order = 5)]
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

		[XmlElement(Order = 6)]
		[DefaultValue("INSERT")]
		public string islemTip
		{
			get
			{
				return islemTipField;
			}
			set
			{
				islemTipField = value;
				RaisePropertyChanged("islemTip");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public eyYetkiliKayit()
		{
			islemTipField = "INSERT";
		}

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
