using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class CmsTypedStream
	{
		private class FullReaderStream : FilterStream
		{
			internal FullReaderStream(Stream input)
				: base(input)
			{
			}

			public override int Read(byte[] buf, int off, int len)
			{
				return Streams.ReadFully(s, buf, off, len);
			}
		}

		private const int BufferSize = 32768;

		private readonly string _oid;

		private readonly Stream _in;

		public string ContentType
		{
			get
			{
				return _oid;
			}
		}

		public Stream ContentStream
		{
			get
			{
				return _in;
			}
		}

		public CmsTypedStream(Stream inStream)
			: this(PkcsObjectIdentifiers.Data.Id, inStream)
		{
		}

		public CmsTypedStream(string oid, Stream inStream, int bufSize = 32768)
		{
			_oid = oid;
			_in = new FullReaderStream(new BufferedStream(inStream, bufSize));
		}

		public void Drain()
		{
			Streams.Drain(_in);
			_in.Close();
		}
	}
}
