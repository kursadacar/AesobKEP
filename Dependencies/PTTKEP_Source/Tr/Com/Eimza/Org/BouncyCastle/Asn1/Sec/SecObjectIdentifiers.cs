using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X9;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Sec
{
	internal abstract class SecObjectIdentifiers
	{
		public static readonly DerObjectIdentifier EllipticCurve = new DerObjectIdentifier("1.3.132.0");

		public static readonly DerObjectIdentifier SecT163k1;

		public static readonly DerObjectIdentifier SecT163r1;

		public static readonly DerObjectIdentifier SecT239k1;

		public static readonly DerObjectIdentifier SecT113r1;

		public static readonly DerObjectIdentifier SecT113r2;

		public static readonly DerObjectIdentifier SecP112r1;

		public static readonly DerObjectIdentifier SecP112r2;

		public static readonly DerObjectIdentifier SecP160r1;

		public static readonly DerObjectIdentifier SecP160k1;

		public static readonly DerObjectIdentifier SecP256k1;

		public static readonly DerObjectIdentifier SecT163r2;

		public static readonly DerObjectIdentifier SecT283k1;

		public static readonly DerObjectIdentifier SecT283r1;

		public static readonly DerObjectIdentifier SecT131r1;

		public static readonly DerObjectIdentifier SecT131r2;

		public static readonly DerObjectIdentifier SecT193r1;

		public static readonly DerObjectIdentifier SecT193r2;

		public static readonly DerObjectIdentifier SecT233k1;

		public static readonly DerObjectIdentifier SecT233r1;

		public static readonly DerObjectIdentifier SecP128r1;

		public static readonly DerObjectIdentifier SecP128r2;

		public static readonly DerObjectIdentifier SecP160r2;

		public static readonly DerObjectIdentifier SecP192k1;

		public static readonly DerObjectIdentifier SecP224k1;

		public static readonly DerObjectIdentifier SecP224r1;

		public static readonly DerObjectIdentifier SecP384r1;

		public static readonly DerObjectIdentifier SecP521r1;

		public static readonly DerObjectIdentifier SecT409k1;

		public static readonly DerObjectIdentifier SecT409r1;

		public static readonly DerObjectIdentifier SecT571k1;

		public static readonly DerObjectIdentifier SecT571r1;

		public static readonly DerObjectIdentifier SecP192r1;

		public static readonly DerObjectIdentifier SecP256r1;

		static SecObjectIdentifiers()
		{
			DerObjectIdentifier ellipticCurve = EllipticCurve;
			SecT163k1 = new DerObjectIdentifier(((ellipticCurve != null) ? ellipticCurve.ToString() : null) + ".1");
			DerObjectIdentifier ellipticCurve2 = EllipticCurve;
			SecT163r1 = new DerObjectIdentifier(((ellipticCurve2 != null) ? ellipticCurve2.ToString() : null) + ".2");
			DerObjectIdentifier ellipticCurve3 = EllipticCurve;
			SecT239k1 = new DerObjectIdentifier(((ellipticCurve3 != null) ? ellipticCurve3.ToString() : null) + ".3");
			DerObjectIdentifier ellipticCurve4 = EllipticCurve;
			SecT113r1 = new DerObjectIdentifier(((ellipticCurve4 != null) ? ellipticCurve4.ToString() : null) + ".4");
			DerObjectIdentifier ellipticCurve5 = EllipticCurve;
			SecT113r2 = new DerObjectIdentifier(((ellipticCurve5 != null) ? ellipticCurve5.ToString() : null) + ".5");
			DerObjectIdentifier ellipticCurve6 = EllipticCurve;
			SecP112r1 = new DerObjectIdentifier(((ellipticCurve6 != null) ? ellipticCurve6.ToString() : null) + ".6");
			DerObjectIdentifier ellipticCurve7 = EllipticCurve;
			SecP112r2 = new DerObjectIdentifier(((ellipticCurve7 != null) ? ellipticCurve7.ToString() : null) + ".7");
			DerObjectIdentifier ellipticCurve8 = EllipticCurve;
			SecP160r1 = new DerObjectIdentifier(((ellipticCurve8 != null) ? ellipticCurve8.ToString() : null) + ".8");
			DerObjectIdentifier ellipticCurve9 = EllipticCurve;
			SecP160k1 = new DerObjectIdentifier(((ellipticCurve9 != null) ? ellipticCurve9.ToString() : null) + ".9");
			DerObjectIdentifier ellipticCurve10 = EllipticCurve;
			SecP256k1 = new DerObjectIdentifier(((ellipticCurve10 != null) ? ellipticCurve10.ToString() : null) + ".10");
			DerObjectIdentifier ellipticCurve11 = EllipticCurve;
			SecT163r2 = new DerObjectIdentifier(((ellipticCurve11 != null) ? ellipticCurve11.ToString() : null) + ".15");
			DerObjectIdentifier ellipticCurve12 = EllipticCurve;
			SecT283k1 = new DerObjectIdentifier(((ellipticCurve12 != null) ? ellipticCurve12.ToString() : null) + ".16");
			DerObjectIdentifier ellipticCurve13 = EllipticCurve;
			SecT283r1 = new DerObjectIdentifier(((ellipticCurve13 != null) ? ellipticCurve13.ToString() : null) + ".17");
			DerObjectIdentifier ellipticCurve14 = EllipticCurve;
			SecT131r1 = new DerObjectIdentifier(((ellipticCurve14 != null) ? ellipticCurve14.ToString() : null) + ".22");
			DerObjectIdentifier ellipticCurve15 = EllipticCurve;
			SecT131r2 = new DerObjectIdentifier(((ellipticCurve15 != null) ? ellipticCurve15.ToString() : null) + ".23");
			DerObjectIdentifier ellipticCurve16 = EllipticCurve;
			SecT193r1 = new DerObjectIdentifier(((ellipticCurve16 != null) ? ellipticCurve16.ToString() : null) + ".24");
			DerObjectIdentifier ellipticCurve17 = EllipticCurve;
			SecT193r2 = new DerObjectIdentifier(((ellipticCurve17 != null) ? ellipticCurve17.ToString() : null) + ".25");
			DerObjectIdentifier ellipticCurve18 = EllipticCurve;
			SecT233k1 = new DerObjectIdentifier(((ellipticCurve18 != null) ? ellipticCurve18.ToString() : null) + ".26");
			DerObjectIdentifier ellipticCurve19 = EllipticCurve;
			SecT233r1 = new DerObjectIdentifier(((ellipticCurve19 != null) ? ellipticCurve19.ToString() : null) + ".27");
			DerObjectIdentifier ellipticCurve20 = EllipticCurve;
			SecP128r1 = new DerObjectIdentifier(((ellipticCurve20 != null) ? ellipticCurve20.ToString() : null) + ".28");
			DerObjectIdentifier ellipticCurve21 = EllipticCurve;
			SecP128r2 = new DerObjectIdentifier(((ellipticCurve21 != null) ? ellipticCurve21.ToString() : null) + ".29");
			DerObjectIdentifier ellipticCurve22 = EllipticCurve;
			SecP160r2 = new DerObjectIdentifier(((ellipticCurve22 != null) ? ellipticCurve22.ToString() : null) + ".30");
			DerObjectIdentifier ellipticCurve23 = EllipticCurve;
			SecP192k1 = new DerObjectIdentifier(((ellipticCurve23 != null) ? ellipticCurve23.ToString() : null) + ".31");
			DerObjectIdentifier ellipticCurve24 = EllipticCurve;
			SecP224k1 = new DerObjectIdentifier(((ellipticCurve24 != null) ? ellipticCurve24.ToString() : null) + ".32");
			DerObjectIdentifier ellipticCurve25 = EllipticCurve;
			SecP224r1 = new DerObjectIdentifier(((ellipticCurve25 != null) ? ellipticCurve25.ToString() : null) + ".33");
			DerObjectIdentifier ellipticCurve26 = EllipticCurve;
			SecP384r1 = new DerObjectIdentifier(((ellipticCurve26 != null) ? ellipticCurve26.ToString() : null) + ".34");
			DerObjectIdentifier ellipticCurve27 = EllipticCurve;
			SecP521r1 = new DerObjectIdentifier(((ellipticCurve27 != null) ? ellipticCurve27.ToString() : null) + ".35");
			DerObjectIdentifier ellipticCurve28 = EllipticCurve;
			SecT409k1 = new DerObjectIdentifier(((ellipticCurve28 != null) ? ellipticCurve28.ToString() : null) + ".36");
			DerObjectIdentifier ellipticCurve29 = EllipticCurve;
			SecT409r1 = new DerObjectIdentifier(((ellipticCurve29 != null) ? ellipticCurve29.ToString() : null) + ".37");
			DerObjectIdentifier ellipticCurve30 = EllipticCurve;
			SecT571k1 = new DerObjectIdentifier(((ellipticCurve30 != null) ? ellipticCurve30.ToString() : null) + ".38");
			DerObjectIdentifier ellipticCurve31 = EllipticCurve;
			SecT571r1 = new DerObjectIdentifier(((ellipticCurve31 != null) ? ellipticCurve31.ToString() : null) + ".39");
			SecP192r1 = X9ObjectIdentifiers.Prime192v1;
			SecP256r1 = X9ObjectIdentifiers.Prime256v1;
		}
	}
}
