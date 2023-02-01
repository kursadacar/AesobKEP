using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class ParsingException : Exception
	{
		private string _message;

		public override string Message
		{
			get
			{
				return _message;
			}
		}

		public ParsingException(string message)
		{
			_message = message;
		}
	}
}
