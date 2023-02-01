using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Security.Certificates;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.X509;
using Tr.Com.Eimza.Org.BouncyCastle.X509.Store;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ocsp
{
	internal class BasicOcspResp : X509ExtensionBase
	{
		private readonly BasicOcspResponse resp;

		private readonly ResponseData data;

		public int Version
		{
			get
			{
				return data.Version.Value.IntValue + 1;
			}
		}

		public RespID ResponderId
		{
			get
			{
				return new RespID(data.ResponderID);
			}
		}

		public DateTime ProducedAt
		{
			get
			{
				return data.ProducedAt.ToDateTime();
			}
		}

		public BasicOcspResponse Resp
		{
			get
			{
				return resp;
			}
		}

		public SingleResp[] Responses
		{
			get
			{
				Asn1Sequence responses = data.Responses;
				SingleResp[] array = new SingleResp[responses.Count];
				for (int i = 0; i != array.Length; i++)
				{
					array[i] = new SingleResp(SingleResponse.GetInstance(responses[i]));
				}
				return array;
			}
		}

		public X509Extensions ResponseExtensions
		{
			get
			{
				return data.ResponseExtensions;
			}
		}

		public string SignatureAlgName
		{
			get
			{
				return OcspUtilities.GetAlgorithmName(resp.SignatureAlgorithm.ObjectID);
			}
		}

		public string SignatureAlgOid
		{
			get
			{
				return resp.SignatureAlgorithm.ObjectID.Id;
			}
		}

		public BasicOcspResp(BasicOcspResponse resp)
		{
			this.resp = resp;
			data = resp.TbsResponseData;
		}

		public byte[] GetTbsResponseData()
		{
			try
			{
				return data.GetDerEncoded();
			}
			catch (IOException e)
			{
				throw new OcspException("problem encoding tbsResponseData", e);
			}
		}

		protected override X509Extensions GetX509Extensions()
		{
			return ResponseExtensions;
		}

		[Obsolete("RespData class is no longer required as all functionality is available on this class")]
		public RespData GetResponseData()
		{
			return new RespData(data);
		}

		public byte[] GetSignature()
		{
			return resp.Signature.GetBytes();
		}

		private IList GetCertList()
		{
			IList list = Platform.CreateArrayList();
			Asn1Sequence certs = resp.Certs;
			if (certs != null)
			{
				foreach (Asn1Encodable item in certs)
				{
					try
					{
						list.Add(new X509Certificate(item.GetEncoded()));
					}
					catch (IOException e)
					{
						throw new OcspException("can't re-encode certificate!", e);
					}
					catch (CertificateException e2)
					{
						throw new OcspException("can't re-encode certificate!", e2);
					}
				}
				return list;
			}
			return list;
		}

		public X509Certificate[] GetCerts()
		{
			IList certList = GetCertList();
			X509Certificate[] array = new X509Certificate[certList.Count];
			for (int i = 0; i < certList.Count; i++)
			{
				array[i] = (X509Certificate)certList[i];
			}
			return array;
		}

		public IX509Store GetCertificates(string type)
		{
			try
			{
				return X509StoreFactory.Create("Certificate/" + type, new X509CollectionStoreParameters(GetCertList()));
			}
			catch (Exception e)
			{
				throw new OcspException("can't setup the CertStore", e);
			}
		}

		public bool Verify(AsymmetricKeyParameter publicKey)
		{
			try
			{
				ISigner signer = SignerUtilities.GetSigner(SignatureAlgName);
				signer.Init(false, publicKey);
				byte[] derEncoded = data.GetDerEncoded();
				signer.BlockUpdate(derEncoded, 0, derEncoded.Length);
				return signer.VerifySignature(GetSignature());
			}
			catch (Exception ex)
			{
				throw new OcspException("exception processing sig: " + ((ex != null) ? ex.ToString() : null), ex);
			}
		}

		public byte[] GetEncoded()
		{
			return resp.GetEncoded();
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			BasicOcspResp basicOcspResp = obj as BasicOcspResp;
			if (basicOcspResp == null)
			{
				return false;
			}
			return resp.Equals(basicOcspResp.resp);
		}

		public override int GetHashCode()
		{
			return resp.GetHashCode();
		}
	}
}
