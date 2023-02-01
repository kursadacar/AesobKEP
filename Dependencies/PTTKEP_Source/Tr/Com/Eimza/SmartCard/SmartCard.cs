using System.Collections.Generic;
using Tr.Com.Eimza.Org.BouncyCastle.X509;
using Tr.Com.Eimza.Pkcs11.H;

namespace Tr.Com.Eimza.SmartCard
{
	internal class SmartCard
	{
		public List<X509Certificate> TokenCertList { get; set; }

		public List<ObjectHandle> PrivateKeyList { get; set; }

		public string SmartCardPIN { get; set; }

		public Tr.Com.Eimza.Pkcs11.H.Pkcs11 Pkcs11Module { get; set; }

		public string InitializedDll { get; set; }

		public bool Done { get; set; }

		public Session Session { get; set; }

		public TokenInfo TokenInfo { get; set; }

		public SlotInfo SlotInfo { get; set; }

		public Slot Slot { get; set; }

		public LibraryInfo LibraryInfo { get; set; }
	}
}
