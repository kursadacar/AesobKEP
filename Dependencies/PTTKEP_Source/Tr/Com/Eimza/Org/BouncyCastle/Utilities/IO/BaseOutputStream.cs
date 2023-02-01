using System;
using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO
{
	internal abstract class BaseOutputStream : Stream
	{
		private bool closed;

		public sealed override bool CanRead
		{
			get
			{
				return false;
			}
		}

		public sealed override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public sealed override bool CanWrite
		{
			get
			{
				return !closed;
			}
		}

		public sealed override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public sealed override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public override void Close()
		{
			closed = true;
		}

		public override void Flush()
		{
		}

		public sealed override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public sealed override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public sealed override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			int num = offset + count;
			for (int i = offset; i < num; i++)
			{
				WriteByte(buffer[i]);
			}
		}

		public virtual void Write(params byte[] buffer)
		{
			Write(buffer, 0, buffer.Length);
		}
	}
}
