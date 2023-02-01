namespace ActiveUp.Net.Dns
{
	internal class NaptrRecord : IRecordData
	{
		private ushort order;

		private ushort priority;

		private string flags;

		private string services;

		private string regexp;

		private string replacement;

		public ushort Order
		{
			get
			{
				return order;
			}
		}

		public ushort Priority
		{
			get
			{
				return priority;
			}
		}

		public string Flags
		{
			get
			{
				return flags;
			}
		}

		public string Services
		{
			get
			{
				return services;
			}
		}

		public string Regexp
		{
			get
			{
				return regexp;
			}
		}

		public string Replacement
		{
			get
			{
				return replacement;
			}
		}

		public NaptrRecord(DataBuffer buffer)
		{
			order = buffer.ReadShortUInt();
			priority = buffer.ReadShortUInt();
			flags = buffer.ReadCharString();
			services = buffer.ReadCharString();
			regexp = buffer.ReadCharString();
			replacement = buffer.ReadCharString();
		}

		public override string ToString()
		{
			return string.Format("Order:{0}, Priority:{1} Flags:{2} Services:{3} RegExp:{4} Replacement:{5}", order, priority, flags, services, regexp, replacement);
		}
	}
}
