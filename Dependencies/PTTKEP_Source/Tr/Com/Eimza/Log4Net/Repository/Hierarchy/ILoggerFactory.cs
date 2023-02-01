namespace Tr.Com.Eimza.Log4Net.Repository.Hierarchy
{
	public interface ILoggerFactory
	{
		Logger CreateLogger(string name);
	}
}
