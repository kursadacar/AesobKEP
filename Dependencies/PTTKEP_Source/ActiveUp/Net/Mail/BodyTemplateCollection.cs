using System;
using System.Collections;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class BodyTemplateCollection : CollectionBase
	{
		public BodyTemplate this[int index]
		{
			get
			{
				return (BodyTemplate)base.List[index];
			}
		}

		public void Add(BodyTemplate bodyTemplate)
		{
			base.List.Add(bodyTemplate);
		}

		public void Add(string content)
		{
			base.List.Add(new BodyTemplate(content));
		}

		public void Add(string content, BodyFormat format)
		{
			base.List.Add(new BodyTemplate(content, format));
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
