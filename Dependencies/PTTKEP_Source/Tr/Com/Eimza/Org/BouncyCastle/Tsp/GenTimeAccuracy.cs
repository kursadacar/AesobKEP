using Tr.Com.Eimza.Org.BouncyCastle.Asn1;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Tsp;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tsp
{
	internal class GenTimeAccuracy
	{
		private Accuracy accuracy;

		public int Seconds
		{
			get
			{
				return GetTimeComponent(accuracy.Seconds);
			}
		}

		public int Millis
		{
			get
			{
				return GetTimeComponent(accuracy.Millis);
			}
		}

		public int Micros
		{
			get
			{
				return GetTimeComponent(accuracy.Micros);
			}
		}

		public GenTimeAccuracy(Accuracy accuracy)
		{
			this.accuracy = accuracy;
		}

		private int GetTimeComponent(DerInteger time)
		{
			if (time != null)
			{
				return time.Value.IntValue;
			}
			return 0;
		}

		public override string ToString()
		{
			return Seconds + "." + Millis.ToString("000") + Micros.ToString("000");
		}
	}
}
