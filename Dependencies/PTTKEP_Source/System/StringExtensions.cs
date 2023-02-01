using System.Text;

namespace System
{
	internal static class StringExtensions
	{
		public static byte[] GetBytes(this string str)
		{
			return Encoding.UTF8.GetBytes(str);
		}
	}
}
