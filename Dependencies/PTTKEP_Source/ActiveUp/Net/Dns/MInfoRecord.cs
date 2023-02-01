namespace ActiveUp.Net.Dns
{
	internal class MInfoRecord : IRecordData
	{
		private string responsibleMailbox;

		private string errorMailbox;

		public string ResponsibleMailbox
		{
			get
			{
				return responsibleMailbox;
			}
		}

		public string ErrorMailbox
		{
			get
			{
				return errorMailbox;
			}
		}

		public MInfoRecord(DataBuffer buffer)
		{
			responsibleMailbox = buffer.ReadDomainName();
			errorMailbox = buffer.ReadDomainName();
		}

		public override string ToString()
		{
			return string.Format("Responsible Mailbox:{0} Error Mailbox:{1}", responsibleMailbox, errorMailbox);
		}
	}
}
