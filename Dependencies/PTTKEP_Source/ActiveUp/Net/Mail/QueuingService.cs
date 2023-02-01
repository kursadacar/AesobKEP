using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public enum QueuingService
	{
		ActiveQ,
		MicrosoftSmtp,
		IpSwitchIMail
	}
}
