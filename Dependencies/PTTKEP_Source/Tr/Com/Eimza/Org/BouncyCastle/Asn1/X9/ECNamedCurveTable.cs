using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Nist;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Sec;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.TeleTrust;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Collections;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X9
{
	internal class ECNamedCurveTable
	{
		public static IEnumerable Names
		{
			get
			{
				IList list = Platform.CreateArrayList();
				CollectionUtilities.AddRange(list, X962NamedCurves.Names);
				CollectionUtilities.AddRange(list, SecNamedCurves.Names);
				CollectionUtilities.AddRange(list, NistNamedCurves.Names);
				CollectionUtilities.AddRange(list, TeleTrusTNamedCurves.Names);
				return list;
			}
		}

		public static X9ECParameters GetByName(string name)
		{
			X9ECParameters byName = X962NamedCurves.GetByName(name);
			if (byName == null)
			{
				byName = SecNamedCurves.GetByName(name);
			}
			if (byName == null)
			{
				byName = TeleTrusTNamedCurves.GetByName(name);
			}
			if (byName == null)
			{
				byName = NistNamedCurves.GetByName(name);
			}
			return byName;
		}

		public static DerObjectIdentifier GetOid(string name)
		{
			DerObjectIdentifier oid = X962NamedCurves.GetOid(name);
			if (oid == null)
			{
				oid = SecNamedCurves.GetOid(name);
			}
			if (oid == null)
			{
				oid = TeleTrusTNamedCurves.GetOid(name);
			}
			if (oid == null)
			{
				oid = NistNamedCurves.GetOid(name);
			}
			return oid;
		}

		public static X9ECParameters GetByOid(DerObjectIdentifier oid)
		{
			X9ECParameters byOid = X962NamedCurves.GetByOid(oid);
			if (byOid == null)
			{
				byOid = SecNamedCurves.GetByOid(oid);
			}
			if (byOid == null)
			{
				byOid = TeleTrusTNamedCurves.GetByOid(oid);
			}
			return byOid;
		}
	}
}
