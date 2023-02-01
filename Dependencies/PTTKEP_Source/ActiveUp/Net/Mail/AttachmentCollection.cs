using System;
using System.Collections;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class AttachmentCollection : MimePartCollection
	{
		public new void Add(MimePart part)
		{
			part.ContentDisposition.Disposition = "attachment";
			base.List.Add(part);
		}

		public new void Add(string path, bool generateContentId)
		{
			MimePart mimePart = new MimePart(path, generateContentId);
			mimePart.ContentDisposition.Disposition = "attachment";
			base.List.Add(mimePart);
		}

		public new void Add(string path, bool generateContentId, string charset)
		{
			MimePart mimePart = new MimePart(path, generateContentId, charset);
			mimePart.ContentDisposition.Disposition = "attachment";
			base.List.Add(mimePart);
		}

		public void Add(byte[] attachment, string filename)
		{
			MimePart mimePart = new MimePart(attachment, filename);
			mimePart.ContentDisposition.Disposition = "attachment";
			base.List.Add(mimePart);
		}

		public void StoreToFolder(string path)
		{
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MimePart mimePart = (MimePart)enumerator.Current;
					mimePart.StoreToFile(path.TrimEnd('\\') + "\\" + mimePart.Filename);
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
		}
	}
}
