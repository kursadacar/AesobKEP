using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class AdoNetAppender : BufferingAppenderSkeleton
	{
		protected bool m_usePreparedCommand;

		protected ArrayList m_parameters;

		private SecurityContext m_securityContext;

		private IDbConnection m_dbConnection;

		private IDbCommand m_dbCommand;

		private string m_connectionString;

		private string m_connectionType;

		private string m_commandText;

		private CommandType m_commandType;

		private bool m_useTransactions;

		private bool m_reconnectOnError;

		public string ConnectionString
		{
			get
			{
				return m_connectionString;
			}
			set
			{
				m_connectionString = value;
			}
		}

		public string ConnectionType
		{
			get
			{
				return m_connectionType;
			}
			set
			{
				m_connectionType = value;
			}
		}

		public string CommandText
		{
			get
			{
				return m_commandText;
			}
			set
			{
				m_commandText = value;
			}
		}

		public CommandType CommandType
		{
			get
			{
				return m_commandType;
			}
			set
			{
				m_commandType = value;
			}
		}

		public bool UseTransactions
		{
			get
			{
				return m_useTransactions;
			}
			set
			{
				m_useTransactions = value;
			}
		}

		public SecurityContext SecurityContext
		{
			get
			{
				return m_securityContext;
			}
			set
			{
				m_securityContext = value;
			}
		}

		public bool ReconnectOnError
		{
			get
			{
				return m_reconnectOnError;
			}
			set
			{
				m_reconnectOnError = value;
			}
		}

		protected IDbConnection Connection
		{
			get
			{
				return m_dbConnection;
			}
			set
			{
				m_dbConnection = value;
			}
		}

		public AdoNetAppender()
		{
			m_connectionType = "System.Data.OleDb.OleDbConnection, System.Data, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
			m_useTransactions = true;
			m_commandType = CommandType.Text;
			m_parameters = new ArrayList();
			m_reconnectOnError = false;
		}

		public override void ActivateOptions()
		{
			base.ActivateOptions();
			m_usePreparedCommand = m_commandText != null && m_commandText.Length > 0;
			if (m_securityContext == null)
			{
				m_securityContext = SecurityContextProvider.DefaultProvider.CreateSecurityContext(this);
			}
			InitializeDatabaseConnection();
			InitializeDatabaseCommand();
		}

		protected override void OnClose()
		{
			base.OnClose();
			if (m_dbCommand != null)
			{
				try
				{
					m_dbCommand.Dispose();
				}
				catch (Exception exception)
				{
					LogLog.Warn("AdoNetAppender: Exception while disposing cached command object", exception);
				}
				m_dbCommand = null;
			}
			if (m_dbConnection != null)
			{
				try
				{
					m_dbConnection.Close();
				}
				catch (Exception exception2)
				{
					LogLog.Warn("AdoNetAppender: Exception while disposing cached connection object", exception2);
				}
				m_dbConnection = null;
			}
		}

		protected override void SendBuffer(LoggingEvent[] events)
		{
			if (m_reconnectOnError && (m_dbConnection == null || m_dbConnection.State != ConnectionState.Open))
			{
				LogLog.Debug("AdoNetAppender: Attempting to reconnect to database. Current Connection State: " + ((m_dbConnection == null) ? "<null>" : m_dbConnection.State.ToString()));
				InitializeDatabaseConnection();
				InitializeDatabaseCommand();
			}
			if (m_dbConnection == null || m_dbConnection.State != ConnectionState.Open)
			{
				return;
			}
			if (m_useTransactions)
			{
				IDbTransaction dbTransaction = null;
				try
				{
					dbTransaction = m_dbConnection.BeginTransaction();
					SendBuffer(dbTransaction, events);
					dbTransaction.Commit();
					return;
				}
				catch (Exception e)
				{
					if (dbTransaction != null)
					{
						try
						{
							dbTransaction.Rollback();
						}
						catch (Exception)
						{
						}
					}
					ErrorHandler.Error("Exception while writing to database", e);
					return;
				}
			}
			SendBuffer(null, events);
		}

		public void AddParameter(AdoNetAppenderParameter parameter)
		{
			m_parameters.Add(parameter);
		}

		protected virtual void SendBuffer(IDbTransaction dbTran, LoggingEvent[] events)
		{
			if (m_usePreparedCommand)
			{
				if (m_dbCommand == null)
				{
					return;
				}
				if (dbTran != null)
				{
					m_dbCommand.Transaction = dbTran;
				}
				LoggingEvent[] array = events;
				foreach (LoggingEvent loggingEvent in array)
				{
					foreach (AdoNetAppenderParameter parameter in m_parameters)
					{
						parameter.FormatValue(m_dbCommand, loggingEvent);
					}
					m_dbCommand.ExecuteNonQuery();
				}
				return;
			}
			using (IDbCommand dbCommand = m_dbConnection.CreateCommand())
			{
				if (dbTran != null)
				{
					dbCommand.Transaction = dbTran;
				}
				LoggingEvent[] array = events;
				foreach (LoggingEvent logEvent in array)
				{
					string logStatement = GetLogStatement(logEvent);
					LogLog.Debug("AdoNetAppender: LogStatement [" + logStatement + "]");
					dbCommand.CommandText = logStatement;
					dbCommand.ExecuteNonQuery();
				}
			}
		}

		protected virtual string GetLogStatement(LoggingEvent logEvent)
		{
			if (Layout == null)
			{
				ErrorHandler.Error("ADOAppender: No Layout specified.");
				return "";
			}
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			Layout.Format(stringWriter, logEvent);
			return stringWriter.ToString();
		}

		private void InitializeDatabaseConnection()
		{
			try
			{
				if (m_dbCommand != null)
				{
					try
					{
						m_dbCommand.Dispose();
					}
					catch (Exception exception)
					{
						LogLog.Warn("AdoNetAppender: Exception while disposing cached command object", exception);
					}
					m_dbCommand = null;
				}
				if (m_dbConnection != null)
				{
					try
					{
						m_dbConnection.Close();
					}
					catch (Exception exception2)
					{
						LogLog.Warn("AdoNetAppender: Exception while disposing cached connection object", exception2);
					}
					m_dbConnection = null;
				}
				m_dbConnection = (IDbConnection)Activator.CreateInstance(ResolveConnectionType());
				m_dbConnection.ConnectionString = m_connectionString;
				using (SecurityContext.Impersonate(this))
				{
					m_dbConnection.Open();
				}
			}
			catch (Exception e)
			{
				ErrorHandler.Error("Could not open database connection [" + m_connectionString + "]", e);
				m_dbConnection = null;
			}
		}

		protected virtual Type ResolveConnectionType()
		{
			try
			{
				return SystemInfo.GetTypeFromString(m_connectionType, true, false);
			}
			catch (Exception e)
			{
				ErrorHandler.Error("Failed to load connection type [" + m_connectionType + "]", e);
				throw;
			}
		}

		private void InitializeDatabaseCommand()
		{
			if (m_dbConnection == null || !m_usePreparedCommand)
			{
				return;
			}
			try
			{
				if (m_dbCommand != null)
				{
					try
					{
						m_dbCommand.Dispose();
					}
					catch (Exception exception)
					{
						LogLog.Warn("AdoNetAppender: Exception while disposing cached command object", exception);
					}
					m_dbCommand = null;
				}
				m_dbCommand = m_dbConnection.CreateCommand();
				m_dbCommand.CommandText = m_commandText;
				m_dbCommand.CommandType = m_commandType;
			}
			catch (Exception e)
			{
				ErrorHandler.Error("Could not create database command [" + m_commandText + "]", e);
				if (m_dbCommand != null)
				{
					try
					{
						m_dbCommand.Dispose();
					}
					catch
					{
					}
					m_dbCommand = null;
				}
			}
			if (m_dbCommand != null)
			{
				try
				{
					foreach (AdoNetAppenderParameter parameter in m_parameters)
					{
						try
						{
							parameter.Prepare(m_dbCommand);
						}
						catch (Exception e2)
						{
							ErrorHandler.Error("Could not add database command parameter [" + parameter.ParameterName + "]", e2);
							throw;
						}
					}
				}
				catch
				{
					try
					{
						m_dbCommand.Dispose();
					}
					catch
					{
					}
					m_dbCommand = null;
				}
			}
			if (m_dbCommand == null)
			{
				return;
			}
			try
			{
				m_dbCommand.Prepare();
			}
			catch (Exception e3)
			{
				ErrorHandler.Error("Could not prepare database command [" + m_commandText + "]", e3);
				try
				{
					m_dbCommand.Dispose();
				}
				catch
				{
				}
				m_dbCommand = null;
			}
		}
	}
}
