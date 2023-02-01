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
	public class eyMimeYap : INotifyPropertyChanged
	{
		private eyKepHesapGirisP kepHesapField;

		private eyPaketTur ePaketTurField;

		private bool ePaketTurFieldSpecified;

		private string ePaketIdField;

		private string kimeField;

		private string konuField;

		private string icerikField;

		private eyIcerikTur eIcerikTurField;

		private bool eIcerikTurFieldSpecified;

		private base64Binary[] eklerField;

		private eyOzetAlg eOzetAlgField;

		private bool eOzetAlgFieldSpecified;

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
		public eyPaketTur ePaketTur
		{
			get
			{
				return ePaketTurField;
			}
			set
			{
				ePaketTurField = value;
				RaisePropertyChanged("ePaketTur");
			}
		}

		[XmlIgnore]
		public bool ePaketTurSpecified
		{
			get
			{
				return ePaketTurFieldSpecified;
			}
			set
			{
				ePaketTurFieldSpecified = value;
				RaisePropertyChanged("ePaketTurSpecified");
			}
		}

		[XmlElement(Order = 2)]
		public string ePaketId
		{
			get
			{
				return ePaketIdField;
			}
			set
			{
				ePaketIdField = value;
				RaisePropertyChanged("ePaketId");
			}
		}

		[XmlElement(Order = 3)]
		public string kime
		{
			get
			{
				return kimeField;
			}
			set
			{
				kimeField = value;
				RaisePropertyChanged("kime");
			}
		}

		[XmlElement(Order = 4)]
		public string konu
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

		[XmlElement(Order = 5)]
		public string icerik
		{
			get
			{
				return icerikField;
			}
			set
			{
				icerikField = value;
				RaisePropertyChanged("icerik");
			}
		}

		[XmlElement(Order = 6)]
		public eyIcerikTur eIcerikTur
		{
			get
			{
				return eIcerikTurField;
			}
			set
			{
				eIcerikTurField = value;
				RaisePropertyChanged("eIcerikTur");
			}
		}

		[XmlIgnore]
		public bool eIcerikTurSpecified
		{
			get
			{
				return eIcerikTurFieldSpecified;
			}
			set
			{
				eIcerikTurFieldSpecified = value;
				RaisePropertyChanged("eIcerikTurSpecified");
			}
		}

		[XmlElement("ekler", Order = 7)]
		public base64Binary[] ekler
		{
			get
			{
				return eklerField;
			}
			set
			{
				eklerField = value;
				RaisePropertyChanged("ekler");
			}
		}

		[XmlElement(Order = 8)]
		public eyOzetAlg eOzetAlg
		{
			get
			{
				return eOzetAlgField;
			}
			set
			{
				eOzetAlgField = value;
				RaisePropertyChanged("eOzetAlg");
			}
		}

		[XmlIgnore]
		public bool eOzetAlgSpecified
		{
			get
			{
				return eOzetAlgFieldSpecified;
			}
			set
			{
				eOzetAlgFieldSpecified = value;
				RaisePropertyChanged("eOzetAlgSpecified");
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
