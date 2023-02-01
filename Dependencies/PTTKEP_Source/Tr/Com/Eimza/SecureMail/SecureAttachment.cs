using System;
using System.IO;

namespace Tr.Com.Eimza.SecureMail
{
	internal class SecureAttachment
	{
		internal byte[] RawBytes { get; private set; }

		public SecureContentType ContentType { get; private set; }

		public SecureAttachment(string fileName)
		{
			RawBytes = File.ReadAllBytes(fileName);
			ContentType = new SecureContentType();
			ContentType.Name = Path.GetFileName(fileName);
		}

		public SecureAttachment(string fileName, string mediaType)
		{
			RawBytes = File.ReadAllBytes(fileName);
			ContentType = new SecureContentType(mediaType);
			ContentType.Name = Path.GetFileName(fileName);
		}

		public SecureAttachment(string fileName, SecureContentType contentType)
		{
			if (contentType == null)
			{
				throw new ArgumentNullException("contentType");
			}
			RawBytes = File.ReadAllBytes(fileName);
			ContentType = contentType;
			ContentType.Name = Path.GetFileName(fileName);
		}

		public SecureAttachment(Stream contentStream, string name)
		{
			if (contentStream == null)
			{
				throw new ArgumentNullException("contentStream");
			}
			BinaryReader binaryReader = new BinaryReader(contentStream);
			contentStream.Position = 0L;
			RawBytes = binaryReader.ReadBytes((int)contentStream.Length);
			ContentType = new SecureContentType();
			ContentType.Name = name;
		}

		public SecureAttachment(Stream contentStream, SecureContentType contentType)
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

		public SecureAttachment(Stream contentStream, string name, string mediaType)
		{
			if (contentStream == null)
			{
				throw new ArgumentNullException("contentStream");
			}
			BinaryReader binaryReader = new BinaryReader(contentStream);
			contentStream.Position = 0L;
			RawBytes = binaryReader.ReadBytes((int)contentStream.Length);
			ContentType = new SecureContentType(mediaType);
			ContentType.Name = name;
		}

		public SecureAttachment(byte[] contentBytes, string name)
		{
			RawBytes = contentBytes;
			ContentType = new SecureContentType();
			ContentType.Name = name;
		}

		public SecureAttachment(byte[] contentBytes, SecureContentType contentType)
		{
			RawBytes = contentBytes;
			ContentType = contentType;
		}

		public SecureAttachment(byte[] contentBytes, string name, string mediaType)
		{
			RawBytes = contentBytes;
			ContentType = new SecureContentType(mediaType);
			ContentType.Name = name;
		}
	}
}
