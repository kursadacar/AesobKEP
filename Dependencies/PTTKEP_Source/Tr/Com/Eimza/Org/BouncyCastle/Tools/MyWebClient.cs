using System;
using System.Net;

namespace Tr.Com.Eimza.Org.BouncyCastle.Tools
{
	internal class MyWebClient : WebClient
	{
		private int timeout = 100000;

		protected override WebRequest GetWebRequest(Uri aAddress)
		{
			HttpWebRequest obj = (HttpWebRequest)base.GetWebRequest(aAddress);
			obj.KeepAlive = false;
			obj.Timeout = timeout;
			return obj;
		}

		public void SetTimeOut(int aTimeOut)
		{
			timeout = aTimeOut;
		}
	}
}
