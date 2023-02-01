namespace ActiveUp.Net.Mail
{
	public interface IFlagCollection
	{
		void Add(IFlag flag);

		void Add(string flagName);
	}
}
