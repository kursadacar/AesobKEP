using System;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg
{
	internal class BcpgInputStream : BaseInputStream
	{
		private class PartialInputStream : BaseInputStream
		{
			private BcpgInputStream m_in;

			private bool partial;

			private int dataLength;

			internal PartialInputStream(BcpgInputStream bcpgIn, bool partial, int dataLength)
			{
				m_in = bcpgIn;
				this.partial = partial;
				this.dataLength = dataLength;
			}

			public override int ReadByte()
			{
				do
				{
					if (dataLength != 0)
					{
						int num = m_in.ReadByte();
						if (num < 0)
						{
							throw new EndOfStreamException("Premature end of stream in PartialInputStream");
						}
						dataLength--;
						return num;
					}
				}
				while (partial && ReadPartialDataLength() >= 0);
				return -1;
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				do
				{
					if (dataLength != 0)
					{
						int count2 = ((dataLength > count || dataLength < 0) ? count : dataLength);
						int num = m_in.Read(buffer, offset, count2);
						if (num < 1)
						{
							throw new EndOfStreamException("Premature end of stream in PartialInputStream");
						}
						dataLength -= num;
						return num;
					}
				}
				while (partial && ReadPartialDataLength() >= 0);
				return 0;
			}

			private int ReadPartialDataLength()
			{
				int num = m_in.ReadByte();
				if (num < 0)
				{
					return -1;
				}
				partial = false;
				if (num < 192)
				{
					dataLength = num;
				}
				else if (num <= 223)
				{
					dataLength = (num - 192 << 8) + m_in.ReadByte() + 192;
				}
				else if (num == 255)
				{
					dataLength = (m_in.ReadByte() << 24) | (m_in.ReadByte() << 16) | (m_in.ReadByte() << 8) | m_in.ReadByte();
				}
				else
				{
					partial = true;
					dataLength = 1 << (num & 0x1F);
				}
				return 0;
			}
		}

		private Stream m_in;

		private bool next;

		private int nextB;

		internal static BcpgInputStream Wrap(Stream inStr)
		{
			if (inStr is BcpgInputStream)
			{
				return (BcpgInputStream)inStr;
			}
			return new BcpgInputStream(inStr);
		}

		private BcpgInputStream(Stream inputStream)
		{
			m_in = inputStream;
		}

		public override int ReadByte()
		{
			if (next)
			{
				next = false;
				return nextB;
			}
			return m_in.ReadByte();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (!next)
			{
				return m_in.Read(buffer, offset, count);
			}
			if (nextB < 0)
			{
				return 0;
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			buffer[offset] = (byte)nextB;
			next = false;
			return 1;
		}

		public byte[] ReadAll()
		{
			return Streams.ReadAll(this);
		}

		public void ReadFully(byte[] buffer, int off, int len)
		{
			if (Streams.ReadFully(this, buffer, off, len) < len)
			{
				throw new EndOfStreamException();
			}
		}

		public void ReadFully(byte[] buffer)
		{
			ReadFully(buffer, 0, buffer.Length);
		}

		public PacketTag NextPacketTag()
		{
			if (!next)
			{
				try
				{
					nextB = m_in.ReadByte();
				}
				catch (EndOfStreamException)
				{
					nextB = -1;
				}
				next = true;
			}
			if (nextB >= 0)
			{
				if (((uint)nextB & 0x40u) != 0)
				{
					return (PacketTag)(nextB & 0x3F);
				}
				return (PacketTag)((nextB & 0x3F) >> 2);
			}
			return (PacketTag)nextB;
		}

		public Packet ReadPacket()
		{
			int num = ReadByte();
			if (num < 0)
			{
				return null;
			}
			if ((num & 0x80) == 0)
			{
				throw new IOException("invalid header encountered");
			}
			bool num2 = (num & 0x40) != 0;
			PacketTag packetTag = PacketTag.Reserved;
			int num3 = 0;
			bool flag = false;
			if (num2)
			{
				packetTag = (PacketTag)(num & 0x3F);
				int num4 = ReadByte();
				if (num4 < 192)
				{
					num3 = num4;
				}
				else if (num4 <= 223)
				{
					int num5 = m_in.ReadByte();
					num3 = (num4 - 192 << 8) + num5 + 192;
				}
				else if (num4 == 255)
				{
					num3 = (m_in.ReadByte() << 24) | (m_in.ReadByte() << 16) | (m_in.ReadByte() << 8) | m_in.ReadByte();
				}
				else
				{
					flag = true;
					num3 = 1 << (num4 & 0x1F);
				}
			}
			else
			{
				int num6 = num & 3;
				packetTag = (PacketTag)((num & 0x3F) >> 2);
				switch (num6)
				{
				case 0:
					num3 = ReadByte();
					break;
				case 1:
					num3 = (ReadByte() << 8) | ReadByte();
					break;
				case 2:
					num3 = (ReadByte() << 24) | (ReadByte() << 16) | (ReadByte() << 8) | ReadByte();
					break;
				case 3:
					flag = true;
					break;
				default:
					throw new IOException("unknown length type encountered");
				}
			}
			BcpgInputStream bcpgIn = ((!(num3 == 0 && flag)) ? new BcpgInputStream(new PartialInputStream(this, flag, num3)) : this);
			switch (packetTag)
			{
			case PacketTag.Reserved:
				return new InputStreamPacket(bcpgIn);
			case PacketTag.PublicKeyEncryptedSession:
				return new PublicKeyEncSessionPacket(bcpgIn);
			case PacketTag.Signature:
				return new SignaturePacket(bcpgIn);
			case PacketTag.SymmetricKeyEncryptedSessionKey:
				return new SymmetricKeyEncSessionPacket(bcpgIn);
			case PacketTag.OnePassSignature:
				return new OnePassSignaturePacket(bcpgIn);
			case PacketTag.SecretKey:
				return new SecretKeyPacket(bcpgIn);
			case PacketTag.PublicKey:
				return new PublicKeyPacket(bcpgIn);
			case PacketTag.SecretSubkey:
				return new SecretSubkeyPacket(bcpgIn);
			case PacketTag.CompressedData:
				return new CompressedDataPacket(bcpgIn);
			case PacketTag.SymmetricKeyEncrypted:
				return new SymmetricEncDataPacket(bcpgIn);
			case PacketTag.Marker:
				return new MarkerPacket(bcpgIn);
			case PacketTag.LiteralData:
				return new LiteralDataPacket(bcpgIn);
			case PacketTag.Trust:
				return new TrustPacket(bcpgIn);
			case PacketTag.UserId:
				return new UserIdPacket(bcpgIn);
			case PacketTag.UserAttribute:
				return new UserAttributePacket(bcpgIn);
			case PacketTag.PublicSubkey:
				return new PublicSubkeyPacket(bcpgIn);
			case PacketTag.SymmetricEncryptedIntegrityProtected:
				return new SymmetricEncIntegrityPacket(bcpgIn);
			case PacketTag.ModificationDetectionCode:
				return new ModDetectionCodePacket(bcpgIn);
			case PacketTag.Experimental1:
			case PacketTag.Experimental2:
			case PacketTag.Experimental3:
			case PacketTag.Experimental4:
				return new ExperimentalPacket(packetTag, bcpgIn);
			default:
				throw new IOException("unknown packet type encountered: " + packetTag);
			}
		}

		public override void Close()
		{
			m_in.Close();
			base.Close();
		}
	}
}
