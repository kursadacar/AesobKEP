using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class MechanismInfo
	{
		private Tr.Com.Eimza.Pkcs11.H4.MechanismInfo _mechanismInfo4;

		private Tr.Com.Eimza.Pkcs11.H8.MechanismInfo _mechanismInfo8;

		private MechanismFlags _mechanismFlags;

		public CKM Mechanism
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismInfo8.Mechanism;
				}
				return _mechanismInfo4.Mechanism;
			}
		}

		public ulong MinKeySize
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismInfo8.MinKeySize;
				}
				return _mechanismInfo4.MinKeySize;
			}
		}

		public ulong MaxKeySize
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _mechanismInfo8.MaxKeySize;
				}
				return _mechanismInfo4.MaxKeySize;
			}
		}

		public MechanismFlags MechanismFlags
		{
			get
			{
				if (_mechanismFlags == null)
				{
					_mechanismFlags = ((UnmanagedLong.Size == 4) ? new MechanismFlags(_mechanismInfo4.MechanismFlags) : new MechanismFlags(_mechanismInfo8.MechanismFlags));
				}
				return _mechanismFlags;
			}
		}

		internal MechanismInfo(Tr.Com.Eimza.Pkcs11.H4.MechanismInfo mechanismInfo)
		{
			if (mechanismInfo == null)
			{
				throw new ArgumentNullException("mechanismInfo");
			}
			_mechanismInfo4 = mechanismInfo;
		}

		internal MechanismInfo(Tr.Com.Eimza.Pkcs11.H8.MechanismInfo mechanismInfo)
		{
			if (mechanismInfo == null)
			{
				throw new ArgumentNullException("mechanismInfo");
			}
			_mechanismInfo8 = mechanismInfo;
		}
	}
}
