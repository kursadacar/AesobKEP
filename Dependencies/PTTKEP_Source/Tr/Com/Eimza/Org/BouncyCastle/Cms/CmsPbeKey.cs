using System;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto.Parameters;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal abstract class CmsPbeKey : ICipherParameters
	{
		internal readonly char[] password;

		internal readonly byte[] salt;

		internal readonly int iterationCount;

		[Obsolete("Will be removed")]
		public string Password
		{
			get
			{
				return new string(password);
			}
		}

		public byte[] Salt
		{
			get
			{
				return Arrays.Clone(salt);
			}
		}

		public int IterationCount
		{
			get
			{
				return iterationCount;
			}
		}

		public string Algorithm
		{
			get
			{
				return "PKCS5S2";
			}
		}

		public string Format
		{
			get
			{
				return "RAW";
			}
		}

		[Obsolete("Use version taking 'char[]' instead")]
		protected CmsPbeKey(string password, byte[] salt, int iterationCount)
			: this(password.ToCharArray(), salt, iterationCount)
		{
		}

		[Obsolete("Use version taking 'char[]' instead")]
		protected CmsPbeKey(string password, AlgorithmIdentifier keyDerivationAlgorithm)
			: this(password.ToCharArray(), keyDerivationAlgorithm)
		{
		}

		protected CmsPbeKey(char[] password, byte[] salt, int iterationCount)
		{
			this.password = (char[])password.Clone();
			this.salt = Arrays.Clone(salt);
			this.iterationCount = iterationCount;
		}

		protected CmsPbeKey(char[] password, AlgorithmIdentifier keyDerivationAlgorithm)
		{
			if (!keyDerivationAlgorithm.ObjectID.Equals(PkcsObjectIdentifiers.IdPbkdf2))
			{
				DerObjectIdentifier objectID = keyDerivationAlgorithm.ObjectID;
				throw new ArgumentException("Unsupported key derivation algorithm: " + ((objectID != null) ? objectID.ToString() : null));
			}
			Pbkdf2Params instance = Pbkdf2Params.GetInstance(keyDerivationAlgorithm.Parameters.ToAsn1Object());
			this.password = (char[])password.Clone();
			salt = instance.GetSalt();
			iterationCount = instance.IterationCount.IntValue;
		}

		~CmsPbeKey()
		{
			Array.Clear(password, 0, password.Length);
		}

		[Obsolete("Use 'Salt' property instead")]
		public byte[] GetSalt()
		{
			return Salt;
		}

		public byte[] GetEncoded()
		{
			return null;
		}

		internal abstract KeyParameter GetEncoded(string algorithmOid);
	}
}
