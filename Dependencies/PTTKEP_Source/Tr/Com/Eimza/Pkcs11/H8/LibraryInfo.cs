using Tr.Com.Eimza.Pkcs11.C;
using Tr.Com.Eimza.Pkcs11.L8;

namespace Tr.Com.Eimza.Pkcs11.H8
{
	internal class LibraryInfo
	{
		private string _cryptokiVersion;

		private string _manufacturerId;

		private ulong _flags;

		private string _libraryDescription;

		private string _libraryVersion;

		public string CryptokiVersion
		{
			get
			{
				return _cryptokiVersion;
			}
		}

		public string ManufacturerId
		{
			get
			{
				return _manufacturerId;
			}
		}

		public ulong Flags
		{
			get
			{
				return _flags;
			}
		}

		public string LibraryDescription
		{
			get
			{
				return _libraryDescription;
			}
		}

		public string LibraryVersion
		{
			get
			{
				return _libraryVersion;
			}
		}

		internal LibraryInfo(CK_INFO ck_info)
		{
			_cryptokiVersion = ConvertUtils.CkVersionToString(ck_info.CryptokiVersion);
			_manufacturerId = ConvertUtils.BytesToUtf8String(ck_info.ManufacturerId, true);
			_flags = ck_info.Flags;
			_libraryDescription = ConvertUtils.BytesToUtf8String(ck_info.LibraryDescription, true);
			_libraryVersion = ConvertUtils.CkVersionToString(ck_info.LibraryVersion);
		}
	}
}
