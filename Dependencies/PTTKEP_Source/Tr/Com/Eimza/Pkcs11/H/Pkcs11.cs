using System;
using System.Collections.Generic;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class Pkcs11 : IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.Pkcs11 _p11_4;

		private Tr.Com.Eimza.Pkcs11.H8.Pkcs11 _p11_8;

		public Pkcs11(string libraryPath, bool useOsLocking)
		{
			if (libraryPath == null)
			{
				throw new ArgumentNullException("libraryPath");
			}
			if (UnmanagedLong.Size == 4)
			{
				_p11_4 = new Tr.Com.Eimza.Pkcs11.H4.Pkcs11(libraryPath, useOsLocking);
			}
			else
			{
				_p11_8 = new Tr.Com.Eimza.Pkcs11.H8.Pkcs11(libraryPath, useOsLocking);
			}
		}

		public Pkcs11(string libraryPath, bool useOsLocking, bool useGetFunctionList)
		{
			if (libraryPath == null)
			{
				throw new ArgumentNullException("libraryPath");
			}
			if (UnmanagedLong.Size == 4)
			{
				_p11_4 = new Tr.Com.Eimza.Pkcs11.H4.Pkcs11(libraryPath, useOsLocking, useGetFunctionList);
			}
			else
			{
				_p11_8 = new Tr.Com.Eimza.Pkcs11.H8.Pkcs11(libraryPath, useOsLocking, useGetFunctionList);
			}
		}

		public LibraryInfo GetInfo()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				return new LibraryInfo(_p11_4.GetInfo());
			}
			return new LibraryInfo(_p11_8.GetInfo());
		}

		public List<Slot> GetSlotList(bool tokenPresent)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				List<Tr.Com.Eimza.Pkcs11.H4.Slot> slotList = _p11_4.GetSlotList(tokenPresent);
				List<Slot> list = new List<Slot>();
				{
					foreach (Tr.Com.Eimza.Pkcs11.H4.Slot item in slotList)
					{
						list.Add(new Slot(item));
					}
					return list;
				}
			}
			List<Tr.Com.Eimza.Pkcs11.H8.Slot> slotList2 = _p11_8.GetSlotList(tokenPresent);
			List<Slot> list2 = new List<Slot>();
			foreach (Tr.Com.Eimza.Pkcs11.H8.Slot item2 in slotList2)
			{
				list2.Add(new Slot(item2));
			}
			return list2;
		}

		public void WaitForSlotEvent(bool dontBlock, out bool eventOccured, out ulong slotId)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				uint slotId2 = 0u;
				_p11_4.WaitForSlotEvent(dontBlock, out eventOccured, out slotId2);
				slotId = slotId2;
			}
			else
			{
				_p11_8.WaitForSlotEvent(dontBlock, out eventOccured, out slotId);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}
			if (disposing)
			{
				if (_p11_4 != null)
				{
					_p11_4.Dispose();
					_p11_4 = null;
				}
				if (_p11_8 != null)
				{
					_p11_8.Dispose();
					_p11_8 = null;
				}
			}
			_disposed = true;
		}

		~Pkcs11()
		{
			Dispose(false);
		}
	}
}
