using System;
using System.Collections;
using System.Globalization;
using System.Security;

namespace Tr.Com.Eimza.Org.BouncyCastle.Utilities
{
	internal abstract class Platform
	{
		public static readonly string NewLine = GetNewLine();

		public static string GetNewLine()
		{
			return Environment.NewLine;
		}

		public static int CompareIgnoreCase(string a, string b)
		{
			return string.Compare(a, b, true);
		}

		public static string GetEnvironmentVariable(string variable)
		{
			try
			{
				return Environment.GetEnvironmentVariable(variable);
			}
			catch (SecurityException)
			{
				return null;
			}
		}

		public static Exception CreateNotImplementedException(string message)
		{
			return new NotImplementedException(message);
		}

		public static IList CreateArrayList()
		{
			return new ArrayList();
		}

		public static IList CreateArrayList(int capacity)
		{
			return new ArrayList(capacity);
		}

		public static IList CreateArrayList(ICollection collection)
		{
			return new ArrayList(collection);
		}

		public static IList CreateArrayList(IEnumerable collection)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object item in collection)
			{
				arrayList.Add(item);
			}
			return arrayList;
		}

		public static IDictionary CreateHashtable()
		{
			return new Hashtable();
		}

		public static IDictionary CreateHashtable(int capacity)
		{
			return new Hashtable(capacity);
		}

		public static IDictionary CreateHashtable(IDictionary dictionary)
		{
			return new Hashtable(dictionary);
		}

		public static string ToLowerInvariant(string s)
		{
			return s.ToLower(CultureInfo.InvariantCulture);
		}

		public static string ToUpperInvariant(string s)
		{
			return s.ToUpper(CultureInfo.InvariantCulture);
		}
	}
}
