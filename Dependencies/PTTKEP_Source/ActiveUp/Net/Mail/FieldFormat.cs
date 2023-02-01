using System;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class FieldFormat
	{
		private string _name;

		private string _format;

		private PaddingDirection _paddingDir;

		private char _paddingChar;

		private int _totalWidth;

		public char PaddingChar
		{
			get
			{
				return _paddingChar;
			}
			set
			{
				_paddingChar = value;
			}
		}

		public int TotalWidth
		{
			get
			{
				return _totalWidth;
			}
			set
			{
				_totalWidth = value;
			}
		}

		public PaddingDirection PaddingDir
		{
			get
			{
				return _paddingDir;
			}
			set
			{
				_paddingDir = value;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public string Format
		{
			get
			{
				return _format;
			}
			set
			{
				_format = value;
			}
		}

		public FieldFormat()
		{
			Name = string.Empty;
			Format = string.Empty;
			PaddingDir = PaddingDirection.Left;
			TotalWidth = 0;
			PaddingChar = ' ';
		}

		public FieldFormat(string name, string format)
		{
			Name = name;
			Format = format;
			PaddingDir = PaddingDirection.Left;
			TotalWidth = 0;
			PaddingChar = ' ';
		}

		public FieldFormat(string name, string format, PaddingDirection paddingDir, int totalWidth, char paddingChar)
		{
			Name = name;
			Format = format;
			PaddingDir = paddingDir;
			TotalWidth = totalWidth;
			PaddingChar = paddingChar;
		}
	}
}
