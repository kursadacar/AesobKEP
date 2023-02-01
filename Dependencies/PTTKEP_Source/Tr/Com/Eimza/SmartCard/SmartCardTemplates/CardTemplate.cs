using System.Collections.Generic;

namespace Tr.Com.Eimza.SmartCard.SmartCardTemplates
{
	internal abstract class CardTemplate : ICardTemplate
	{
		protected SmartCardType cardType;

		internal SmartCardType CardType
		{
			get
			{
				return cardType;
			}
			set
			{
				cardType = value;
			}
		}

		internal CardTemplate(SmartCardType aCardType)
		{
			cardType = aCardType;
		}

		internal CardTemplate()
		{
		}

		public abstract List<string> GetATRHashes();
	}
}
