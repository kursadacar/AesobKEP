using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.Tls
{
	internal class OcspStatusRequest
	{
		protected readonly IList mResponderIDList;

		protected readonly X509Extensions mRequestExtensions;

		public virtual IList ResponderIDList
		{
			get
			{
				return mResponderIDList;
			}
		}

		public virtual X509Extensions RequestExtensions
		{
			get
			{
				return mRequestExtensions;
			}
		}

		public OcspStatusRequest(IList responderIDList, X509Extensions requestExtensions)
		{
			mResponderIDList = responderIDList;
			mRequestExtensions = requestExtensions;
		}

		public virtual void Encode(Stream output)
		{
			if (mResponderIDList == null || mResponderIDList.Count < 1)
			{
				TlsUtilities.WriteUint16(0, output);
			}
			else
			{
				MemoryStream memoryStream = new MemoryStream();
				for (int i = 0; i < mResponderIDList.Count; i++)
				{
					TlsUtilities.WriteOpaque16(((ResponderID)mResponderIDList[i]).GetEncoded("DER"), memoryStream);
				}
				TlsUtilities.CheckUint16(memoryStream.Length);
				TlsUtilities.WriteUint16((int)memoryStream.Length, output);
				memoryStream.WriteTo(output);
			}
			if (mRequestExtensions == null)
			{
				TlsUtilities.WriteUint16(0, output);
				return;
			}
			byte[] encoded = mRequestExtensions.GetEncoded("DER");
			TlsUtilities.CheckUint16(encoded.Length);
			TlsUtilities.WriteUint16(encoded.Length, output);
			output.Write(encoded, 0, encoded.Length);
		}

		public static OcspStatusRequest Parse(Stream input)
		{
			IList list = Platform.CreateArrayList();
			int num = TlsUtilities.ReadUint16(input);
			if (num > 0)
			{
				MemoryStream memoryStream = new MemoryStream(TlsUtilities.ReadFully(num, input), false);
				do
				{
					ResponderID instance = ResponderID.GetInstance(TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque16(memoryStream)));
					list.Add(instance);
				}
				while (memoryStream.Position < memoryStream.Length);
			}
			X509Extensions requestExtensions = null;
			int num2 = TlsUtilities.ReadUint16(input);
			if (num2 > 0)
			{
				requestExtensions = X509Extensions.GetInstance(TlsUtilities.ReadDerObject(TlsUtilities.ReadFully(num2, input)));
			}
			return new OcspStatusRequest(list, requestExtensions);
		}
	}
}
