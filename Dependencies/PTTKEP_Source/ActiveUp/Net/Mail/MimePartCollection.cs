using System;
using System.Collections;
using System.IO;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class MimePartCollection : CollectionBase
	{
		public MimePart this[int index]
		{
			get
			{
				return (MimePart)base.List[index];
			}
		}

		public MimePart this[string filename]
		{
			get
			{
				foreach (MimePart item in base.List)
				{
					if (item.ContentDisposition.FileName == filename)
					{
						return item;
					}
				}
				return null;
			}
		}

		public void Add(MimePart part)
		{
			base.List.Add(part);
		}

		public void Add(string path, bool generateContentId)
		{
			base.List.Add(new MimePart(path, generateContentId));
		}

		public void Add(string path, bool generateContentId, string charset)
		{
			base.List.Add(new MimePart(path, generateContentId, charset));
		}

		internal MimePartCollection ConcatMessagesAsPart(MessageCollection input)
		{
			MimePartCollection mimePartCollection = new MimePartCollection();
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MimePart part = (MimePart)enumerator.Current;
					mimePartCollection.Add(part);
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
			foreach (Message item in input)
			{
				mimePartCollection.Add(item.ToMimePart());
			}
			return mimePartCollection;
		}

		public static MimePartCollection operator +(MimePartCollection first, MimePartCollection second)
		{
			foreach (MimePart item in second)
			{
				first.Add(item);
			}
			return first;
		}

		public bool Contains(string filename)
		{
			filename = Path.GetFileName(filename);
			foreach (MimePart item in base.List)
			{
				if (item.ContentDisposition.FileName == filename)
				{
					return true;
				}
			}
			return false;
		}
	}
}
