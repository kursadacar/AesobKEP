using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.L8
{
	internal static class CkmUtils
	{
		public static CK_MECHANISM CreateMechanism(CKM mechanism)
		{
			return CreateMechanism(Convert.ToUInt64((uint)mechanism));
		}

		public static CK_MECHANISM CreateMechanism(ulong mechanism)
		{
			return _CreateMechanism(mechanism, null);
		}

		public static CK_MECHANISM CreateMechanism(CKM mechanism, byte[] parameter)
		{
			return CreateMechanism(Convert.ToUInt64((uint)mechanism), parameter);
		}

		public static CK_MECHANISM CreateMechanism(ulong mechanism, byte[] parameter)
		{
			return _CreateMechanism(mechanism, parameter);
		}

		public static CK_MECHANISM CreateMechanism(CKM mechanism, object parameterStructure)
		{
			if (parameterStructure == null)
			{
				throw new ArgumentNullException("parameterStructure");
			}
			return CreateMechanism(Convert.ToUInt64((uint)mechanism), parameterStructure);
		}

		public static CK_MECHANISM CreateMechanism(ulong mechanism, object parameterStructure)
		{
			if (parameterStructure == null)
			{
				throw new ArgumentNullException("parameterStructure");
			}
			CK_MECHANISM result = default(CK_MECHANISM);
			result.Mechanism = mechanism;
			result.ParameterLen = Convert.ToUInt64(UnmanagedMemory.SizeOf(parameterStructure.GetType()));
			result.Parameter = UnmanagedMemory.Allocate(Convert.ToInt32(result.ParameterLen));
			UnmanagedMemory.Write(result.Parameter, parameterStructure);
			return result;
		}

		private static CK_MECHANISM _CreateMechanism(ulong mechanism, byte[] parameter)
		{
			CK_MECHANISM result = default(CK_MECHANISM);
			result.Mechanism = mechanism;
			if (parameter != null && parameter.Length != 0)
			{
				result.Parameter = UnmanagedMemory.Allocate(parameter.Length);
				UnmanagedMemory.Write(result.Parameter, parameter);
				result.ParameterLen = Convert.ToUInt64(parameter.Length);
			}
			else
			{
				result.Parameter = IntPtr.Zero;
				result.ParameterLen = 0uL;
			}
			return result;
		}
	}
}
