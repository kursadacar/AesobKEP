using System;
using System.Collections;
using System.IO;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.Layout.Pattern;
using Tr.Com.Eimza.Log4Net.Util;
using Tr.Com.Eimza.Log4Net.Util.PatternStringConverters;

namespace Tr.Com.Eimza.Log4Net.Layout
{
	public class PatternLayout : LayoutSkeleton
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

		public const string DefaultConversionPattern = "%message%newline";

		public const string DetailConversionPattern = "%timestamp [%thread] %level %logger %ndc - %message%newline";

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

		static PatternLayout()
		{
			s_globalRulesRegistry = new Hashtable(45);
			s_globalRulesRegistry.Add("literal", typeof(LiteralPatternConverter));
			s_globalRulesRegistry.Add("newline", typeof(NewLinePatternConverter));
			s_globalRulesRegistry.Add("n", typeof(NewLinePatternConverter));
			s_globalRulesRegistry.Add("c", typeof(LoggerPatternConverter));
			s_globalRulesRegistry.Add("logger", typeof(LoggerPatternConverter));
			s_globalRulesRegistry.Add("C", typeof(TypeNamePatternConverter));
			s_globalRulesRegistry.Add("class", typeof(TypeNamePatternConverter));
			s_globalRulesRegistry.Add("type", typeof(TypeNamePatternConverter));
			s_globalRulesRegistry.Add("d", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.DatePatternConverter));
			s_globalRulesRegistry.Add("date", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.DatePatternConverter));
			s_globalRulesRegistry.Add("exception", typeof(ExceptionPatternConverter));
			s_globalRulesRegistry.Add("F", typeof(FileLocationPatternConverter));
			s_globalRulesRegistry.Add("file", typeof(FileLocationPatternConverter));
			s_globalRulesRegistry.Add("l", typeof(FullLocationPatternConverter));
			s_globalRulesRegistry.Add("location", typeof(FullLocationPatternConverter));
			s_globalRulesRegistry.Add("L", typeof(LineLocationPatternConverter));
			s_globalRulesRegistry.Add("line", typeof(LineLocationPatternConverter));
			s_globalRulesRegistry.Add("m", typeof(MessagePatternConverter));
			s_globalRulesRegistry.Add("message", typeof(MessagePatternConverter));
			s_globalRulesRegistry.Add("M", typeof(MethodLocationPatternConverter));
			s_globalRulesRegistry.Add("method", typeof(MethodLocationPatternConverter));
			s_globalRulesRegistry.Add("p", typeof(LevelPatternConverter));
			s_globalRulesRegistry.Add("level", typeof(LevelPatternConverter));
			s_globalRulesRegistry.Add("P", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.PropertyPatternConverter));
			s_globalRulesRegistry.Add("property", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.PropertyPatternConverter));
			s_globalRulesRegistry.Add("properties", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.PropertyPatternConverter));
			s_globalRulesRegistry.Add("r", typeof(RelativeTimePatternConverter));
			s_globalRulesRegistry.Add("timestamp", typeof(RelativeTimePatternConverter));
			s_globalRulesRegistry.Add("t", typeof(ThreadPatternConverter));
			s_globalRulesRegistry.Add("thread", typeof(ThreadPatternConverter));
			s_globalRulesRegistry.Add("x", typeof(NdcPatternConverter));
			s_globalRulesRegistry.Add("ndc", typeof(NdcPatternConverter));
			s_globalRulesRegistry.Add("X", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.PropertyPatternConverter));
			s_globalRulesRegistry.Add("mdc", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.PropertyPatternConverter));
			s_globalRulesRegistry.Add("a", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.AppDomainPatternConverter));
			s_globalRulesRegistry.Add("appdomain", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.AppDomainPatternConverter));
			s_globalRulesRegistry.Add("u", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.IdentityPatternConverter));
			s_globalRulesRegistry.Add("identity", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.IdentityPatternConverter));
			s_globalRulesRegistry.Add("utcdate", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.UtcDatePatternConverter));
			s_globalRulesRegistry.Add("utcDate", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.UtcDatePatternConverter));
			s_globalRulesRegistry.Add("UtcDate", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.UtcDatePatternConverter));
			s_globalRulesRegistry.Add("w", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.UserNamePatternConverter));
			s_globalRulesRegistry.Add("username", typeof(Tr.Com.Eimza.Log4Net.Layout.Pattern.UserNamePatternConverter));
		}

		public PatternLayout()
			: this("%message%newline")
		{
		}

		public PatternLayout(string pattern)
		{
			IgnoresException = true;
			m_pattern = pattern;
			if (m_pattern == null)
			{
				m_pattern = "%message%newline";
			}
			ActivateOptions();
		}

		protected virtual PatternParser CreatePatternParser(string pattern)
		{
			PatternParser patternParser = new PatternParser(pattern);
			foreach (DictionaryEntry item in s_globalRulesRegistry)
			{
				patternParser.PatternConverters[item.Key] = item.Value;
			}
			foreach (DictionaryEntry item2 in m_instanceRulesRegistry)
			{
				patternParser.PatternConverters[item2.Key] = item2.Value;
			}
			return patternParser;
		}

		public override void ActivateOptions()
		{
			m_head = CreatePatternParser(m_pattern).Parse();
			for (PatternConverter patternConverter = m_head; patternConverter != null; patternConverter = patternConverter.Next)
			{
				PatternLayoutConverter patternLayoutConverter = patternConverter as PatternLayoutConverter;
				if (patternLayoutConverter != null && !patternLayoutConverter.IgnoresException)
				{
					IgnoresException = false;
					break;
				}
			}
		}

		public override void Format(TextWriter writer, LoggingEvent loggingEvent)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			if (loggingEvent == null)
			{
				throw new ArgumentNullException("loggingEvent");
			}
			for (PatternConverter patternConverter = m_head; patternConverter != null; patternConverter = patternConverter.Next)
			{
				patternConverter.Format(writer, loggingEvent);
			}
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
