using System.Collections;

namespace ActiveUp.Net.Mail
{
	public class BayesianFilter
	{
		public static bool AnalyzeMessage(string subject, string body, string spamWordsFilename, string hamWordsFilename, string ignoreWordsFilename)
		{
			Hashtable table = new Hashtable();
			Tokenizer.LoadFromFile(spamWordsFilename, ref table);
			Hashtable table2 = new Hashtable();
			Tokenizer.LoadFromFile(hamWordsFilename, ref table2);
			Hashtable table3 = new Hashtable();
			Tokenizer.LoadFromFile(ignoreWordsFilename, ref table3);
			string[] array = Tokenizer.Parse(subject + " " + body);
			float num = 0f;
			float num2 = 0f;
			string[] array2 = array;
			foreach (string key in array2)
			{
				if (!table3.Contains(key))
				{
					float num3 = (table.ContainsKey(key) ? ((float)table[key]) : 0f);
					float num4 = (table2.ContainsKey(key) ? ((float)table2[key]) : 0f);
					if (num3 != 0f || num4 != 0f)
					{
						float num5 = num3 / (float)table.Count;
						float num6 = num4 / (float)table2.Count;
						float num7 = num5 / (num5 + num6);
						float num8 = 1f;
						float num9 = 0.5f;
						float num10 = num3 + num4;
						float num11 = (num8 * num9 + num10 * num7) / (num8 + num10);
						num = ((num == 0f) ? num11 : (num * num11));
						num2 = ((num2 == 0f) ? (1f - num11) : (num2 * (1f - num11)));
					}
				}
			}
			float num12 = num / (num + num2);
			if ((double)num12 <= 0.45)
			{
				return false;
			}
			if ((double)num12 >= 0.55)
			{
				return true;
			}
			return false;
		}

		public static void ReportMessage(Message message, string filename)
		{
			string text = string.Empty;
			if (string.IsNullOrEmpty(message.BodyHtml.Text))
			{
				text += message.BodyHtml.TextStripped;
			}
			if (string.IsNullOrEmpty(message.BodyText.Text))
			{
				text += message.BodyText.Text;
			}
			string[] words = Tokenizer.Parse(Codec.RFC2047Decode(message.Subject) + " " + text);
			Tokenizer.AddWords(filename, words);
		}
	}
}
