using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class SecurityParameters
	{
		internal int entity = -1;

		internal int cipherSuite = -1;

		internal byte compressionAlgorithm;

		internal int prfAlgorithm = -1;

		internal int verifyDataLength = -1;

		internal byte[] masterSecret;

		internal byte[] clientRandom;

		internal byte[] serverRandom;

		internal byte[] sessionHash;

		internal short maxFragmentLength = -1;

		internal bool truncatedHMac;

		internal bool encryptThenMac;

		internal bool extendedMasterSecret;

		public virtual int Entity
		{
			get
			{
				return entity;
			}
		}

		public virtual int CipherSuite
		{
			get
			{
				return cipherSuite;
			}
		}

		public byte CompressionAlgorithm
		{
			get
			{
				return compressionAlgorithm;
			}
		}

		public virtual int PrfAlgorithm
		{
			get
			{
				return prfAlgorithm;
			}
		}

		public virtual int VerifyDataLength
		{
			get
			{
				return verifyDataLength;
			}
		}

		public virtual byte[] MasterSecret
		{
			get
			{
				return masterSecret;
			}
		}

		public virtual byte[] ClientRandom
		{
			get
			{
				return clientRandom;
			}
		}

		public virtual byte[] ServerRandom
		{
			get
			{
				return serverRandom;
			}
		}

		public virtual byte[] SessionHash
		{
			get
			{
				return sessionHash;
			}
		}

		internal virtual void Clear()
		{
			if (masterSecret != null)
			{
				Arrays.Fill(masterSecret, 0);
				masterSecret = null;
			}
		}
	}
}
