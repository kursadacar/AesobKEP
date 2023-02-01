using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Sec;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X9;
using Tr.Com.Eimza.Org.BouncyCastle.Math;
using Tr.Com.Eimza.Org.BouncyCastle.Math.EC;
using Tr.Com.Eimza.Org.BouncyCastle.Math.EC.Custom.Djb;
using Tr.Com.Eimza.Org.BouncyCastle.Math.EC.Custom.Sec;
using Tr.Com.Eimza.Org.BouncyCastle.Math.EC.Endo;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Encoders;

namespace Tr.Com.Eimza.Org.BouncyCastle.Crypto.EC
{
	internal sealed class CustomNamedCurves
	{
		internal class Curve25519Holder : X9ECParametersHolder
		{
			internal static readonly X9ECParametersHolder Instance = new Curve25519Holder();

			private Curve25519Holder()
			{
			}

			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				ECCurve eCCurve = ConfigureCurve(new Curve25519());
				ECPoint g = eCCurve.DecodePoint(Hex.Decode("042AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD245A20AE19A1B8A086B4E01EDD2C7748D14C923D4D7E6D7C61B229E9C5A27ECED3D9"));
				return new X9ECParameters(eCCurve, g, eCCurve.Order, eCCurve.Cofactor, seed);
			}
		}

		internal class Secp192k1Holder : X9ECParametersHolder
		{
			internal static readonly X9ECParametersHolder Instance = new Secp192k1Holder();

