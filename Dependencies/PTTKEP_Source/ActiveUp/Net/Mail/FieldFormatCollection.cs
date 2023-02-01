using System;
using System.Collections;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class FieldFormatCollection : CollectionBase
	{
		public FieldFormat this[int index]
		{
			get
			{
				return (FieldFormat)base.List[index];
			}
		}

		public FieldFormat this[string name]
		{
			get
			{
				foreach (FieldFormat item in base.List)
				{
					if (item.Name.ToLower() == name.ToLower())
					{
						return item;
					}
				}
				return null;
			}
		}

		public void Add(FieldFormat fieldFormat)
		{
			base.List.Add(fieldFormat);
		}

		public void Add(string name, string format)
		{
			base.List.Add(new FieldFormat(name, format));
		}

		public void Add(string name, string format, PaddingDirection paddingDir, int totalWidth, char paddingChar)
		{
			base.List.Add(new FieldFormat(name, format, paddingDir, totalWidth, paddingChar));
		}

		public void Remove(int index)
		{
			if (index < base.Count || index >= 0)
			{
				base.List.RemoveAt(index);
			}
		}

		public bool Contains(string name)
		{
			foreach (FieldFormat item in base.List)
			{
				if (item.Name.ToLower() == name.ToLower())
				{
					return true;
				}
			}
			return false;
		}
	}
}
