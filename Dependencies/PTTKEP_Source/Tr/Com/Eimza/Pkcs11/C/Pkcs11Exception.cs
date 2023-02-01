using System;

namespace Tr.Com.Eimza.Pkcs11.C
{
	internal class Pkcs11Exception : Exception
	{
		private string _method;

		private CKR _rv;

		public string Method
		{
			get
			{
				return _method;
			}
		}

		public CKR RV
		{
			get
			{
				return _rv;
			}
		}

		public Pkcs11Exception(string method, CKR rv)
			: base(string.Format("Method {0} returned {1}", method, rv.ToString()))
		{
			_method = method;
			_rv = rv;
		}
	}
}
