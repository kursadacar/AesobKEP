using System;
using System.Collections;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class TraceInfoCollection : CollectionBase
	{
		public TraceInfo this[int index]
		{
			get
			{
				return (TraceInfo)base.InnerList[index];
			}
			set
			{
				base.InnerList[index] = value;
			}
		}

		public void Add(TraceInfo traceInfo)
		{
			base.InnerList.Add(traceInfo);
		}
	}
}
