using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class Attachment : MimePart
	{
		private bool _isLoaded;

		private string _originalPath;
	}
}
