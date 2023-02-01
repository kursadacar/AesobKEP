using System;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ocsp
{
	internal class RespData : X509ExtensionBase
	{
		internal readonly ResponseData data;

		public int Version
		{
			get
			{
				return data.Version.Value.IntValue + 1;
			}
		}

		public DateTime ProducedAt
		{
			get
			{
				return data.ProducedAt.ToDateTime();
			}
		}

		public X509Extensions ResponseExtensions
		{
			get
			{
				return data.ResponseExtensions;
			}
		}

		public RespData(ResponseData data)
		{
			this.data = data;
		}

		public RespID GetResponderId()
		{
			return new RespID(data.ResponderID);
		}

		public SingleResp[] GetResponses()
		{
			Asn1Sequence responses = data.Responses;
			SingleResp[] array = new SingleResp[responses.Count];
			for (int i = 0; i != array.Length; i++)
			{
				array[i] = new SingleResp(SingleResponse.GetInstance(responses[i]));
			}
			return array;
		}

		protected override X509Extensions GetX509Extensions()
		{
			return ResponseExtensions;
		}
	}
}
