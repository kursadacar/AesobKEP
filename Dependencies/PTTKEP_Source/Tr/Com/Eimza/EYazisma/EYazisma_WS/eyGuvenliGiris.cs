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
	public class eyGuvenliGiris : INotifyPropertyChanged
	{
		private eyKepHesapGirisP kepHesapField;

		private eyGirisTur girisTurField;

		private bool girisTurFieldSpecified;

		private string smsKeyField;

		private base64Binary eCadesBesField;

		private string eGuvenlikIdField;

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

		[XmlElement(Order = 2)]
		public string smsKey
		{
			get
			{
				return smsKeyField;
			}
			set
			{
				smsKeyField = value;
				RaisePropertyChanged("smsKey");
			}
		}

		[XmlElement(Order = 3)]
		public base64Binary eCadesBes
		{
			get
			{
				return eCadesBesField;
			}
			set
			{
				eCadesBesField = value;
				RaisePropertyChanged("eCadesBes");
			}
		}

		[XmlElement(Order = 4)]
		public string eGuvenlikId
		{
			get
			{
				return eGuvenlikIdField;
			}
			set
			{
				eGuvenlikIdField = value;
				RaisePropertyChanged("eGuvenlikId");
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
