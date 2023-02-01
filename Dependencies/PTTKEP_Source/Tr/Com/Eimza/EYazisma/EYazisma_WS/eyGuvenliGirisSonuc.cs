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
	public class eyGuvenliGirisSonuc : INotifyPropertyChanged
	{
		private string durumField;

		private string hataaciklamaField;

		private string eGuvenlikIdField;

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
