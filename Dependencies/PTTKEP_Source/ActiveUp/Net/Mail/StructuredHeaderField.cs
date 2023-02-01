using System;
using System.Collections.Specialized;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class StructuredHeaderField
	{
		private NameValueCollection _params = new NameValueCollection();

		public NameValueCollection Parameters
		{
			get
			{
				return _params;
			}
			set
			{
				_params = value;
			}
		}

		public StructuredHeaderField(NameValueCollection parameters)
		{
			Initialize(parameters);
		}

		public StructuredHeaderField()
		{
		}

		private void Initialize(NameValueCollection parameters)
		{
			_params = parameters;
		}
	}
}
