namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal abstract class PskTlsClient : AbstractTlsClient
	{
		protected TlsPskIdentity mPskIdentity;

		public PskTlsClient(TlsPskIdentity pskIdentity)
			: this(new DefaultTlsCipherFactory(), pskIdentity)
		{
		}

		public PskTlsClient(TlsCipherFactory cipherFactory, TlsPskIdentity pskIdentity)
			: base(cipherFactory)
		{
			mPskIdentity = pskIdentity;
		}

		public override int[] GetCipherSuites()
		{
			return new int[4] { 49207, 49205, 182, 148 };
		}

		public override TlsKeyExchange GetKeyExchange()
		{
			switch (mSelectedCipherSuite)
			{
			case 45:
			case 142:
			case 143:
			case 144:
			case 145:
			case 170:
			case 171:
			case 178:
			case 179:
			case 180:
			case 181:
			case 49296:
			case 49297:
			case 49302:
			case 49303:
			case 49318:
			case 49319:
			case 49322:
			case 49323:
			case 58396:
			case 58397:
				return CreatePskKeyExchange(14);
			case 49203:
			case 49204:
			case 49205:
			case 49206:
			case 49207:
			case 49208:
			case 49209:
			case 49210:
			case 49211:
			case 49306:
			case 49307:
			case 58392:
			case 58393:
				return CreatePskKeyExchange(24);
			case 44:
			case 138:
			case 139:
			case 140:
			case 141:
			case 168:
			case 169:
			case 174:
			case 175:
			case 176:
			case 177:
			case 49294:
			case 49295:
			case 49300:
			case 49301:
			case 49316:
			case 49317:
			case 49320:
			case 49321:
			case 58390:
			case 58391:
				return CreatePskKeyExchange(13);
			case 46:
			case 146:
			case 147:
			case 148:
			case 149:
			case 172:
			case 173:
			case 182:
			case 183:
			case 184:
			case 185:
			case 49298:
			case 49299:
			case 49304:
			case 49305:
			case 58394:
			case 58395:
				return CreatePskKeyExchange(15);
			default:
				throw new TlsFatalAlert(80);
			}
		}

		public override TlsCipher GetCipher()
		{
			switch (mSelectedCipherSuite)
			{
			case 139:
			case 143:
			case 147:
			case 49204:
				return mCipherFactory.CreateCipher(mContext, 7, 2);
			case 140:
			case 144:
			case 148:
			case 49205:
				return mCipherFactory.CreateCipher(mContext, 8, 2);
			case 174:
			case 178:
			case 182:
			case 49207:
				return mCipherFactory.CreateCipher(mContext, 8, 3);
			case 49316:
			case 49318:
				return mCipherFactory.CreateCipher(mContext, 15, 0);
			case 49320:
			case 49322:
				return mCipherFactory.CreateCipher(mContext, 16, 0);
			case 168:
			case 170:
			case 172:
				return mCipherFactory.CreateCipher(mContext, 10, 0);
			case 141:
			case 145:
			case 149:
			case 49206:
				return mCipherFactory.CreateCipher(mContext, 9, 2);
			case 175:
			case 179:
			case 183:
			case 49208:
				return mCipherFactory.CreateCipher(mContext, 9, 4);
			case 49317:
			case 49319:
				return mCipherFactory.CreateCipher(mContext, 17, 0);
			case 49321:
			case 49323:
				return mCipherFactory.CreateCipher(mContext, 18, 0);
			case 169:
			case 171:
			case 173:
				return mCipherFactory.CreateCipher(mContext, 11, 0);
			case 49300:
			case 49302:
			case 49304:
			case 49306:
				return mCipherFactory.CreateCipher(mContext, 12, 3);
			case 49294:
			case 49296:
			case 49298:
				return mCipherFactory.CreateCipher(mContext, 19, 0);
			case 49301:
			case 49303:
			case 49305:
			case 49307:
				return mCipherFactory.CreateCipher(mContext, 13, 4);
			case 49295:
			case 49297:
			case 49299:
				return mCipherFactory.CreateCipher(mContext, 20, 0);
			case 58390:
			case 58392:
			case 58394:
			case 58396:
				return mCipherFactory.CreateCipher(mContext, 100, 2);
			case 44:
			case 45:
			case 46:
			case 49209:
				return mCipherFactory.CreateCipher(mContext, 0, 2);
			case 176:
			case 180:
			case 184:
			case 49210:
				return mCipherFactory.CreateCipher(mContext, 0, 3);
			case 177:
			case 181:
			case 185:
			case 49211:
				return mCipherFactory.CreateCipher(mContext, 0, 4);
			case 138:
			case 142:
			case 146:
			case 49203:
				return mCipherFactory.CreateCipher(mContext, 2, 2);
			case 58391:
			case 58393:
			case 58395:
			case 58397:
				return mCipherFactory.CreateCipher(mContext, 101, 2);
			default:
				throw new TlsFatalAlert(80);
			}
		}

		protected virtual TlsKeyExchange CreatePskKeyExchange(int keyExchange)
		{
			return new TlsPskKeyExchange(keyExchange, mSupportedSignatureAlgorithms, mPskIdentity, null, mNamedCurves, mClientECPointFormats, mServerECPointFormats);
		}
	}
}
