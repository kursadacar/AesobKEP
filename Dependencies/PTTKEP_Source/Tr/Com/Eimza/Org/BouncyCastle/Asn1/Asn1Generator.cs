using System.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1
{
	internal abstract class Asn1Generator
	{
		private Stream _out;

		protected Stream Out
		{
			get
			{
				return _out;
			}
		}

		protected Asn1Generator(Stream outStream)
		{
			_out = outStream;
		}

		public abstract void AddObject(Asn1Encodable obj);

		public abstract Stream GetRawOutputStream();

		public abstract void Close();
	}
}
