using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters
{
	[Obsolete("Use AeadParameters")]
	internal class CcmParameters : AeadParameters
	{
		public CcmParameters(KeyParameter key, int macSize, byte[] nonce, byte[] associatedText)
			: base(key, macSize, nonce, associatedText)
		{
		}
	}
}
