using System;
using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Collections;

namespace Tr.Com.Eimza.Org.BouncyCastle.Pkcs
{
	internal abstract class Pkcs12Entry
	{
		private readonly IDictionary attributes;

		public Asn1Encodable this[DerObjectIdentifier oid]
		{
			get
			{
				return (Asn1Encodable)attributes[oid.Id];
			}
		}

		public Asn1Encodable this[string oid]
		{
			get
			{
				return (Asn1Encodable)attributes[oid];
			}
		}

		public IEnumerable BagAttributeKeys
		{
			get
			{
				return new EnumerableProxy(attributes.Keys);
			}
		}

		protected internal Pkcs12Entry(IDictionary attributes)
		{
			this.attributes = attributes;
			foreach (DictionaryEntry attribute in attributes)
			{
				if (!(attribute.Key is string))
				{
					throw new ArgumentException("Attribute keys must be of type: " + typeof(string).FullName, "attributes");
				}
				if (!(attribute.Value is Asn1Encodable))
				{
					throw new ArgumentException("Attribute values must be of type: " + typeof(Asn1Encodable).FullName, "attributes");
				}
			}
		}

		[Obsolete("Use 'object[index]' syntax instead")]
		public Asn1Encodable GetBagAttribute(DerObjectIdentifier oid)
		{
			return (Asn1Encodable)attributes[oid.Id];
		}

		[Obsolete("Use 'object[index]' syntax instead")]
		public Asn1Encodable GetBagAttribute(string oid)
		{
			return (Asn1Encodable)attributes[oid];
		}

		[Obsolete("Use 'BagAttributeKeys' property")]
		public IEnumerator GetBagAttributeKeys()
		{
			return attributes.Keys.GetEnumerator();
		}
	}
}
