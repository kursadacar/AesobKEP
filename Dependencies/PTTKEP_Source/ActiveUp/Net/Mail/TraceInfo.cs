using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class TraceInfo
	{
		private string _from = string.Empty;

		private string _by = string.Empty;

		private string _via = string.Empty;

		private string _with = string.Empty;

		private string _for = string.Empty;

		private string _id = string.Empty;

		private string _source = string.Empty;

		private DateTime _date;

		public string From
		{
			get
			{
				return _from;
			}
			set
			{
				_from = value;
			}
		}

		public string By
		{
			get
			{
				return _by;
			}
			set
			{
				_by = value;
			}
		}

		public string Via
		{
			get
			{
				return _via;
			}
			set
			{
				_via = value;
			}
		}

		public string With
		{
			get
			{
				return _with;
			}
			set
			{
				_with = value;
			}
		}

		public string Id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		public string For
		{
			get
			{
				return _for;
			}
			set
			{
				_for = value;
			}
		}

		public DateTime Date
		{
			get
			{
				return _date;
			}
			set
			{
				_date = value;
			}
		}

		public TraceInfo()
		{
		}

		public TraceInfo(string from, DateTime date, string by, string via, string with, string ffor, string id)
		{
			Initialize(from, date, by, via, with, ffor, id);
		}

		public TraceInfo(string from, DateTime date, string by, string via, string with, string ffor)
		{
			Initialize(from, date, by, via, with, ffor, string.Empty);
		}

		public TraceInfo(string from, DateTime date, string by, string via, string with)
		{
			Initialize(from, date, by, via, with, string.Empty, string.Empty);
		}

		public TraceInfo(string from, DateTime date, string by, string via)
		{
			Initialize(from, date, by, via, string.Empty, string.Empty, string.Empty);
		}

		public TraceInfo(string from, DateTime date, string by)
		{
			Initialize(from, date, by, string.Empty, string.Empty, string.Empty, string.Empty);
		}

		private void Initialize(string from, DateTime date, string by, string via, string with, string ffor, string id)
		{
			_from = from;
			_by = by;
			_via = via;
			_with = with;
			_for = ffor;
			_id = id;
			_date = date;
		}

		public override string ToString()
		{
			string text = string.Empty;
			if (!From.Equals(string.Empty))
			{
				text = text + " from " + From + "\r\n ";
			}
			if (!By.Equals(string.Empty))
			{
				text = text + " by " + By + "\r\n ";
			}
			if (!With.Equals(string.Empty))
			{
				text = text + " with " + With + "\r\n ";
			}
			if (!For.Equals(string.Empty))
			{
				text = text + " for " + For + "\r\n ";
			}
			if (!Via.Equals(string.Empty))
			{
				text = text + " via " + Via + "\r\n ";
			}
			if (!Id.Equals(string.Empty))
			{
				text = text + " id " + Id + "\r\n ";
			}
			if (string.IsNullOrEmpty(text))
			{
				return "";
			}
			return text.Remove(0, text.Length - 3) + ";" + Date.ToString("r");
		}
	}
}
