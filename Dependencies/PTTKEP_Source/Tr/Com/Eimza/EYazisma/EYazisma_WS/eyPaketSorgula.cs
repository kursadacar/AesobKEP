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
	public class eyPaketSorgula : INotifyPropertyChanged
	{
		private eyKepHesapGirisP kepHesapField;

		private DateTime ilktarihField;

		private bool ilktarihFieldSpecified;

		private DateTime sontarihField;

		private bool sontarihFieldSpecified;

		private string dizinField;

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
		public DateTime ilktarih
		{
			get
			{
				return ilktarihField;
			}
			set
			{
				ilktarihField = value;
				RaisePropertyChanged("ilktarih");
			}
		}

		[XmlIgnore]
		public bool ilktarihSpecified
		{
			get
			{
				return ilktarihFieldSpecified;
			}
			set
			{
				ilktarihFieldSpecified = value;
				RaisePropertyChanged("ilktarihSpecified");
			}
		}

		[XmlElement(Order = 2)]
		public DateTime sontarih
		{
			get
			{
				return sontarihField;
			}
			set
			{
				sontarihField = value;
				RaisePropertyChanged("sontarih");
			}
		}

		[XmlIgnore]
		public bool sontarihSpecified
		{
			get
			{
				return sontarihFieldSpecified;
			}
			set
			{
				sontarihFieldSpecified = value;
				RaisePropertyChanged("sontarihSpecified");
			}
		}

		[XmlElement(Order = 3)]
		[DefaultValue("INBOX")]
		public string dizin
		{
			get
			{
				return dizinField;
			}
			set
			{
				dizinField = value;
				RaisePropertyChanged("dizin");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public eyPaketSorgula()
		{
			dizinField = "INBOX";
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
