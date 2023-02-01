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
	public class eyYukleSonuc : INotifyPropertyChanged
	{
		private int durumField;

		private bool durumFieldSpecified;

		private string kepIdField;

		private string hataaciklamaField;

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
