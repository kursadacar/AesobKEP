using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO;

namespace Tr.Com.Eimza.Org.BouncyCastle.Asn1
{
	internal abstract class LimitedInputStream : BaseInputStream
	{
		protected readonly Stream _in;

		private readonly int _limit;

		internal LimitedInputStream(Stream inStream, int limit)
		{
			_in = inStream;
			_limit = limit;
		}

		internal virtual int GetRemaining()
		{
			return _limit;
		}

		protected virtual void SetParentEofDetect(bool on)
		{
			if (_in is IndefiniteLengthInputStream)
			{
				((IndefiniteLengthInputStream)_in).SetEofOn00(on);
			}
		}
	}
}
