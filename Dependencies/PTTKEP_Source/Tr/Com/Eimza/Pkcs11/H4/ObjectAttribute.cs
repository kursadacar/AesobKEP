using System;
using System.Collections.Generic;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L4;

namespace Tr.Com.Eimza.Pkcs11.H4
{
	internal class ObjectAttribute : IDisposable
	{
		private bool _disposed;

		private CK_ATTRIBUTE _ckAttribute;

		internal CK_ATTRIBUTE CkAttribute
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return _ckAttribute;
			}
		}

		public uint Type
		{
			get
			{
				if (_disposed)
				{
					throw new ObjectDisposedException(GetType().FullName);
				}
				return _ckAttribute.type;
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
				return _ckAttribute.valueLen == uint.MaxValue;
			}
		}

		internal ObjectAttribute(CK_ATTRIBUTE attribute)
		{
			_ckAttribute = attribute;
		}

		public ObjectAttribute(uint type)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type);
		}

		public ObjectAttribute(CKA type)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type);
		}

		public ObjectAttribute(uint type, uint value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public ObjectAttribute(CKA type, uint value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public ObjectAttribute(CKA type, CKC value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public ObjectAttribute(CKA type, CKK value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public ObjectAttribute(CKA type, CKO value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public uint GetValueAsUint()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			uint value = 0u;
			CkaUtils.ConvertValue(ref _ckAttribute, out value);
			return value;
		}

		public ObjectAttribute(uint type, bool value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public ObjectAttribute(CKA type, bool value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public bool GetValueAsBool()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			bool value = false;
			CkaUtils.ConvertValue(ref _ckAttribute, out value);
			return value;
		}

		public ObjectAttribute(uint type, string value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public ObjectAttribute(CKA type, string value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public string GetValueAsString()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			string value = null;
			CkaUtils.ConvertValue(ref _ckAttribute, out value);
			return value;
		}

		public ObjectAttribute(uint type, byte[] value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public ObjectAttribute(CKA type, byte[] value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public byte[] GetValueAsByteArray()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			byte[] value = null;
			CkaUtils.ConvertValue(ref _ckAttribute, out value);
			return value;
		}

		public ObjectAttribute(uint type, DateTime value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public ObjectAttribute(CKA type, DateTime value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public DateTime? GetValueAsDateTime()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			DateTime? value = null;
			CkaUtils.ConvertValue(ref _ckAttribute, out value);
			return value;
		}

		public ObjectAttribute(uint type, List<ObjectAttribute> value)
		{
			CK_ATTRIBUTE[] array = null;
			if (value != null)
			{
				array = new CK_ATTRIBUTE[value.Count];
				for (int i = 0; i < value.Count; i++)
				{
					array[i] = value[i].CkAttribute;
				}
			}
			_ckAttribute = CkaUtils.CreateAttribute(type, array);
		}

		public ObjectAttribute(CKA type, List<ObjectAttribute> value)
		{
			CK_ATTRIBUTE[] array = null;
			if (value != null)
			{
				array = new CK_ATTRIBUTE[value.Count];
				for (int i = 0; i < value.Count; i++)
				{
					array[i] = value[i].CkAttribute;
				}
			}
			_ckAttribute = CkaUtils.CreateAttribute(type, array);
		}

		public List<ObjectAttribute> GetValueAsObjectAttributeList()
		{
			throw new NotImplementedException();
		}

		public ObjectAttribute(uint type, List<uint> value)
		{
			uint[] value2 = null;
			if (value != null)
			{
				value2 = value.ToArray();
			}
			_ckAttribute = CkaUtils.CreateAttribute(type, value2);
		}

		public ObjectAttribute(CKA type, List<uint> value)
		{
			uint[] value2 = null;
			if (value != null)
			{
				value2 = value.ToArray();
			}
			_ckAttribute = CkaUtils.CreateAttribute(type, value2);
		}

		public List<uint> GetValueAsUintList()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			uint[] value = null;
			CkaUtils.ConvertValue(ref _ckAttribute, out value);
			if (value != null)
			{
				return new List<uint>(value);
			}
			return null;
		}

		public ObjectAttribute(uint type, List<CKM> value)
		{
			CKM[] value2 = null;
			if (value != null)
			{
				value2 = value.ToArray();
			}
			_ckAttribute = CkaUtils.CreateAttribute(type, value2);
		}

		public ObjectAttribute(CKA type, List<CKM> value)
		{
			CKM[] value2 = null;
			if (value != null)
			{
				value2 = value.ToArray();
			}
			_ckAttribute = CkaUtils.CreateAttribute(type, value2);
		}

		public List<CKM> GetValueAsCkmList()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			CKM[] value = null;
			CkaUtils.ConvertValue(ref _ckAttribute, out value);
			if (value != null)
			{
				return new List<CKM>(value);
			}
			return null;
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
				UnmanagedMemory.Free(ref _ckAttribute.value);
				_ckAttribute.valueLen = 0u;
				_disposed = true;
			}
		}

		~ObjectAttribute()
		{
			Dispose(false);
		}
	}
}
