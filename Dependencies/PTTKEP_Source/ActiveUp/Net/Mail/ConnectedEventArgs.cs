using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class ConnectedEventArgs : EventArgs
	{
		private string serverresponse;

		public string ServerResponse
		{
			get
			{
				return serverresponse;
			}
		}

		public ConnectedEventArgs(string serverresponse)
		{
			this.serverresponse = serverresponse;
		}
	}
}
