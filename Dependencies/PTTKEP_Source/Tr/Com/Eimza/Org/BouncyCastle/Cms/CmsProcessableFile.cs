using System;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class CmsProcessableFile : CmsProcessable, CmsReadable
	{
		private const int DefaultBufSize = 32768;

		private readonly FileInfo _file;

		private readonly int _bufSize;

		public CmsProcessableFile(FileInfo file, int bufSize = 32768)
		{
			_file = file;
			_bufSize = bufSize;
		}

		public virtual Stream GetInputStream()
		{
			return new FileStream(_file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, _bufSize);
		}

		public virtual void Write(Stream zOut)
		{
			Stream inputStream = GetInputStream();
			Streams.PipeAll(inputStream, zOut);
			inputStream.Close();
		}

		[Obsolete]
		public virtual object GetContent()
		{
			return _file;
		}
	}
}
