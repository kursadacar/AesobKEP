using System;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1.Cmp
{
	internal class PopoDecKeyChallContent : Asn1Encodable
	{
		private readonly Asn1Sequence content;

		private PopoDecKeyChallContent(Asn1Sequence seq)
		{
			content = seq;
		}

		public static PopoDecKeyChallContent GetInstance(object obj)
		{
			if (obj is PopoDecKeyChallContent)
			{
				return (PopoDecKeyChallContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PopoDecKeyChallContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + obj.GetType().Name, "obj");
		}

		public virtual Challenge[] ToChallengeArray()
		{
			Challenge[] array = new Challenge[content.Count];
			for (int i = 0; i != array.Length; i++)
			{
				array[i] = Challenge.GetInstance(content[i]);
			}
			return array;
		}

		public override Asn1Object ToAsn1Object()
		{
			return content;
		}
	}
}
