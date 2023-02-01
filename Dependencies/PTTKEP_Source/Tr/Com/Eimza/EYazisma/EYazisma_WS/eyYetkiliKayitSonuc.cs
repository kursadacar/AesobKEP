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
	public class eyYetkiliKayitSonuc : INotifyPropertyChanged
	{
		private int? durumField;

		private bool durumFieldSpecified;

		private string hataaciklamaField;

		private string parolaField;

		private string sifreField;

		[XmlElement(IsNullable = true, Order = 0)]
		public int? durum
		{
			get
			{
				return durumField;
			}
			set
			{
				durumField = value;
				RaisePropertyChanged("durum");
			}
		}

		[XmlIgnore]
		public bool durumSpecified
		{
			get
			{
				return durumFieldSpecified;
			}
			set
			{
				durumFieldSpecified = value;
				RaisePropertyChanged("durumSpecified");
			}
		}

		[XmlElement(IsNullable = true, Order = 1)]
		public string hataaciklama
		{
			get
			{
				return hataaciklamaField;
			}
			set
			{
				hataaciklamaField = value;
				RaisePropertyChanged("hataaciklama");
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
