using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509.Qualified
{
	internal class TypeOfBiometricData : Asn1Encodable, IAsn1Choice
	{
		public const int Picture = 0;

		public const int HandwrittenSignature = 1;

		internal Asn1Encodable obj;

		public bool IsPredefined
		{
			get
			{
				return obj is DerInteger;
			}
		}

		public int PredefinedBiometricType
		{
			get
			{
				return ((DerInteger)obj).Value.IntValue;
			}
		}

		public DerObjectIdentifier BiometricDataOid
		{
			get
			{
				return (DerObjectIdentifier)obj;
			}
		}

		public static TypeOfBiometricData GetInstance(object obj)
		{
			if (obj == null || obj is TypeOfBiometricData)
			{
				return (TypeOfBiometricData)obj;
			}
			if (obj is DerInteger)
			{
				return new TypeOfBiometricData(DerInteger.GetInstance(obj).Value.IntValue);
			}
			if (obj is DerObjectIdentifier)
			{
				return new TypeOfBiometricData(DerObjectIdentifier.GetInstance(obj));
			}
			throw new ArgumentException("unknown object in GetInstance: " + obj.GetType().FullName, "obj");
		}

		public TypeOfBiometricData(int predefinedBiometricType)
		{
			if (predefinedBiometricType == 0 || predefinedBiometricType == 1)
			{
				obj = new DerInteger(predefinedBiometricType);
				return;
			}
			throw new ArgumentException("unknow PredefinedBiometricType : " + predefinedBiometricType);
		}

		public TypeOfBiometricData(DerObjectIdentifier biometricDataOid)
		{
			obj = biometricDataOid;
		}

		public override Asn1Object ToAsn1Object()
		{
			return obj.ToAsn1Object();
		}
	}
}
