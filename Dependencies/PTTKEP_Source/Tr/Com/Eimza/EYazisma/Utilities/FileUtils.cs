using System;
using System.IO;

namespace Tr.Com.Eimza.EYazisma.Utilities
{
	public static class FileUtils
	{
		public static bool IsFilePathValid(string a_path)
		{
			if (a_path.Trim() == string.Empty)
			{
				return false;
			}
			string pathRoot;
			string fileName;
			try
			{
				pathRoot = Path.GetPathRoot(a_path);
				fileName = Path.GetFileName(a_path);
			}
			catch (ArgumentException)
			{
				return false;
			}
			if (fileName.Trim() == string.Empty)
			{
				return false;
			}
			if (pathRoot.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
			{
				return false;
			}
			if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
			{
				return false;
			}
			return true;
		}

		public static void SaveFile(byte[] fileValue, string savePath)
		{
			if (File.Exists(savePath))
			{
				throw new Exception("Dosya Zaten Mevcut. Adres : " + savePath);
			}
			if (!IsFilePathValid(savePath))
			{
				throw new Exception("Dosya Kayıt Adresi Hatalı. Adres : " + savePath);
			}
			BinaryWriter binaryWriter = null;
			try
			{
				binaryWriter = new BinaryWriter(File.OpenWrite(savePath));
				binaryWriter.Write(fileValue);
			}
			catch (Exception innerException)
			{
				throw new Exception("Dosyas Diske Kaydedilemedi.", innerException);
			}
			finally
			{
				if (binaryWriter != null)
				{
					binaryWriter.Close();
				}
			}
		}
	}
}
