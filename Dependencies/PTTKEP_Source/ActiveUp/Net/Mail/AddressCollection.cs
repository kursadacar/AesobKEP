using System;
using System.Collections.Generic;
using System.Linq;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class AddressCollection : List<Address>
	{
		public string Merged
		{
			get
			{
				return this.Aggregate("", (string current, Address address) => current + address.Merged + ",").TrimEnd(',');
			}
		}

		public string Links
		{
			get
			{
				return this.Aggregate("", (string current, Address address) => current + address.Link + ";").TrimEnd(';');
			}
		}

		public static AddressCollection operator +(AddressCollection first, AddressCollection second)
		{
			foreach (Address item in second)
			{
				first.Add(item);
			}
			return first;
		}

		public new void Add(Address address)
		{
			base.Add(address);
		}

		public void Add(string email)
		{
			base.Add(new Address(email));
		}

		public void Add(string email, string name)
		{
			base.Add(new Address(email, name));
		}

		public void Add(string email, string name, string charset)
		{
			base.Add(new Address(email, name, charset));
		}

		public void Remove(int index)
		{
			if (index < base.Count || index >= 0)
			{
				RemoveAt(index);
			}
		}
	}
}
