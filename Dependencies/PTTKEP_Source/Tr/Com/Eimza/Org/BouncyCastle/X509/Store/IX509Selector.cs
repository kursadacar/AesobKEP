using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.X509.Store
{
	internal interface IX509Selector : ICloneable
	{
		bool Match(object obj);
	}
}
