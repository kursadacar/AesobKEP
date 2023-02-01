using System;
using System.Collections.Generic;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4;

namespace Tr.Com.Eimza.Pkcs11.H4
{
	internal class Pkcs11 : IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.L4.Pkcs11 _p11;

		public Pkcs11(string libraryPath, bool useOsLocking)
		{
			if (libraryPath == null)
			{
				throw new ArgumentNullException("libraryPath");
			}
			_p11 = new Tr.Com.Eimza.Pkcs11.L4.Pkcs11(libraryPath);
			CK_C_INITIALIZE_ARGS initArgs = null;
			if (useOsLocking)
			{
				initArgs = new CK_C_INITIALIZE_ARGS
				{
					Flags = 2u
				};
			}
			CKR cKR = _p11.C_Initialize(initArgs);
			if (cKR != 0 && cKR != CKR.CKR_CRYPTOKI_ALREADY_INITIALIZED)
			{
				throw new Pkcs11Exception("C_Initialize", cKR);
			}
		}

		public Pkcs11(string libraryPath, bool useOsLocking, bool useGetFunctionList)
		{
			if (libraryPath == null)
			{
				throw new ArgumentNullException("libraryPath");
			}
			_p11 = new Tr.Com.Eimza.Pkcs11.L4.Pkcs11(libraryPath, useGetFunctionList);
			CK_C_INITIALIZE_ARGS initArgs = null;
			if (useOsLocking)
			{
				initArgs = new CK_C_INITIALIZE_ARGS
				{
					Flags = 2u
				};
			}
			CKR cKR = _p11.C_Initialize(initArgs);
			if (cKR != 0 && cKR != CKR.CKR_CRYPTOKI_ALREADY_INITIALIZED)
			{
				throw new Pkcs11Exception("C_Initialize", cKR);
			}
		}

		public LibraryInfo GetInfo()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			CK_INFO info = default(CK_INFO);
			CKR cKR = _p11.C_GetInfo(ref info);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GetInfo", cKR);
			}
			return new LibraryInfo(info);
		}

		public List<Slot> GetSlotList(bool tokenPresent)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			uint count = 0u;
			CKR cKR = _p11.C_GetSlotList(tokenPresent, null, ref count);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GetSlotList", cKR);
			}
			uint[] array = new uint[count];
			cKR = _p11.C_GetSlotList(tokenPresent, array, ref count);
			if (cKR != 0)
			{
				throw new Pkcs11Exception("C_GetSlotList", cKR);
			}
			if (array.Length != count)
			{
				Array.Resize(ref array, Convert.ToInt32(count));
			}
			List<Slot> list = new List<Slot>();
			uint[] array2 = array;
			foreach (uint slotId in array2)
			{
				list.Add(new Slot(_p11, slotId));
			}
			return list;
		}

		public void WaitForSlotEvent(bool dontBlock, out bool eventOccured, out uint slotId)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			uint flags = (dontBlock ? 1u : 0u);
			uint slot = 0u;
			CKR cKR = _p11.C_WaitForSlotEvent(flags, ref slot, IntPtr.Zero);
			if (dontBlock)
			{
				switch (cKR)
				{
				case CKR.CKR_OK:
					eventOccured = true;
					slotId = slot;
					break;
				case CKR.CKR_NO_EVENT:
					eventOccured = false;
					slotId = slot;
					break;
				default:
					throw new Pkcs11Exception("C_WaitForSlotEvent", cKR);
				}
			}
			else
			{
				if (cKR != 0)
				{
					throw new Pkcs11Exception("C_WaitForSlotEvent", cKR);
				}
				eventOccured = true;
				slotId = slot;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing && _p11 != null)
				{
					_p11.C_Finalize(IntPtr.Zero);
					_p11.Dispose();
					_p11 = null;
				}
				_disposed = true;
			}
		}

		~Pkcs11()
		{
			Dispose(false);
		}
	}
}
