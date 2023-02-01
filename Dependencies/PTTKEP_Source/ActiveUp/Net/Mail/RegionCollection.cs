using System;
using System.Collections;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class RegionCollection : CollectionBase
	{
		public Region this[int index]
		{
			get
			{
				return (Region)base.List[index];
			}
		}

		public Region this[string regionID]
		{
			get
			{
				foreach (Region item in base.List)
				{
					if (item.RegionID == regionID)
					{
						return item;
					}
				}
				return null;
			}
		}

		public void Add(Region region)
		{
			base.List.Add(region);
		}

		public void Add(string regionid, string url)
		{
			base.List.Add(new Region(regionid, url));
		}

		public void Remove(int index)
		{
			if (index < base.Count || index >= 0)
			{
				base.List.RemoveAt(index);
			}
		}

		public bool Contains(string regionID)
		{
			foreach (Region item in base.List)
			{
				if (item.RegionID == regionID)
				{
					return true;
				}
			}
			return false;
		}
	}
}
