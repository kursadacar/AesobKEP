using System;

namespace Tr.Com.Eimza.Smime
{
	[Flags]
	internal enum SMailDeliveryNotificationOptions
	{
		Delay = 4,
		Never = 0x8000000,
		None = 0,
		OnFailure = 2,
		OnSuccess = 1
	}
}
