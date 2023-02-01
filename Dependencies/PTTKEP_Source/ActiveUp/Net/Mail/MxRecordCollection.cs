using System;
using System.Collections;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class MxRecordCollection : CollectionBase
	{
		public MxRecord this[int index]
		{
			get
			{
				return (MxRecord)base.List[index];
			}
		}

		public void Add(MxRecord mxRecord)
		{
			base.List.Add(mxRecord);
		}

		public void Add(string exchange, int preference)
		{
			base.List.Add(new MxRecord(exchange, preference));
		}

		public void Remove(int index)
		{
			if (index < base.Count || index >= 0)
			{
				base.List.RemoveAt(index);
			}
		}

		public MxRecord GetPrefered()
		{
			int num = 0;
			for (int i = 0; i < base.List.Count; i++)
			{
				if (num == -1 || num > this[i].Preference)
				{
					num = i;
				}
			}
			if (num < base.Count && num != -1)
			{
				return this[num];
			}
			return null;
		}
	}
}
