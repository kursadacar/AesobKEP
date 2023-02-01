using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Asn1.Utilities;

namespace Tr.Com.Eimza.Org.BouncyCastle.Bcpg.OpenPgp
{
	internal class WrappedGeneratorStream : FilterStream
	{
		private readonly IStreamGenerator gen;

		public WrappedGeneratorStream(IStreamGenerator gen, Stream str)
			: base(str)
		{
			this.gen = gen;
		}

		public override void Close()
		{
			gen.Close();
		}
	}
}
