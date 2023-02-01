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
	public class eyTevdiListesiSonuc : INotifyPropertyChanged
	{
		private int durumField;

		private bool durumFieldSpecified;

		private string hataaciklamaField;

		private string tevdiListeNoField;

		private decimal toplamUcretField;

		private bool toplamUcretFieldSpecified;

		private string[] barkodField;

		[XmlElement(Order = 0)]
		public int durum
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

		[XmlElement(Order = 1)]
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
		public string TevdiListeNo
		{
			get
			{
				return tevdiListeNoField;
			}
			set
			{
				tevdiListeNoField = value;
				RaisePropertyChanged("TevdiListeNo");
			}
		}

		[XmlElement(Order = 3)]
		public decimal ToplamUcret
		{
			get
			{
				return toplamUcretField;
			}
			set
			{
				toplamUcretField = value;
				RaisePropertyChanged("ToplamUcret");
			}
		}

		[XmlIgnore]
		public bool ToplamUcretSpecified
		{
			get
			{
				return toplamUcretFieldSpecified;
			}
			set
			{
				toplamUcretFieldSpecified = value;
				RaisePropertyChanged("ToplamUcretSpecified");
			}
		}

		[XmlElement("Barkod", Order = 4)]
		public string[] Barkod
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
