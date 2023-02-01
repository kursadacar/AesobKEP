using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp;
using Tr.Com.Eimza.Org.BouncyCastle.Ocsp;

namespace System
{
	internal static class BouncyCastleExtensions
	{
		public static BasicOcspResponse ToBasicOcspResponse(this OcspResponse ocspResponse)
		{
			return BasicOcspResponse.GetInstance(Asn1Object.FromByteArray(ocspResponse.ResponseBytes.Response.GetOctets()));
		}

		public static BasicOcspResponse ToBasicOcspResponse(this BasicOcspResp basicOcspResp)
		{
			return basicOcspResp.Resp;
		}

		public static BasicOcspResp ToBasicOcspResp(this BasicOcspResponse basicOcspResponse)
		{
			return new BasicOcspResp(basicOcspResponse);
		}

		public static BasicOcspResp ToBasicOcspResp(this OcspResponse ocspResponse)
		{
			return new BasicOcspResp(BasicOcspResponse.GetInstance(Asn1Object.FromByteArray(ocspResponse.ResponseBytes.Response.GetOctets())));
		}

		public static BasicOcspResponse ToBasicOcspResponse(this OcspResp ocspResp)
		{
			return BasicOcspResponse.GetInstance(Asn1Object.FromByteArray(ocspResp.Resp.ResponseBytes.Response.GetOctets()));
		}

		public static BasicOcspResp ToBasicOcspResp(this OcspResp ocspResp)
		{
			return new BasicOcspResp(BasicOcspResponse.GetInstance(Asn1Object.FromByteArray(ocspResp.Resp.ResponseBytes.Response.GetOctets())));
		}

		public static OcspResponse ToOcspResponse(this BasicOcspResponse basicOcspResponse)
		{
			OcspResponseStatus responseStatus = new OcspResponseStatus(0);
			ResponseBytes responseBytes = new ResponseBytes(OcspObjectIdentifiers.PkixOcspNonce, new DerOctetString(basicOcspResponse.GetDerEncoded()));
			return new OcspResponse(responseStatus, responseBytes);
		}

		public static OcspResponse ToOcspResponse(this BasicOcspResp basicOcspResp)
		{
			OcspResponseStatus responseStatus = new OcspResponseStatus(0);
			ResponseBytes responseBytes = new ResponseBytes(OcspObjectIdentifiers.PkixOcspNonce, new DerOctetString(basicOcspResp.Resp.GetDerEncoded()));
			return new OcspResponse(responseStatus, responseBytes);
		}

		public static OcspResponse ToOcspResponse(this OcspResp basicOcspResponse)
		{
			return basicOcspResponse.Resp;
		}

		public static OcspResp ToOcspResp(this OcspResponse basicOcspResponse)
		{
			return new OcspResp(basicOcspResponse);
		}

		public static OcspResp ToOcspResp(this BasicOcspResponse basicOcspResponse)
		{
			OcspResponseStatus responseStatus = new OcspResponseStatus(0);
			ResponseBytes responseBytes = new ResponseBytes(OcspObjectIdentifiers.PkixOcspNonce, Asn1OctetString.GetInstance(basicOcspResponse.GetDerEncoded()));
			return new OcspResp(new OcspResponse(responseStatus, responseBytes));
		}

		public static OcspResp ToOcspResp(this BasicOcspResp basicOcspResp)
		{
			OcspResponseStatus responseStatus = new OcspResponseStatus(0);
			ResponseBytes responseBytes = new ResponseBytes(OcspObjectIdentifiers.PkixOcspNonce, Asn1OctetString.GetInstance(basicOcspResp.Resp.GetDerEncoded()));
			return new OcspResp(new OcspResponse(responseStatus, responseBytes));
		}
	}
}
