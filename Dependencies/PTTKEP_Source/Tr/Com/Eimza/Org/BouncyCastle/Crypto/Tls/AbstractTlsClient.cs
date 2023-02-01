using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal abstract class AbstractTlsClient : AbstractTlsPeer, TlsClient, TlsPeer
	{
		protected TlsCipherFactory mCipherFactory;

		protected TlsClientContext mContext;

		protected IList mSupportedSignatureAlgorithms;

		protected int[] mNamedCurves;

		protected byte[] mClientECPointFormats;

		protected byte[] mServerECPointFormats;

		protected int mSelectedCipherSuite;

		protected short mSelectedCompressionMethod;

		public virtual ProtocolVersion ClientHelloRecordLayerVersion
		{
			get
			{
				return ClientVersion;
			}
		}

		public virtual ProtocolVersion ClientVersion
		{
			get
			{
				return ProtocolVersion.TLSv12;
			}
		}

		public virtual ProtocolVersion MinimumVersion
		{
			get
			{
				return ProtocolVersion.TLSv10;
			}
		}

		public AbstractTlsClient()
			: this(new DefaultTlsCipherFactory())
		{
		}

		public AbstractTlsClient(TlsCipherFactory cipherFactory)
		{
			mCipherFactory = cipherFactory;
		}

		public virtual void Init(TlsClientContext context)
		{
			mContext = context;
		}

		public virtual TlsSession GetSessionToResume()
		{
			return null;
		}

		public virtual IDictionary GetClientExtensions()
		{
			IDictionary dictionary = null;
			if (TlsUtilities.IsSignatureAlgorithmsExtensionAllowed(mContext.ClientVersion))
			{
				byte[] array = new byte[5] { 6, 5, 4, 3, 2 };
				byte[] array2 = new byte[1] { 1 };
				mSupportedSignatureAlgorithms = Platform.CreateArrayList();
				for (int i = 0; i < array.Length; i++)
				{
					for (int j = 0; j < array2.Length; j++)
					{
						mSupportedSignatureAlgorithms.Add(new SignatureAndHashAlgorithm(array[i], array2[j]));
					}
				}
				mSupportedSignatureAlgorithms.Add(new SignatureAndHashAlgorithm(2, 2));
				dictionary = TlsExtensionsUtilities.EnsureExtensionsInitialised(dictionary);
				TlsUtilities.AddSignatureAlgorithmsExtension(dictionary, mSupportedSignatureAlgorithms);
			}
			if (TlsEccUtilities.ContainsEccCipherSuites(GetCipherSuites()))
			{
				mNamedCurves = new int[2] { 23, 24 };
				mClientECPointFormats = new byte[3] { 0, 1, 2 };
				dictionary = TlsExtensionsUtilities.EnsureExtensionsInitialised(dictionary);
				TlsEccUtilities.AddSupportedEllipticCurvesExtension(dictionary, mNamedCurves);
				TlsEccUtilities.AddSupportedPointFormatsExtension(dictionary, mClientECPointFormats);
			}
			return dictionary;
		}

		public virtual void NotifyServerVersion(ProtocolVersion serverVersion)
		{
			if (!MinimumVersion.IsEqualOrEarlierVersionOf(serverVersion))
			{
				throw new TlsFatalAlert(70);
			}
		}

		public abstract int[] GetCipherSuites();

		public virtual byte[] GetCompressionMethods()
		{
			return new byte[1];
		}

		public virtual void NotifySessionID(byte[] sessionID)
		{
		}

		public virtual void NotifySelectedCipherSuite(int selectedCipherSuite)
		{
			mSelectedCipherSuite = selectedCipherSuite;
		}

		public virtual void NotifySelectedCompressionMethod(byte selectedCompressionMethod)
		{
			mSelectedCompressionMethod = selectedCompressionMethod;
		}

		public virtual void ProcessServerExtensions(IDictionary serverExtensions)
		{
			if (serverExtensions != null)
			{
				if (serverExtensions.Contains(13))
				{
					throw new TlsFatalAlert(47);
				}
				if (TlsEccUtilities.GetSupportedEllipticCurvesExtension(serverExtensions) != null)
				{
					throw new TlsFatalAlert(47);
				}
				mServerECPointFormats = TlsEccUtilities.GetSupportedPointFormatsExtension(serverExtensions);
				if (mServerECPointFormats != null && !TlsEccUtilities.IsEccCipherSuite(mSelectedCipherSuite))
				{
					throw new TlsFatalAlert(47);
				}
			}
		}

		public virtual void ProcessServerSupplementalData(IList serverSupplementalData)
		{
			if (serverSupplementalData != null)
			{
				throw new TlsFatalAlert(10);
			}
		}

		public abstract TlsKeyExchange GetKeyExchange();

		public abstract TlsAuthentication GetAuthentication();

		public virtual IList GetClientSupplementalData()
		{
			return null;
		}

		public override TlsCompression GetCompression()
		{
			switch (mSelectedCompressionMethod)
			{
			case 0:
				return new TlsNullCompression();
			case 1:
				return new TlsDeflateCompression();
			default:
				throw new TlsFatalAlert(80);
			}
		}

		public virtual void NotifyNewSessionTicket(NewSessionTicket newSessionTicket)
		{
		}
	}
}
