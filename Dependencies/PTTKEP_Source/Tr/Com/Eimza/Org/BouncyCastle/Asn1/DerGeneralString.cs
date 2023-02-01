using System;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1
{
	internal class DerGeneralString : DerStringBase
	{
		private readonly string str;

		public static DerGeneralString GetInstance(object obj)
		{
			if (obj == null || obj is DerGeneralString)
			{
				return (DerGeneralString)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + obj.GetType().Name);
		}

		public static DerGeneralString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerGeneralString)
			{
				return GetInstance(@object);
			}
			return new DerGeneralString(((Asn1OctetString)@object).GetOctets());
		}

		public DerGeneralString(byte[] str)
			: this(Strings.FromAsciiByteArray(str))
		{
		}

		public DerGeneralString(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		public override string GetString()
		{
			return str;
		}

		public byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(str);
		}

		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(27, GetOctets());
		}

		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerGeneralString derGeneralString = asn1Object as DerGeneralString;
			if (derGeneralString == null)
			{
				return false;
			}
			return str.Equals(derGeneralString.str);
		}
	}
}
