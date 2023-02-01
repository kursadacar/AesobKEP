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
	public class eyYukleTebligat : INotifyPropertyChanged
	{
		private eyKepHesapGirisP kepHesapField;

		private base64Binary ePaketField;

		private string birimIdField;

		private string birimAdiField;

		private string barkodField;

		private bool donusluField;

		private bool donusluFieldSpecified;

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
		public string BirimId
		{
			get
			{
				return birimIdField;
			}
			set
			{
				birimIdField = value;
				RaisePropertyChanged("BirimId");
			}
		}

		[XmlElement(Order = 3)]
		public string BirimAdi
		{
			get
			{
				return birimAdiField;
			}
			set
			{
				birimAdiField = value;
				RaisePropertyChanged("BirimAdi");
			}
		}

		[XmlElement(Order = 4)]
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

		[XmlElement(Order = 5)]
		public bool Donuslu
		{
			get
			{
				return donusluField;
			}
			set
			{
				donusluField = value;
				RaisePropertyChanged("Donuslu");
			}
		}

		[XmlIgnore]
		public bool DonusluSpecified
		{
			get
			{
				return donusluFieldSpecified;
			}
			set
			{
				donusluFieldSpecified = value;
				RaisePropertyChanged("DonusluSpecified");
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
