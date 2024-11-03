using Aesob.Docs.Data;
using Aesob.Docs.Services;
using Aesob.Web.Library;

namespace Aesob.KEP.Library
{
	public static class DocumentUtilities
	{
		public static async Task LoginToDocsService(string username, string password)
		{
			var loginResult = await UserService.TryLogin(username, password);
			if (loginResult?.IsSuccess == true)
			{
				Debug.Print($"Logged in as : {username}");
				User.Current = loginResult.User;
			}
			else
			{
				Debug.Print($"Faile to login as: {username}\n{loginResult?.ErrorMessage}");
			}
		}

		public static async Task AddPackageToEDYS(PackageMailContent packageData, string docsUsername, string docsPassword)
		{
			Debug.Print("Adding package to EDYS");

			if (User.Current == null || User.Current.Id < 0)
			{
				Debug.Print($"User was invalid, trying to login again.");

				for (int i = 0; i < 5; i++)
				{
					Debug.Print($"Login attempt: {i + 1}");
					await LoginToDocsService(docsUsername, docsPassword);
					if (User.Current?.Id >= 0)
					{
						break;
					}
				}

				if (User.Current == null || User.Current.Id < 0)
				{
					Debug.Print($"Login attempts failed.");
				}
			}

			var sourcesResult = await DocumentService.GetDocumentSources();
			if (sourcesResult?.IsSuccessful != true)
			{
				Debug.Print($"Failed to retrieve sources from docs: {sourcesResult.ErrorMessage}");
				return;
			}

			var source = sourcesResult.Value?.FirstOrDefault(s => s.Id == 114);
			if (source == null)
			{
				Debug.Print($"Failed to retrieve KEP source");
				return;
			}

			var document = new Document()
			{
				Name = packageData.Subject,
				Description = $"Kep İletisi ({packageData.KepSıraNo}) - {packageData.Content}",
				IsOutgoing = false,
				UploadDate = DateTime.Now,
				LastRevisionDate = DateTime.Now,
				Source = source,
				SourceId = source.Id,
				OwnerUserId = 10
			};

			var files = new List<DocumentFile>();
			foreach (var attachment in packageData.Attachments)
			{
				var aesobDocumentFile = new DocumentFile()
				{
					Name = $"Kep İletisi Eki: {packageData.KepSıraNo} - {attachment.Name}",
					Description = $"{packageData.KepSıraNo} no'lu kep iletisi eki",
					FileContent = attachment.Value,
					FileSize = attachment.Value.Length,
					FileName = attachment.Name,
					VisibleFileName = attachment.Name,
					Document = document,
					DocumentId = -1,
				};

				files.Add(aesobDocumentFile);
			}

			document.DocumentFiles = files.ToArray();

			var uploadResult = await DocumentService.UploadDocument(document);
			if (uploadResult?.IsSuccessful == true)
			{
				Debug.Print($"Document successfully added to document server. ({document.Name})");
			}
			else
			{
				Debug.Print($"Failed to add document to document server: {uploadResult?.ErrorMessage}");
			}
		}

	}
}
