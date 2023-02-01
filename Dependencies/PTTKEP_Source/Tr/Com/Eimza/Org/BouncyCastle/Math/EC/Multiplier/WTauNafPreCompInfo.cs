namespace Tr.Com.Eimza.Org.BouncyCastle.Math.EC.Multiplier
{
	internal class WTauNafPreCompInfo : PreCompInfo
	{
		protected F2mPoint[] m_preComp;

		public virtual F2mPoint[] PreComp
		{
			get
			{
				return m_preComp;
			}
			set
			{
				m_preComp = value;
			}
		}
	}
}
