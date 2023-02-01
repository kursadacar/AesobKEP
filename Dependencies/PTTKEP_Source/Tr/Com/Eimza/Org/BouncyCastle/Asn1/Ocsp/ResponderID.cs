using System;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Ocsp
{
	internal class ResponderID : Asn1Encodable, IAsn1Choice
	{
		private readonly Asn1Encodable id;

		public virtual Asn1OctetString ID
		{
			get
			{
				if (id is X509Name)
				{
					return null;
				}
				return Asn1OctetString.GetInstance(id);
			}
		}

		public virtual X509Name Name
		{
			get
			{
				if (id is Asn1OctetString)
				{
					return null;
				}
				return X509Name.GetInstance(id);
			}
		}

		public int ResponderIDType
		{
			get
			{
				if (id != null)
				{
					return ((DerTaggedObject)ToAsn1Object()).TagNo;
				}
				return -1;
			}
		}

		public static ResponderID GetInstance(object obj)
		{
			if (obj == null || obj is ResponderID)
			{
				return (ResponderID)obj;
			}
			if (obj is DerOctetString)
			{
				return new ResponderID((DerOctetString)obj);
			}
			if (obj is Asn1TaggedObject)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
				if (asn1TaggedObject.TagNo == 1)
				{
					return new ResponderID(X509Name.GetInstance(asn1TaggedObject, true));
				}
				return new ResponderID(Asn1OctetString.GetInstance(asn1TaggedObject, true));
			}
			return new ResponderID(X509Name.GetInstance(obj));
		}

		public ResponderID(Asn1OctetString id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			this.id = id;
		}

		public ResponderID(X509Name id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			this.id = id;
		}

		public static ResponderID GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return GetInstance(obj.GetObject());
		}

		public virtual byte[] GetKeyHash()
		{
			if (id is Asn1OctetString)
			{
				return ((Asn1OctetString)id).GetOctets();
			}
			if (id is X509Name)
			{
				return ((X509Name)id).GetDerEncoded();
			}
			return null;
		}

		public virtual string GetKeyString()
		{
			if (id is Asn1OctetString)
			{
				return ((Asn1OctetString)id).ToString();
			}
			if (id is X509Name)
			{
				return ((X509Name)id).StringValue;
			}
			return null;
		}

		public override Asn1Object ToAsn1Object()
		{
			if (id is Asn1OctetString)
			{
				return new DerTaggedObject(true, 2, id);
			}
			return new DerTaggedObject(true, 1, id);
		}
	}
}
