using System;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Text;

namespace Tr.Com.Eimza.Smime
{
	internal class SMailContentType
	{
		public string Name
		{
			get
			{
				return InternalContentType.Name;
			}
			set
			{
				Encoding enc = ((CharSet != null) ? Encoding.GetEncoding(CharSet) : Encoding.UTF8);
				InternalContentType.Name = SMailTransferEncoder.WrapIfEspecialsExist(SMailTransferEncoder.EncodeSubjectRFC2047(value, enc));
			}
		}

		public StringDictionary Parameters
		{
			get
			{
				return InternalContentType.Parameters;
			}
		}

		public string MediaType
		{
			get
			{
				return InternalContentType.MediaType;
			}
			set
			{
				InternalContentType.MediaType = value;
			}
		}

		public string CharSet
		{
			get
			{
				return InternalContentType.CharSet;
			}
			set
			{
				InternalContentType.CharSet = value;
			}
		}

		internal string Boundary
		{
			get
			{
				return InternalContentType.Boundary;
			}
			set
			{
				InternalContentType.Boundary = value;
			}
		}

		internal ContentType InternalContentType { get; private set; }

		public SMailContentType()
		{
			InternalContentType = new ContentType();
		}

		public SMailContentType(string contentType)
		{
			InternalContentType = new ContentType(contentType);
		}

		internal void GenerateBoundary()
		{
			Boundary = "------=_Part_" + Guid.NewGuid().ToString();
		}

		public override string ToString()
		{
			return InternalContentType.ToString();
		}
	}
}
