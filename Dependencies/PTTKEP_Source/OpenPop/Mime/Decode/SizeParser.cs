using System;
using System.Collections.Generic;
using System.Globalization;

namespace OpenPop.Mime.Decode
{
	internal static class SizeParser
	{
		private static readonly Dictionary<string, long> UnitsToMultiplicator = InitializeSizes();

		private static Dictionary<string, long> InitializeSizes()
		{
			return new Dictionary<string, long>
			{
				{ "", 1L },
				{ "B", 1L },
				{ "KB", 1024L },
				{ "MB", 1048576L },
				{ "GB", 1073741824L },
				{ "TB", 1099511627776L }
			};
		}

		public static long Parse(string value)
		{
			value = value.Trim();
			string text = ExtractUnit(value);
			string s = value.Substring(0, value.Length - text.Length).Trim();
			long num = MultiplicatorForUnit(text);
			double num2 = double.Parse(s, NumberStyles.Number, CultureInfo.InvariantCulture);
			return (long)((double)num * num2);
		}

		private static string ExtractUnit(string sizeWithUnit)
		{
			int num = sizeWithUnit.Length - 1;
			int i;
			for (i = 0; i <= num && sizeWithUnit[num - i] != ' ' && !IsDigit(sizeWithUnit[num - i]); i++)
			{
			}
			return sizeWithUnit.Substring(sizeWithUnit.Length - i).ToUpperInvariant();
		}

		private static bool IsDigit(char value)
		{
			if (value >= '0')
			{
				return value <= '9';
			}
			return false;
		}

		private static long MultiplicatorForUnit(string unit)
		{
			unit = unit.ToUpperInvariant();
			if (!UnitsToMultiplicator.ContainsKey(unit))
			{
				throw new ArgumentException("illegal or unknown unit: \"" + unit + "\"", "unit");
			}
			return UnitsToMultiplicator[unit];
		}
	}
}
