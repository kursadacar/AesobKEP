using System;

namespace Tr.Com.Eimza.Log4Net.Core
{
	public interface IErrorHandler
	{
		void Error(string message, Exception e, ErrorCode errorCode);

		void Error(string message, Exception e);

		void Error(string message);
	}
}
