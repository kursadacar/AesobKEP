using System;
using System.Collections.Generic;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509
{
	internal class DnComponents
	{
		private static DnComponents obj;

		private static Dictionary<string, DerObjectIdentifier> oids;

		private static readonly string[] DnObjectsForward;

		private static string[] DnObjectsReverse;

		public static readonly string DNEMAIL;

		public static readonly string DNQUALIFIER;

		public static readonly string UID;

		public static readonly string COMMONNAME;

		public static readonly string SN;

		public static readonly string GIVENNAME;

		public static readonly string INITIALS;

		public static readonly string SURNAME;

		public static readonly string TITLE;

		public static readonly string ORGANIZATIONUNIT;

		public static readonly string ORGANIZATION;

		public static readonly string LOCALE;

		public static readonly string STATE;

		public static readonly string DOMAINCOMPONENT;

		public static readonly string COUNTRY;

		public static readonly string UNSTRUCTUREDADDRESS;

		public static readonly string UNSTRUCTUREDNAME;

		public static readonly string POSTALCODE;

		public static readonly string BUSINESSCATEGORY;

		public static readonly string POSTALADDRESS;

		public static readonly string TELEPHONENUMBER;

		public static readonly string PSEUDONYM;

		public static readonly string STREET;

		public static readonly string NAME;

		public static readonly string RFC822NAME;

		public static readonly string DNSNAME;

		public static readonly string IPADDRESS;

		public static readonly string UNIFORMRESOURCEID;

		public static readonly string DIRECTORYNAME;

		public static readonly string UPN;

		public static readonly string GUID;

		public static readonly string KRB5PRINCIPAL;

		public static readonly string PERMANENTIDENTIFIER;

		public static readonly string OTHERNAME;

		public static readonly string X400ADDRESS;

		public static readonly string EDIPARTNAME;

		public static readonly string REGISTEREDID;

		public static readonly string DATEOFBIRTH;

		public static readonly string PLACEOFBIRTH;

		public static readonly string GENDER;

		public static readonly string COUNTRYOFCITIZENSHIP;

		public static readonly string COUNTRYOFRESIDENCE;

		private static Dictionary<string, int> dnNameIdMap;

		private static Dictionary<string, int> profileNameIdMap;

		private static Dictionary<int, string> dnIdToProfileNameMap;

		private static Dictionary<int, int> dnIdToProfileIdMap;

		private static Dictionary<int, int> profileIdToDnIdMap;

		private static Dictionary<int, string> dnErrorTextMap;

		private static Dictionary<string, string> profileNameLanguageMap;

		private static Dictionary<int, string> profileIdLanguageMap;

		private static Dictionary<int, string> dnIdErrorMap;

		private static Dictionary<int, string> dnIdToExtractorFieldMap;

		private static Dictionary<int, string> altNameIdToExtractorFieldMap;

		private static Dictionary<int, string> dirAttrIdToExtractorFieldMap;

		private static List<string> dnProfileFields;

		private static readonly LinkedList<string> dnProfileFieldsHashSet;

		private static List<string> dnLanguageTexts;

		private static List<int> dnDnIds;

		private static List<string> altNameFields;

		private static readonly LinkedList<string> altNameFieldsHashSet;

		private static List<string> altNameLanguageTexts;

		private static List<int> altNameDnIds;

		private static List<string> dirAttrFields;

		private static readonly LinkedList<string> dirAttrFieldsHashSet;

		private static List<string> dirAttrLanguageTexts;

		private static List<int> dirAttrDnIds;

		private static List<string> dnExtractorFields;

		private static List<string> altNameExtractorFields;

		private static List<string> dirAttrExtractorFields;

		static DnComponents()
		{
			obj = new DnComponents();
			oids = new Dictionary<string, DerObjectIdentifier>();
			DnObjectsForward = new string[28]
			{
				"street", "pseudonym", "telephonenumber", "postaladdress", "businesscategory", "postalcode", "unstructuredaddress", "unstructuredname", "emailaddress", "e",
				"email", "dn", "uid", "cn", "name", "sn", "serialnumber", "gn", "givenname", "initials",
				"surname", "t", "ou", "o", "l", "st", "dc", "c"
			};
			DnObjectsReverse = null;
			DNEMAIL = "DNEMAIL";
			DNQUALIFIER = "DN";
			UID = "UID";
			COMMONNAME = "COMMONNAME";
			SN = "SN";
			GIVENNAME = "GIVENNAME";
			INITIALS = "INITIALS";
			SURNAME = "SURNAME";
			TITLE = "TITLE";
			ORGANIZATIONUNIT = "ORGANIZATIONUNIT";
			ORGANIZATION = "ORGANIZATION";
			LOCALE = "LOCALE";
			STATE = "STATE";
			DOMAINCOMPONENT = "DOMAINCOMPONENT";
			COUNTRY = "COUNTRY";
			UNSTRUCTUREDADDRESS = "UNSTRUCTUREDADDRESS";
			UNSTRUCTUREDNAME = "UNSTRUCTUREDNAME";
			POSTALCODE = "POSTALCODE";
			BUSINESSCATEGORY = "BUSINESSCATEGORY";
			POSTALADDRESS = "POSTALADDRESS";
			TELEPHONENUMBER = "TELEPHONENUMBER";
			PSEUDONYM = "PSEUDONYM";
			STREET = "STREET";
			NAME = "NAME";
			RFC822NAME = "RFC822NAME";
			DNSNAME = "DNSNAME";
			IPADDRESS = "IPADDRESS";
			UNIFORMRESOURCEID = "UNIFORMRESOURCEID";
			DIRECTORYNAME = "DIRECTORYNAME";
			UPN = "UPN";
			GUID = "GUID";
			KRB5PRINCIPAL = "KRB5PRINCIPAL";
			PERMANENTIDENTIFIER = "PERMANENTIDENTIFIER";
			OTHERNAME = "OTHERNAME";
			X400ADDRESS = "X400ADDRESS";
			EDIPARTNAME = "EDIPARTNAME";
			REGISTEREDID = "REGISTEREDID";
			DATEOFBIRTH = "DATEOFBIRTH";
			PLACEOFBIRTH = "PLACEOFBIRTH";
			GENDER = "GENDER";
			COUNTRYOFCITIZENSHIP = "COUNTRYOFCITIZENSHIP";
			COUNTRYOFRESIDENCE = "COUNTRYOFRESIDENCE";
			dnNameIdMap = new Dictionary<string, int>();
			profileNameIdMap = new Dictionary<string, int>();
			dnIdToProfileNameMap = new Dictionary<int, string>();
			dnIdToProfileIdMap = new Dictionary<int, int>();
			profileIdToDnIdMap = new Dictionary<int, int>();
			dnErrorTextMap = new Dictionary<int, string>();
			profileNameLanguageMap = new Dictionary<string, string>();
			profileIdLanguageMap = new Dictionary<int, string>();
			dnIdErrorMap = new Dictionary<int, string>();
			dnIdToExtractorFieldMap = new Dictionary<int, string>();
			altNameIdToExtractorFieldMap = new Dictionary<int, string>();
			dirAttrIdToExtractorFieldMap = new Dictionary<int, string>();
			dnProfileFields = new List<string>();
			dnProfileFieldsHashSet = new LinkedList<string>();
			dnLanguageTexts = new List<string>();
			dnDnIds = new List<int>();
			altNameFields = new List<string>();
			altNameFieldsHashSet = new LinkedList<string>();
			altNameLanguageTexts = new List<string>();
			altNameDnIds = new List<int>();
			dirAttrFields = new List<string>();
			dirAttrFieldsHashSet = new LinkedList<string>();
			dirAttrLanguageTexts = new List<string>();
			dirAttrDnIds = new List<int>();
			dnExtractorFields = new List<string>();
			altNameExtractorFields = new List<string>();
			dirAttrExtractorFields = new List<string>();
			oids.Add("c", X509Name.C);
			oids.Add("dc", X509Name.DC);
			oids.Add("st", X509Name.ST);
			oids.Add("l", X509Name.L);
			oids.Add("o", X509Name.O);
			oids.Add("ou", X509Name.OU);
			oids.Add("t", X509Name.T);
			oids.Add("surname", X509Name.Surname);
			oids.Add("initials", X509Name.Initials);
			oids.Add("givenname", X509Name.GivenName);
			oids.Add("gn", X509Name.GivenName);
			oids.Add("sn", X509Name.Surname);
			oids.Add("serialnumber", X509Name.SerialNumber);
			oids.Add("cn", X509Name.CN);
			oids.Add("uid", X509Name.UID);
			oids.Add("dn", X509Name.DnQualifier);
			oids.Add("emailaddress", X509Name.EmailAddress);
			oids.Add("e", X509Name.EmailAddress);
			oids.Add("email", X509Name.EmailAddress);
			oids.Add("unstructuredname", X509Name.UnstructuredName);
			oids.Add("unstructuredaddress", X509Name.UnstructuredAddress);
			oids.Add("postalcode", X509Name.PostalCode);
			oids.Add("businesscategory", X509Name.BusinessCategory);
			oids.Add("postaladdress", X509Name.PostalAddress);
			oids.Add("telephonenumber", X509Name.TelephoneNumber);
			oids.Add("pseudonym", X509Name.Pseudonym);
			oids.Add("street", X509Name.Street);
			oids.Add("name", X509Name.Name);
		}

		public static string[] GetDnObjects(bool ldaporder)
		{
			if (ldaporder)
			{
				return DnObjectsForward;
			}
			return GetDnObjectsReverse();
		}

		public static DerObjectIdentifier GetOid(string o)
		{
			return oids[o];
		}

		protected static string[] GetDnObjectsReverse()
		{
			if (DnObjectsReverse == null)
			{
				DnObjectsReverse = (string[])DnObjectsForward.Clone();
				Array.Reverse(DnObjectsReverse);
			}
			return DnObjectsReverse;
		}
	}
}
