using System;

namespace ActiveUp.Net.Mail
{
	internal class ProductHelper
	{
		public static string GetTrialString(string stringToModify, TrialStringType trialStringType)
		{
			if (stringToModify == null)
			{
				stringToModify = string.Empty;
			}
			Random random = new Random();
			string randomString = GetRandomString(9 + random.Next(5));
			string text = string.Format(" (This email has been created using the trial version of ActiveUp.MailSystem. When you register the product, this message and all trial texts disappear. In addition of this text, the subject, the attachment filenames and the recipient/sender names are modified with a random trial string (ex:{0}). http://www.activeup.com) ", randomString);
			int startIndex = random.Next(stringToModify.Length);
			switch (trialStringType)
			{
			default:
				return stringToModify.Insert(startIndex, randomString);
			case TrialStringType.LongHtml:
				return stringToModify.Insert(startIndex, string.Format("<table bgcolor=\"#F00000\" border=\"1\"><tr><td>{0}</td></tr></table>", text));
			case TrialStringType.LongText:
				return stringToModify.Insert(startIndex, text);
			}
		}

		public static string GetRandomString(int length)
		{
			string text = "abcdefghijkl mnopqrstu vwxyz, .:+éèçà*ùµ~&=%;?  ";
			Random random = new Random();
			string text2 = string.Empty;
			string text3 = ((random.Next(2) == 0) ? "TRIAL" : "EVAL");
			for (int i = 0; i < length - text3.Length - 2; i++)
			{
				int index = random.Next(text.Length);
				text2 += text[index];
			}
			text2 = text2.Insert(random.Next(text2.Length), text3);
			return " " + text2 + " ";
		}
	}
}
