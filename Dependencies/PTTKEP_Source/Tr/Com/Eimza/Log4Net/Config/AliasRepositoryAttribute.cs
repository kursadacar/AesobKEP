using System;

namespace Tr.Com.Eimza.Log4Net.Config
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class AliasRepositoryAttribute : Attribute
	{
		private string m_name;

		public string Name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public AliasRepositoryAttribute(string name)
		{
			Name = name;
		}
	}
}
