using System;

namespace ActiveUp.Net.Dns
{
	public class DnsQueryException : Exception
	{
		private Exception[] exceptions;

		public DnsQueryException(string msg, Exception[] exs)
			: base(msg)
		{
			exceptions = exs;
		}
	}
}
