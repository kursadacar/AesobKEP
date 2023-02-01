using System;
using System.Collections;
using System.Data;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class HeaderCollection : CollectionBase
	{
		public Header this[int index]
		{
			get
			{
				return (Header)base.List[index];
			}
		}

		public void Add(Header header)
		{
			base.List.Add(header);
		}

		public DataTable GetBindableTable(string args)
		{
			DataTable dataTable = new DataTable("BindableTable");
			if (args.IndexOf("from") != -1)
			{
				dataTable.Columns.Add("From");
			}
			if (args.IndexOf("subject") != -1)
			{
				dataTable.Columns.Add("Subject");
			}
			if (args.IndexOf("sender") != -1)
			{
				dataTable.Columns.Add("Sender");
			}
			if (args.IndexOf("replyto") != -1)
			{
				dataTable.Columns.Add("ReplyTo");
			}
			if (args.IndexOf("torecipient") != -1)
			{
				dataTable.Columns.Add("To");
			}
			if (args.IndexOf("ccrecipient") != -1)
			{
				dataTable.Columns.Add("Cc");
			}
			if (args.IndexOf("date") != -1)
			{
				dataTable.Columns.Add("Date");
			}
			if (args.IndexOf("priority") != -1)
			{
				dataTable.Columns.Add("Priority");
			}
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Header header = (Header)enumerator.Current;
					string[] array = new string[dataTable.Columns.Count];
					for (int i = 0; i < dataTable.Columns.Count; i++)
					{
						switch (dataTable.Columns[i].Caption)
						{
						case "From":
							array[i] = header.From.Merged;
							break;
						case "Subject":
							array[i] = header.Subject;
							break;
						case "Sender":
							array[i] = ((header.Sender.Email != "Undefined") ? header.Sender.Merged : header.From.Merged);
							break;
						case "ReplyTo":
							array[i] = header.ReplyTo.Merged;
							break;
						case "To":
							array[i] = header.To.Merged;
							break;
						case "Cc":
							if (header.Cc != null)
							{
								array[i] = header.Cc.Merged;
							}
							break;
						case "Date":
							array[i] = ((header.Date != DateTime.MinValue) ? header.Date.ToString() : header.DateString);
							break;
						case "Priority":
							array[i] = header.Priority.ToString();
							break;
						}
					}
					DataRowCollection rows = dataTable.Rows;
					object[] values = array;
					rows.Add(values);
				}
				return dataTable;
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
