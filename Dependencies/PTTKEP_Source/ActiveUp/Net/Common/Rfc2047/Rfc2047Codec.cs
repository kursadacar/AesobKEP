using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ActiveUp.Net.Mail;

namespace ActiveUp.Net.Common.Rfc2047
{
	public static class Rfc2047Codec
	{
		private static readonly char[] _linearWhiteSpaceChars = new char[4] { ' ', '\t', '\r', '\n' };

		private static readonly Regex _linearWhiteSpaceRegex = new Regex(Join("|", _linearWhiteSpaceChars), RegexOptions.Compiled);

		private static readonly Regex EncodedWordRegex = new Regex("(=\\?)(?<charset>[^(\\?)]*)(\\?)(?<encoding>[BbQq])(\\?)(?<message>[^ ?]*)(\\?=)", RegexOptions.CultureInvariant);

		public static string Join(string sp, char[] regex)
		{
			string text = "";
			for (int i = 0; i < regex.Length; i++)
			{
				text += regex[i];
				if (i == regex.Length - 1)
				{
					break;
				}
				text += sp;
			}
			return text;
		}

		public static string Decode(string input)
		{
			bool flag = StripComments(ref input);
			IEnumerator<string> enumerator = TokenizeOnLinearWhiteSpace(input).GetEnumerator();
			if (!enumerator.MoveNext())
			{
				return string.Empty;
			}
			string text = OutputTokens(WrapTokensAndLinkThem(enumerator));
			if (flag)
			{
				return "(" + text + ")";
			}
			return text;
		}

		private static TokenBase WrapTokensAndLinkThem(IEnumerator<string> tokenEnumerator)
		{
			TokenBase tokenBase = WrapToken(tokenEnumerator.Current);
			TokenBase tokenBase2 = ((tokenBase is Separator) ? null : tokenBase);
			TokenBase tokenBase3 = ((tokenBase is Separator) ? tokenBase : null);
			while (tokenEnumerator.MoveNext())
			{
				TokenBase tokenBase4 = WrapToken(tokenEnumerator.Current);
				if (tokenBase4 is Separator)
				{
					tokenBase2.NextSeparator = tokenBase4;
					tokenBase3 = tokenBase4;
					continue;
				}
				if (tokenBase2 != null)
				{
					tokenBase2.NextWord = tokenBase4;
				}
				tokenBase3.NextWord = tokenBase4;
				tokenBase2 = tokenBase4;
			}
			return tokenBase;
		}

		private static string OutputTokens(TokenBase token)
		{
			if (token == null)
			{
				return string.Empty;
			}
			string text = ((token.NextSeparator != null) ? token.NextSeparator.GetStringValue() : string.Empty);
			if (token is AsciiWord)
			{
				return token.GetStringValue() + text + OutputTokens(token.NextWord);
			}
			if (token is EncodedWord)
			{
				EncodedWord encodedWord = token as EncodedWord;
				if (encodedWord.CanBeConcatenatedWith(encodedWord.NextWord))
				{
					return OutputTokens(EncodedWord.Concat(encodedWord, encodedWord.NextWord as EncodedWord));
				}
				if (encodedWord.NextWord is EncodedWord)
				{
					return token.GetStringValue() + OutputTokens(token.NextWord);
				}
				return token.GetStringValue() + text + OutputTokens(token.NextWord);
			}
			if (token is Separator)
			{
				if (token.NextWord is EncodedWord)
				{
					return OutputTokens(token.NextWord);
				}
				return token.GetStringValue() + OutputTokens(token.NextWord);
			}
			throw new InvalidOperationException("Unknown token type");
		}

		internal static IEnumerable<string> TokenizeOnLinearWhiteSpace(string input)
		{
			int num = 0;
			int i = 0;
			while (i < input.Length)
			{
				if (IsLinearWhiteSpace(input[i]))
				{
					if (i != num)
					{
						yield return input.Substring(num, i - num);
					}
					yield return EatUpSeparator(input, ref i);
					num = i + 1;
				}
				int num2 = i + 1;
				i = num2;
			}
			if (input.Length != num)
			{
				yield return input.Substring(num, input.Length - num);
			}
		}

		private static bool IsLinearWhiteSpace(char input)
		{
			return _linearWhiteSpaceChars.Contains(input);
		}

		private static string EatUpSeparator(string input, ref int i)
		{
			List<char> list = new List<char> { input[i] };
			int num = i + 1;
			while (num < input.Length && IsLinearWhiteSpace(input[num]))
			{
				list.Add(input[num]);
				i = num++;
			}
			return new string(list.ToArray());
		}

		private static TokenBase WrapToken(string rawToken)
		{
			if (_linearWhiteSpaceRegex.IsMatch(rawToken))
			{
				return new Separator(rawToken);
			}
			Match match = EncodedWordRegex.Match(rawToken);
			if (match.Success)
			{
				return new EncodedWord(match.Groups["message"].Value, match.Groups["charset"].Value, match.Groups["encoding"].Value);
			}
			return new AsciiWord(rawToken);
		}

		private static bool StripComments(ref string input)
		{
			bool result = input.StartsWith("(") && input.EndsWith(")");
			input = input.Trim('(', ')');
			return result;
		}

		public static string Encode(string input, string charset, bool isBase64 = true)
		{
			if (isBase64)
			{
				return "=?" + charset + "?B?" + Convert.ToBase64String(Codec.GetEncoding(charset).GetBytes(input)) + "?=";
			}
			return "=?" + charset + "?Q?" + Codec.ToQuotedPrintable(input, charset) + "?=";
		}
	}
}
