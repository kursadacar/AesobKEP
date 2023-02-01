using System;

namespace Tr.Com.Eimza.Log4Net.Core
{
	public abstract class SecurityContext
	{
		public abstract IDisposable Impersonate(object state);
	}
}
