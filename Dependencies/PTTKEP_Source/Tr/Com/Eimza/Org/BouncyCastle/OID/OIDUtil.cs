using System.Text;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;

namespace Tr.Com.Eimza.Org.BouncyCastle.OID
{
	internal static class OIDUtil
	{
		public static string Concat(int[] aOID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < aOID.Length; i++)
			{
				stringBuilder.Append(aOID[i]);
				if (i < aOID.Length - 1)
				{
					stringBuilder.Append('.');
				}
			}
			return stringBuilder.ToString();
		}

		public static DerObjectIdentifier ConvertIdentifier(int[] aOID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < aOID.Length; i++)
			{
				stringBuilder.Append(aOID[i]);
				if (i < aOID.Length - 1)
				{
					stringBuilder.Append('.');
				}
			}
			return new DerObjectIdentifier(stringBuilder.ToString());
		}

		public static int[] Parse(string aInput)
		{
			if (aInput != null)
			{
				string[] array = aInput.Split('.');
				int[] array2 = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = int.Parse(array[i]);
				}
				return array2;
			}
			return new int[0];
		}

		public static DerObjectIdentifier ParseOID(string aInput)
		{
			return ConvertIdentifier(Parse(aInput));
		}

		public static DerObjectIdentifier FromUrnOID(string aOID)
		{
			return new DerObjectIdentifier(aOID.Substring("urn:oid:".Length));
		}

		public static int[] FromURN(string aOID)
		{
			return Parse(aOID.Substring("urn:oid:".Length));
		}

		public static string ToURN(DerObjectIdentifier aOID)
		{
			StringBuilder stringBuilder = new StringBuilder("urn:oid:");
			stringBuilder.Append(aOID.Id);
			return stringBuilder.ToString();
		}

		public static string ToURN(int[] aOID)
		{
			StringBuilder stringBuilder = new StringBuilder("urn:oid:");
			stringBuilder.Append(Concat(aOID));
			return stringBuilder.ToString();
		}

		public static string ToURN(string aOID)
		{
			StringBuilder stringBuilder = new StringBuilder("urn:oid:");
			stringBuilder.Append(aOID);
			return stringBuilder.ToString();
		}
	}
}
