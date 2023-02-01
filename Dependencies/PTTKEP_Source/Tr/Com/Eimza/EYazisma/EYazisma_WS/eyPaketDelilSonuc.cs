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
	public class eyPaketDelilSonuc : INotifyPropertyChanged
	{
		private int?[] durumField;

		private string[] delilIdField;

		private int?[] delilTurIdField;

		private string[] delilaciklamaField;

		private DateTime?[] tarihField;

		private string[] kephsField;

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

		[XmlElement("delilId", IsNullable = true, Order = 1)]
		public string[] delilId
		{
			get
			{
				return delilIdField;
			}
			set
			{
				delilIdField = value;
				RaisePropertyChanged("delilId");
			}
		}

		[XmlElement("delilTurId", IsNullable = true, Order = 2)]
		public int?[] delilTurId
		{
			get
			{
				return delilTurIdField;
			}
			set
			{
				delilTurIdField = value;
				RaisePropertyChanged("delilTurId");
			}
		}

		[XmlElement("delilaciklama", IsNullable = true, Order = 3)]
		public string[] delilaciklama
		{
			get
			{
				return delilaciklamaField;
			}
			set
			{
				delilaciklamaField = value;
				RaisePropertyChanged("delilaciklama");
			}
		}

		[XmlElement("tarih", IsNullable = true, Order = 4)]
		public DateTime?[] tarih
		{
			get
			{
				return tarihField;
			}
			set
			{
				tarihField = value;
				RaisePropertyChanged("tarih");
			}
		}

		[XmlElement("kephs", IsNullable = true, Order = 5)]
		public string[] kephs
		{
			get
			{
				return kephsField;
			}
			set
			{
				kephsField = value;
				RaisePropertyChanged("kephs");
			}
		}

		[XmlElement("hataaciklama", IsNullable = true, Order = 6)]
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
