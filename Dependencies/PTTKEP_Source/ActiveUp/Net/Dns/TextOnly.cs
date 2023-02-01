using System.Collections.Generic;

namespace ActiveUp.Net.Dns
{
	public class TextOnly : IRecordData
	{
		private List<string> text;

		protected string Text
		{
			get
			{
				string text = string.Empty;
				foreach (string item in this.text)
				{
					text = text + item + "\n";
				}
				return text;
			}
		}

		protected int Count
		{
			get
			{
				return text.Count;
			}
		}

		protected List<string> Strings
		{
			get
			{
				return text;
			}
		}

		public TextOnly()
		{
			text = new List<string>();
		}

		public TextOnly(DataBuffer buffer)
		{
			text = new List<string>();
			while (buffer.Next > 0)
			{
				text.Add(buffer.ReadCharString());
			}
		}

		public TextOnly(DataBuffer buffer, int length)
		{
			int num = length;
			int position = buffer.Position;
			text = new List<string>();
			byte next = buffer.Next;
			while (length > 0)
			{
				text.Add(buffer.ReadCharString());
				length -= next + 1;
				if (length < 0)
				{
					buffer.Position = position - num;
					throw new DnsQueryException("Buffer Over Run in TextOnly Record Data Type", null);
				}
				next = buffer.Next;
			}
			if (length > 0)
			{
				buffer.Position = position - num;
				throw new DnsQueryException("Buffer Under Run in TextOnly Record Data Type", null);
			}
		}

		public override string ToString()
		{
			return "Text: " + Text;
		}
	}
}
