using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class Region
	{
		private string _regionID;

		private string _url;

		private string _content;

		private string _nulltext;

		private bool _Loaded;

		public string NullText
		{
			get
			{
				return _nulltext;
			}
			set
			{
				_nulltext = value;
			}
		}

		public string Content
		{
			get
			{
				if (!_Loaded)
				{
					if (_url.Length > 0)
					{
						_content = LoadFileContent(_url);
						if (_content.Length == 0)
						{
							_content = _nulltext;
						}
						_Loaded = true;
					}
					else
					{
						_content = _nulltext;
					}
				}
				return _content;
			}
		}

		public string RegionID
		{
			get
			{
				return _regionID;
			}
			set
			{
				_regionID = value;
			}
		}

		public string URL
		{
			get
			{
				return _url;
			}
			set
			{
				_url = value;
			}
		}

		public Region()
		{
			RegionID = string.Empty;
			URL = string.Empty;
			NullText = string.Empty;
		}

		public Region(string regionid, string url)
		{
			RegionID = regionid;
			URL = url;
			NullText = string.Empty;
		}

		public string LoadFileContent(string filename)
		{
			string empty = string.Empty;
			if (filename.ToUpper().StartsWith("HTTP://") || filename.ToUpper().StartsWith("HTTPS://"))
			{
				WebRequest webRequest = WebRequest.Create(filename);
				try
				{
					empty = new StreamReader(webRequest.GetResponse().GetResponseStream()).ReadToEnd();
					MatchCollection matchCollection = new Regex("<body.*?>(.*?)</body>", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline).Matches(empty);
					if (matchCollection.Count > 0)
					{
						foreach (Match item in matchCollection)
						{
							empty = item.Groups[1].Captures[0].Value;
						}
						return empty;
					}
					return empty;
				}
				catch
				{
					return "";
				}
			}
			if (filename.ToUpper().StartsWith("FILE://"))
			{
				filename = filename.Substring(7);
			}
			if (File.Exists(filename))
			{
				TextReader textReader = TextReader.Synchronized(new StreamReader(filename, Encoding.Default));
				empty = textReader.ReadToEnd();
				textReader.Close();
			}
			else
			{
				empty = "";
			}
			return empty;
		}
	}
}
