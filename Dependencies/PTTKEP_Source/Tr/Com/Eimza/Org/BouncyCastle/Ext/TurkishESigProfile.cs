using System;
using System.Collections.Generic;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1;

namespace Tr.Com.Eimza.Org.BouncyCastle.Ext
{
	internal class TurkishESigProfile
	{
		protected static readonly EImzaProfilleriDocInfo docInfo = new EImzaProfilleriDocInfo();

		public static readonly List<TurkishESigProfile> profiles = new List<TurkishESigProfile>();

		public static readonly DerObjectIdentifier P2_1_OID = new DerObjectIdentifier("2.16.792.1.61.0.1.5070.3.1.1");

		public static readonly DerObjectIdentifier P3_1_OID = new DerObjectIdentifier("2.16.792.1.61.0.1.5070.3.2.1");

		public static readonly DerObjectIdentifier P4_1_OID = new DerObjectIdentifier("2.16.792.1.61.0.1.5070.3.3.1");

		public static readonly TurkishESigProfile P1_1 = new TurkishESigProfile(null, docInfo);

		public static readonly TurkishESigProfile P2_1 = new TurkishESigProfile(P2_1_OID, docInfo);

		public static readonly TurkishESigProfile P3_1 = new TurkishESigProfile(P3_1_OID, docInfo);

		public static readonly TurkishESigProfile P4_1 = new TurkishESigProfile(P4_1_OID, docInfo);

		public ProfileDocInfo ProfileDocInfo { get; private set; }

		protected DerObjectIdentifier ProfileOID { get; private set; }

		private TurkishESigProfile(DerObjectIdentifier aOid, ProfileDocInfo aDocInfo)
		{
			ProfileOID = aOid;
			ProfileDocInfo = aDocInfo;
			profiles.Add(this);
		}

		public bool Equals(TurkishESigProfile aProfile)
		{
			try
			{
				if (aProfile.ProfileOID == null)
				{
					return false;
				}
				if (ProfileOID.Id.Equals(aProfile.ProfileOID.Id))
				{
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
			return false;
		}

		public byte[] GetDigestofProfile(DerObjectIdentifier aDigestAlgOid)
		{
			return ProfileDocInfo.GetDigestOfProfile(aDigestAlgOid);
		}

		public static TurkishESigProfile GetSignatureProfile(string aOid)
		{
			foreach (TurkishESigProfile profile in profiles)
			{
				if (profile.ProfileOID != null && profile.ProfileOID.Id.Equals(aOid))
				{
					return profile;
				}
			}
			return null;
		}

		public static TurkishESigProfile GetSignatureProfile(DerObjectIdentifier aOid)
		{
			foreach (TurkishESigProfile profile in profiles)
			{
				if (profile.ProfileOID != null && profile.ProfileOID.Id.Equals(aOid.Id))
				{
					return profile;
				}
			}
			return null;
		}
	}
}
