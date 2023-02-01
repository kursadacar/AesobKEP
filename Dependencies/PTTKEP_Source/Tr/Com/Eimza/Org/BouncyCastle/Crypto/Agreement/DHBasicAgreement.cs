using System;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Agreement
{
	internal class DHBasicAgreement : IBasicAgreement
	{
		private DHPrivateKeyParameters key;

		private DHParameters dhParams;

		public virtual void Init(ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			if (!(parameters is DHPrivateKeyParameters))
			{
				throw new ArgumentException("DHEngine expects DHPrivateKeyParameters");
			}
			key = (DHPrivateKeyParameters)parameters;
			dhParams = key.Parameters;
		}

		public virtual int GetFieldSize()
		{
			return (key.Parameters.P.BitLength + 7) / 8;
		}

		public virtual BigInteger CalculateAgreement(ICipherParameters pubKey)
		{
			if (key == null)
			{
				throw new InvalidOperationException("Agreement algorithm not initialised");
			}
			DHPublicKeyParameters obj = (DHPublicKeyParameters)pubKey;
			if (!obj.Parameters.Equals(dhParams))
			{
				throw new ArgumentException("Diffie-Hellman public key has wrong parameters.");
			}
			return obj.Y.ModPow(key.X, dhParams.P);
		}
	}
}
