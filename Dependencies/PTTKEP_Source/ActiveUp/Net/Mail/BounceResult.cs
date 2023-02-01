using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class BounceResult
	{
		private int _level;

		private string _email = string.Empty;

		public int Level
		{
			get
			{
				return _level;
			}
			set
			{
				_level = value;
			}
		}

		public string Email
		{
			get
			{
				return _email;
			}
			set
			{
				_email = value;
			}
		}

		public BounceResult()
		{
			_level = 0;
			_email = string.Empty;
		}

		public BounceResult(int level, string email)
		{
			_level = level;
			_email = email;
		}
	}
}
