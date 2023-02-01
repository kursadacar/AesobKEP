using System;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;
using Tr.Com.Eimza.Org.BouncyCastle.Crypto;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ocsp
{
	internal class RespID
	{
		internal readonly ResponderID id;

		public RespID(ResponderID id)
		{
			this.id = id;
		}

		public RespID(X509Name name)
		{
			id = new ResponderID(name);
		}

		public RespID(AsymmetricKeyParameter publicKey)
		{
			try
			{
				SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey);
				byte[] str = DigestUtilities.CalculateDigest("SHA1", subjectPublicKeyInfo.PublicKeyData.GetBytes());
				id = new ResponderID(new DerOctetString(str));
			}
			catch (Exception ex)
			{
				throw new OcspException("problem creating ID: " + ((ex != null) ? ex.ToString() : null), ex);
			}
		}

		public ResponderID ToAsn1Object()
		{
			return id;
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			RespID respID = obj as RespID;
			if (respID == null)
			{
				return false;
			}
			return id.Equals(respID.id);
		}

		public override int GetHashCode()
		{
			return id.GetHashCode();
		}
	}
}
