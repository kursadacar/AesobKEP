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
	public class eyPaketKepId : INotifyPropertyChanged
	{
		private eyKepHesapGirisP kepHesapField;

		private string kepIdField;

		private int kepSiraNoField;

		private bool kepSiraNoFieldSpecified;

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
		public string kepId
		{
			get
			{
				return kepIdField;
			}
			set
			{
				kepIdField = value;
				RaisePropertyChanged("kepId");
			}
		}

		[XmlElement(Order = 2)]
		public int kepSiraNo
		{
			get
			{
				return kepSiraNoField;
			}
			set
			{
				kepSiraNoField = value;
				RaisePropertyChanged("kepSiraNo");
			}
		}

		[XmlIgnore]
		public bool kepSiraNoSpecified
		{
			get
			{
				return kepSiraNoFieldSpecified;
			}
			set
			{
				kepSiraNoFieldSpecified = value;
				RaisePropertyChanged("kepSiraNoSpecified");
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

		public eyPaketKepId()
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
