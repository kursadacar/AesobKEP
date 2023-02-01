using System;
using System.Collections;
using System.Data;

namespace ActiveUp.Net.Mail
{
	[Serializable]
	public class MessageCollection : CollectionBase
	{
		public Message this[int index]
		{
			get
			{
				return (Message)base.List[index];
			}
		}

		public void Add(Message msg)
		{
			base.List.Add(msg);
		}

		public DataTable GetBindableTable(string args)
		{
			DataTable dataTable = new DataTable("BindableTable");
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
			if (args.IndexOf("attach") != -1)
			{
				dataTable.Columns.Add("Attachments");
			}
			if (args.IndexOf("date") != -1)
			{
				dataTable.Columns.Add("Date");
			}
			if (args.IndexOf("priority") != -1)
			{
				dataTable.Columns.Add("Priority");
			}
			if (args.IndexOf("size") != -1)
			{
				dataTable.Columns.Add("Size");
			}
			IEnumerator enumerator = GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Message message = (Message)enumerator.Current;
					string[] array = new string[dataTable.Columns.Count];
					for (int i = 0; i < dataTable.Columns.Count; i++)
					{
						switch (dataTable.Columns[i].Caption)
						{
						case "Subject":
							array[i] = message.Subject;
							break;
						case "Sender":
							array[i] = ((message.Sender.Email != "Undefined") ? message.Sender.Merged : message.From.Merged);
							break;
						case "ReplyTo":
							array[i] = message.ReplyTo.Merged;
							break;
						case "To":
							array[i] = message.To.Merged;
							break;
						case "Cc":
							if (message.Cc != null)
							{
								array[i] = message.Cc.Merged;
							}
							break;
						case "Attachments":
							array[i] = message.Attachments.Count.ToString();
							break;
						case "Date":
							array[i] = ((message.Date != DateTime.MinValue) ? message.Date.ToString() : message.DateString);
							break;
						case "Priority":
							array[i] = message.Priority.ToString();
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
