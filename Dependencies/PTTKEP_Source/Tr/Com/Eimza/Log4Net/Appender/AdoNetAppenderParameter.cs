using System;
using System.Data;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Layout;

namespace Tr.Com.Eimza.Log4Net.Appender
{
	public class AdoNetAppenderParameter
	{
		private string m_parameterName;

		private DbType m_dbType;

		private bool m_inferType = true;

		private byte m_precision;

		private byte m_scale;

		private int m_size;

		private IRawLayout m_layout;

		public string ParameterName
		{
			get
			{
				return m_parameterName;
			}
			set
			{
				m_parameterName = value;
			}
		}

		public DbType DbType
		{
			get
			{
				return m_dbType;
			}
			set
			{
				m_dbType = value;
				m_inferType = false;
			}
		}

		public byte Precision
		{
			get
			{
				return m_precision;
			}
			set
			{
				m_precision = value;
			}
		}

		public byte Scale
		{
			get
			{
				return m_scale;
			}
			set
			{
				m_scale = value;
			}
		}

		public int Size
		{
			get
			{
				return m_size;
			}
			set
			{
				m_size = value;
			}
		}

		public IRawLayout Layout
		{
			get
			{
				return m_layout;
			}
			set
			{
				m_layout = value;
			}
		}

		public AdoNetAppenderParameter()
		{
			m_precision = 0;
			m_scale = 0;
			m_size = 0;
		}

		public virtual void Prepare(IDbCommand command)
		{
			IDbDataParameter dbDataParameter = command.CreateParameter();
			dbDataParameter.ParameterName = m_parameterName;
			if (!m_inferType)
			{
				dbDataParameter.DbType = m_dbType;
			}
			if (m_precision != 0)
			{
				dbDataParameter.Precision = m_precision;
			}
			if (m_scale != 0)
			{
				dbDataParameter.Scale = m_scale;
			}
			if (m_size != 0)
			{
				dbDataParameter.Size = m_size;
			}
			command.Parameters.Add(dbDataParameter);
		}

		public virtual void FormatValue(IDbCommand command, LoggingEvent loggingEvent)
		{
			IDbDataParameter obj = (IDbDataParameter)command.Parameters[m_parameterName];
			object obj2 = Layout.Format(loggingEvent);
			if (obj2 == null)
			{
				obj2 = DBNull.Value;
			}
			obj.Value = obj2;
		}
	}
}
