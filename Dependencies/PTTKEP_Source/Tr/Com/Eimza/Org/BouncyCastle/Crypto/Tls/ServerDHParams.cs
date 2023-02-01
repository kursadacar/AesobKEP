using System;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Math;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class ServerDHParams
	{
		protected readonly DHPublicKeyParameters mPublicKey;

		public virtual DHPublicKeyParameters PublicKey
		{
			get
			{
				return mPublicKey;
			}
		}

		public ServerDHParams(DHPublicKeyParameters publicKey)
		{
			if (publicKey == null)
			{
				throw new ArgumentNullException("publicKey");
			}
			mPublicKey = publicKey;
		}

		public virtual void Encode(Stream output)
		{
			DHParameters parameters = mPublicKey.Parameters;
			BigInteger y = mPublicKey.Y;
			TlsDHUtilities.WriteDHParameter(parameters.P, output);
			TlsDHUtilities.WriteDHParameter(parameters.G, output);
			TlsDHUtilities.WriteDHParameter(y, output);
		}

		public static ServerDHParams Parse(Stream input)
		{
			BigInteger p = TlsDHUtilities.ReadDHParameter(input);
			BigInteger g = TlsDHUtilities.ReadDHParameter(input);
			return new ServerDHParams(new DHPublicKeyParameters(TlsDHUtilities.ReadDHParameter(input), new DHParameters(p, g)));
		}
	}
}
