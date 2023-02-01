namespace ActiveUp.Net.Dns
{
	internal class LocRecord : IRecordData
	{
		private short version;

		private short size;

		private short horzPrecision;

		private short vertPrecision;

		private long lattitude;

		private long longitude;

		private long altitude;

		public short Version
		{
			get
			{
				return version;
			}
		}

		public short Size
		{
			get
			{
				return size;
			}
		}

		public short HorzPrecision
		{
			get
			{
				return horzPrecision;
			}
		}

		public short VertPrecision
		{
			get
			{
				return vertPrecision;
			}
		}

		public long Lattitude
		{
			get
			{
				return lattitude;
			}
		}

		public long Longitude
		{
			get
			{
				return longitude;
			}
		}

		public long Altitude
		{
			get
			{
				return altitude;
			}
		}

		public LocRecord(DataBuffer buffer)
		{
			version = buffer.ReadShortInt();
			size = buffer.ReadShortInt();
			horzPrecision = buffer.ReadShortInt();
			vertPrecision = buffer.ReadShortInt();
			lattitude = buffer.ReadInt();
			longitude = buffer.ReadInt();
			altitude = buffer.ReadInt();
		}

		public override string ToString()
		{
			return string.Format("Version:{0} Size:{1} Horz Precision:{2} Veret Precision:{3} Lattitude:{4} Longitude:{5} Altitude:{6}", version, size, horzPrecision, vertPrecision, lattitude, longitude, altitude);
		}
	}
}
