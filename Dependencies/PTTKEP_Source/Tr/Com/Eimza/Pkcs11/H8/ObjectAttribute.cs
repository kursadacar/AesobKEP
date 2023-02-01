using System;
using System.Collections.Generic;
using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8;

namespace Tr.Com.Eimza.Pkcs11.H8
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

		public ulong Type
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
				return _ckAttribute.valueLen == ulong.MaxValue;
			}
		}

		internal ObjectAttribute(CK_ATTRIBUTE attribute)
		{
			_ckAttribute = attribute;
		}

		public ObjectAttribute(ulong type)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type);
		}

		public ObjectAttribute(CKA type)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type);
		}

		public ObjectAttribute(ulong type, ulong value)
		{
			_ckAttribute = CkaUtils.CreateAttribute(type, value);
		}

		public ObjectAttribute(CKA type, ulong value)
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

		public ulong GetValueAsUlong()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			ulong value = 0uL;
			CkaUtils.ConvertValue(ref _ckAttribute, out value);
			return value;
		}

		public ObjectAttribute(ulong type, bool value)
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

		public ObjectAttribute(ulong type, string value)
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

		public ObjectAttribute(ulong type, byte[] value)
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

		public ObjectAttribute(ulong type, DateTime value)
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

		public ObjectAttribute(ulong type, List<ObjectAttribute> value)
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

		public ObjectAttribute(ulong type, List<ulong> value)
		{
			ulong[] value2 = null;
			if (value != null)
			{
				value2 = value.ToArray();
			}
			_ckAttribute = CkaUtils.CreateAttribute(type, value2);
		}

		public ObjectAttribute(CKA type, List<ulong> value)
		{
			ulong[] value2 = null;
			if (value != null)
			{
				value2 = value.ToArray();
			}
			_ckAttribute = CkaUtils.CreateAttribute(type, value2);
		}

		public List<ulong> GetValueAsUlongList()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			ulong[] value = null;
			CkaUtils.ConvertValue(ref _ckAttribute, out value);
			if (value != null)
			{
				return new List<ulong>(value);
			}
			return null;
		}

		public ObjectAttribute(ulong type, List<CKM> value)
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
				_ckAttribute.valueLen = 0uL;
				_disposed = true;
			}
		}

		~ObjectAttribute()
		{
			Dispose(false);
		}
	}
}
