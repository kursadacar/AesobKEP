using System;
using System.Collections.Generic;

namespace ActiveUp.Net.Dns
{
	public class DnsAnswer
	{
		private ReturnCode returnCode = ReturnCode.Other;

		private bool authoritative;

		private bool recursive;

		private bool truncated;

		private List<Question> questions;

		private List<Answer> answers;

		public List<Server> servers;

		private List<Record> additional;

		private List<Exception> exceptions;

		public ReturnCode ReturnCode
		{
			get
			{
				return returnCode;
			}
		}

		public bool Authoritative
		{
			get
			{
				return authoritative;
			}
		}

		public bool Recursive
		{
			get
			{
				return recursive;
			}
		}

		public bool Truncated
		{
			get
			{
				return truncated;
			}
		}

		public List<Question> Questions
		{
			get
			{
				return questions;
			}
		}

		public List<Answer> Answers
		{
			get
			{
				return answers;
			}
		}

		public List<Server> Servers
		{
			get
			{
				return servers;
			}
		}

		public List<Record> Additional
		{
			get
			{
				return additional;
			}
		}

		public List<Exception> Exceptions
		{
			get
			{
				return exceptions;
			}
		}

		public List<DnsEntry> Entries
		{
			get
			{
				List<DnsEntry> list = new List<DnsEntry>();
				foreach (Answer answer in answers)
				{
					list.Add(answer);
				}
				foreach (Server server in servers)
				{
					list.Add(server);
				}
				foreach (Record item in additional)
				{
					list.Add(item);
				}
				return list;
			}
		}

		public DnsAnswer(byte[] response)
		{
			questions = new List<Question>();
			answers = new List<Answer>();
			servers = new List<Server>();
			additional = new List<Record>();
			exceptions = new List<Exception>();
			DataBuffer dataBuffer = new DataBuffer(response, 2);
			byte b = dataBuffer.ReadByte();
			byte b2 = dataBuffer.ReadByte();
			int num = b2 & 0xF;
			if (num > 6)
			{
				num = 6;
			}
			returnCode = (ReturnCode)num;
			authoritative = TestBit(b, 2);
			recursive = TestBit(b2, 8);
			truncated = TestBit(b, 1);
			int num2 = dataBuffer.ReadBEShortInt();
			int num3 = dataBuffer.ReadBEShortInt();
			int num4 = dataBuffer.ReadBEShortInt();
			int num5 = dataBuffer.ReadBEShortInt();
			for (int i = 0; i < num2; i++)
			{
				try
				{
					questions.Add(new Question(dataBuffer));
				}
				catch (Exception item)
				{
					exceptions.Add(item);
				}
			}
			for (int j = 0; j < num3; j++)
			{
				try
				{
					answers.Add(new Answer(dataBuffer));
				}
				catch (Exception item2)
				{
					exceptions.Add(item2);
				}
			}
			for (int k = 0; k < num4; k++)
			{
				try
				{
					servers.Add(new Server(dataBuffer));
				}
				catch (Exception item3)
				{
					exceptions.Add(item3);
				}
			}
			for (int l = 0; l < num5; l++)
			{
				try
				{
					additional.Add(new Record(dataBuffer));
				}
				catch (Exception item4)
				{
					exceptions.Add(item4);
				}
			}
		}

		private bool TestBit(byte b, byte pos)
		{
			byte b2 = (byte)(1 << (int)pos);
			return (b & b2) != 0;
		}
	}
}
