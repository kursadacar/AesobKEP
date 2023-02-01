using System.Collections;

namespace Tr.Com.Eimza.Org.BouncyCastle.X509.Store
{
	internal interface IX509Store
	{
		ICollection GetMatches(IX509Selector selector);
	}
}
