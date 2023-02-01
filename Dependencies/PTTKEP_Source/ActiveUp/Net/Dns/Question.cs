namespace ActiveUp.Net.Dns
{
	public class Question
	{
		private string domain;

		private RecordType recType;

		private int classType;

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

		public Question(DataBuffer buffer)
		{
			domain = buffer.ReadDomainName();
			recType = (RecordType)buffer.ReadBEShortInt();
			classType = buffer.ReadBEShortInt();
		}
	}
}
