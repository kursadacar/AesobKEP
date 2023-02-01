using System;
using System.Collections.Generic;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class ObjectAttribute : IDisposable
	{
		private bool _disposed;

		private Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute _objectAttribute4;

		private Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute _objectAttribute8;

		internal Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute ObjectAttribute4
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return _objectAttribute4;
			}
		}

		internal Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute ObjectAttribute8
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return _objectAttribute8;
			}
		}

		public ulong Type
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return _objectAttribute8.Type;
				}
				return _objectAttribute4.Type;
			}
		}

		public bool CannotBeRead
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				if (UnmanagedLong.Size != 4)
				{
					return _objectAttribute8.CannotBeRead;
				}
				return _objectAttribute4.CannotBeRead;
			}
		}

		internal ObjectAttribute(Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute objectAttribute)
		{
			if (objectAttribute == null)
			{
				throw new ArgumentNullException("objectAttribute");
			}
			_objectAttribute4 = objectAttribute;
		}

		internal ObjectAttribute(Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute objectAttribute)
		{
			if (objectAttribute == null)
			{
				throw new ArgumentNullException("objectAttribute");
			}
			_objectAttribute8 = objectAttribute;
		}

		public ObjectAttribute(ulong type)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(Convert.ToUInt32(type));
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type);
			}
		}

		public ObjectAttribute(CKA type)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(type);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type);
			}
		}

		public ObjectAttribute(ulong type, ulong value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(Convert.ToUInt32(type), Convert.ToUInt32(value));
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public ObjectAttribute(CKA type, ulong value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(type, Convert.ToUInt32(value));
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public ObjectAttribute(CKA type, CKC value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(type, value);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public ObjectAttribute(CKA type, CKK value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(type, value);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public ObjectAttribute(CKA type, CKO value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(type, value);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public ulong GetValueAsUlong()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				return _objectAttribute4.GetValueAsUint();
			}
			return _objectAttribute8.GetValueAsUlong();
		}

		public ObjectAttribute(ulong type, bool value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(Convert.ToUInt32(type), value);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public ObjectAttribute(CKA type, bool value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(type, value);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public bool GetValueAsBool()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				return _objectAttribute4.GetValueAsBool();
			}
			return _objectAttribute8.GetValueAsBool();
		}

		public ObjectAttribute(ulong type, string value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(Convert.ToUInt32(type), value);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public ObjectAttribute(CKA type, string value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(type, value);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public string GetValueAsString()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				return _objectAttribute4.GetValueAsString();
			}
			return _objectAttribute8.GetValueAsString();
		}

		public ObjectAttribute(ulong type, byte[] value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(Convert.ToUInt32(type), value);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public ObjectAttribute(CKA type, byte[] value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(type, value);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public byte[] GetValueAsByteArray()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				return _objectAttribute4.GetValueAsByteArray();
			}
			return _objectAttribute8.GetValueAsByteArray();
		}

		public ObjectAttribute(ulong type, DateTime value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(Convert.ToUInt32(type), value);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public ObjectAttribute(CKA type, DateTime value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(type, value);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public DateTime? GetValueAsDateTime()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				return _objectAttribute4.GetValueAsDateTime();
			}
			return _objectAttribute8.GetValueAsDateTime();
		}

		public ObjectAttribute(ulong type, List<ObjectAttribute> value)
		{
			if (UnmanagedLong.Size == 4)
			{
				List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> value2 = ConvertToH4List(value);
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(Convert.ToUInt32(type), value2);
			}
			else
			{
				List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> value3 = ConvertToH8List(value);
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value3);
			}
		}

		public ObjectAttribute(CKA type, List<ObjectAttribute> value)
		{
			if (UnmanagedLong.Size == 4)
			{
				List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> value2 = ConvertToH4List(value);
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(type, value2);
			}
			else
			{
				List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> value3 = ConvertToH8List(value);
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value3);
			}
		}

		public List<ObjectAttribute> GetValueAsObjectAttributeList()
		{
			throw new NotImplementedException();
		}

		public ObjectAttribute(ulong type, List<ulong> value)
		{
			if (UnmanagedLong.Size == 4)
			{
				List<uint> list = null;
				if (value != null)
				{
					list = new List<uint>();
					for (int i = 0; i < value.Count; i++)
					{
						list.Add(Convert.ToUInt32(value[i]));
					}
				}
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(Convert.ToUInt32(type), list);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public ObjectAttribute(CKA type, List<ulong> value)
		{
			if (UnmanagedLong.Size == 4)
			{
				List<uint> list = null;
				if (value != null)
				{
					list = new List<uint>();
					for (int i = 0; i < value.Count; i++)
					{
						list.Add(Convert.ToUInt32(value[i]));
					}
				}
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(type, list);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public List<ulong> GetValueAsUlongList()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				List<uint> valueAsUintList = _objectAttribute4.GetValueAsUintList();
				List<ulong> list = null;
				if (valueAsUintList != null)
				{
					list = new List<ulong>();
					for (int i = 0; i < valueAsUintList.Count; i++)
					{
						list.Add(Convert.ToUInt64(valueAsUintList[i]));
					}
				}
				return list;
			}
			return _objectAttribute8.GetValueAsUlongList();
		}

		public ObjectAttribute(ulong type, List<CKM> value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(Convert.ToUInt32(type), value);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public ObjectAttribute(CKA type, List<CKM> value)
		{
			if (UnmanagedLong.Size == 4)
			{
				_objectAttribute4 = new Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute(type, value);
			}
			else
			{
				_objectAttribute8 = new Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute(type, value);
			}
		}

		public List<CKM> GetValueAsCkmList()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			if (UnmanagedLong.Size == 4)
			{
				return _objectAttribute4.GetValueAsCkmList();
			}
			return _objectAttribute8.GetValueAsCkmList();
		}

		internal static List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> ConvertToH4List(List<ObjectAttribute> attributes)
		{
			List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> list = null;
			if (attributes != null)
			{
				list = new List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute>();
				for (int i = 0; i < attributes.Count; i++)
				{
					list.Add(attributes[i].ObjectAttribute4);
				}
			}
			return list;
		}

		internal static List<ObjectAttribute> ConvertFromH4List(List<Tr.Com.Eimza.Pkcs11.H4.ObjectAttribute> hlaAttributes)
		{
			List<ObjectAttribute> list = null;
			if (hlaAttributes != null)
			{
				list = new List<ObjectAttribute>();
				for (int i = 0; i < hlaAttributes.Count; i++)
				{
					list.Add(new ObjectAttribute(hlaAttributes[i]));
				}
			}
			return list;
		}

		internal static List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> ConvertToH8List(List<ObjectAttribute> attributes)
		{
			List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> list = null;
			if (attributes != null)
			{
				list = new List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute>();
				for (int i = 0; i < attributes.Count; i++)
				{
					list.Add(attributes[i].ObjectAttribute8);
				}
			}
			return list;
		}

		internal static List<ObjectAttribute> ConvertFromH8List(List<Tr.Com.Eimza.Pkcs11.H8.ObjectAttribute> hlaAttributes)
		{
			List<ObjectAttribute> list = null;
			if (hlaAttributes != null)
			{
				list = new List<ObjectAttribute>();
				for (int i = 0; i < hlaAttributes.Count; i++)
				{
					list.Add(new ObjectAttribute(hlaAttributes[i]));
				}
			}
			return list;
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
				if (_objectAttribute4 != null)
				{
					_objectAttribute4.Dispose();
					_objectAttribute4 = null;
				}
				if (_objectAttribute8 != null)
				{
					_objectAttribute8.Dispose();
					_objectAttribute8 = null;
				}
			}
			_disposed = true;
		}

		~ObjectAttribute()
		{
			Dispose(false);
		}
	}
}
