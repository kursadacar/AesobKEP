using System;

namespace Tr.Com.Eimza.SecureMail
{
	[Flags]
	internal enum SecureDeliveryNotificationOptions
	{
		Delay = 4,
		Never = 0x8000000,
		None = 0,
		OnFailure = 2,
		OnSuccess = 1
	}
}
