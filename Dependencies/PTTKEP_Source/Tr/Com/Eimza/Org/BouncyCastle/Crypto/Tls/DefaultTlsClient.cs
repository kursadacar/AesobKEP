namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal abstract class DefaultTlsClient : AbstractTlsClient
	{
		public DefaultTlsClient()
		{
		}

		public DefaultTlsClient(TlsCipherFactory cipherFactory)
			: base(cipherFactory)
		{
		}

		public override int[] GetCipherSuites()
		{
			return new int[6] { 49199, 49191, 49171, 156, 60, 47 };
		}

		public override TlsKeyExchange GetKeyExchange()
		{
			switch (mSelectedCipherSuite)
			{
			case 13:
			case 48:
			case 54:
			case 62:
			case 66:
			case 104:
			case 133:
			case 151:
			case 164:
			case 165:
			case 187:
			case 193:
			case 49282:
			case 49283:
				return CreateDHKeyExchange(7);
			case 16:
			case 49:
			case 55:
			case 63:
			case 67:
			case 105:
			case 134:
			case 152:
			case 160:
			case 161:
			case 188:
			case 194:
			case 49278:
			case 49279:
				return CreateDHKeyExchange(9);
			case 19:
			case 50:
			case 56:
			case 64:
			case 68:
			case 106:
			case 135:
			case 153:
			case 162:
			case 163:
			case 189:
			case 195:
			case 49280:
			case 49281:
				return CreateDheKeyExchange(3);
			case 22:
			case 51:
			case 57:
			case 69:
			case 103:
			case 107:
			case 136:
			case 154:
			case 158:
			case 159:
			case 190:
			case 196:
			case 49276:
			case 49277:
			case 49310:
			case 49311:
			case 49314:
			case 49315:
			case 52245:
			case 58398:
			case 58399:
				return CreateDheKeyExchange(5);
			case 49153:
			case 49154:
			case 49155:
			case 49156:
			case 49157:
			case 49189:
			case 49190:
			case 49197:
			case 49198:
			case 49268:
			case 49269:
			case 49288:
			case 49289:
				return CreateECDHKeyExchange(16);
			case 49163:
			case 49164:
			case 49165:
			case 49166:
			case 49167:
			case 49193:
			case 49194:
			case 49201:
			case 49202:
			case 49272:
			case 49273:
			case 49292:
			case 49293:
				return CreateECDHKeyExchange(18);
			case 49158:
			case 49159:
			case 49160:
			case 49161:
			case 49162:
			case 49187:
			case 49188:
			case 49195:
			case 49196:
			case 49266:
			case 49267:
			case 49286:
			case 49287:
			case 52244:
			case 58388:
			case 58389:
				return CreateECDheKeyExchange(17);
			case 49168:
			case 49169:
			case 49170:
			case 49171:
			case 49172:
			case 49191:
			case 49192:
			case 49199:
			case 49200:
			case 49270:
			case 49271:
			case 49290:
			case 49291:
			case 52243:
			case 58386:
			case 58387:
				return CreateECDheKeyExchange(19);
			case 1:
			case 2:
			case 4:
			case 5:
			case 10:
			case 47:
			case 53:
			case 59:
			case 60:
			case 61:
			case 65:
			case 132:
			case 150:
			case 156:
			case 157:
			case 186:
			case 192:
			case 49274:
			case 49275:
			case 49308:
			case 49309:
			case 49312:
			case 49313:
			case 58384:
			case 58385:
				return CreateRsaKeyExchange();
			default:
				throw new TlsFatalAlert(80);
			}
		}

		public override TlsCipher GetCipher()
		{
			switch (mSelectedCipherSuite)
			{
			case 10:
			case 13:
			case 16:
			case 19:
			case 22:
			case 49155:
			case 49160:
			case 49165:
			case 49170:
				return mCipherFactory.CreateCipher(mContext, 7, 2);
			case 52243:
			case 52244:
			case 52245:
				return mCipherFactory.CreateCipher(mContext, 102, 0);
			case 47:
			case 48:
			case 49:
			case 50:
			case 51:
			case 49156:
			case 49161:
			case 49166:
			case 49171:
				return mCipherFactory.CreateCipher(mContext, 8, 2);
			case 60:
			case 62:
			case 63:
			case 64:
			case 103:
			case 49187:
			case 49189:
			case 49191:
			case 49193:
				return mCipherFactory.CreateCipher(mContext, 8, 3);
			case 49308:
			case 49310:
				return mCipherFactory.CreateCipher(mContext, 15, 0);
			case 49312:
			case 49314:
				return mCipherFactory.CreateCipher(mContext, 16, 0);
			case 156:
			case 158:
			case 160:
			case 162:
			case 164:
			case 49195:
			case 49197:
			case 49199:
			case 49201:
				return mCipherFactory.CreateCipher(mContext, 10, 0);
			case 53:
			case 54:
			case 55:
			case 56:
			case 57:
			case 49157:
			case 49162:
			case 49167:
			case 49172:
				return mCipherFactory.CreateCipher(mContext, 9, 2);
			case 61:
			case 104:
			case 105:
			case 106:
			case 107:
				return mCipherFactory.CreateCipher(mContext, 9, 3);
			case 49188:
			case 49190:
			case 49192:
			case 49194:
				return mCipherFactory.CreateCipher(mContext, 9, 4);
			case 49309:
			case 49311:
				return mCipherFactory.CreateCipher(mContext, 17, 0);
			case 49313:
			case 49315:
				return mCipherFactory.CreateCipher(mContext, 18, 0);
			case 157:
			case 159:
			case 161:
			case 163:
			case 165:
			case 49196:
			case 49198:
			case 49200:
			case 49202:
				return mCipherFactory.CreateCipher(mContext, 11, 0);
			case 65:
			case 66:
			case 67:
			case 68:
			case 69:
				return mCipherFactory.CreateCipher(mContext, 12, 2);
			case 186:
			case 187:
			case 188:
			case 189:
			case 190:
			case 49266:
			case 49268:
			case 49270:
			case 49272:
				return mCipherFactory.CreateCipher(mContext, 12, 3);
			case 49274:
			case 49276:
			case 49278:
			case 49280:
			case 49282:
			case 49286:
			case 49288:
			case 49290:
			case 49292:
				return mCipherFactory.CreateCipher(mContext, 19, 0);
			case 132:
			case 133:
			case 134:
			case 135:
			case 136:
				return mCipherFactory.CreateCipher(mContext, 13, 2);
			case 192:
			case 193:
			case 194:
			case 195:
			case 196:
				return mCipherFactory.CreateCipher(mContext, 13, 3);
			case 49275:
			case 49277:
			case 49279:
			case 49281:
			case 49283:
			case 49287:
			case 49289:
			case 49291:
			case 49293:
				return mCipherFactory.CreateCipher(mContext, 20, 0);
			case 49267:
			case 49269:
			case 49271:
			case 49273:
				return mCipherFactory.CreateCipher(mContext, 13, 4);
			case 58384:
			case 58386:
			case 58388:
			case 58398:
				return mCipherFactory.CreateCipher(mContext, 100, 2);
			case 1:
				return mCipherFactory.CreateCipher(mContext, 0, 1);
			case 2:
			case 49153:
			case 49158:
			case 49163:
			case 49168:
				return mCipherFactory.CreateCipher(mContext, 0, 2);
			case 59:
				return mCipherFactory.CreateCipher(mContext, 0, 3);
			case 4:
				return mCipherFactory.CreateCipher(mContext, 2, 1);
			case 5:
			case 49154:
			case 49159:
			case 49164:
			case 49169:
				return mCipherFactory.CreateCipher(mContext, 2, 2);
			case 58385:
			case 58387:
			case 58389:
			case 58399:
				return mCipherFactory.CreateCipher(mContext, 101, 2);
			case 150:
			case 151:
			case 152:
			case 153:
			case 154:
				return mCipherFactory.CreateCipher(mContext, 14, 2);
			default:
				throw new TlsFatalAlert(80);
			}
		}

		protected virtual TlsKeyExchange CreateDHKeyExchange(int keyExchange)
		{
			return new TlsDHKeyExchange(keyExchange, mSupportedSignatureAlgorithms, null);
		}

		protected virtual TlsKeyExchange CreateDheKeyExchange(int keyExchange)
		{
			return new TlsDheKeyExchange(keyExchange, mSupportedSignatureAlgorithms, null);
		}

		protected virtual TlsKeyExchange CreateECDHKeyExchange(int keyExchange)
		{
			return new TlsECDHKeyExchange(keyExchange, mSupportedSignatureAlgorithms, mNamedCurves, mClientECPointFormats, mServerECPointFormats);
		}

		protected virtual TlsKeyExchange CreateECDheKeyExchange(int keyExchange)
		{
			return new TlsECDheKeyExchange(keyExchange, mSupportedSignatureAlgorithms, mNamedCurves, mClientECPointFormats, mServerECPointFormats);
		}

		protected virtual TlsKeyExchange CreateRsaKeyExchange()
		{
			return new TlsRsaKeyExchange(mSupportedSignatureAlgorithms);
		}
	}
}
