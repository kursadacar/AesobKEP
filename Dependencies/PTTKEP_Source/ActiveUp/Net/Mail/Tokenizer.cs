using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace ActiveUp.Net.Mail
{
	public class Tokenizer
	{
		public static readonly string NewLine = Environment.NewLine;

		public static string[] Parse(string source)
		{
			string[] array = CleanInput(source).Split(' ');
			ArrayList arrayList = new ArrayList();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i].ToLower();
				if (!arrayList.Contains(text))
				{
					arrayList.Add(text);
				}
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		public static void LoadFromFile(string fileName, ref Hashtable table)
		{
			StreamReader streamReader = new StreamReader(fileName);
			string[] array = Parse(streamReader.ReadToEnd());
			streamReader.Close();
			string[] array2 = array;
			foreach (string key in array2)
			{
				if (table.ContainsKey(key))
				{
					table[key] = 1f + (float)table[key];
				}
				else
				{
					table.Add(key, 1f);
				}
			}
		}

		public static void AddWords(string fileName, string[] Words)
		{
			Hashtable table = new Hashtable();
			LoadFromFile(fileName, ref table);
			TeachListFile(fileName, Words, table);
		}

		public static void TeachListFile(string fileName, string[] msgTokens, Hashtable currentHash)
		{
			StreamWriter streamWriter = new StreamWriter(fileName, true);
			foreach (string text in msgTokens)
			{
				if (!currentHash.ContainsKey(text))
				{
					streamWriter.Write(text + " ");
				}
			}
			streamWriter.Close();
		}

		private static string CleanInput(string strIn)
		{
			return Regex.Replace(strIn, "[^\\w\\'@-]", " ");
		}
	}
}
