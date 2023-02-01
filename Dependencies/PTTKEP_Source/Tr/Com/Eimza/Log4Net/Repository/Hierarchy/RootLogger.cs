using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Repository.Hierarchy
{
	public class RootLogger : Logger
	{
		public override Level EffectiveLevel
		{
			get
			{
				return base.Level;
			}
		}

		public override Level Level
		{
			get
			{
				return base.Level;
			}
			set
			{
				if (value == null)
				{
					LogLog.Error("RootLogger: You have tried to set a null level to root.", new LogException());
				}
				else
				{
					base.Level = value;
				}
			}
		}

		public RootLogger(Level level)
			: base("root")
		{
			Level = level;
		}
	}
}
