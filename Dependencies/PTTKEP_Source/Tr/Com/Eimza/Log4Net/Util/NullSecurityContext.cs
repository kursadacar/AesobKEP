using System;
using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Util
{
	public sealed class NullSecurityContext : SecurityContext
	{
		public static readonly NullSecurityContext Instance = new NullSecurityContext();

		private NullSecurityContext()
		{
		}

		public override IDisposable Impersonate(object state)
		{
			return null;
		}
	}
}
