using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal class SimpleAttributeTableGenerator : CmsAttributeTableGenerator
	{
		private readonly AttributeTable attributes;

		public SimpleAttributeTableGenerator(AttributeTable attributes)
		{
			this.attributes = attributes;
		}

		public virtual AttributeTable GetAttributes(IDictionary parameters)
		{
			return attributes;
		}
	}
}
