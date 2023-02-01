using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cms;

namespace Tr.Com.Eimza.Org.BouncyCastle.Cms
{
	internal interface CmsAttributeTableGenerator
	{
		AttributeTable GetAttributes(IDictionary parameters);
	}
}
