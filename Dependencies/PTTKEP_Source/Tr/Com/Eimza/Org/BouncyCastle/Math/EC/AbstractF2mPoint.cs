using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Math.EC
{
	internal abstract class AbstractF2mPoint : ECPointBase
	{
		protected AbstractF2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression)
			: base(curve, x, y, withCompression)
		{
		}

		protected AbstractF2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
			: base(curve, x, y, zs, withCompression)
		{
		}

		protected override bool SatisfiesCurveEquation()
		{
			ECCurve curve = Curve;
			ECFieldElement rawXCoord = base.RawXCoord;
			ECFieldElement rawYCoord = base.RawYCoord;
			ECFieldElement eCFieldElement = curve.A;
			ECFieldElement eCFieldElement2 = curve.B;
			int coordinateSystem = curve.CoordinateSystem;
			ECFieldElement eCFieldElement5;
			ECFieldElement eCFieldElement4;
			if (coordinateSystem == 6)
			{
				ECFieldElement eCFieldElement3 = base.RawZCoords[0];
				bool isOne = eCFieldElement3.IsOne;
				if (rawXCoord.IsZero)
				{
					eCFieldElement4 = rawYCoord.Square();
					eCFieldElement5 = eCFieldElement2;
					if (!isOne)
					{
						ECFieldElement b = eCFieldElement3.Square();
						eCFieldElement5 = eCFieldElement5.Multiply(b);
					}
				}
				else
				{
					ECFieldElement eCFieldElement6 = rawYCoord;
					ECFieldElement eCFieldElement7 = rawXCoord.Square();
					if (isOne)
					{
						eCFieldElement4 = eCFieldElement6.Square().Add(eCFieldElement6).Add(eCFieldElement);
						eCFieldElement5 = eCFieldElement7.Square().Add(eCFieldElement2);
					}
					else
					{
						ECFieldElement eCFieldElement8 = eCFieldElement3.Square();
						ECFieldElement y = eCFieldElement8.Square();
						eCFieldElement4 = eCFieldElement6.Add(eCFieldElement3).MultiplyPlusProduct(eCFieldElement6, eCFieldElement, eCFieldElement8);
						eCFieldElement5 = eCFieldElement7.SquarePlusProduct(eCFieldElement2, y);
					}
					eCFieldElement4 = eCFieldElement4.Multiply(eCFieldElement7);
				}
			}
			else
			{
				eCFieldElement4 = rawYCoord.Add(rawXCoord).Multiply(rawYCoord);
				switch (coordinateSystem)
				{
				case 1:
				{
					ECFieldElement eCFieldElement9 = base.RawZCoords[0];
					if (!eCFieldElement9.IsOne)
					{
						ECFieldElement b2 = eCFieldElement9.Square();
						ECFieldElement b3 = eCFieldElement9.Multiply(b2);
						eCFieldElement4 = eCFieldElement4.Multiply(eCFieldElement9);
						eCFieldElement = eCFieldElement.Multiply(eCFieldElement9);
						eCFieldElement2 = eCFieldElement2.Multiply(b3);
					}
					break;
				}
				default:
					throw new InvalidOperationException("unsupported coordinate system");
				case 0:
					break;
				}
				eCFieldElement5 = rawXCoord.Add(eCFieldElement).Multiply(rawXCoord.Square()).Add(eCFieldElement2);
			}
			return eCFieldElement4.Equals(eCFieldElement5);
		}
	}
}
