using System.Collections;

namespace Tr.Com.Eimza.Log4Net.Repository.Hierarchy
{
	internal sealed class ProvisionNode : ArrayList
	{
		internal ProvisionNode(Logger log)
		{
			Add(log);
		}
	}
}
