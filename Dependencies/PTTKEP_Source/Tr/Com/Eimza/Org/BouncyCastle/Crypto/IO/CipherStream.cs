using System;
using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.IO
{
	internal class CipherStream : Stream
	{
		internal Stream stream;

		internal IBufferedCipher inCipher;

		internal IBufferedCipher outCipher;

		private byte[] mInBuf;

		private int mInPos;

		private bool inStreamEnded;

		public IBufferedCipher ReadCipher
		{
			get
			{
				return inCipher;
			}
		}

		public IBufferedCipher WriteCipher
		{
			get
			{
				return outCipher;
			}
		}

		public override bool CanRead
		{
			get
			{
				if (stream.CanRead)
				{
					return inCipher != null;
				}
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				if (stream.CanWrite)
				{
					return outCipher != null;
				}
				return false;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
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

		public CipherStream(Stream stream, IBufferedCipher readCipher, IBufferedCipher writeCipher)
		{
			this.stream = stream;
			if (readCipher != null)
			{
				inCipher = readCipher;
				mInBuf = null;
			}
			if (writeCipher != null)
			{
				outCipher = writeCipher;
			}
		}

		public override int ReadByte()
		{
			if (inCipher == null)
			{
				return stream.ReadByte();
			}
			if ((mInBuf == null || mInPos >= mInBuf.Length) && !FillInBuf())
			{
				return -1;
			}
			return mInBuf[mInPos++];
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (inCipher == null)
			{
				return stream.Read(buffer, offset, count);
			}
			int i;
			int num;
			for (i = 0; i < count; i += num)
			{
				if ((mInBuf == null || mInPos >= mInBuf.Length) && !FillInBuf())
				{
					break;
				}
				num = System.Math.Min(count - i, mInBuf.Length - mInPos);
				Array.Copy(mInBuf, mInPos, buffer, offset + i, num);
				mInPos += num;
			}
			return i;
		}

		private bool FillInBuf()
		{
			if (inStreamEnded)
			{
				return false;
			}
			mInPos = 0;
			do
			{
				mInBuf = ReadAndProcessBlock();
			}
			while (!inStreamEnded && mInBuf == null);
			return mInBuf != null;
		}

		private byte[] ReadAndProcessBlock()
		{
			int blockSize = inCipher.GetBlockSize();
			byte[] array = new byte[(blockSize == 0) ? 256 : blockSize];
			int num = 0;
			do
			{
				int num2 = stream.Read(array, num, array.Length - num);
				if (num2 < 1)
				{
					inStreamEnded = true;
					break;
				}
				num += num2;
			}
			while (num < array.Length);
			byte[] array2 = (inStreamEnded ? inCipher.DoFinal(array, 0, num) : inCipher.ProcessBytes(array));
			if (array2 != null && array2.Length == 0)
			{
				array2 = null;
			}
			return array2;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			if (outCipher == null)
			{
				stream.Write(buffer, offset, count);
				return;
			}
			byte[] array = outCipher.ProcessBytes(buffer, offset, count);
			if (array != null)
			{
				stream.Write(array, 0, array.Length);
			}
		}

		public override void WriteByte(byte b)
		{
			if (outCipher == null)
			{
				stream.WriteByte(b);
				return;
			}
			byte[] array = outCipher.ProcessByte(b);
			if (array != null)
			{
				stream.Write(array, 0, array.Length);
			}
		}

		public override void Close()
		{
			if (outCipher != null)
			{
				byte[] array = outCipher.DoFinal();
				stream.Write(array, 0, array.Length);
				stream.Flush();
			}
			stream.Close();
		}

		public override void Flush()
		{
			stream.Flush();
		}

		public sealed override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public sealed override void SetLength(long length)
		{
			throw new NotSupportedException();
		}
	}
}
