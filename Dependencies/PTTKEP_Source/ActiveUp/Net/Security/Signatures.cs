using System;
using System.Security.Cryptography.Pkcs;

namespace ActiveUp.Net.Security
{
	[Serializable]
	public class Signatures
	{
		[NonSerialized]
		private Signature _dk;

		[NonSerialized]
		private SignedCms _smime;

		public Signature DomainKeys
		{
			get
			{
				return _dk;
			}
			set
			{
				_dk = value;
			}
		}

		public SignedCms Smime
		{
			get
			{
				return _smime;
			}
			set
			{
				_smime = value;
			}
		}
	}
}
