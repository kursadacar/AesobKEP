using System;

namespace ActiveUp.Net.Dns
{
	public class DnsEntry
	{
		private string domain;

		private RecordType recType;

		private int classType;

		private int ttl;

		private IRecordData data;

		public string Domain
		{
			get
			{
				return domain;
			}
		}

		public RecordType RecType
		{
			get
			{
				return recType;
			}
		}

		public int ClassType
		{
			get
			{
				return classType;
			}
		}

		public int Ttl
		{
			get
			{
				return ttl;
			}
		}

		public IRecordData Data
		{
			get
			{
				return data;
			}
		}

		public DnsEntry(DataBuffer buffer)
		{
			try
			{
				domain = buffer.ReadDomainName();
				buffer.ReadByte();
				recType = (RecordType)buffer.ReadShortInt();
				classType = buffer.ReadShortInt();
				ttl = buffer.ReadInt();
				int length = buffer.ReadByte();
				switch (recType)
				{
				case RecordType.A:
					data = new ARecord(buffer);
					break;
				case RecordType.NS:
					data = new NSRecord(buffer);
					break;
				case RecordType.CNAME:
					data = new CNameRecord(buffer);
					break;
				case RecordType.SOA:
					data = new SoaRecord(buffer);
					break;
				case RecordType.MB:
					data = new MBRecord(buffer);
					break;
				case RecordType.MG:
					data = new MGRecord(buffer);
					break;
				case RecordType.MR:
					data = new MRRecord(buffer);
					break;
				case RecordType.NULL:
					data = new NullRecord(buffer, length);
					break;
				case RecordType.WKS:
					data = new WksRecord(buffer, length);
					break;
				case RecordType.PTR:
					data = new PtrRecord(buffer);
					break;
				case RecordType.HINFO:
					data = new HInfoRecord(buffer, length);
					break;
				case RecordType.MINFO:
					data = new MInfoRecord(buffer);
					break;
				case RecordType.MX:
					data = new MXRecord(buffer);
					break;
				case RecordType.TXT:
					data = new TxtRecord(buffer, length);
					break;
				case RecordType.RP:
					data = new RPRecord(buffer);
					break;
				case RecordType.AFSDB:
					data = new AfsdbRecord(buffer);
					break;
				case RecordType.X25:
					data = new X25Record(buffer);
					break;
				case RecordType.ISDN:
					data = new IsdnRecord(buffer);
					break;
				case RecordType.RT:
					data = new RTRecord(buffer);
					break;
				case RecordType.NSAP:
					data = new NsapRecord(buffer, length);
					break;
				case RecordType.SIG:
					data = new SigRecord(buffer, length);
					break;
				case RecordType.KEY:
					data = new KeyRecord(buffer, length);
					break;
				case RecordType.PX:
					data = new PXRecord(buffer);
					break;
				case RecordType.AAAA:
					data = new AAAARecord(buffer);
					break;
				case RecordType.LOC:
					data = new LocRecord(buffer);
					break;
				case RecordType.SRV:
					data = new SrvRecord(buffer);
					break;
				case RecordType.NAPTR:
					data = new NaptrRecord(buffer);
					break;
				case RecordType.KX:
					data = new KXRecord(buffer);
					break;
				case RecordType.A6:
					data = new A6Record(buffer);
					break;
				case RecordType.DNAME:
					data = new DNameRecord(buffer);
					break;
				case RecordType.DS:
					data = new DSRecord(buffer, length);
					break;
				case RecordType.TKEY:
					data = new TKeyRecord(buffer);
					break;
				case RecordType.TSIG:
					data = new TSigRecord(buffer);
					break;
				default:
					throw new DnsQueryException("Invalid DNS Record Type in DNS Response", null);
				}
			}
			catch (Exception ex)
			{
				data = new ExceptionRecord(ex.Message);
				throw ex;
			}
		}
	}
}
