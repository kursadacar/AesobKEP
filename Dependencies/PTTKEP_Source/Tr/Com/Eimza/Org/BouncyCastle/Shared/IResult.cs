using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Shared
{
	internal interface IResult
	{
		string ResultMessage { get; set; }

		bool IsVerify { get; set; }

		ResultState State { get; set; }

		Exception InnerException { get; set; }

		string CheckerName { get; set; }
	}
}
