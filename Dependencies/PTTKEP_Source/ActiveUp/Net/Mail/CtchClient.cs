using System.Net.Sockets;
using System.Text;

namespace ActiveUp.Net.Mail
{
	public class CtchClient
	{
		public static CtchResponse QueryServer(string host, int port, string messageFilename)
		{
			return QueryServer(host, port, null, messageFilename);
		}

		public static CtchResponse QueryServer(string host, int port, Message message)
		{
			return QueryServer(host, port, message, string.Empty);
		}

		private static CtchResponse QueryServer(string host, int port, Message message, string filename)
		{
			bool flag = true;
			if (message != null)
			{
				flag = false;
			}
			else
			{
				message = Parser.ParseMessageFromFile(filename);
			}
			string arg = "0000001";
			string text = string.Format("X-CTCH-PVer: {0}\r\nX-CTCH-MailFrom: {1}\r\nX-CTCH-SenderIP: {2}\r\n", arg, message.Sender.Email, message.SenderIP);
			text = ((!flag) ? (text + string.Format("\r\n{0}", message.ToMimeString())) : (text + string.Format("X-CTCH-FileName: {0}\r\n", filename)));
			string s = string.Format("POST /ctasd/{0} HTTP/1.0\r\nContent-Length: {1}\r\n\r\n", flag ? "ClassifyMessage_File" : "ClassifyMessage_Inline", text.Length) + text;
			TcpClient tcpClient = new TcpClient();
			tcpClient.Connect(host, port);
			byte[] bytes = Encoding.ASCII.GetBytes(s);
			NetworkStream stream = tcpClient.GetStream();
			stream.Write(bytes, 0, bytes.Length);
			bytes = new byte[256];
			string empty = string.Empty;
			int count = stream.Read(bytes, 0, bytes.Length);
			CtchResponse result = CtchResponse.ParseFromString(Encoding.ASCII.GetString(bytes, 0, count));
			stream.Close();
			tcpClient.Close();
			return result;
		}
	}
}
