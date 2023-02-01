using System.Collections.Generic;

namespace Tr.Com.Eimza.Org.BouncyCastle.Shared
{
	internal class ResultList
	{
		public string ResultMessages { get; set; }

		public bool IsSuccess { get; set; }

		public ResultState ResultState { get; set; }

		public List<ResultList> InnerResultList { get; set; }

		public string CheckerName { get; set; }

		public ResultList(string result, bool isSuccess, ResultState state)
		{
			ResultMessages = result;
			IsSuccess = isSuccess;
			ResultState = state;
		}
	}
}
