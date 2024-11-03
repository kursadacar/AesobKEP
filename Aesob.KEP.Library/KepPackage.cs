using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Aesob.KEP.Library
{
	[Serializable]
	public class KepPackage
	{
		public string ContentType;
		public string FileName;
		public int ValueLength;
		public byte[] Value;

		public KepPackage(string contentType, string fileName, byte[] value)
		{
			ContentType = contentType;
			FileName = fileName;
			Value = value;
			ValueLength = value.Length;
		}
	}
}
