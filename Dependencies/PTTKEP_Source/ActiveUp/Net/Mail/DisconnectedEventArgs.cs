using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class DisconnectedEventArgs : EventArgs
	{
		private string serverresponse;

		public string ServerResponse
		{
			get
			{
				return serverresponse;
			}
		}

		public DisconnectedEventArgs(string serverresponse)
		{
			this.serverresponse = serverresponse;
		}
	}
}
