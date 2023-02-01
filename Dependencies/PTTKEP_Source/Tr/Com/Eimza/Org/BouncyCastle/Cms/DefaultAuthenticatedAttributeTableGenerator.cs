using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class DefaultAuthenticatedAttributeTableGenerator : CmsAttributeTableGenerator
	{
		private readonly IDictionary table;

		public DefaultAuthenticatedAttributeTableGenerator()
		{
			table = Platform.CreateHashtable();
		}

		public DefaultAuthenticatedAttributeTableGenerator(AttributeTable attributeTable)
		{
			table = ((attributeTable != null) ? attributeTable.ToDictionary() : Platform.CreateHashtable());
		}

		protected virtual IDictionary CreateStandardAttributeTable(IDictionary parameters)
		{
			IDictionary dictionary = Platform.CreateHashtable(table);
			if (!dictionary.Contains(CmsAttributes.ContentType))
			{
				DerObjectIdentifier obj = (DerObjectIdentifier)parameters[CmsAttributeTableParameter.ContentType];
				Attribute attribute = new Attribute(CmsAttributes.ContentType, new DerSet(obj));
				dictionary[attribute.AttrType] = attribute;
			}
			if (!dictionary.Contains(CmsAttributes.MessageDigest))
			{
				byte[] str = (byte[])parameters[CmsAttributeTableParameter.Digest];
				Attribute attribute2 = new Attribute(CmsAttributes.MessageDigest, new DerSet(new DerOctetString(str)));
				dictionary[attribute2.AttrType] = attribute2;
			}
			return dictionary;
		}

		public virtual AttributeTable GetAttributes(IDictionary parameters)
		{
			return new AttributeTable(CreateStandardAttributeTable(parameters));
		}
	}
}
