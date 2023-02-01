using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class MxRecord
	{
		private string _exchange;

		private int _preference;

		public string Exchange
		{
			get
			{
				return _exchange;
			}
			set
			{
				_exchange = value;
			}
		}

		public int Preference
		{
			get
			{
				return _preference;
			}
			set
			{
				_preference = value;
			}
		}

		public MxRecord()
		{
			_exchange = string.Empty;
			_preference = 10;
		}

		public MxRecord(string exchange, int preference)
		{
			_exchange = exchange;
			_preference = preference;
		}
	}
}
