using System;
using System.IO;

namespace Tr.Com.Eimza.Smime
{
	internal class SMailAttachment
	{
		internal byte[] RawBytes { get; private set; }

		public SMailContentType ContentType { get; private set; }

		public SMailAttachment(string fileName)
		{
			RawBytes = File.ReadAllBytes(fileName);
			ContentType = new SMailContentType();
			ContentType.Name = Path.GetFileName(fileName);
		}

		public SMailAttachment(string fileName, string mediaType)
		{
			RawBytes = File.ReadAllBytes(fileName);
			ContentType = new SMailContentType(mediaType);
			ContentType.Name = Path.GetFileName(fileName);
		}

		public SMailAttachment(string fileName, SMailContentType contentType)
		{
			if (contentType == null)
			{
				throw new ArgumentNullException("contentType");
			}
			RawBytes = File.ReadAllBytes(fileName);
			ContentType = contentType;
			ContentType.Name = Path.GetFileName(fileName);
		}

		public SMailAttachment(Stream contentStream, string name)
		{
			if (contentStream == null)
			{
				throw new ArgumentNullException("contentStream");
			}
			BinaryReader binaryReader = new BinaryReader(contentStream);
			contentStream.Position = 0L;
			RawBytes = binaryReader.ReadBytes((int)contentStream.Length);
			ContentType = new SMailContentType();
			ContentType.Name = name;
		}

		public SMailAttachment(Stream contentStream, SMailContentType contentType)
		{
			if (contentStream == null)
			{
				throw new ArgumentNullException("contentStream");
			}
			BinaryReader binaryReader = new BinaryReader(contentStream);
			contentStream.Position = 0L;
			RawBytes = binaryReader.ReadBytes((int)contentStream.Length);
			ContentType = contentType;
		}

		public SMailAttachment(Stream contentStream, string name, string mediaType)
		{
			if (contentStream == null)
			{
				throw new ArgumentNullException("contentStream");
			}
			BinaryReader binaryReader = new BinaryReader(contentStream);
			contentStream.Position = 0L;
			RawBytes = binaryReader.ReadBytes((int)contentStream.Length);
			ContentType = new SMailContentType(mediaType);
			ContentType.Name = name;
		}

		public SMailAttachment(byte[] contentBytes, string name)
		{
			RawBytes = contentBytes;
			ContentType = new SMailContentType();
			ContentType.Name = name;
		}

		public SMailAttachment(byte[] contentBytes, SMailContentType contentType)
		{
			RawBytes = contentBytes;
			ContentType = contentType;
		}

		public SMailAttachment(byte[] contentBytes, string name, string mediaType)
		{
			RawBytes = contentBytes;
			ContentType = new SMailContentType(mediaType);
			ContentType.Name = name;
		}
	}
}
