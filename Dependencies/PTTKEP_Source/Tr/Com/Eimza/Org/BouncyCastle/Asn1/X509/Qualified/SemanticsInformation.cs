using System;
using System.Collections;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.X509.Qualified
{
	internal class SemanticsInformation : Asn1Encodable
	{
		private readonly DerObjectIdentifier semanticsIdentifier;

		private readonly GeneralName[] nameRegistrationAuthorities;

		public DerObjectIdentifier SemanticsIdentifier
		{
			get
			{
				return semanticsIdentifier;
			}
		}

		public static SemanticsInformation GetInstance(object obj)
		{
			if (obj == null || obj is SemanticsInformation)
			{
				return (SemanticsInformation)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new SemanticsInformation(Asn1Sequence.GetInstance(obj));
			}
			throw new ArgumentException("unknown object in GetInstance: " + obj.GetType().FullName, "obj");
		}

		public SemanticsInformation(Asn1Sequence seq)
		{
			if (seq.Count < 1)
			{
				throw new ArgumentException("no objects in SemanticsInformation");
			}
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			object obj = enumerator.Current;
			if (obj is DerObjectIdentifier)
			{
				semanticsIdentifier = DerObjectIdentifier.GetInstance(obj);
				obj = ((!enumerator.MoveNext()) ? null : enumerator.Current);
			}
			if (obj != null)
			{
				Asn1Sequence instance = Asn1Sequence.GetInstance(obj);
				nameRegistrationAuthorities = new GeneralName[instance.Count];
				for (int i = 0; i < instance.Count; i++)
				{
					nameRegistrationAuthorities[i] = GeneralName.GetInstance(instance[i]);
				}
			}
		}

		public SemanticsInformation(DerObjectIdentifier semanticsIdentifier, GeneralName[] generalNames)
		{
			this.semanticsIdentifier = semanticsIdentifier;
			nameRegistrationAuthorities = generalNames;
		}

		public SemanticsInformation(DerObjectIdentifier semanticsIdentifier)
		{
			this.semanticsIdentifier = semanticsIdentifier;
		}

		public SemanticsInformation(GeneralName[] generalNames)
		{
			nameRegistrationAuthorities = generalNames;
		}

		public GeneralName[] GetNameRegistrationAuthorities()
		{
			return nameRegistrationAuthorities;
		}

		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector();
			if (semanticsIdentifier != null)
			{
				asn1EncodableVector.Add(semanticsIdentifier);
			}
			if (nameRegistrationAuthorities != null)
			{
				Asn1Encodable[] array = new Asn1Encodable[1];
				Asn1Encodable[] v = nameRegistrationAuthorities;
				array[0] = new DerSequence(v);
				asn1EncodableVector.Add(array);
			}
			return new DerSequence(asn1EncodableVector);
		}
	}
}
