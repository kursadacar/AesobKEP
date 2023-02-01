using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class ListTemplate
	{
		private string _name;

		private string _content;

		private string _regionID;

		private string _nulltext;

		private int _count;

		private bool _hascount;

		private object _dataSource;

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

		public int Count
		{
			get
			{
				if (!_hascount)
				{
					if (DataSource == null)
					{
						_count = 0;
					}
					else
					{
						IEnumerator enumerator = GetEnumerator(DataSource);
						if (enumerator != null && enumerator.MoveNext())
						{
							_count = 1;
						}
						else
						{
							_count = 0;
						}
					}
					_hascount = true;
				}
				return _count;
			}
		}

		public ListTemplate()
		{
			_name = string.Empty;
			_regionID = string.Empty;
			_content = string.Empty;
			_nulltext = string.Empty;
		}

		public ListTemplate(string name, string content)
		{
			_name = name;
			_content = content;
			_regionID = string.Empty;
			_nulltext = string.Empty;
		}

		public ListTemplate(string name, string content, string regionid)
		{
			_name = name;
			_content = content;
			_regionID = regionid;
			_nulltext = string.Empty;
		}

		private IEnumerator GetEnumerator(object dataSource)
		{
			IEnumerator result = null;
			if (dataSource is IEnumerable)
			{
				result = ((IEnumerable)dataSource).GetEnumerator();
			}
			if (dataSource is IListSource)
			{
				result = ((IListSource)dataSource).GetList().GetEnumerator();
			}
			if (dataSource is DataRow)
			{
				DataView dataView = new DataView(((DataRow)dataSource).Table);
				ArrayList arrayList = new ArrayList();
				foreach (DataRowView item in dataView)
				{
					if (item.Row == dataSource)
					{
						arrayList.Add(item);
						break;
					}
				}
				result = arrayList.GetEnumerator();
			}
			return result;
		}
	}
}
