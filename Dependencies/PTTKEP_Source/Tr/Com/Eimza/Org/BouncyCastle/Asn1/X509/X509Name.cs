using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Pkcs;
using Tr.Com.Eimza.Org.BouncyCastle.Tools;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.Encoders;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509
{
	internal class X509Name : Asn1Encodable
	{
		public static readonly DerObjectIdentifier C;

		public static readonly DerObjectIdentifier O;

		public static readonly DerObjectIdentifier OU;

		public static readonly DerObjectIdentifier T;

		public static readonly DerObjectIdentifier CN;

		public static readonly DerObjectIdentifier Street;

		public static readonly DerObjectIdentifier SerialNumber;

		public static readonly DerObjectIdentifier L;

		public static readonly DerObjectIdentifier ST;

		public static readonly DerObjectIdentifier Surname;

		public static readonly DerObjectIdentifier GivenName;

		public static readonly DerObjectIdentifier Initials;

		public static readonly DerObjectIdentifier Generation;

		public static readonly DerObjectIdentifier UniqueIdentifier;

		public static readonly DerObjectIdentifier BusinessCategory;

		public static readonly DerObjectIdentifier PostalCode;

		public static readonly DerObjectIdentifier DnQualifier;

		public static readonly DerObjectIdentifier Pseudonym;

		public static readonly DerObjectIdentifier DateOfBirth;

		public static readonly DerObjectIdentifier PlaceOfBirth;

		public static readonly DerObjectIdentifier Gender;

		public static readonly DerObjectIdentifier CountryOfCitizenship;

		public static readonly DerObjectIdentifier CountryOfResidence;

		public static readonly DerObjectIdentifier NameAtBirth;

		public static readonly DerObjectIdentifier PostalAddress;

		public static readonly DerObjectIdentifier DmdName;

		public static readonly DerObjectIdentifier TelephoneNumber;

		public static readonly DerObjectIdentifier Name;

		public static readonly DerObjectIdentifier EmailAddress;

		public static readonly DerObjectIdentifier UnstructuredName;

		public static readonly DerObjectIdentifier UnstructuredAddress;

		public static readonly DerObjectIdentifier E;

		public static readonly DerObjectIdentifier DC;

		public static readonly DerObjectIdentifier UID;

		private static readonly bool[] defaultReverse;

		public static readonly Hashtable DefaultSymbols;

		public static readonly Hashtable RFC2253Symbols;

		public static readonly Hashtable RFC1779Symbols;

		public static readonly Hashtable DefaultLookup;

		private IList ordering = Platform.CreateArrayList();

		private readonly X509NameEntryConverter converter;

		private IList values = Platform.CreateArrayList();

		private IList added = Platform.CreateArrayList();

		private Asn1Sequence seq;

		public static bool DefaultReverse
		{
			get
			{
				return defaultReverse[0];
			}
			set
			{
				defaultReverse[0] = value;
			}
		}

		public IList Ordering
		{
			get
			{
				return ordering;
			}
		}

		public IList Values
		{
			get
			{
				return values;
			}
			set
			{
				values = value;
			}
		}

		public IList Added
		{
			get
			{
				return added;
			}
			set
			{
				added = value;
			}
		}

		public string StringValueNewLine
		{
			get
			{
				DefaultReverse = true;
				return ToString(DefaultReverse, DefaultSymbols, Environment.NewLine);
			}
		}

		public string CommonName
		{
			get
			{
				return GetNameFromOID(CN);
			}
		}

		public string Organization
		{
			get
			{
				return GetNameFromOID(O);
			}
		}

		public string OrganizationUnit
		{
			get
			{
				return GetNameFromOID(OU);
			}
		}

		public string Country
		{
			get
			{
				return GetNameFromOID(C);
			}
		}

		public string StateOrProvince
		{
			get
			{
				return GetNameFromOID(ST);
			}
		}

		public string Locality
		{
			get
			{
				return GetNameFromOID(L);
			}
		}

		public string Serial_Number
		{
			get
			{
				return GetNameFromOID(SerialNumber);
			}
		}

		public string E_MailAdress
		{
			get
			{
				return GetNameFromOID(EmailAddress);
			}
		}

		public string Given_Name
		{
			get
			{
				return GetNameFromOID(GivenName);
			}
		}

		public string StringValue
		{
			get
			{
				return LdapDNUtil.Normalize(ToString());
			}
		}

		static X509Name()
		{
			C = new DerObjectIdentifier("2.5.4.6");
			O = new DerObjectIdentifier("2.5.4.10");
			OU = new DerObjectIdentifier("2.5.4.11");
			T = new DerObjectIdentifier("2.5.4.12");
			CN = new DerObjectIdentifier("2.5.4.3");
			Street = new DerObjectIdentifier("2.5.4.9");
			SerialNumber = new DerObjectIdentifier("2.5.4.5");
			L = new DerObjectIdentifier("2.5.4.7");
			ST = new DerObjectIdentifier("2.5.4.8");
			Surname = new DerObjectIdentifier("2.5.4.4");
			GivenName = new DerObjectIdentifier("2.5.4.42");
			Initials = new DerObjectIdentifier("2.5.4.43");
			Generation = new DerObjectIdentifier("2.5.4.44");
			UniqueIdentifier = new DerObjectIdentifier("2.5.4.45");
			BusinessCategory = new DerObjectIdentifier("2.5.4.15");
			PostalCode = new DerObjectIdentifier("2.5.4.17");
			DnQualifier = new DerObjectIdentifier("2.5.4.46");
			Pseudonym = new DerObjectIdentifier("2.5.4.65");
			DateOfBirth = new DerObjectIdentifier("1.3.6.1.5.5.7.9.1");
			PlaceOfBirth = new DerObjectIdentifier("1.3.6.1.5.5.7.9.2");
			Gender = new DerObjectIdentifier("1.3.6.1.5.5.7.9.3");
			CountryOfCitizenship = new DerObjectIdentifier("1.3.6.1.5.5.7.9.4");
			CountryOfResidence = new DerObjectIdentifier("1.3.6.1.5.5.7.9.5");
			NameAtBirth = new DerObjectIdentifier("1.3.36.8.3.14");
			PostalAddress = new DerObjectIdentifier("2.5.4.16");
			DmdName = new DerObjectIdentifier("2.5.4.54");
			TelephoneNumber = X509ObjectIdentifiers.id_at_telephoneNumber;
			Name = X509ObjectIdentifiers.id_at_name;
			EmailAddress = PkcsObjectIdentifiers.Pkcs9AtEmailAddress;
			UnstructuredName = PkcsObjectIdentifiers.Pkcs9AtUnstructuredName;
			UnstructuredAddress = PkcsObjectIdentifiers.Pkcs9AtUnstructuredAddress;
			E = EmailAddress;
			DC = new DerObjectIdentifier("0.9.2342.19200300.100.1.25");
			UID = new DerObjectIdentifier("0.9.2342.19200300.100.1.1");
			defaultReverse = new bool[1];
			DefaultSymbols = new Hashtable();
			RFC2253Symbols = new Hashtable();
			RFC1779Symbols = new Hashtable();
			DefaultLookup = new Hashtable();
			DefaultSymbols.Add(C, "C");
			DefaultSymbols.Add(O, "O");
			DefaultSymbols.Add(T, "T");
			DefaultSymbols.Add(OU, "OU");
			DefaultSymbols.Add(CN, "CN");
			DefaultSymbols.Add(L, "L");
			DefaultSymbols.Add(ST, "ST");
			DefaultSymbols.Add(SerialNumber, "SERIALNUMBER");
			DefaultSymbols.Add(EmailAddress, "E");
			DefaultSymbols.Add(DC, "DC");
			DefaultSymbols.Add(UID, "UID");
			DefaultSymbols.Add(Street, "STREET");
			DefaultSymbols.Add(Surname, "SURNAME");
			DefaultSymbols.Add(GivenName, "GIVENNAME");
			DefaultSymbols.Add(Initials, "INITIALS");
			DefaultSymbols.Add(Generation, "GENERATION");
			DefaultSymbols.Add(UnstructuredAddress, "unstructuredAddress");
			DefaultSymbols.Add(UnstructuredName, "unstructuredName");
			DefaultSymbols.Add(UniqueIdentifier, "UniqueIdentifier");
			DefaultSymbols.Add(DnQualifier, "DN");
			DefaultSymbols.Add(Pseudonym, "Pseudonym");
			DefaultSymbols.Add(PostalAddress, "PostalAddress");
			DefaultSymbols.Add(NameAtBirth, "NameAtBirth");
			DefaultSymbols.Add(CountryOfCitizenship, "CountryOfCitizenship");
			DefaultSymbols.Add(CountryOfResidence, "CountryOfResidence");
			DefaultSymbols.Add(Gender, "Gender");
			DefaultSymbols.Add(PlaceOfBirth, "PlaceOfBirth");
			DefaultSymbols.Add(DateOfBirth, "DateOfBirth");
			DefaultSymbols.Add(PostalCode, "PostalCode");
			DefaultSymbols.Add(BusinessCategory, "BusinessCategory");
			DefaultSymbols.Add(TelephoneNumber, "TelephoneNumber");
			RFC2253Symbols.Add(C, "C");
			RFC2253Symbols.Add(O, "O");
			RFC2253Symbols.Add(OU, "OU");
			RFC2253Symbols.Add(CN, "CN");
			RFC2253Symbols.Add(L, "L");
			RFC2253Symbols.Add(ST, "ST");
			RFC2253Symbols.Add(Street, "STREET");
			RFC2253Symbols.Add(DC, "DC");
			RFC2253Symbols.Add(UID, "UID");
			RFC1779Symbols.Add(C, "C");
			RFC1779Symbols.Add(O, "O");
			RFC1779Symbols.Add(OU, "OU");
			RFC1779Symbols.Add(CN, "CN");
			RFC1779Symbols.Add(L, "L");
			RFC1779Symbols.Add(ST, "ST");
			RFC1779Symbols.Add(Street, "STREET");
			DefaultLookup.Add("c", C);
			DefaultLookup.Add("o", O);
			DefaultLookup.Add("t", T);
			DefaultLookup.Add("ou", OU);
			DefaultLookup.Add("cn", CN);
			DefaultLookup.Add("l", L);
			DefaultLookup.Add("st", ST);
			DefaultLookup.Add("serialnumber", SerialNumber);
			DefaultLookup.Add("street", Street);
			DefaultLookup.Add("emailaddress", E);
			DefaultLookup.Add("dc", DC);
			DefaultLookup.Add("e", E);
			DefaultLookup.Add("uid", UID);
			DefaultLookup.Add("surname", Surname);
			DefaultLookup.Add("givenname", GivenName);
			DefaultLookup.Add("initials", Initials);
			DefaultLookup.Add("generation", Generation);
			DefaultLookup.Add("unstructuredaddress", UnstructuredAddress);
			DefaultLookup.Add("unstructuredname", UnstructuredName);
			DefaultLookup.Add("uniqueidentifier", UniqueIdentifier);
			DefaultLookup.Add("dn", DnQualifier);
			DefaultLookup.Add("pseudonym", Pseudonym);
			DefaultLookup.Add("postaladdress", PostalAddress);
			DefaultLookup.Add("nameofbirth", NameAtBirth);
			DefaultLookup.Add("countryofcitizenship", CountryOfCitizenship);
			DefaultLookup.Add("countryofresidence", CountryOfResidence);
			DefaultLookup.Add("gender", Gender);
			DefaultLookup.Add("placeofbirth", PlaceOfBirth);
			DefaultLookup.Add("dateofbirth", DateOfBirth);
			DefaultLookup.Add("postalcode", PostalCode);
			DefaultLookup.Add("businesscategory", BusinessCategory);
			DefaultLookup.Add("telephonenumber", TelephoneNumber);
		}

		public static X509Name GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		public static X509Name GetInstance(object obj)
		{
			if (obj == null || obj is X509Name)
			{
				return (X509Name)obj;
			}
			if (obj != null)
			{
				return new X509Name(Asn1Sequence.GetInstance(obj));
			}
			throw new ArgumentException("null object in factory", "obj");
		}

		public static X509Name GetInstance(byte[] obj)
		{
			DerSequence obj2 = (DerSequence)Asn1Object.FromByteArray(obj);
			if (obj != null)
			{
				return new X509Name(Asn1Sequence.GetInstance(obj2));
			}
			throw new ArgumentException("null object in factory", "obj");
		}

		protected X509Name()
		{
		}

		protected X509Name(Asn1Sequence seq)
		{
			this.seq = seq;
			foreach (Asn1Encodable item in seq)
			{
				Asn1Set instance = Asn1Set.GetInstance(item.ToAsn1Object());
				for (int i = 0; i < instance.Count; i++)
				{
					Asn1Sequence instance2 = Asn1Sequence.GetInstance(instance[i].ToAsn1Object());
					if (instance2.Count != 2)
					{
						throw new ArgumentException("badly sized pair");
					}
					ordering.Add(DerObjectIdentifier.GetInstance(instance2[0].ToAsn1Object()));
					Asn1Object asn1Object = instance2[1].ToAsn1Object();
					if (asn1Object is IAsn1String && !(asn1Object is DerUniversalString))
					{
						string text = ((IAsn1String)asn1Object).GetString();
						if (text.StartsWith("#"))
						{
							text = "\\" + text;
						}
						values.Add(text);
					}
					else
					{
						values.Add("#" + Hex.ToHexString(asn1Object.GetEncoded()));
					}
					added.Add(i != 0);
				}
			}
		}

		public X509Name(IList ordering, IDictionary attributes)
			: this(ordering, attributes, new X509DefaultEntryConverter())
		{
		}

		public X509Name(IList ordering, IDictionary attributes, X509NameEntryConverter converter)
		{
			this.converter = converter;
			foreach (DerObjectIdentifier item in ordering)
			{
				object obj = attributes[item];
				if (obj == null)
				{
					throw new ArgumentException("No attribute for object id - " + ((item != null) ? item.ToString() : null) + " - passed to distinguished name");
				}
				this.ordering.Add(item);
				added.Add(false);
				values.Add(obj);
			}
		}

		public X509Name(IList oids, IList values)
			: this(oids, values, new X509DefaultEntryConverter())
		{
		}

		public X509Name(IList oids, IList values, X509NameEntryConverter converter)
		{
			this.converter = converter;
			if (oids.Count != values.Count)
			{
				throw new ArgumentException("'oids' must be same length as 'values'.");
			}
			for (int i = 0; i < oids.Count; i++)
			{
				ordering.Add(oids[i]);
				this.values.Add(values[i]);
				added.Add(false);
			}
		}

		public void AppendRDN(IList oids, IList values)
		{
			if (oids.Count != values.Count)
			{
				throw new ArgumentException("'oids' must be same length as 'values'.");
			}
			for (int i = 0; i < oids.Count; i++)
			{
				ordering.Add(oids[i]);
				this.values.Add(values[i]);
				added.Add(false);
			}
		}

		public X509Name(string dirName)
			: this(DefaultReverse, DefaultLookup, dirName)
		{
		}

		public X509Name(string dirName, X509NameEntryConverter converter)
			: this(DefaultReverse, DefaultLookup, dirName, converter)
		{
		}

		public X509Name(bool reverse, string dirName)
			: this(reverse, DefaultLookup, dirName)
		{
		}

		public X509Name(bool reverse, string dirName, X509NameEntryConverter converter)
			: this(reverse, DefaultLookup, dirName, converter)
		{
		}

		public X509Name(bool reverse, IDictionary lookUp, string dirName)
			: this(reverse, lookUp, dirName, new X509DefaultEntryConverter())
		{
		}

		private DerObjectIdentifier DecodeOid(string name, IDictionary lookUp)
		{
			if (Platform.ToUpperInvariant(name).StartsWith("OID."))
			{
				return new DerObjectIdentifier(name.Substring(4));
			}
			if (name[0] >= '0' && name[0] <= '9')
			{
				return new DerObjectIdentifier(name);
			}
			DerObjectIdentifier obj = (DerObjectIdentifier)lookUp[Platform.ToLowerInvariant(name)];
			if (obj == null)
			{
				throw new ArgumentException("Unknown object id - " + name + " - passed to distinguished name");
			}
			return obj;
		}

		public X509Name(bool reverse, IDictionary lookUp, string dirName, X509NameEntryConverter converter)
		{
			this.converter = converter;
			X509NameTokenizer x509NameTokenizer = new X509NameTokenizer(dirName);
			while (x509NameTokenizer.HasMoreTokens())
			{
				string text = x509NameTokenizer.NextToken();
				int num = text.IndexOf('=');
				if (num == -1)
				{
					throw new ArgumentException("badly formated directory string");
				}
				string name = text.Substring(0, num);
				string text2 = text.Substring(num + 1);
				DerObjectIdentifier value = DecodeOid(name, lookUp);
				if (text2.IndexOf('+') > 0)
				{
					X509NameTokenizer x509NameTokenizer2 = new X509NameTokenizer(text2, '+');
					string value2 = x509NameTokenizer2.NextToken();
					ordering.Add(value);
					values.Add(value2);
					added.Add(false);
					while (x509NameTokenizer2.HasMoreTokens())
					{
						string text3 = x509NameTokenizer2.NextToken();
						int num2 = text3.IndexOf('=');
						string name2 = text3.Substring(0, num2);
						string value3 = text3.Substring(num2 + 1);
						ordering.Add(DecodeOid(name2, lookUp));
						values.Add(value3);
						added.Add(true);
					}
				}
				else
				{
					ordering.Add(value);
					values.Add(text2);
					added.Add(false);
				}
			}
			if (!reverse)
			{
				return;
			}
			IList list = Platform.CreateArrayList();
			IList list2 = Platform.CreateArrayList();
			IList list3 = Platform.CreateArrayList();
			int num3 = 1;
			for (int i = 0; i < ordering.Count; i++)
			{
				if (!(bool)added[i])
				{
					num3 = 0;
				}
				int index = num3++;
				list.Insert(index, ordering[i]);
				list2.Insert(index, values[i]);
				list3.Insert(index, added[i]);
			}
			ordering = list;
			values = list2;
			added = list3;
		}

		public IList GetOidList()
		{
			return Platform.CreateArrayList(ordering);
		}

		public IList GetValueList()
		{
			return GetValueList(null);
		}

		public IList GetValueList(DerObjectIdentifier oid)
		{
			IList list = Platform.CreateArrayList();
			for (int i = 0; i != values.Count; i++)
			{
				if (oid == null || oid.Equals(ordering[i]))
				{
					string text = (string)values[i];
					if (text.StartsWith("\\#"))
					{
						text = text.Substring(1);
					}
					list.Add(text);
				}
			}
			return list;
		}

		public override Asn1Object ToAsn1Object()
		{
			if (seq == null)
			{
				Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
				Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector();
				DerObjectIdentifier derObjectIdentifier = null;
				for (int i = 0; i != ordering.Count; i++)
				{
					DerObjectIdentifier derObjectIdentifier2 = (DerObjectIdentifier)ordering[i];
					string value = (string)values[i];
					if (derObjectIdentifier != null && !(bool)added[i])
					{
						asn1EncodableVector.Add(new DerSet(asn1EncodableVector2));
						asn1EncodableVector2 = new Asn1EncodableVector();
					}
					asn1EncodableVector2.Add(new DerSequence(derObjectIdentifier2, converter.GetConvertedValue(derObjectIdentifier2, value)));
					derObjectIdentifier = derObjectIdentifier2;
				}
				asn1EncodableVector.Add(new DerSet(asn1EncodableVector2));
				seq = new DerSequence(asn1EncodableVector);
			}
			return seq;
		}

		public bool Equivalent(X509Name other, bool inOrder)
		{
			if (!inOrder)
			{
				return Equivalent(other);
			}
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			int count = ordering.Count;
			if (count != other.ordering.Count)
			{
				return false;
			}
			for (int i = 0; i < count; i++)
			{
				DerObjectIdentifier obj = (DerObjectIdentifier)ordering[i];
				DerObjectIdentifier obj2 = (DerObjectIdentifier)other.ordering[i];
				if (!obj.Equals(obj2))
				{
					return false;
				}
				string s = (string)values[i];
				string s2 = (string)other.values[i];
				if (!equivalentStrings(s, s2))
				{
					return false;
				}
			}
			return true;
		}

		public bool Equivalent(X509Name other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			int count = ordering.Count;
			if (count != other.ordering.Count)
			{
				return false;
			}
			bool[] array = new bool[count];
			int num;
			int num2;
			int num3;
			if (ordering[0].Equals(other.ordering[0]))
			{
				num = 0;
				num2 = count;
				num3 = 1;
			}
			else
			{
				num = count - 1;
				num2 = -1;
				num3 = -1;
			}
			for (int i = num; i != num2; i += num3)
			{
				bool flag = false;
				DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)ordering[i];
				string s = (string)values[i];
				for (int j = 0; j < count; j++)
				{
					if (array[j])
					{
						continue;
					}
					DerObjectIdentifier obj = (DerObjectIdentifier)other.ordering[j];
					if (derObjectIdentifier.Equals(obj))
					{
						string s2 = (string)other.values[j];
						if (equivalentStrings(s, s2))
						{
							array[j] = true;
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		private static bool equivalentStrings(string s1, string s2)
		{
			string text = canonicalize(s1);
			string text2 = canonicalize(s2);
			if (!text.Equals(text2))
			{
				text = stripInternalSpaces(text);
				text2 = stripInternalSpaces(text2);
				if (!text.Equals(text2))
				{
					return false;
				}
			}
			return true;
		}

		private static string canonicalize(string s)
		{
			string text = Platform.ToLowerInvariant(s).Trim();
			if (text.StartsWith("#"))
			{
				Asn1Object asn1Object = decodeObject(text);
				if (asn1Object is IAsn1String)
				{
					text = Platform.ToLowerInvariant(((IAsn1String)asn1Object).GetString()).Trim();
				}
			}
			return text;
		}

		private static Asn1Object decodeObject(string v)
		{
			try
			{
				return Asn1Object.FromByteArray(Hex.Decode(v.Substring(1)));
			}
			catch (IOException ex)
			{
				throw new InvalidOperationException("unknown encoding in name: " + ex.Message, ex);
			}
		}

		private static string stripInternalSpaces(string str)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (str.Length != 0)
			{
				char c = str[0];
				stringBuilder.Append(c);
				for (int i = 1; i < str.Length; i++)
				{
					char c2 = str[i];
					if (c != ' ' || c2 != ' ')
					{
						stringBuilder.Append(c2);
					}
					c = c2;
				}
			}
			return stringBuilder.ToString();
		}

		private void AppendValue(StringBuilder buf, IDictionary oidSymbols, DerObjectIdentifier oid, string val)
		{
			string text = (string)oidSymbols[oid];
			if (text != null)
			{
				buf.Append(text);
			}
			else
			{
				buf.Append(oid.Id);
			}
			buf.Append('=');
			int i = buf.Length;
			buf.Append(val);
			int num = buf.Length;
			if (val.StartsWith("\\#"))
			{
				i += 2;
			}
			for (; i != num; i++)
			{
				if (buf[i] == ',' || buf[i] == '"' || buf[i] == '\\' || buf[i] == '+' || buf[i] == '=' || buf[i] == '<' || buf[i] == '>' || buf[i] == ';')
				{
					buf.Insert(i++, "\\");
					num++;
				}
			}
		}

		public string ToString(bool reverse, IDictionary oidSymbols, string Separator)
		{
			ArrayList arrayList = new ArrayList();
			StringBuilder stringBuilder = null;
			for (int i = 0; i < ordering.Count; i++)
			{
				if ((bool)added[i])
				{
					stringBuilder.Append('+');
					AppendValue(stringBuilder, oidSymbols, (DerObjectIdentifier)ordering[i], (string)values[i]);
				}
				else
				{
					stringBuilder = new StringBuilder();
					AppendValue(stringBuilder, oidSymbols, (DerObjectIdentifier)ordering[i], (string)values[i]);
					arrayList.Add(stringBuilder);
				}
			}
			if (reverse)
			{
				arrayList.Reverse();
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			if (arrayList.Count > 0)
			{
				stringBuilder2.Append(arrayList[0].ToString());
				for (int j = 1; j < arrayList.Count; j++)
				{
					stringBuilder2.Append(Separator);
					stringBuilder2.Append(arrayList[j].ToString());
				}
			}
			return stringBuilder2.ToString();
		}

		public string ToString(bool reverse, IDictionary oidSymbols)
		{
			ArrayList arrayList = new ArrayList();
			StringBuilder stringBuilder = null;
			for (int i = 0; i < ordering.Count; i++)
			{
				if ((bool)added[i])
				{
					stringBuilder.Append('+');
					AppendValue(stringBuilder, oidSymbols, (DerObjectIdentifier)ordering[i], (string)values[i]);
				}
				else
				{
					stringBuilder = new StringBuilder();
					AppendValue(stringBuilder, oidSymbols, (DerObjectIdentifier)ordering[i], (string)values[i]);
					arrayList.Add(stringBuilder);
				}
			}
			if (reverse)
			{
				arrayList.Reverse();
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			if (arrayList.Count > 0)
			{
				stringBuilder2.Append(arrayList[0].ToString());
				for (int j = 1; j < arrayList.Count; j++)
				{
					stringBuilder2.Append(',');
					stringBuilder2.Append(arrayList[j].ToString());
				}
			}
			return stringBuilder2.ToString();
		}

		public override string ToString()
		{
			DefaultReverse = true;
			return ToString(DefaultReverse, DefaultSymbols);
		}

		public string ToString(bool reverse)
		{
			return ToString(reverse, DefaultSymbols);
		}

		private string GetNameFromOID(DerObjectIdentifier derObjectIdentifier)
		{
			for (int i = 0; i < ordering.Count; i++)
			{
				DerObjectIdentifier derObjectIdentifier2 = ordering[i] as DerObjectIdentifier;
				if (derObjectIdentifier2 != null && derObjectIdentifier2.Id.Equals(derObjectIdentifier.Id))
				{
					return values[i] as string;
				}
			}
			return null;
		}

		private static List<DerObjectIdentifier> GetX509FieldOrder(bool ldaporder)
		{
			List<DerObjectIdentifier> list = new List<DerObjectIdentifier>();
			string[] dnObjects = DnComponents.GetDnObjects(ldaporder);
			foreach (string o in dnObjects)
			{
				list.Add(DnComponents.GetOid(o));
			}
			return list;
		}

		private string GetOrderedX509Name(bool ldaporder = true)
		{
			bool ldaporder2 = !IsDNReversed(ToString());
			List<DerObjectIdentifier> list = new List<DerObjectIdentifier>();
			List<string> list2 = new List<string>();
			List<DerObjectIdentifier> x509FieldOrder = GetX509FieldOrder(ldaporder2);
			HashSet<DerObjectIdentifier> hashSet = new HashSet<DerObjectIdentifier>();
			foreach (DerObjectIdentifier item in x509FieldOrder)
			{
				if (hashSet.Contains(item))
				{
					continue;
				}
				hashSet.Add(item);
				for (int i = 0; i < ordering.Count; i++)
				{
					DerObjectIdentifier derObjectIdentifier = ordering[i] as DerObjectIdentifier;
					if (derObjectIdentifier != null && derObjectIdentifier.Id.Equals(item.Id))
					{
						list.Add(derObjectIdentifier);
						list2.Add(values[i] as string);
					}
				}
			}
			list.Reverse();
			list2.Reverse();
			ordering = list;
			values = list2;
			return ToString();
		}

		private static bool IsDNReversed(string dn)
		{
			bool result = false;
			if (dn != null)
			{
				string text = null;
				string text2 = null;
				X509NameTokenizer x509NameTokenizer = new X509NameTokenizer(dn);
				if (x509NameTokenizer.HasMoreTokens())
				{
					text = x509NameTokenizer.NextToken().Trim();
				}
				while (x509NameTokenizer.HasMoreTokens())
				{
					text2 = x509NameTokenizer.NextToken().Trim();
				}
				string[] dnObjects = DnComponents.GetDnObjects(true);
				if (text != null && text2 != null)
				{
					text = text.Substring(0, text.IndexOf('='));
					text2 = text2.Substring(0, text2.IndexOf('='));
					int num = 0;
					int num2 = 0;
					for (int i = 0; i < dnObjects.Length; i++)
					{
						if (text.Equals(dnObjects[i]))
						{
							num = i;
						}
						if (text2.Equals(dnObjects[i]))
						{
							num2 = i;
						}
					}
					if (num2 < num)
					{
						result = true;
					}
				}
			}
			return result;
		}
	}
}
