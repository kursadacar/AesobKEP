using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web.UI;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class ConditionalCollection : CollectionBase
	{
		public Condition this[int index]
		{
			get
			{
				return (Condition)base.List[index];
			}
		}

		public Condition this[string field]
		{
			get
			{
				foreach (Condition item in base.List)
				{
					if (item.Field.ToLower() == field.ToLower())
					{
						return item;
					}
				}
				return null;
			}
		}

		public void Add(Condition condition)
		{
			base.List.Add(condition);
		}

		public void Add(string regionid, string field, string aValue)
		{
			base.List.Add(new Condition(regionid, field, aValue));
		}

		public void Add(string regionid, string field, string aValue, OperatorType aOperator, bool casesensitive)
		{
			base.List.Add(new Condition(regionid, field, aValue, aOperator, casesensitive));
		}

		public void Remove(int index)
		{
			if (index < base.Count || index >= 0)
			{
				base.List.RemoveAt(index);
			}
		}

		public bool Contains(string field)
		{
			foreach (Condition item in base.List)
			{
				if (item.Field.ToLower() == field.ToLower())
				{
					return true;
				}
			}
			return false;
		}

		public bool ContainsRegion(string regionID)
		{
			foreach (Condition item in base.List)
			{
				if (item.RegionID.ToLower() == regionID.ToLower())
				{
					return true;
				}
			}
			return false;
		}

		public bool RemoveRegion(string regionID)
		{
			foreach (Condition item in base.List)
			{
				if (item.RegionID.ToLower() == regionID.ToLower() && !item.Match)
				{
					return true;
				}
			}
			return false;
		}

		public void ClearMatch()
		{
			foreach (Condition item in base.List)
			{
				item.Match = item.Operator == OperatorType.NotExists;
			}
		}

		public void Validate(string field, object aValue)
		{
			foreach (Condition item in base.List)
			{
				if (item.Field.ToLower() == field.ToLower())
				{
					item.Validate(aValue);
				}
			}
		}

		public void Validate(object dataSource)
		{
			try
			{
				string empty = string.Empty;
				IEnumerator enumerator = GetEnumerator(dataSource);
				if (enumerator == null)
				{
					return;
				}
				if (dataSource is IListSource && !(dataSource is DataRowView))
				{
					while (enumerator.MoveNext())
					{
						Validate(Convert.ToString(DataBinder.Eval(enumerator.Current, "Key")), Convert.ToString(DataBinder.Eval(enumerator.Current, "Value")));
					}
					return;
				}
				DataColumnCollection columns = GetColumns(dataSource);
				while (enumerator.MoveNext())
				{
					foreach (DataColumn item in columns)
					{
						Validate(item.ColumnName, Convert.ToString(DataBinder.GetPropertyValue(enumerator.Current, item.ColumnName)));
					}
				}
			}
			catch
			{
			}
		}

		private DataColumnCollection GetColumns(object dataSource)
		{
			if (dataSource == null)
			{
				return null;
			}
			if (dataSource is DataSet)
			{
				return ((DataSet)dataSource).Tables[0].Columns;
			}
			if (dataSource is DataTable)
			{
				return ((DataTable)dataSource).Columns;
			}
			if (dataSource is DataView)
			{
				return ((DataView)dataSource).Table.Columns;
			}
			if (dataSource is DataRowView)
			{
				return ((DataRowView)dataSource).Row.Table.Columns;
			}
			if (dataSource is DataRow)
			{
				return ((DataRow)dataSource).Table.Columns;
			}
			return null;
		}

		private IEnumerator GetEnumerator(object dataSource)
		{
			if (dataSource == null)
			{
				return null;
			}
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
