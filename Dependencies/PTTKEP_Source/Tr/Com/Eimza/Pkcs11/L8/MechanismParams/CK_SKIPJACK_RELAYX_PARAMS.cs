using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L8.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_SKIPJACK_RELAYX_PARAMS
	{
		public ulong OldWrappedXLen;

		public IntPtr OldWrappedX;

		public ulong OldPasswordLen;

		public IntPtr OldPassword;

		public ulong OldPublicDataLen;

		public IntPtr OldPublicData;

		public ulong OldRandomLen;

		public IntPtr OldRandomA;

		public ulong NewPasswordLen;

		public IntPtr NewPassword;

		public ulong NewPublicDataLen;

		public IntPtr NewPublicData;

		public ulong NewRandomLen;

		public IntPtr NewRandomA;
	}
}
