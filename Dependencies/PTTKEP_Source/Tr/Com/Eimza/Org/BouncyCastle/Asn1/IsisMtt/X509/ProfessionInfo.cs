using System;
using System.Collections;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.X500;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.IsisMtt.X509
{
	internal class ProfessionInfo : Asn1Encodable
	{
		public static readonly DerObjectIdentifier Rechtsanwltin;

		public static readonly DerObjectIdentifier Rechtsanwalt;

		public static readonly DerObjectIdentifier Rechtsbeistand;

		public static readonly DerObjectIdentifier Steuerberaterin;

		public static readonly DerObjectIdentifier Steuerberater;

		public static readonly DerObjectIdentifier Steuerbevollmchtigte;

		public static readonly DerObjectIdentifier Steuerbevollmchtigter;

		public static readonly DerObjectIdentifier Notarin;

		public static readonly DerObjectIdentifier Notar;

		public static readonly DerObjectIdentifier Notarvertreterin;

		public static readonly DerObjectIdentifier Notarvertreter;

		public static readonly DerObjectIdentifier Notariatsverwalterin;

		public static readonly DerObjectIdentifier Notariatsverwalter;

		public static readonly DerObjectIdentifier Wirtschaftsprferin;

		public static readonly DerObjectIdentifier Wirtschaftsprfer;

		public static readonly DerObjectIdentifier VereidigteBuchprferin;

		public static readonly DerObjectIdentifier VereidigterBuchprfer;

		public static readonly DerObjectIdentifier Patentanwltin;

		public static readonly DerObjectIdentifier Patentanwalt;

		private readonly NamingAuthority namingAuthority;

		private readonly Asn1Sequence professionItems;

		private readonly Asn1Sequence professionOids;

		private readonly string registrationNumber;

		private readonly Asn1OctetString addProfessionInfo;

		public virtual Asn1OctetString AddProfessionInfo
		{
			get
			{
				return addProfessionInfo;
			}
		}

		public virtual NamingAuthority NamingAuthority
		{
			get
			{
				return namingAuthority;
			}
		}

		public virtual string RegistrationNumber
		{
			get
			{
				return registrationNumber;
			}
		}

		public static ProfessionInfo GetInstance(object obj)
		{
			if (obj == null || obj is ProfessionInfo)
			{
				return (ProfessionInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new ProfessionInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + obj.GetType().Name, "obj");
		}

		private ProfessionInfo(Asn1Sequence seq)
		{
			if (seq.Count > 5)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			Asn1Encodable asn1Encodable = (Asn1Encodable)enumerator.Current;
			if (asn1Encodable is Asn1TaggedObject)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)asn1Encodable;
				if (asn1TaggedObject.TagNo != 0)
				{
					throw new ArgumentException("Bad tag number: " + asn1TaggedObject.TagNo);
				}
				namingAuthority = NamingAuthority.GetInstance(asn1TaggedObject, true);
				enumerator.MoveNext();
				asn1Encodable = (Asn1Encodable)enumerator.Current;
			}
			professionItems = Asn1Sequence.GetInstance(asn1Encodable);
			if (enumerator.MoveNext())
			{
				asn1Encodable = (Asn1Encodable)enumerator.Current;
				if (asn1Encodable is Asn1Sequence)
				{
					professionOids = Asn1Sequence.GetInstance(asn1Encodable);
				}
				else if (asn1Encodable is DerPrintableString)
				{
					registrationNumber = DerPrintableString.GetInstance(asn1Encodable).GetString();
				}
				else
				{
					if (!(asn1Encodable is Asn1OctetString))
					{
						throw new ArgumentException("Bad object encountered: " + asn1Encodable.GetType().Name);
					}
					addProfessionInfo = Asn1OctetString.GetInstance(asn1Encodable);
				}
			}
			if (enumerator.MoveNext())
			{
				asn1Encodable = (Asn1Encodable)enumerator.Current;
				if (asn1Encodable is DerPrintableString)
				{
					registrationNumber = DerPrintableString.GetInstance(asn1Encodable).GetString();
				}
				else
				{
					if (!(asn1Encodable is DerOctetString))
					{
						throw new ArgumentException("Bad object encountered: " + asn1Encodable.GetType().Name);
					}
					addProfessionInfo = (DerOctetString)asn1Encodable;
				}
			}
			if (enumerator.MoveNext())
			{
				asn1Encodable = (Asn1Encodable)enumerator.Current;
				if (!(asn1Encodable is DerOctetString))
				{
					throw new ArgumentException("Bad object encountered: " + asn1Encodable.GetType().Name);
				}
				addProfessionInfo = (DerOctetString)asn1Encodable;
			}
		}

		public ProfessionInfo(NamingAuthority namingAuthority, DirectoryString[] professionItems, DerObjectIdentifier[] professionOids, string registrationNumber, Asn1OctetString addProfessionInfo)
		{
			this.namingAuthority = namingAuthority;
			Asn1Encodable[] v = professionItems;
			this.professionItems = new DerSequence(v);
			if (professionOids != null)
			{
				v = professionOids;
				this.professionOids = new DerSequence(v);
			}
			this.registrationNumber = registrationNumber;
			this.addProfessionInfo = addProfessionInfo;
		}

		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
			if (namingAuthority != null)
			{
				asn1EncodableVector.Add(new DerTaggedObject(true, 0, namingAuthority));
			}
			asn1EncodableVector.Add(professionItems);
			if (professionOids != null)
			{
				asn1EncodableVector.Add(professionOids);
			}
			if (registrationNumber != null)
			{
				asn1EncodableVector.Add(new DerPrintableString(registrationNumber, true));
			}
			if (addProfessionInfo != null)
			{
				asn1EncodableVector.Add(addProfessionInfo);
			}
			return new DerSequence(asn1EncodableVector);
		}

		public virtual DirectoryString[] GetProfessionItems()
		{
			DirectoryString[] array = new DirectoryString[professionItems.Count];
			for (int i = 0; i < professionItems.Count; i++)
			{
				array[i] = DirectoryString.GetInstance(professionItems[i]);
			}
			return array;
		}

		public virtual DerObjectIdentifier[] GetProfessionOids()
		{
			if (professionOids == null)
			{
				return new DerObjectIdentifier[0];
			}
			DerObjectIdentifier[] array = new DerObjectIdentifier[professionOids.Count];
			for (int i = 0; i < professionOids.Count; i++)
			{
				array[i] = DerObjectIdentifier.GetInstance(professionOids[i]);
			}
			return array;
		}

		static ProfessionInfo()
		{
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Rechtsanwltin = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern.ToString() : null) + ".1");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern2 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Rechtsanwalt = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern2 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern2.ToString() : null) + ".2");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern3 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Rechtsbeistand = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern3 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern3.ToString() : null) + ".3");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern4 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Steuerberaterin = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern4 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern4.ToString() : null) + ".4");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern5 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Steuerberater = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern5 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern5.ToString() : null) + ".5");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern6 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Steuerbevollmchtigte = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern6 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern6.ToString() : null) + ".6");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern7 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Steuerbevollmchtigter = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern7 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern7.ToString() : null) + ".7");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern8 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Notarin = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern8 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern8.ToString() : null) + ".8");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern9 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Notar = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern9 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern9.ToString() : null) + ".9");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern10 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Notarvertreterin = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern10 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern10.ToString() : null) + ".10");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern11 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Notarvertreter = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern11 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern11.ToString() : null) + ".11");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern12 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Notariatsverwalterin = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern12 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern12.ToString() : null) + ".12");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern13 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Notariatsverwalter = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern13 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern13.ToString() : null) + ".13");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern14 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Wirtschaftsprferin = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern14 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern14.ToString() : null) + ".14");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern15 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Wirtschaftsprfer = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern15 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern15.ToString() : null) + ".15");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern16 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			VereidigteBuchprferin = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern16 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern16.ToString() : null) + ".16");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern17 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			VereidigterBuchprfer = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern17 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern17.ToString() : null) + ".17");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern18 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Patentanwltin = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern18 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern18.ToString() : null) + ".18");
			DerObjectIdentifier idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern19 = NamingAuthority.IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern;
			Patentanwalt = new DerObjectIdentifier(((idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern19 != null) ? idIsisMttATNamingAuthoritiesRechtWirtschaftSteuern19.ToString() : null) + ".19");
		}
	}
}
