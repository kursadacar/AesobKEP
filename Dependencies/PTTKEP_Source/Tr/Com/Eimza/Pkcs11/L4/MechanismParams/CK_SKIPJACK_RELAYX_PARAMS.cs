using System;
using System.Runtime.InteropServices;

namespace Tr.Com.Eimza.Pkcs11.L4.MechanismParams
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	internal struct CK_SKIPJACK_RELAYX_PARAMS
	{
		public uint OldWrappedXLen;

		public IntPtr OldWrappedX;

		public uint OldPasswordLen;

		public IntPtr OldPassword;

		public uint OldPublicDataLen;

		public IntPtr OldPublicData;

		public uint OldRandomLen;

		public IntPtr OldRandomA;

		public uint NewPasswordLen;

		public IntPtr NewPassword;

		public uint NewPublicDataLen;

		public IntPtr NewPublicData;

		public uint NewRandomLen;

		public IntPtr NewRandomA;
	}
}
