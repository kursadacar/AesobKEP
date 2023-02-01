using System;
using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg
{
	internal class ElGamalSecretBcpgKey : BcpgObject, IBcpgKey
	{
		internal MPInteger x;

		public string Format
		{
			get
			{
				return "PGP";
			}
		}

		public BigInteger X
		{
			get
			{
				return x.Value;
			}
		}

		public ElGamalSecretBcpgKey(BcpgInputStream bcpgIn)
		{
			x = new MPInteger(bcpgIn);
		}

		public ElGamalSecretBcpgKey(BigInteger x)
		{
			this.x = new MPInteger(x);
		}

		public override byte[] GetEncoded()
		{
			try
			{
				return base.GetEncoded();
			}
			catch (Exception)
			{
				return null;
			}
		}

		public override void Encode(BcpgOutputStream bcpgOut)
		{
			bcpgOut.WriteObject(x);
		}
	}
}
