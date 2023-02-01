using System;
using System.Collections;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class ServerCollection : CollectionBase
	{
		public Server this[int index]
		{
			get
			{
				return (Server)base.List[index];
			}
		}

		public static ServerCollection operator +(ServerCollection first, ServerCollection second)
		{
			foreach (Server item in second)
			{
				first.Add(item);
			}
			return first;
		}

		public void Add(Server server)
		{
			base.List.Add(server);
		}

		public void Add(string host, int port)
		{
			base.List.Add(new Server(host, port));
		}

		public void Add(string host)
		{
			base.List.Add(new Server(host, 25));
		}

		public void Remove(int index)
		{
			if (index < base.Count || index >= 0)
			{
				base.List.RemoveAt(index);
			}
		}
	}
}
