using System.IO;
using Tr.Com.Eimza.Org.BouncyCastle.Security;
using Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO.Pem;

namespace Tr.Com.Eimza.Org.BouncyCastle.OpenSsl
{
	internal class PemWriter : Tr.Com.Eimza.Org.BouncyCastle.Utilities.IO.Pem.PemWriter
	{
		public PemWriter(TextWriter writer)
			: base(writer)
		{
		}

		public void WriteObject(object obj)
		{
			try
			{
				base.WriteObject((PemObjectGenerator)new MiscPemGenerator(obj));
			}
			catch (PemGenerationException ex)
			{
				if (ex.InnerException is IOException)
				{
					throw (IOException)ex.InnerException;
				}
				throw ex;
			}
		}

		public void WriteObject(object obj, string algorithm, char[] password, SecureRandom random)
		{
			base.WriteObject((PemObjectGenerator)new MiscPemGenerator(obj, algorithm, password, random));
		}
	}
}