			private Secp192k1Holder()
			{
			}

			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				GlvTypeBParameters p = new GlvTypeBParameters(new BigInteger("bb85691939b869c1d087f601554b96b80cb4f55b35f433c2", 16), new BigInteger("3d84f26c12238d7b4f3d516613c1759033b1a5800175d0b1", 16), new BigInteger[2]
				{
					new BigInteger("71169be7330b3038edb025f1", 16),
					new BigInteger("-b3fb3400dec5c4adceb8655c", 16)
				}, new BigInteger[2]
				{
					new BigInteger("12511cfe811d0f4e6bc688b4d", 16),
					new BigInteger("71169be7330b3038edb025f1", 16)
				}, new BigInteger("71169be7330b3038edb025f1d0f9", 16), new BigInteger("b3fb3400dec5c4adceb8655d4c94", 16), 208);
				ECCurve eCCurve = ConfigureCurveGlv(new SecP192K1Curve(), p);
				ECPoint g = eCCurve.DecodePoint(Hex.Decode("04DB4FF10EC057E9AE26B07D0280B7F4341DA5D1B1EAE06C7D9B2F2F6D9C5628A7844163D015BE86344082AA88D95E2F9D"));
				return new X9ECParameters(eCCurve, g, eCCurve.Order, eCCurve.Cofactor, seed);
			}
		}

		internal class Secp192r1Holder : X9ECParametersHolder
		{
			internal static readonly X9ECParametersHolder Instance = new Secp192r1Holder();

			private Secp192r1Holder()
			{
			}

			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("3045AE6FC8422F64ED579528D38120EAE12196D5");
				ECCurve eCCurve = ConfigureCurve(new SecP192R1Curve());
				ECPoint g = eCCurve.DecodePoint(Hex.Decode("04188DA80EB03090F67CBF20EB43A18800F4FF0AFD82FF101207192B95FFC8DA78631011ED6B24CDD573F977A11E794811"));
				return new X9ECParameters(eCCurve, g, eCCurve.Order, eCCurve.Cofactor, seed);
			}
		}

		internal class Secp224k1Holder : X9ECParametersHolder
		{
			internal static readonly X9ECParametersHolder Instance = new Secp224k1Holder();

			private Secp224k1Holder()
			{
			}

			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				GlvTypeBParameters p = new GlvTypeBParameters(new BigInteger("fe0e87005b4e83761908c5131d552a850b3f58b749c37cf5b84d6768", 16), new BigInteger("60dcd2104c4cbc0be6eeefc2bdd610739ec34e317f9b33046c9e4788", 16), new BigInteger[2]
				{
					new BigInteger("6b8cf07d4ca75c88957d9d670591", 16),
					new BigInteger("-b8adf1378a6eb73409fa6c9c637d", 16)
				}, new BigInteger[2]
				{
					new BigInteger("1243ae1b4d71613bc9f780a03690e", 16),
					new BigInteger("6b8cf07d4ca75c88957d9d670591", 16)
				}, new BigInteger("6b8cf07d4ca75c88957d9d67059037a4", 16), new BigInteger("b8adf1378a6eb73409fa6c9c637ba7f5", 16), 240);
				ECCurve eCCurve = ConfigureCurveGlv(new SecP224K1Curve(), p);
				ECPoint g = eCCurve.DecodePoint(Hex.Decode("04A1455B334DF099DF30FC28A169A467E9E47075A90F7E650EB6B7A45C7E089FED7FBA344282CAFBD6F7E319F7C0B0BD59E2CA4BDB556D61A5"));
				return new X9ECParameters(eCCurve, g, eCCurve.Order, eCCurve.Cofactor, seed);
			}
		}

		internal class Secp224r1Holder : X9ECParametersHolder
		{
			internal static readonly X9ECParametersHolder Instance = new Secp224r1Holder();

			private Secp224r1Holder()
			{
			}

			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("BD71344799D5C7FCDC45B59FA3B9AB8F6A948BC5");
				ECCurve eCCurve = ConfigureCurve(new SecP224R1Curve());
				ECPoint g = eCCurve.DecodePoint(Hex.Decode("04B70E0CBD6BB4BF7F321390B94A03C1D356C21122343280D6115C1D21BD376388B5F723FB4C22DFE6CD4375A05A07476444D5819985007E34"));
				return new X9ECParameters(eCCurve, g, eCCurve.Order, eCCurve.Cofactor, seed);
			}
		}

		internal class Secp256k1Holder : X9ECParametersHolder
		{
			internal static readonly X9ECParametersHolder Instance = new Secp256k1Holder();

			private Secp256k1Holder()
			{
			}

			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = null;
				GlvTypeBParameters p = new GlvTypeBParameters(new BigInteger("7ae96a2b657c07106e64479eac3434e99cf0497512f58995c1396c28719501ee", 16), new BigInteger("5363ad4cc05c30e0a5261c028812645a122e22ea20816678df02967c1b23bd72", 16), new BigInteger[2]
				{
					new BigInteger("3086d221a7d46bcde86c90e49284eb15", 16),
					new BigInteger("-e4437ed6010e88286f547fa90abfe4c3", 16)
				}, new BigInteger[2]
				{
					new BigInteger("114ca50f7a8e2f3f657c1108d9d44cfd8", 16),
					new BigInteger("3086d221a7d46bcde86c90e49284eb15", 16)
				}, new BigInteger("3086d221a7d46bcde86c90e49284eb153dab", 16), new BigInteger("e4437ed6010e88286f547fa90abfe4c42212", 16), 272);
				ECCurve eCCurve = ConfigureCurveGlv(new SecP256K1Curve(), p);
				ECPoint g = eCCurve.DecodePoint(Hex.Decode("0479BE667EF9DCBBAC55A06295CE870B07029BFCDB2DCE28D959F2815B16F81798483ADA7726A3C4655DA4FBFC0E1108A8FD17B448A68554199C47D08FFB10D4B8"));
				return new X9ECParameters(eCCurve, g, eCCurve.Order, eCCurve.Cofactor, seed);
			}
		}

		internal class Secp256r1Holder : X9ECParametersHolder
		{
			internal static readonly X9ECParametersHolder Instance = new Secp256r1Holder();

			private Secp256r1Holder()
			{
			}

			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("C49D360886E704936A6678E1139D26B7819F7E90");
				ECCurve eCCurve = ConfigureCurve(new SecP256R1Curve());
				ECPoint g = eCCurve.DecodePoint(Hex.Decode("046B17D1F2E12C4247F8BCE6E563A440F277037D812DEB33A0F4A13945D898C2964FE342E2FE1A7F9B8EE7EB4A7C0F9E162BCE33576B315ECECBB6406837BF51F5"));
				return new X9ECParameters(eCCurve, g, eCCurve.Order, eCCurve.Cofactor, seed);
			}
		}

		internal class Secp384r1Holder : X9ECParametersHolder
		{
			internal static readonly X9ECParametersHolder Instance = new Secp384r1Holder();

			private Secp384r1Holder()
			{
			}

			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("A335926AA319A27A1D00896A6773A4827ACDAC73");
				ECCurve eCCurve = ConfigureCurve(new SecP384R1Curve());
				ECPoint g = eCCurve.DecodePoint(Hex.Decode("04AA87CA22BE8B05378EB1C71EF320AD746E1D3B628BA79B9859F741E082542A385502F25DBF55296C3A545E3872760AB73617DE4A96262C6F5D9E98BF9292DC29F8F41DBD289A147CE9DA3113B5F0B8C00A60B1CE1D7E819D7A431D7C90EA0E5F"));
				return new X9ECParameters(eCCurve, g, eCCurve.Order, eCCurve.Cofactor, seed);
			}
		}

		internal class Secp521r1Holder : X9ECParametersHolder
		{
			internal static readonly X9ECParametersHolder Instance = new Secp521r1Holder();

			private Secp521r1Holder()
			{
			}

			protected override X9ECParameters CreateParameters()
			{
				byte[] seed = Hex.Decode("D09E8800291CB85396CC6717393284AAA0DA64BA");
				ECCurve eCCurve = ConfigureCurve(new SecP521R1Curve());
				ECPoint g = eCCurve.DecodePoint(Hex.Decode("0400C6858E06B70404E9CD9E3ECB662395B4429C648139053FB521F828AF606B4D3DBAA14B5E77EFE75928FE1DC127A2FFA8DE3348B3C1856A429BF97E7E31C2E5BD66011839296A789A3BC0045C8A5FB42C7D1BD998F54449579B446817AFBD17273E662C97EE72995EF42640C550B9013FAD0761353C7086A272C24088BE94769FD16650"));
				return new X9ECParameters(eCCurve, g, eCCurve.Order, eCCurve.Cofactor, seed);
			}
		}

		private static readonly IDictionary nameToCurve;

		private static readonly IDictionary nameToOid;

		private static readonly IDictionary oidToCurve;

		private static readonly IDictionary oidToName;

		public static IEnumerable Names
		{
			get
			{
				return new EnumerableProxy(nameToCurve.Keys);
			}
		}

		private CustomNamedCurves()
		{
		}

		private static BigInteger FromHex(string hex)
		{
			return new BigInteger(1, Hex.Decode(hex));
		}

		private static ECCurve ConfigureCurve(ECCurve curve)
		{
			return curve;
		}

		private static ECCurve ConfigureCurveGlv(ECCurve c, GlvTypeBParameters p)
		{
			return c.Configure().SetEndomorphism(new GlvTypeBEndomorphism(c, p)).Create();
		}

		private static void DefineCurve(string name, X9ECParametersHolder holder)
		{
			nameToCurve.Add(name, holder);
		}

		private static void DefineCurve(string name, DerObjectIdentifier oid, X9ECParametersHolder holder)
		{
			nameToCurve.Add(name, holder);
			nameToOid.Add(name, oid);
			oidToName.Add(oid, name);
			oidToCurve.Add(oid, holder);
		}

		private static void DefineCurveAlias(string alias, DerObjectIdentifier oid)
		{
			alias = Platform.ToLowerInvariant(alias);
			nameToOid.Add(alias, oid);
			nameToCurve.Add(alias, oidToCurve[oid]);
		}

		static CustomNamedCurves()
		{
			nameToCurve = Platform.CreateHashtable();
			nameToOid = Platform.CreateHashtable();
			oidToCurve = Platform.CreateHashtable();
			oidToName = Platform.CreateHashtable();
			DefineCurve("curve25519", Curve25519Holder.Instance);
			DefineCurve("secp192k1", SecObjectIdentifiers.SecP192k1, Secp192k1Holder.Instance);
			DefineCurve("secp192r1", SecObjectIdentifiers.SecP192r1, Secp192r1Holder.Instance);
			DefineCurve("secp224k1", SecObjectIdentifiers.SecP224k1, Secp224k1Holder.Instance);
			DefineCurve("secp224r1", SecObjectIdentifiers.SecP224r1, Secp224r1Holder.Instance);
			DefineCurve("secp256k1", SecObjectIdentifiers.SecP256k1, Secp256k1Holder.Instance);
			DefineCurve("secp256r1", SecObjectIdentifiers.SecP256r1, Secp256r1Holder.Instance);
			DefineCurve("secp384r1", SecObjectIdentifiers.SecP384r1, Secp384r1Holder.Instance);
			DefineCurve("secp521r1", SecObjectIdentifiers.SecP521r1, Secp521r1Holder.Instance);
			DefineCurveAlias("P-192", SecObjectIdentifiers.SecP192r1);
			DefineCurveAlias("P-224", SecObjectIdentifiers.SecP224r1);
			DefineCurveAlias("P-256", SecObjectIdentifiers.SecP256r1);
			DefineCurveAlias("P-384", SecObjectIdentifiers.SecP384r1);
			DefineCurveAlias("P-521", SecObjectIdentifiers.SecP521r1);
		}

		public static X9ECParameters GetByName(string name)
		{
			X9ECParametersHolder x9ECParametersHolder = (X9ECParametersHolder)nameToCurve[Platform.ToLowerInvariant(name)];
			if (x9ECParametersHolder != null)
			{
				return x9ECParametersHolder.Parameters;
			}
			return null;
		}

		public static X9ECParameters GetByOid(DerObjectIdentifier oid)
		{
			X9ECParametersHolder x9ECParametersHolder = (X9ECParametersHolder)oidToCurve[oid];
			if (x9ECParametersHolder != null)
			{
				return x9ECParametersHolder.Parameters;
			}
			return null;
		}

		public static DerObjectIdentifier GetOid(string name)
		{
			return (DerObjectIdentifier)nameToOid[Platform.ToLowerInvariant(name)];
		}

		public static string GetName(DerObjectIdentifier oid)
		{
			return (string)oidToName[oid];
		}
	}
}
