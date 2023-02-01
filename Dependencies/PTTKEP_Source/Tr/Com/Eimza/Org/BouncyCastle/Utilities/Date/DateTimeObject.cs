using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Utilities.Date
{
	internal sealed class DateTimeObject
	{
		private readonly DateTime dt;

		public DateTime Value
		{
			get
			{
				return dt;
			}
		}

		public DateTimeObject(DateTime dt)
		{
			this.dt = dt;
		}

		public override string ToString()
		{
			return dt.ToString();
		}
	}
}
