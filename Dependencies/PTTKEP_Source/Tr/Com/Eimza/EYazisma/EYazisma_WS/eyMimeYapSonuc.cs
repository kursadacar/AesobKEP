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
	public class eyMimeYapSonuc : INotifyPropertyChanged
	{
		private string durumField;

		private string hataaciklamaField;

		private string eHashField;

		private string mesajidField;

		[XmlElement(Order = 0)]
		public string durum
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
		public string eHash
		{
			get
			{
				return eHashField;
			}
			set
			{
				eHashField = value;
				RaisePropertyChanged("eHash");
			}
		}

		[XmlElement(Order = 3)]
		public string mesajid
		{
			get
			{
				return mesajidField;
			}
			set
			{
				mesajidField = value;
				RaisePropertyChanged("mesajid");
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
