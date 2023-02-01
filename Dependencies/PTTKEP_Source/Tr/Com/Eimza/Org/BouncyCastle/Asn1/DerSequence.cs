using System;
using System.Collections;
using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1
{
	internal class DerSequence : Asn1Sequence
	{
		public static readonly DerSequence Empty = new DerSequence();

		public static DerSequence FromVector(Asn1EncodableVector v)
		{
			if (v.Count >= 1)
			{
				return new DerSequence(v);
			}
			return Empty;
		}

		public DerSequence()
			: base(0)
		{
		}

		public DerSequence(Asn1Encodable obj)
			: base(1)
		{
			AddObject(obj);
		}

		public DerSequence(params Asn1Encodable[] v)
			: base(v.Length)
		{
			foreach (Asn1Encodable obj in v)
			{
				AddObject(obj);
			}
		}

		public DerSequence(Asn1EncodableVector v)
			: base(v.Count)
		{
			foreach (Asn1Encodable item in v)
			{
				AddObject(item);
			}
		}

		internal override void Encode(DerOutputStream derOut)
		{
			MemoryStream memoryStream = new MemoryStream();
			DerOutputStream derOutputStream = new DerOutputStream(memoryStream);
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Asn1Encodable obj = (Asn1Encodable)enumerator.Current;
					derOutputStream.WriteObject(obj);
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			derOutputStream.Close();
			byte[] bytes = memoryStream.ToArray();
			derOut.WriteEncoded(48, bytes);
		}
	}
}
