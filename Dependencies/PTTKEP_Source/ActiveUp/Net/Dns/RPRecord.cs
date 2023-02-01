namespace ActiveUp.Net.Dns
{
	internal class RPRecord : IRecordData
	{
		private string responsibleMailbox;

		private string textDomain;

		public string ResponsibleMailbox
		{
			get
			{
				return responsibleMailbox;
			}
		}

		public string TextDomain
		{
			get
			{
				return textDomain;
			}
		}

		public RPRecord(DataBuffer buffer)
		{
			responsibleMailbox = buffer.ReadDomainName();
			textDomain = buffer.ReadDomainName();
		}

		public override string ToString()
		{
			return string.Format("Responsible Mailbox:{0} Text Domain:{1}", responsibleMailbox, textDomain);
		}
	}
}
