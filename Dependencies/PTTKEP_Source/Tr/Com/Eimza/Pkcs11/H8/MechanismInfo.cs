using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8;

namespace Tr.Com.Eimza.Pkcs11.H8
{
	internal class MechanismInfo
	{
		private CKM _mechanism;

		private ulong _minKeySize;

		private ulong _maxKeySize;

		private MechanismFlags _mechanismFlags;

		public CKM Mechanism
		{
			get
			{
				return _mechanism;
			}
		}

		public ulong MinKeySize
		{
			get
			{
				return _minKeySize;
			}
		}

		public ulong MaxKeySize
		{
			get
			{
				return _maxKeySize;
			}
		}

		public MechanismFlags MechanismFlags
		{
			get
			{
				return _mechanismFlags;
			}
		}

		internal MechanismInfo(CKM mechanism, CK_MECHANISM_INFO ck_mechanism_info)
		{
			_mechanism = mechanism;
			_minKeySize = ck_mechanism_info.MinKeySize;
			_maxKeySize = ck_mechanism_info.MaxKeySize;
			_mechanismFlags = new MechanismFlags(ck_mechanism_info.Flags);
		}
	}
}
