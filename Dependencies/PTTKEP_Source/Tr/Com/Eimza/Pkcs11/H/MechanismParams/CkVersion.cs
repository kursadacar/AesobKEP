using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H.MechanismParams
{
    internal class CkVersion : IMechanismParams
	{
		private Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkVersion _params4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkVersion _params8;

		public byte Major
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _params8.Major;
				}
				return _params4.Major;
			}
		}

		public byte Minor
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _params8.Minor;
				}
				return _params4.Minor;
			}
		}

		public CkVersion(byte major, byte minor)
		{
			if (UnmanagedLong.Size == 4)
			{
				_params4 = new Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkVersion(major, minor);
			}
			else
			{
				_params8 = new Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkVersion(major, minor);
			}
		}

		internal CkVersion(Tr.Com.Eimza.Pkcs11.H4.MechanismParams.CkVersion ckVersion)
		{
			if (ckVersion == null)
			{
				throw new ArgumentNullException("ckVersion");
			}
			_params4 = ckVersion;
		}

		internal CkVersion(Tr.Com.Eimza.Pkcs11.H8.MechanismParams.CkVersion ckVersion)
		{
			if (ckVersion == null)
			{
				throw new ArgumentNullException("ckVersion");
			}
			_params8 = ckVersion;
		}

		public object ToMarshalableStructure()
		{
			if (UnmanagedLong.Size == 4)
			{
				return _params4.ToMarshalableStructure();
			}
			return _params8.ToMarshalableStructure();
		}

		public override string ToString()
		{
			string text = null;
			if (UnmanagedLong.Size == 4)
			{
				return ConvertUtils.CkVersionToString((Tr.Com.Eimza.Pkcs11.L4.CK_VERSION)_params4.ToMarshalableStructure());
			}
			return ConvertUtils.CkVersionToString((Tr.Com.Eimza.Pkcs11.L8.CK_VERSION)_params8.ToMarshalableStructure());
		}
	}
}
