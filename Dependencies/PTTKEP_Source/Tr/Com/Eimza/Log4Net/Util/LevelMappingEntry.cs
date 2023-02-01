using Tr.Com.Eimza.Log4Net.Core;

namespace Tr.Com.Eimza.Log4Net.Util
{
	public abstract class LevelMappingEntry : IOptionHandler
	{
		private Level m_level;

		public Level Level
		{
			get
			{
				return m_level;
			}
			set
			{
				m_level = value;
			}
		}

		public virtual void ActivateOptions()
		{
		}
	}
}
