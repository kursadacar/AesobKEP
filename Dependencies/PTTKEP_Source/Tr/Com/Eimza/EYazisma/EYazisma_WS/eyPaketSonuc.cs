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
	public class eyPaketSonuc : INotifyPropertyChanged
	{
		private int?[] durumField;

		private string[] kepIdField;

		private string[] orgMesajIdField;

		private int?[] kepSiraNoField;

		private string[] fromField;

		private string[] fromKepField;

		private string[] konuField;

		private string[] turField;

		private string[] turIdField;

		private string[] hataaciklamaField;

		[XmlElement("durum", IsNullable = true, Order = 0)]
		public int?[] durum
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

		[XmlElement("kepId", IsNullable = true, Order = 1)]
		public string[] kepId
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

		[XmlElement("OrgMesajId", IsNullable = true, Order = 2)]
		public string[] OrgMesajId
		{
			get
			{
				return orgMesajIdField;
			}
			set
			{
				orgMesajIdField = value;
				RaisePropertyChanged("OrgMesajId");
			}
		}

		[XmlElement("kepSiraNo", IsNullable = true, Order = 3)]
		public int?[] kepSiraNo
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

		[XmlElement("from", IsNullable = true, Order = 4)]
		public string[] from
		{
			get
			{
				return fromField;
			}
			set
			{
				fromField = value;
				RaisePropertyChanged("from");
			}
		}

		[XmlElement("fromKep", IsNullable = true, Order = 5)]
		public string[] fromKep
		{
			get
			{
				return fromKepField;
			}
			set
			{
				fromKepField = value;
				RaisePropertyChanged("fromKep");
			}
		}

		[XmlElement("konu", IsNullable = true, Order = 6)]
		public string[] konu
		{
			get
			{
				return konuField;
			}
			set
			{
				konuField = value;
				RaisePropertyChanged("konu");
			}
		}

		[XmlElement("tur", IsNullable = true, Order = 7)]
		public string[] tur
		{
			get
			{
				return turField;
			}
			set
			{
				turField = value;
				RaisePropertyChanged("tur");
			}
		}

		[XmlElement("turId", IsNullable = true, Order = 8)]
		public string[] turId
		{
			get
			{
				return turIdField;
			}
			set
			{
				turIdField = value;
				RaisePropertyChanged("turId");
			}
		}

		[XmlElement("hataaciklama", IsNullable = true, Order = 9)]
		public string[] hataaciklama
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
