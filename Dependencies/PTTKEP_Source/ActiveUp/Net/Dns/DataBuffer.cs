using System.Net;
using System.Text;

namespace ActiveUp.Net.Dns
{
	public class DataBuffer
	{
		private byte[] data;

		private int pos;

		public byte Next
		{
			get
			{
				return data[pos];
			}
		}

		public int Position
		{
			get
			{
				return pos;
			}
			set
			{
				pos = value;
			}
		}

		public DataBuffer(byte[] data)
			: this(data, 0)
		{
		}

		public DataBuffer(byte[] data, int pos)
		{
			this.data = data;
			this.pos = pos;
		}

		public byte ReadByte()
		{
			return data[pos++];
		}

		public short ReadShortInt()
		{
			return (short)(ReadByte() | (ReadByte() << 8));
		}

		public short ReadBEShortInt()
		{
			return (short)((ReadByte() << 8) | ReadByte());
		}

		public ushort ReadShortUInt()
		{
			return (ushort)(ReadByte() | (ReadByte() << 8));
		}

		public ushort ReadBEShortUInt()
		{
			return (ushort)((ReadByte() << 8) | ReadByte());
		}

		public int ReadInt()
		{
			return (ReadBEShortUInt() << 16) | ReadBEShortUInt();
		}

		public uint ReadUInt()
		{
			return (uint)((ReadBEShortUInt() << 16) | ReadBEShortUInt());
		}

		public long ReadLongInt()
		{
			return ReadInt() | ReadInt();
		}

		public string ReadDomainName()
		{
			return ReadDomainName(1);
		}

		public string ReadDomainName(int depth)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			for (num = ReadByte(); num != 0; num = ReadByte())
			{
				if ((num & 0xC0) == 192)
				{
					int num2 = ((num & 0x3F) << 8) | ReadByte();
					int num3 = pos;
					pos = num2;
					stringBuilder.Append(ReadDomainName(depth + 1));
					pos = num3;
					return stringBuilder.ToString();
				}
				for (int i = 0; i < num; i++)
				{
					stringBuilder.Append((char)ReadByte());
				}
				if (Next != 0)
				{
					stringBuilder.Append('.');
				}
			}
			return stringBuilder.ToString();
		}

		public IPAddress ReadIPAddress()
		{
			byte[] array = new byte[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = ReadByte();
			}
			return new IPAddress(array);
		}

		public IPAddress ReadIPv6Address()
		{
			byte[] array = new byte[16];
			for (int i = 0; i < 16; i++)
			{
				array[i] = ReadByte();
			}
			return new IPAddress(array);
		}

		public byte[] ReadBytes(int length)
		{
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = ReadByte();
			}
			return array;
		}

		public string ReadCharString()
		{
			int num = ReadByte();
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < num; i++)
			{
				stringBuilder.Append((char)ReadByte());
			}
			return stringBuilder.ToString();
		}
	}
}
