using System;
using System.IO;

namespace Tr.Com.Eimza.Smime
{
	internal class TemporaryDirectory : IDisposable
	{
		public string DirectoryPath { get; private set; }

		public TemporaryDirectory()
		{
			DirectoryPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			Directory.CreateDirectory(DirectoryPath);
		}

		public void Dispose()
		{
			if (Directory.Exists(DirectoryPath))
			{
				Directory.Delete(DirectoryPath, true);
			}
		}
	}
}
