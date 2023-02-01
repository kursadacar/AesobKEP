using System.Collections.Generic;

namespace Tr.Com.Eimza.SmartCard.SmartCardTemplates
{
	internal interface ICardTemplate
	{
		List<string> GetATRHashes();
	}
}
