using System;
using System.Linq;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class ContentDisposition : StructuredHeaderField
	{
		public const int Inline = 1;

		public const int Attachment = 2;

		private string _disposition = string.Empty;

		public string FileName
		{
			get
			{
				if (base.Parameters["filename"] != null)
				{
					return "\"" + base.Parameters["filename"].Trim('"') + "\"";
				}
				if (base.Parameters["\tfilename"] != null)
				{
					return base.Parameters["\tfilename"].Trim('"').Trim('\t');
				}
				return null;
			}
			set
			{
				if (base.Parameters["filename"] != null)
				{
					base.Parameters["filename"] = "\"" + value.Trim('"') + "\"";
				}
				else
				{
					base.Parameters.Add("filename", "\"" + value.Trim('"') + "\"");
				}
			}
		}

		public string Disposition
		{
			get
			{
				return _disposition;
			}
			set
			{
				_disposition = value;
			}
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty = empty + "Content-Disposition: " + Disposition;
			return base.Parameters.AllKeys.Aggregate(empty, (string current, string key) => current + ";\r\n\t" + key + "=" + base.Parameters[key]);
		}

		public static bool operator ==(ContentDisposition t1, int t2)
		{
			if ((t1.Disposition.ToLower() == "inline" && t2 == 1) || (t1.Disposition.ToLower() == "attachment" && t2 == 2))
			{
				return true;
			}
			return false;
		}

		public static bool operator !=(ContentDisposition t1, int t2)
		{
			if ((t1.Disposition.ToLower() == "inline" && t2 == 1) || (t1.Disposition.ToLower() == "attachment" && t2 == 2))
			{
				return false;
			}
			return true;
		}
	}
}
