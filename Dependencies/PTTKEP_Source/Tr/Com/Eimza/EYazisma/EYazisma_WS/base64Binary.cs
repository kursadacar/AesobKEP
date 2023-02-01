using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Tr.Com.Eimza.EYazisma.EYazisma_WS
{
	[Serializable]
	[GeneratedCode("System.Xml", "4.6.81.0")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[XmlType(Namespace = "http://www.w3.org/2005/05/xmlmime")]
	public class base64Binary : INotifyPropertyChanged
	{
		private string contentTypeField;

		private string fileNameField;

		private byte[] valueField;

		[XmlAttribute(Form = XmlSchemaForm.Qualified)]
		public string contentType
		{
			get
			{
				return contentTypeField;
			}
			set
			{
				contentTypeField = value;
				RaisePropertyChanged("contentType");
			}
		}

		[XmlAttribute(Form = XmlSchemaForm.Qualified)]
		public string fileName
		{
			get
			{
				return fileNameField;
			}
			set
			{
				fileNameField = value;
				RaisePropertyChanged("fileName");
			}
		}

		[XmlText(DataType = "base64Binary")]
		public byte[] Value
		{
			get
			{
				return valueField;
			}
			set
			{
				valueField = value;
				RaisePropertyChanged("Value");
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
