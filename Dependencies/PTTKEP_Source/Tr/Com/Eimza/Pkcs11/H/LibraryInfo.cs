using System;
using Tr.Com.Eimza.Pkcs11.C;

namespace Tr.Com.Eimza.Pkcs11.H
{
    internal class LibraryInfo
	{
		private Tr.Com.Eimza.Pkcs11.H4.LibraryInfo _libraryInfo4;

		private Tr.Com.Eimza.Pkcs11.H8.LibraryInfo _libraryInfo8;

		public string CryptokiVersion
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _libraryInfo8.CryptokiVersion;
				}
				return _libraryInfo4.CryptokiVersion;
			}
		}

		public string ManufacturerId
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _libraryInfo8.ManufacturerId;
				}
				return _libraryInfo4.ManufacturerId;
			}
		}

		public ulong Flags
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _libraryInfo8.Flags;
				}
				return _libraryInfo4.Flags;
			}
		}

		public string LibraryDescription
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _libraryInfo8.LibraryDescription;
				}
				return _libraryInfo4.LibraryDescription;
			}
		}

		public string LibraryVersion
		{
			get
			{
				if (UnmanagedLong.Size != 4)
				{
					return _libraryInfo8.LibraryVersion;
				}
				return _libraryInfo4.LibraryVersion;
			}
		}

		internal LibraryInfo(Tr.Com.Eimza.Pkcs11.H4.LibraryInfo libraryInfo)
		{
			if (libraryInfo == null)
			{
				throw new ArgumentNullException("libraryInfo");
			}
			_libraryInfo4 = libraryInfo;
		}

		internal LibraryInfo(Tr.Com.Eimza.Pkcs11.H8.LibraryInfo libraryInfo)
		{
			if (libraryInfo == null)
			{
				throw new ArgumentNullException("libraryInfo");
			}
			_libraryInfo8 = libraryInfo;
		}
	}
}
