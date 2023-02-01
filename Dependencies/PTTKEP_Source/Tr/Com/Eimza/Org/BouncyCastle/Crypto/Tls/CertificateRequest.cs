using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class CertificateRequest
	{
		protected readonly byte[] mCertificateTypes;

		protected readonly IList mSupportedSignatureAlgorithms;

		protected readonly IList mCertificateAuthorities;

		public virtual byte[] CertificateTypes
		{
			get
			{
				return mCertificateTypes;
			}
		}

		public virtual IList SupportedSignatureAlgorithms
		{
			get
			{
				return mSupportedSignatureAlgorithms;
			}
		}

		public virtual IList CertificateAuthorities
		{
			get
			{
				return mCertificateAuthorities;
			}
		}

		public CertificateRequest(byte[] certificateTypes, IList supportedSignatureAlgorithms, IList certificateAuthorities)
		{
			mCertificateTypes = certificateTypes;
			mSupportedSignatureAlgorithms = supportedSignatureAlgorithms;
			mCertificateAuthorities = certificateAuthorities;
		}

		public virtual void Encode(Stream output)
		{
			if (mCertificateTypes == null || mCertificateTypes.Length == 0)
			{
				TlsUtilities.WriteUint8(0, output);
			}
			else
			{
				TlsUtilities.WriteUint8ArrayWithUint8Length(mCertificateTypes, output);
			}
			if (mSupportedSignatureAlgorithms != null)
			{
				TlsUtilities.EncodeSupportedSignatureAlgorithms(mSupportedSignatureAlgorithms, false, output);
			}
			if (mCertificateAuthorities == null || mCertificateAuthorities.Count < 1)
			{
				TlsUtilities.WriteUint16(0, output);
				return;
			}
			IList list = Platform.CreateArrayList(mCertificateAuthorities.Count);
			int num = 0;
			foreach (Asn1Encodable mCertificateAuthority in mCertificateAuthorities)
			{
				byte[] encoded = mCertificateAuthority.GetEncoded("DER");
				list.Add(encoded);
				num += encoded.Length + 2;
			}
			TlsUtilities.CheckUint16(num);
			TlsUtilities.WriteUint16(num, output);
			foreach (byte[] item in list)
			{
				TlsUtilities.WriteOpaque16(item, output);
			}
		}

		public static CertificateRequest Parse(TlsContext context, Stream input)
		{
			int num = TlsUtilities.ReadUint8(input);
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = TlsUtilities.ReadUint8(input);
			}
			IList supportedSignatureAlgorithms = null;
			if (TlsUtilities.IsTlsV12(context))
			{
				supportedSignatureAlgorithms = TlsUtilities.ParseSupportedSignatureAlgorithms(false, input);
			}
			IList list = Platform.CreateArrayList();
			MemoryStream memoryStream = new MemoryStream(TlsUtilities.ReadOpaque16(input), false);
			while (memoryStream.Position < memoryStream.Length)
			{
				Asn1Object obj = TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque16(memoryStream));
				list.Add(X509Name.GetInstance(obj));
			}
			return new CertificateRequest(array, supportedSignatureAlgorithms, list);
		}
	}
}
