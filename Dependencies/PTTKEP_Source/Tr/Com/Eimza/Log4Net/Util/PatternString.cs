using System;
using System.Collections;
using System.Globalization;
using System.IO;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Util.PatternStringConverters;

namespace Tr.Com.Eimza.Log4Net.Util
{
	public class PatternString : IOptionHandler
	{
		public sealed class ConverterInfo
		{
			private string m_name;

			private Type m_type;

			public string Name
			{
				get
				{
					return m_name;
				}
				set
				{
					m_name = value;
				}
			}

			public Type Type
			{
				get
				{
					return m_type;
				}
				set
				{
					m_type = value;
				}
			}
		}

		private static Hashtable s_globalRulesRegistry;

		private string m_pattern;

		private PatternConverter m_head;

		private Hashtable m_instanceRulesRegistry = new Hashtable();

		public string ConversionPattern
		{
			get
			{
				return m_pattern;
			}
			set
			{
				m_pattern = value;
			}
		}

		static PatternString()
		{
			s_globalRulesRegistry = new Hashtable(15);
			s_globalRulesRegistry.Add("appdomain", typeof(AppDomainPatternConverter));
			s_globalRulesRegistry.Add("date", typeof(DatePatternConverter));
			s_globalRulesRegistry.Add("env", typeof(EnvironmentPatternConverter));
			s_globalRulesRegistry.Add("identity", typeof(IdentityPatternConverter));
			s_globalRulesRegistry.Add("literal", typeof(LiteralPatternConverter));
			s_globalRulesRegistry.Add("newline", typeof(NewLinePatternConverter));
			s_globalRulesRegistry.Add("processid", typeof(ProcessIdPatternConverter));
			s_globalRulesRegistry.Add("property", typeof(PropertyPatternConverter));
			s_globalRulesRegistry.Add("random", typeof(RandomStringPatternConverter));
			s_globalRulesRegistry.Add("username", typeof(UserNamePatternConverter));
			s_globalRulesRegistry.Add("utcdate", typeof(UtcDatePatternConverter));
			s_globalRulesRegistry.Add("utcDate", typeof(UtcDatePatternConverter));
			s_globalRulesRegistry.Add("UtcDate", typeof(UtcDatePatternConverter));
		}

		public PatternString()
		{
		}

		public PatternString(string pattern)
		{
			m_pattern = pattern;
			ActivateOptions();
		}

		public virtual void ActivateOptions()
		{
			m_head = CreatePatternParser(m_pattern).Parse();
		}

		private PatternParser CreatePatternParser(string pattern)
		{
			PatternParser patternParser = new PatternParser(pattern);
			foreach (DictionaryEntry item in s_globalRulesRegistry)
			{
				patternParser.PatternConverters.Add(item.Key, item.Value);
			}
			foreach (DictionaryEntry item2 in m_instanceRulesRegistry)
			{
				patternParser.PatternConverters[item2.Key] = item2.Value;
			}
			return patternParser;
		}

		public void Format(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			for (PatternConverter patternConverter = m_head; patternConverter != null; patternConverter = patternConverter.Next)
			{
				patternConverter.Format(writer, null);
			}
		}

		public string Format()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			Format(stringWriter);
			return stringWriter.ToString();
		}

		public void AddConverter(ConverterInfo converterInfo)
		{
			AddConverter(converterInfo.Name, converterInfo.Type);
		}

		public void AddConverter(string name, Type type)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if ((object)type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!typeof(PatternConverter).IsAssignableFrom(type))
			{
				throw new ArgumentException("The converter type specified [" + (((object)type != null) ? type.ToString() : null) + "] must be a subclass of log4net.Util.PatternConverter", "type");
			}
			m_instanceRulesRegistry[name] = type;
		}
	}
}
