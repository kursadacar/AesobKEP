using System;
using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class DefaultSignedAttributeTableGenerator : CmsAttributeTableGenerator
	{
		private readonly IDictionary table;

		public DefaultSignedAttributeTableGenerator()
		{
			table = Platform.CreateHashtable();
		}

		public DefaultSignedAttributeTableGenerator(AttributeTable attributeTable)
		{
			table = ((attributeTable != null) ? attributeTable.ToDictionary() : Platform.CreateHashtable());
		}

		protected virtual Hashtable createStandardAttributeTable(IDictionary parameters)
		{
			Hashtable hashtable = new Hashtable(table);
			DoCreateStandardAttributeTable(parameters, hashtable);
			return hashtable;
		}

		private void DoCreateStandardAttributeTable(IDictionary parameters, IDictionary std)
		{
			if (parameters.Contains(CmsAttributeTableParameter.ContentType) && !std.Contains(CmsAttributes.ContentType))
			{
				DerObjectIdentifier obj = (DerObjectIdentifier)parameters[CmsAttributeTableParameter.ContentType];
				Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute = new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute(CmsAttributes.ContentType, new DerSet(obj));
				std[attribute.AttrType] = attribute;
			}
			if (!std.Contains(CmsAttributes.SigningTime))
			{
				Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute2 = new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute(CmsAttributes.SigningTime, new DerSet(new Time(DateTime.UtcNow)));
				std[attribute2.AttrType] = attribute2;
			}
			if (!std.Contains(CmsAttributes.MessageDigest))
			{
				byte[] str = (byte[])parameters[CmsAttributeTableParameter.Digest];
				Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute attribute3 = new Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms.Attribute(CmsAttributes.MessageDigest, new DerSet(new DerOctetString(str)));
				std[attribute3.AttrType] = attribute3;
			}
		}

		public virtual AttributeTable GetAttributes(IDictionary parameters)
		{
			return new AttributeTable((IDictionary)createStandardAttributeTable(parameters));
		}
	}
}
