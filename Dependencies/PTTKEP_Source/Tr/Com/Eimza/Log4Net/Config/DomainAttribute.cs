using System;

namespace Tr.Com.Eimza.Log4Net.Config
{
	[Serializable]
	[AttributeUsage(AttributeTargets.Assembly)]
	[Obsolete("Use RepositoryAttribute instead of DomainAttribute")]
	public sealed class DomainAttribute : RepositoryAttribute
	{
		public DomainAttribute()
		{
		}

		public DomainAttribute(string name)
			: base(name)
		{
		}
	}
}
