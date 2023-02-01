using System;
using System.Collections;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class ListTemplateCollection : CollectionBase
	{
		public ListTemplate this[int index]
		{
			get
			{
				return (ListTemplate)base.List[index];
			}
		}

		public void Add(ListTemplate listTemplate)
		{
			base.List.Add(listTemplate);
		}

		public void Add(string name, string content)
		{
			base.List.Add(new ListTemplate(name, content));
		}

		public void Remove(int index)
		{
			if (index < base.Count || index >= 0)
			{
				base.List.RemoveAt(index);
			}
		}
	}
}
