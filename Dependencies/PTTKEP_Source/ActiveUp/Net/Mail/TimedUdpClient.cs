using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class TimedUdpClient : UdpClient
	{
		private byte[] _returnReceive;

		private bool _errorOccurs;

		private Thread _threadReceive;

		private IPEndPoint _remote;

		private Mutex _mutexReturnReceive = new Mutex(false);

		private Mutex _mutexErrorOccurs = new Mutex(false);

		private int _timeout = 2000;

		public int Timeout
		{
			get
			{
				return _timeout;
			}
			set
			{
				_timeout = value;
			}
		}

		public new byte[] Receive(ref IPEndPoint remote)
		{
			_remote = remote;
			_threadReceive = new Thread(StartReceive);
			_threadReceive.Start();
			Thread.Sleep(_timeout);
			_mutexErrorOccurs.WaitOne();
			if (_errorOccurs)
			{
				_mutexErrorOccurs.ReleaseMutex();
				_threadReceive.Abort();
				throw new Exception("Connection timed out");
			}
			_mutexErrorOccurs.ReleaseMutex();
			return _returnReceive;
		}

		private void StartReceive()
		{
			_mutexErrorOccurs.WaitOne();
			_errorOccurs = true;
			_mutexErrorOccurs.ReleaseMutex();
			try
			{
				byte[] returnReceive = base.Receive(ref _remote);
				_mutexReturnReceive.WaitOne();
				_returnReceive = returnReceive;
				_mutexReturnReceive.ReleaseMutex();
				_errorOccurs = false;
			}
			catch (SocketException)
			{
			}
			catch (ThreadAbortException)
			{
			}
		}
	}
}
