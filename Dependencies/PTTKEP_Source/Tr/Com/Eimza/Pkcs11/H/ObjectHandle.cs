using System;
using System.Collections.Generic;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class ObjectHandle
	{
		private Tr.Com.Eimza.Pkcs11.H4.ObjectHandle _objectHandle4;

		private Tr.Com.Eimza.Pkcs11.H8.ObjectHandle _objectHandle8;

		internal Tr.Com.Eimza.Pkcs11.H4.ObjectHandle ObjectHandle4
		{
			get
			{
				return _objectHandle4;
			}
		}

		internal Tr.Com.Eimza.Pkcs11.H8.ObjectHandle ObjectHandle8
		{
			get
			{
				return _objectHandle8;
			}
		}

		public ulong ObjectId
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _objectHandle8.ObjectId;
				}
				return _objectHandle4.ObjectId;
			}
		}

		public ObjectHandle()
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectHandle4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectHandle();
			}
			else
			{
				_objectHandle8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectHandle();
			}
		}

		internal ObjectHandle(Tr.Com.Eimza.Pkcs11.H4.ObjectHandle objectHandle)
		{
			if (objectHandle == null)
			{
				throw new ArgumentNullException("objectHandle");
			}
			_objectHandle4 = objectHandle;
		}

		internal ObjectHandle(Tr.Com.Eimza.Pkcs11.H8.ObjectHandle objectHandle)
		{
			if (objectHandle == null)
			{
				throw new ArgumentNullException("objectHandle");
			}
			_objectHandle8 = objectHandle;
		}

		internal static List<Tr.Com.Eimza.Pkcs11.H4.ObjectHandle> ConvertToH4List(List<ObjectHandle> objectHandles)
		{
			List<Tr.Com.Eimza.Pkcs11.H4.ObjectHandle> list = null;
			if (objectHandles != null)
			{
				list = new List<Tr.Com.Eimza.Pkcs11.H4.ObjectHandle>();
				for (int i = 0; i < objectHandles.Count; i++)
				{
					list.Add(objectHandles[i].ObjectHandle4);
				}
			}
			return list;
		}

		internal static List<ObjectHandle> ConvertFromH4List(List<Tr.Com.Eimza.Pkcs11.H4.ObjectHandle> hlaObjectHandles)
		{
			List<ObjectHandle> list = null;
			if (hlaObjectHandles != null)
			{
				list = new List<ObjectHandle>();
				for (int i = 0; i < hlaObjectHandles.Count; i++)
				{
					list.Add(new ObjectHandle(hlaObjectHandles[i]));
				}
			}
			return list;
		}

		internal static List<Tr.Com.Eimza.Pkcs11.H8.ObjectHandle> ConvertToH8List(List<ObjectHandle> objectHandles)
		{
			List<Tr.Com.Eimza.Pkcs11.H8.ObjectHandle> list = null;
			if (objectHandles != null)
			{
				list = new List<Tr.Com.Eimza.Pkcs11.H8.ObjectHandle>();
				for (int i = 0; i < objectHandles.Count; i++)
				{
					list.Add(objectHandles[i].ObjectHandle8);
				}
			}
			return list;
		}

		internal static List<ObjectHandle> ConvertFromH8List(List<Tr.Com.Eimza.Pkcs11.H8.ObjectHandle> hlaObjectHandles)
		{
			List<ObjectHandle> list = null;
			if (hlaObjectHandles != null)
			{
				list = new List<ObjectHandle>();
				for (int i = 0; i < hlaObjectHandles.Count; i++)
				{
					list.Add(new ObjectHandle(hlaObjectHandles[i]));
				}
			}
			return list;
		}
	}
}
