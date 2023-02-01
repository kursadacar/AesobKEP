namespace ActiveUp.Net.Security
{
	public class Canonicalizer
	{
		public static string Canonicalize(string input, CanonicalizationAlgorithm algorithm)
		{
			if (algorithm.Equals(CanonicalizationAlgorithm.NoFws))
			{
				return NoFws(input);
			}
			if (algorithm.Equals(CanonicalizationAlgorithm.Simple))
			{
				return Simple(input);
			}
			return input;
		}

		private static string NoFws(string input)
		{
			return input.Replace(" ", "").Replace("\t", "");
		}

		private static string Simple(string input)
		{
			return input;
		}
	}
}
