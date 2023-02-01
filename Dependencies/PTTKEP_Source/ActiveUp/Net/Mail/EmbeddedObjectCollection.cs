using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class EmbeddedObjectCollection : MimePartCollection
	{
		public new void Add(MimePart part)
		{
			part.ContentDisposition.Disposition = "inline";
			base.List.Add(part);
		}

		public new string Add(string path, bool generateContentId)
		{
			MimePart mimePart = new MimePart(path, generateContentId);
			mimePart.ContentDisposition.Disposition = "inline";
			base.List.Add(mimePart);
			return mimePart.EmbeddedObjectContentId;
		}

		public virtual string Add(string path, string contentId)
		{
			MimePart mimePart = new MimePart(path, contentId);
			mimePart.ContentDisposition.Disposition = "inline";
			base.List.Add(mimePart);
			return mimePart.EmbeddedObjectContentId;
		}

		public virtual string Add(string path, string contentId, string charset)
		{
			MimePart mimePart = new MimePart(path, contentId, charset);
			mimePart.ContentDisposition.Disposition = "inline";
			base.List.Add(mimePart);
			return mimePart.EmbeddedObjectContentId;
		}

		public new string Add(string path, bool generateContentId, string charset)
		{
			MimePart mimePart = new MimePart(path, generateContentId, charset);
			mimePart.ContentDisposition.Disposition = "inline";
			base.List.Add(mimePart);
			return mimePart.ContentId;
		}
	}
}
