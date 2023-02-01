using System;
using System.Data;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class BodyTemplate
	{
		private string _content;

		private BodyFormat _format;

		private object _dataSource;

		public object DataSource
		{
			get
			{
				return _dataSource;
			}
			set
			{
				if (value.GetType().ToString() == "System.Data.DataSet")
				{
					_dataSource = ((DataSet)value).Tables[0];
				}
				else if (value.GetType().ToString() == "System.Data.DataRow")
				{
					_dataSource = ((DataRow)value).Table.Clone();
					((DataTable)_dataSource).Rows.Add(((DataRow)value).ItemArray);
				}
				else
				{
					_dataSource = value;
				}
			}
		}

		public BodyFormat Format
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

		public string Content
		{
			get
			{
				return _content;
			}
			set
			{
				_content = value;
			}
		}

		public BodyTemplate()
		{
			_content = string.Empty;
			_format = BodyFormat.Text;
		}

		public BodyTemplate(string content)
		{
			_content = content;
			_format = BodyFormat.Text;
		}

		public BodyTemplate(string content, BodyFormat format)
		{
			_content = content;
			_format = format;
		}
	}
}
