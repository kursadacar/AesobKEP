using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal abstract class SrpTlsClient : AbstractTlsClient
	{
		protected byte[] mIdentity;

		protected byte[] mPassword;

		protected virtual bool RequireSrpServerExtension
		{
			get
			{
				return false;
			}
		}

		public SrpTlsClient(byte[] identity, byte[] password)
			: this(new DefaultTlsCipherFactory(), identity, password)
		{
		}

		public SrpTlsClient(TlsCipherFactory cipherFactory, byte[] identity, byte[] password)
			: base(cipherFactory)
		{
			mIdentity = Arrays.Clone(identity);
			mPassword = Arrays.Clone(password);
		}

		public override int[] GetCipherSuites()
		{
			return new int[1] { 49182 };
		}

		public override IDictionary GetClientExtensions()
		{
			IDictionary dictionary = TlsExtensionsUtilities.EnsureExtensionsInitialised(base.GetClientExtensions());
			TlsSrpUtilities.AddSrpExtension(dictionary, mIdentity);
			return dictionary;
		}

		public override void ProcessServerExtensions(IDictionary serverExtensions)
		{
			if (!TlsUtilities.HasExpectedEmptyExtensionData(serverExtensions, 12, 47) && RequireSrpServerExtension)
			{
				throw new TlsFatalAlert(47);
			}
			base.ProcessServerExtensions(serverExtensions);
		}

		public override TlsKeyExchange GetKeyExchange()
		{
			switch (mSelectedCipherSuite)
			{
			case 49178:
			case 49181:
			case 49184:
				return CreateSrpKeyExchange(21);
			case 49179:
			case 49182:
			case 49185:
				return CreateSrpKeyExchange(23);
			case 49180:
			case 49183:
			case 49186:
				return CreateSrpKeyExchange(22);
			default:
				throw new TlsFatalAlert(80);
			}
		}

		public override TlsCipher GetCipher()
		{
			switch (mSelectedCipherSuite)
			{
			case 49178:
			case 49179:
			case 49180:
				return mCipherFactory.CreateCipher(mContext, 7, 2);
			case 49181:
			case 49182:
			case 49183:
				return mCipherFactory.CreateCipher(mContext, 8, 2);
			case 49184:
			case 49185:
			case 49186:
				return mCipherFactory.CreateCipher(mContext, 9, 2);
			default:
				throw new TlsFatalAlert(80);
			}
		}

		protected virtual TlsKeyExchange CreateSrpKeyExchange(int keyExchange)
		{
			return new TlsSrpKeyExchange(keyExchange, mSupportedSignatureAlgorithms, mIdentity, mPassword);
		}
	}
}
