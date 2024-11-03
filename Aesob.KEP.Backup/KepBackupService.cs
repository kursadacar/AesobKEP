using System.IO.Packaging;
using System.ServiceModel;
using System.Xml;
using Aesob.Docs.Services;
using Aesob.KEP.Library;
using Aesob.Web.Core.Public;
using Aesob.Web.Library;
using Aesob.Web.Library.Service;
using Tr.Com.Eimza.EYazisma;
using Tr.Com.Eimza.EYazisma.EYazisma_WS;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Aesob.KEP.Backup
{
	public class KepBackupService : IAesobService
	{
		private IAesobService _thisAsInterface;

		private HttpClient _httpClient;
		private EYazismaApi _eYazisma;

		private bool _uploadDocuments;
		private string _docsUsername;
		private string _docsPassword;

		public KepBackupService()
		{
			_httpClient = new HttpClient();
			_thisAsInterface = this;
		}

		public async Task Start()
		{
			var configs = GetWebServiceConfigs();

			var account = _thisAsInterface.GetConfig("Hesap").Value;
			var id = _thisAsInterface.GetConfig("TC").Value;
			var password = _thisAsInterface.GetConfig("Parola").Value;
			var passCode = _thisAsInterface.GetConfig("Sifre").Value;
			var endpointAddress = _thisAsInterface.GetConfig("EndPoint").Value;
			var mode = _thisAsInterface.GetConfig("Mode").Value;
			_uploadDocuments = _thisAsInterface.GetConfig("UploadDocuments").Value == "true";
			_docsUsername = _thisAsInterface.GetConfig("AesobDocsLoginUsername").Value;
			_docsPassword = _thisAsInterface.GetConfig("AesobDocsLoginPassword").Value;

			_eYazisma = new EYazismaApi(account, id, password, passCode, configs, endpointAddress);

			Debug.Print($"Starting KEP backup. Mode : {mode}");

			await Task.Delay(10);

			const string downloadDirectoryName = "PackageDownloads";
			string downloadDirectory = Path.GetFullPath(downloadDirectoryName);

			if (mode == "READ")
			{
				await ReadBackups(downloadDirectory);
			}
			else if(mode == "WRITE")
			{
				await DownloadAndWriteBackups(downloadDirectory);
			}
			else
			{
				Debug.FailedAssert($"Mode is not recognized: {mode}");
			}
		}

		private async Task ReadBackups(string downloadDirectory)
		{
#if DEBUG
			//await DocumentUtilities.LoginToDocsService(_docsUsername, _docsPassword);
			//var clearResult = await DocumentService.ClearDuplicates();
#endif

			Debug.Print($"Reading backups...");

			var files = Directory.GetFiles(downloadDirectory);
			foreach(var file in files)
			{
				var buffer = File.ReadAllBytes(file);
				var content = BitDeserializeContent(buffer);

				if (_uploadDocuments)
				{
					var mailContents = PackageUtilities.GetMailContentFor(_eYazisma, content);
					foreach (var mailContent in mailContents)
					{
						await DocumentUtilities.AddPackageToEDYS(mailContent, _docsUsername, _docsPassword);
					}
				}
			}

			Debug.Print($"Finished reading backups...");
		}

		private async Task DownloadAndWriteBackups(string downloadDirectory)
		{
			Debug.Print($"Downloading and writing backups...");

			if (!Directory.Exists(downloadDirectory))
			{
				Debug.Print($"Creating PackageDownloads directory in: {downloadDirectory}");
				Directory.CreateDirectory(downloadDirectory);
			}

			var startDate = DateTime.Now;

			while (startDate.Year > 2020)
			{
				startDate = startDate.AddMonths(-3);
				var endDate = startDate;
				endDate = endDate.AddMonths(3);

				await CheckPackagesBetweenDates(startDate, endDate, downloadDirectory);
			}
		}

		private async Task CheckPackagesBetweenDates(DateTime startDate, DateTime endDate, string downloadDirectory)
		{
			Debug.Print($"Retrieving packages between dates: {startDate} - {endDate}");

			var packages = _eYazisma.PaketSorgula(startDate, endDate);
			if (packages == null || packages.KepId == null || packages.KepSiraNo == null || packages.HataAciklama.Length == 1)
			{
				Debug.Print($"Failed to retrieve packages: ({packages?.Durum?.ElementAt(0)}) {packages?.HataAciklama?.ElementAt(0)}");
				return;
			}

			for(int i = 0; i < packages.KepId.Length; i++)
			{
				var kepId = packages.KepId[i];
				if (string.IsNullOrEmpty(kepId))
				{
					Debug.Print($"[ERROR] Failed to unpack a package...");
					continue;
				}

				if(packages.KepSiraNo == null || i >= packages.KepSiraNo.Length || packages.KepSiraNo[i] < 0)
				{
					Debug.Print($"[ERROR] Failed to retrieve package no for: {kepId}");
					continue;
				}

				int packageNo = packages.KepSiraNo[i].Value;

				string packagePath = Path.Combine(downloadDirectory, packageNo.ToString() + ".kepPkg");
				if (File.Exists(packagePath))
				{
					Debug.Print($"Record for: {packageNo} already exists. Skipping...");
					continue;
				}

				Debug.Print($"Downloading package: {packageNo}");
				var downloadResult = _eYazisma.PaketIndir(kepId, "", EYazismaPart.ALL);
				if (downloadResult == null || downloadResult.Durum != 0)
				{
					Debug.Print($"[ERROR] Failed to download package: {kepId}. Hata: {(downloadResult?.HataAciklama ?? "-")}");
					continue;
				}


				var packageContent = CreatePackageFor(downloadResult);
				var buffer = BitSerializeContent(packageContent.ToArray());
				var deserializedContent = BitDeserializeContent(buffer);

				if(deserializedContent.Length != packageContent.Length)
				{
					Debug.FailedAssert($"Serialized content can't be correctly deserialized!");
					continue;
				}

				if(!CompareContents(packageContent, deserializedContent))
				{
					Debug.FailedAssert($"Serialized content can't be correctly deserialized!");
					continue;
				}

				using (var stream = new FileStream(packagePath, FileMode.OpenOrCreate))
				{
					BinaryWriter writer = new BinaryWriter(stream);
					writer.Write(buffer);
				}

				if (_uploadDocuments)
				{
					var mailContents = PackageUtilities.GetMailContentFor(_eYazisma, packageContent);
					foreach (var mailContent in mailContents)
					{
						await DocumentUtilities.AddPackageToEDYS(mailContent, _docsUsername, _docsPassword);
					}
				}

				Debug.Print($"Save package to file: {packagePath}");
			}
		}

		private KepPackage[] CreatePackageFor(EyPaketIndirSonuc downloadResult)
		{
			if(downloadResult?.EyazismaPaketi == null)
			{
				return null;
			}

			KepPackage[] packageContent = new KepPackage[downloadResult.EyazismaPaketi.Length];
			for (int i = 0; i < packageContent.Length; i++)
			{
				var downloadedData = downloadResult.EyazismaPaketi[i];
				KepPackage package = new KepPackage(downloadedData.contentType, downloadedData.fileName, downloadedData.Value);
				packageContent[i] = package;
			}
			return packageContent;
		}

		private bool CompareContents(KepPackage[] left, KepPackage[] right)
		{
			for(int i = 0; i < left.Length; i++)
			{
				var leftData = left[i];
				var rightData = right[i];

				if(leftData.ContentType != rightData.ContentType)
				{
					return false;
				}
				
				if(leftData.FileName != rightData.FileName)
				{
					return false;
				}

				if(leftData.ValueLength != rightData.ValueLength)
				{
					return false;
				}

				if(leftData.Value?.Length != rightData.Value?.Length)
				{
					return false;
				}

				for(int d = 0; d < leftData.Value.Length; d++)
				{
					if (leftData.Value[d] != rightData.Value[d])
					{
						return false;
					}
				}
			}

			return true;
		}

		private KepPackage[] BitDeserializeContent(byte[] buffer)
		{
			var result = new List<KepPackage>();

			using (var stream = new MemoryStream(buffer, false))
			{
				BinaryReader reader = new BinaryReader(stream);

				while (stream.Position < stream.Length)
				{
					var contentType = reader.ReadString();
					var fileName = reader.ReadString();
					var valueLength = reader.ReadInt32();
					var value = reader.ReadBytes(valueLength);

					var data = new KepPackage(contentType, fileName, value);
					result.Add(data);
				}

			}

			return result.ToArray();
		}

		private byte[] BitSerializeContent(KepPackage[] packages)
		{
			using(var stream = new MemoryStream())
			{
				BinaryWriter writer = new BinaryWriter(stream);
				foreach(var package in packages)
				{
					writer.Write(package.ContentType);
					writer.Write(package.FileName);
					writer.Write(package.ValueLength);
					writer.Write(package.Value);
				}

				return stream.ToArray();
			}
		}

		public async Task Stop()
		{
			await Task.Delay(1);
		}

		public async Task Update(float dt)
		{
			await Task.Delay(1);
		}

		private BasicHttpBinding GetWebServiceConfigs()
		{
			BasicHttpBinding basicHttpBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);

			int sizeMultiplier = 2;

			basicHttpBinding.Name = "eyServisSOAPBinding";
			basicHttpBinding.CloseTimeout = new TimeSpan(0, 10, 0);
			basicHttpBinding.OpenTimeout = new TimeSpan(0, 10, 0);
			basicHttpBinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
			basicHttpBinding.SendTimeout = new TimeSpan(0, 10, 0);
			basicHttpBinding.AllowCookies = false;
			basicHttpBinding.BypassProxyOnLocal = true;
			basicHttpBinding.MaxBufferPoolSize = 33554432 * sizeMultiplier;
			basicHttpBinding.MaxReceivedMessageSize = 33554432 * sizeMultiplier;
			basicHttpBinding.MaxBufferSize = 33554432 * sizeMultiplier;
			basicHttpBinding.UseDefaultWebProxy = true;
			basicHttpBinding.MessageEncoding = WSMessageEncoding.Mtom;

			XmlDictionaryReaderQuotas readerQuotas = new XmlDictionaryReaderQuotas();
			readerQuotas.MaxDepth = 32;
			readerQuotas.MaxStringContentLength = 41943040 * sizeMultiplier;
			readerQuotas.MaxArrayLength = 16384 * sizeMultiplier;
			readerQuotas.MaxBytesPerRead = 4096 * sizeMultiplier;
			readerQuotas.MaxNameTableCharCount = 16384 * sizeMultiplier;

			basicHttpBinding.ReaderQuotas = readerQuotas;
			basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;

			return basicHttpBinding;
		}
	}
}
