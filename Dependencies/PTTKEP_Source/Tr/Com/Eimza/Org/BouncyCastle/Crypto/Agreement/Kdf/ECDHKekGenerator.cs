using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Generators;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Agreement.Kdf
{
	internal class ECDHKekGenerator : IDerivationFunction
	{
		private readonly IDerivationFunction kdf;

		private DerObjectIdentifier algorithm;

		private int keySize;

		private byte[] z;

		public virtual IDigest Digest
		{
			get
			{
				return kdf.Digest;
			}
		}

		public ECDHKekGenerator(IDigest digest)
		{
			kdf = new Kdf2BytesGenerator(digest);
		}

		public virtual void Init(IDerivationParameters param)
		{
			DHKdfParameters dHKdfParameters = (DHKdfParameters)param;
			algorithm = dHKdfParameters.Algorithm;
			keySize = dHKdfParameters.KeySize;
			z = dHKdfParameters.GetZ();
		}

		public virtual int GenerateBytes(byte[] outBytes, int outOff, int len)
		{
			DerSequence derSequence = new DerSequence(new AlgorithmIdentifier(algorithm, DerNull.Instance), new DerTaggedObject(true, 2, new DerOctetString(Pack.UInt32_To_BE((uint)keySize))));
			kdf.Init(new KdfParameters(z, derSequence.GetDerEncoded()));
			return kdf.GenerateBytes(outBytes, outOff, len);
		}
	}
}
