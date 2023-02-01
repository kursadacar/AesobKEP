using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal sealed class SessionParameters
	{
		internal sealed class Builder
		{
			private int mCipherSuite = -1;

			private short mCompressionAlgorithm = -1;

			private byte[] mMasterSecret;

			private Certificate mPeerCertificate;

			private byte[] mEncodedServerExtensions;

			public SessionParameters Build()
			{
				Validate(mCipherSuite >= 0, "cipherSuite");
				Validate(mCompressionAlgorithm >= 0, "compressionAlgorithm");
				Validate(mMasterSecret != null, "masterSecret");
				return new SessionParameters(mCipherSuite, (byte)mCompressionAlgorithm, mMasterSecret, mPeerCertificate, mEncodedServerExtensions);
			}

			public Builder SetCipherSuite(int cipherSuite)
			{
				mCipherSuite = cipherSuite;
				return this;
			}

			public Builder SetCompressionAlgorithm(byte compressionAlgorithm)
			{
				mCompressionAlgorithm = compressionAlgorithm;
				return this;
			}

			public Builder SetMasterSecret(byte[] masterSecret)
			{
				mMasterSecret = masterSecret;
				return this;
			}

			public Builder SetPeerCertificate(Certificate peerCertificate)
			{
				mPeerCertificate = peerCertificate;
				return this;
			}

			public Builder SetServerExtensions(IDictionary serverExtensions)
			{
				if (serverExtensions == null)
				{
					mEncodedServerExtensions = null;
				}
				else
				{
					MemoryStream memoryStream = new MemoryStream();
					TlsProtocol.WriteExtensions(memoryStream, serverExtensions);
					mEncodedServerExtensions = memoryStream.ToArray();
				}
				return this;
			}

			private void Validate(bool condition, string parameter)
			{
				if (!condition)
				{
					throw new InvalidOperationException("Required session parameter '" + parameter + "' not configured");
				}
			}
		}

		private int mCipherSuite;

		private byte mCompressionAlgorithm;

		private byte[] mMasterSecret;

		private Certificate mPeerCertificate;

		private byte[] mEncodedServerExtensions;

		public int CipherSuite
		{
			get
			{
				return mCipherSuite;
			}
		}

		public byte CompressionAlgorithm
		{
			get
			{
				return mCompressionAlgorithm;
			}
		}

		public byte[] MasterSecret
		{
			get
			{
				return mMasterSecret;
			}
		}

		public Certificate PeerCertificate
		{
			get
			{
				return mPeerCertificate;
			}
		}

		private SessionParameters(int cipherSuite, byte compressionAlgorithm, byte[] masterSecret, Certificate peerCertificate, byte[] encodedServerExtensions)
		{
			mCipherSuite = cipherSuite;
			mCompressionAlgorithm = compressionAlgorithm;
			mMasterSecret = Arrays.Clone(masterSecret);
			mPeerCertificate = peerCertificate;
			mEncodedServerExtensions = encodedServerExtensions;
		}

		public void Clear()
		{
			if (mMasterSecret != null)
			{
				Arrays.Fill(mMasterSecret, 0);
			}
		}

		public SessionParameters Copy()
		{
			return new SessionParameters(mCipherSuite, mCompressionAlgorithm, mMasterSecret, mPeerCertificate, mEncodedServerExtensions);
		}

		public IDictionary ReadServerExtensions()
		{
			if (mEncodedServerExtensions == null)
			{
				return null;
			}
			return TlsProtocol.ReadExtensions(new MemoryStream(mEncodedServerExtensions, false));
		}
	}
}
