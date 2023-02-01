using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Tr.Com.Eimza.Log4Net;

namespace Tr.Com.Eimza.SmartCard
{
	internal class WinsCard
	{
		private static ASCIIEncoding ascii = new ASCIIEncoding();

		private static readonly ILog mLogger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static char NULL_CHAR = '\0';

		private static long SCARD_E_INSUFFICIENT_BUFFER = 2148532232L;

		private static int SCARD_PROTOCOL_T0 = 1;

		private static int SCARD_PROTOCOL_T1 = 2;

		private static int SCARD_RESET_CARD = 1;

		private static uint SCARD_SCOPE_USER = 0u;

		private static int SCARD_SHARE_SHARED = 2;

		private static long SCARD_W_REMOVED_CARD = 2148532329L;

		private static void Disconnect(IntPtr session)
		{
			long num = SCardDisconnect(session, SCARD_RESET_CARD);
			if (num != 0L)
			{
				mLogger.Error("SCardDisconnect Error: " + num.ToString("X"));
			}
		}

		public static byte[] GetAtr(string terminalName)
		{
			long num = 0L;
			IntPtr phContext = IntPtr.Zero;
			num = SCardEstablishContext(SCARD_SCOPE_USER, 0, 0, ref phContext);
			if (num != 0L)
			{
				ReleaseContext(phContext);
			}
			IntPtr phCard = IntPtr.Zero;
			int ActiveProtocol = 0;
			byte[] bytesWithNullEnd = GetBytesWithNullEnd(terminalName);
			int pcchReaderLen = bytesWithNullEnd.Length;
			num = SCardConnect(phContext, bytesWithNullEnd, SCARD_SHARE_SHARED, SCARD_PROTOCOL_T0 | SCARD_PROTOCOL_T1, ref phCard, ref ActiveProtocol);
			if (num != 0L)
			{
				ReleaseContext(phContext);
			}
			byte[] pbAtr = new byte[0];
			int pcbAtrLen = 0;
			int pdwState = 0;
			num = SCardStatus(phCard, bytesWithNullEnd, ref pcchReaderLen, ref pdwState, ref ActiveProtocol, pbAtr, ref pcbAtrLen);
			if (num != 0L && num != SCARD_E_INSUFFICIENT_BUFFER)
			{
				ReleaseContext(phContext);
			}
			pbAtr = new byte[pcbAtrLen];
			num = SCardStatus(phCard, bytesWithNullEnd, ref pcchReaderLen, ref pdwState, ref ActiveProtocol, pbAtr, ref pcbAtrLen);
			if (num != 0L)
			{
				ReleaseContext(phContext);
			}
			Disconnect(phCard);
			ReleaseContext(phContext);
			return pbAtr;
		}

		private static byte[] GetBytesWithNullEnd(string terminalName)
		{
			byte[] bytes = ascii.GetBytes(terminalName);
			byte[] array = new byte[bytes.Length + 2];
			Array.Copy(bytes, 0, array, 0, bytes.Length);
			array[bytes.Length] = 0;
			array[bytes.Length + 1] = 0;
			return array;
		}

		public static string[] GetTerminalList()
		{
			long num = 0L;
			IntPtr phContext = IntPtr.Zero;
			int pcchReaders = 0;
			int num2 = -1;
			List<string> list = new List<string>();
			num = SCardEstablishContext(SCARD_SCOPE_USER, 0, 0, ref phContext);
			if (num != 0L)
			{
				ReleaseContext(phContext);
				return list.ToArray();
			}
			num = SCardListReaders(phContext, null, null, ref pcchReaders);
			if (num != 0L)
			{
				ReleaseContext(phContext);
				return list.ToArray();
			}
			byte[] array = new byte[pcchReaders];
			num = SCardListReaders(phContext, null, array, ref pcchReaders);
			if (num != 0L)
			{
				ReleaseContext(phContext);
				return list.ToArray();
			}
			string text = ascii.GetString(array);
			int num3 = pcchReaders;
			while (text[0] != NULL_CHAR)
			{
				num2 = text.IndexOf(NULL_CHAR);
				string text2 = text.Substring(0, num2);
				list.Add(text2);
				num3 -= text2.Length + 1;
				text = text.Substring(num2 + 1, num3);
			}
			foreach (string item in new List<string>(list.ToArray()))
			{
				IntPtr phCard = IntPtr.Zero;
				int ActiveProtocol = 0;
				byte[] bytes = ascii.GetBytes(item);
				int num4 = ascii.GetBytes(item).Length;
				num = SCardConnect(phContext, bytes, SCARD_SHARE_SHARED, SCARD_PROTOCOL_T0 | SCARD_PROTOCOL_T1, ref phCard, ref ActiveProtocol);
				if (num == SCARD_W_REMOVED_CARD)
				{
					list.Remove(item);
				}
				else if (num != 0L)
				{
					list.Remove(item);
				}
				num = SCardDisconnect(phCard, SCARD_RESET_CARD);
			}
			ReleaseContext(phContext);
			return list.ToArray();
		}

		private static void ReleaseContext(IntPtr hContext)
		{
			SCardReleaseContext(hContext);
		}

		[DllImport("WinScard.dll", CharSet = CharSet.Ansi)]
		public static extern uint SCardConnect(IntPtr hContext, byte[] cReaderName, int dwShareMode, int dwPrefProtocol, ref IntPtr phCard, ref int ActiveProtocol);

		[DllImport("WinScard.dll")]
		public static extern uint SCardDisconnect(IntPtr hCard, int Disposition);

		[DllImport("WinScard.dll")]
		public static extern uint SCardEstablishContext(uint dwScope, int nNotUsed1, int nNotUsed2, ref IntPtr phContext);

		[DllImport("WinScard.dll")]
		public static extern uint SCardFreeMemory(IntPtr hContext, string cResourceToFree);

		[DllImport("WinScard.dll")]
		public static extern uint SCardGetAttrib(IntPtr hContext, int dwAttrId, ref byte[] bytRecvAttr, ref int nRecLen);

		[DllImport("WinScard.dll")]
		public static extern uint SCardListReaderGroups(IntPtr hContext, ref string cGroups, ref int nStringSize);

		[DllImport("WinScard.dll", CharSet = CharSet.Ansi)]
		public static extern uint SCardListReaders(IntPtr hContext, byte[] mszGroups, byte[] mszReaders, ref int pcchReaders);

		[DllImport("WinScard.dll")]
		public static extern uint SCardReleaseContext(IntPtr phContext);

		[DllImport("WinScard.dll")]
		public static extern uint SCardStatus(IntPtr hCard, byte[] mszReaders, ref int pcchReaderLen, ref int pdwState, ref int pdwProtocol, byte[] pbAtr, ref int pcbAtrLen);
	}
}
