using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Xml;
using Tr.Com.Eimza.Log4Net.Appender;
using Tr.Com.Eimza.Log4Net.Core;
using Tr.Com.Eimza.Log4Net.ObjectRenderer;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.Repository.Hierarchy
{
	public class XmlHierarchyConfigurator
	{
		private enum ConfigUpdateMode
		{
			Merge,
			Overwrite
		}

		private const string CONFIGURATION_TAG = "log4net";

		private const string RENDERER_TAG = "renderer";

		private const string APPENDER_TAG = "appender";

		private const string APPENDER_REF_TAG = "appender-ref";

		private const string PARAM_TAG = "param";

		private const string CATEGORY_TAG = "category";

		private const string PRIORITY_TAG = "priority";

		private const string LOGGER_TAG = "logger";

		private const string NAME_ATTR = "name";

		private const string TYPE_ATTR = "type";

		private const string VALUE_ATTR = "value";

		private const string ROOT_TAG = "root";

		private const string LEVEL_TAG = "level";

		private const string REF_ATTR = "ref";

		private const string ADDITIVITY_ATTR = "additivity";

		private const string THRESHOLD_ATTR = "threshold";

		private const string CONFIG_DEBUG_ATTR = "configDebug";

		private const string INTERNAL_DEBUG_ATTR = "debug";

		private const string CONFIG_UPDATE_MODE_ATTR = "update";

		private const string RENDERING_TYPE_ATTR = "renderingClass";

		private const string RENDERED_TYPE_ATTR = "renderedClass";

		private const string INHERITED = "inherited";

		private Hashtable m_appenderBag;

		private readonly Hierarchy m_hierarchy;

		public XmlHierarchyConfigurator(Hierarchy hierarchy)
		{
			m_hierarchy = hierarchy;
			m_appenderBag = new Hashtable();
		}

		public void Configure(XmlElement element)
		{
			if (element == null || m_hierarchy == null)
			{
				return;
			}
			if (element.LocalName != "log4net")
			{
				LogLog.Error("XmlHierarchyConfigurator: Xml element is - not a <log4net> element.");
				return;
			}
			if (!LogLog.InternalDebugging)
			{
				string attribute = element.GetAttribute("debug");
				LogLog.Debug("XmlHierarchyConfigurator: debug attribute [" + attribute + "].");
				if (attribute.Length > 0 && attribute != "null")
				{
					LogLog.InternalDebugging = OptionConverter.ToBoolean(attribute, true);
				}
				else
				{
					LogLog.Debug("XmlHierarchyConfigurator: Ignoring debug attribute.");
				}
				string attribute2 = element.GetAttribute("configDebug");
				if (attribute2.Length > 0 && attribute2 != "null")
				{
					LogLog.Warn("XmlHierarchyConfigurator: The \"configDebug\" attribute is deprecated.");
					LogLog.Warn("XmlHierarchyConfigurator: Use the \"debug\" attribute instead.");
					LogLog.InternalDebugging = OptionConverter.ToBoolean(attribute2, true);
				}
			}
			ConfigUpdateMode configUpdateMode = ConfigUpdateMode.Merge;
			string attribute3 = element.GetAttribute("update");
			if (attribute3 != null && attribute3.Length > 0)
			{
				try
				{
					configUpdateMode = (ConfigUpdateMode)OptionConverter.ConvertStringTo(typeof(ConfigUpdateMode), attribute3);
				}
				catch
				{
					LogLog.Error("XmlHierarchyConfigurator: Invalid update attribute value [" + attribute3 + "]");
				}
			}
			LogLog.Debug("XmlHierarchyConfigurator: Configuration update mode [" + configUpdateMode.ToString() + "].");
			if (configUpdateMode == ConfigUpdateMode.Overwrite)
			{
				m_hierarchy.ResetConfiguration();
				LogLog.Debug("XmlHierarchyConfigurator: Configuration reset before reading config.");
			}
			foreach (XmlNode childNode in element.ChildNodes)
			{
				if (childNode.NodeType == XmlNodeType.Element)
				{
					XmlElement xmlElement = (XmlElement)childNode;
					if (xmlElement.LocalName == "logger")
					{
						ParseLogger(xmlElement);
					}
					else if (xmlElement.LocalName == "category")
					{
						ParseLogger(xmlElement);
					}
					else if (xmlElement.LocalName == "root")
					{
						ParseRoot(xmlElement);
					}
					else if (xmlElement.LocalName == "renderer")
					{
						ParseRenderer(xmlElement);
					}
					else if (!(xmlElement.LocalName == "appender"))
					{
						SetParameter(xmlElement, m_hierarchy);
					}
				}
			}
			string attribute4 = element.GetAttribute("threshold");
			LogLog.Debug("XmlHierarchyConfigurator: Hierarchy Threshold [" + attribute4 + "]");
			if (attribute4.Length > 0 && attribute4 != "null")
			{
				Level level = (Level)ConvertStringTo(typeof(Level), attribute4);
				if (level != null)
				{
					m_hierarchy.Threshold = level;
				}
				else
				{
					LogLog.Warn("XmlHierarchyConfigurator: Unable to set hierarchy threshold using value [" + attribute4 + "] (with acceptable conversion types)");
				}
			}
		}

		protected IAppender FindAppenderByReference(XmlElement appenderRef)
		{
			string attribute = appenderRef.GetAttribute("ref");
			IAppender appender = (IAppender)m_appenderBag[attribute];
			if (appender != null)
			{
				return appender;
			}
			XmlElement xmlElement = null;
			if (attribute != null && attribute.Length > 0)
			{
				foreach (XmlElement item in appenderRef.OwnerDocument.GetElementsByTagName("appender"))
				{
					if (item.GetAttribute("name") == attribute)
					{
						xmlElement = item;
						break;
					}
				}
			}
			if (xmlElement == null)
			{
				LogLog.Error("XmlHierarchyConfigurator: No appender named [" + attribute + "] could be found.");
				return null;
			}
			appender = ParseAppender(xmlElement);
			if (appender != null)
			{
				m_appenderBag[attribute] = appender;
			}
			return appender;
		}

		protected IAppender ParseAppender(XmlElement appenderElement)
		{
			string attribute = appenderElement.GetAttribute("name");
			string attribute2 = appenderElement.GetAttribute("type");
			LogLog.Debug("XmlHierarchyConfigurator: Loading Appender [" + attribute + "] type: [" + attribute2 + "]");
			try
			{
				IAppender appender = (IAppender)Activator.CreateInstance(SystemInfo.GetTypeFromString(attribute2, true, true));
				appender.Name = attribute;
				foreach (XmlNode childNode in appenderElement.ChildNodes)
				{
					if (childNode.NodeType != XmlNodeType.Element)
					{
						continue;
					}
					XmlElement xmlElement = (XmlElement)childNode;
					if (xmlElement.LocalName == "appender-ref")
					{
						string attribute3 = xmlElement.GetAttribute("ref");
						IAppenderAttachable appenderAttachable = appender as IAppenderAttachable;
						if (appenderAttachable != null)
						{
							LogLog.Debug("XmlHierarchyConfigurator: Attaching appender named [" + attribute3 + "] to appender named [" + appender.Name + "].");
							IAppender appender2 = FindAppenderByReference(xmlElement);
							if (appender2 != null)
							{
								appenderAttachable.AddAppender(appender2);
							}
						}
						else
						{
							LogLog.Error("XmlHierarchyConfigurator: Requesting attachment of appender named [" + attribute3 + "] to appender named [" + appender.Name + "] which does not implement log4net.Core.IAppenderAttachable.");
						}
					}
					else
					{
						SetParameter(xmlElement, appender);
					}
				}
				IOptionHandler optionHandler = appender as IOptionHandler;
				if (optionHandler != null)
				{
					optionHandler.ActivateOptions();
				}
				LogLog.Debug("XmlHierarchyConfigurator: Created Appender [" + attribute + "]");
				return appender;
			}
			catch (Exception exception)
			{
				LogLog.Error("XmlHierarchyConfigurator: Could not create Appender [" + attribute + "] of type [" + attribute2 + "]. Reported error follows.", exception);
				return null;
			}
		}

		protected void ParseLogger(XmlElement loggerElement)
		{
			string attribute = loggerElement.GetAttribute("name");
			LogLog.Debug("XmlHierarchyConfigurator: Retrieving an instance of log4net.Repository.Logger for logger [" + attribute + "].");
			Logger logger = m_hierarchy.GetLogger(attribute) as Logger;
			lock (logger)
			{
				bool additivity = OptionConverter.ToBoolean(loggerElement.GetAttribute("additivity"), true);
				LogLog.Debug("XmlHierarchyConfigurator: Setting [" + logger.Name + "] additivity to [" + additivity + "].");
				logger.Additivity = additivity;
				ParseChildrenOfLoggerElement(loggerElement, logger, false);
			}
		}

		protected void ParseRoot(XmlElement rootElement)
		{
			Logger root = m_hierarchy.Root;
			lock (root)
			{
				ParseChildrenOfLoggerElement(rootElement, root, true);
			}
		}

		protected void ParseChildrenOfLoggerElement(XmlElement catElement, Logger log, bool isRoot)
		{
			log.RemoveAllAppenders();
			foreach (XmlNode childNode in catElement.ChildNodes)
			{
				if (childNode.NodeType != XmlNodeType.Element)
				{
					continue;
				}
				XmlElement xmlElement = (XmlElement)childNode;
				if (xmlElement.LocalName == "appender-ref")
				{
					IAppender appender = FindAppenderByReference(xmlElement);
					string attribute = xmlElement.GetAttribute("ref");
					if (appender != null)
					{
						LogLog.Debug("XmlHierarchyConfigurator: Adding appender named [" + attribute + "] to logger [" + log.Name + "].");
						log.AddAppender(appender);
					}
					else
					{
						LogLog.Error("XmlHierarchyConfigurator: Appender named [" + attribute + "] not found.");
					}
				}
				else if (xmlElement.LocalName == "level" || xmlElement.LocalName == "priority")
				{
					ParseLevel(xmlElement, log, isRoot);
				}
				else
				{
					SetParameter(xmlElement, log);
				}
			}
			IOptionHandler optionHandler = log as IOptionHandler;
			if (optionHandler != null)
			{
				optionHandler.ActivateOptions();
			}
		}

		protected void ParseRenderer(XmlElement element)
		{
			string attribute = element.GetAttribute("renderingClass");
			string attribute2 = element.GetAttribute("renderedClass");
			LogLog.Debug("XmlHierarchyConfigurator: Rendering class [" + attribute + "], Rendered class [" + attribute2 + "].");
			IObjectRenderer objectRenderer = (IObjectRenderer)OptionConverter.InstantiateByClassName(attribute, typeof(IObjectRenderer), null);
			if (objectRenderer == null)
			{
				LogLog.Error("XmlHierarchyConfigurator: Could not instantiate renderer [" + attribute + "].");
				return;
			}
			try
			{
				m_hierarchy.RendererMap.Put(SystemInfo.GetTypeFromString(attribute2, true, true), objectRenderer);
			}
			catch (Exception exception)
			{
				LogLog.Error("XmlHierarchyConfigurator: Could not find class [" + attribute2 + "].", exception);
			}
		}

		protected void ParseLevel(XmlElement element, Logger log, bool isRoot)
		{
			string text = log.Name;
			if (isRoot)
			{
				text = "root";
			}
			string attribute = element.GetAttribute("value");
			LogLog.Debug("XmlHierarchyConfigurator: Logger [" + text + "] Level string is [" + attribute + "].");
			if ("inherited" == attribute)
			{
				if (isRoot)
				{
					LogLog.Error("XmlHierarchyConfigurator: Root level cannot be inherited. Ignoring directive.");
					return;
				}
				LogLog.Debug("XmlHierarchyConfigurator: Logger [" + text + "] level set to inherit from parent.");
				log.Level = null;
				return;
			}
			log.Level = log.Hierarchy.LevelMap[attribute];
			if (log.Level == null)
			{
				LogLog.Error("XmlHierarchyConfigurator: Undefined level [" + attribute + "] on Logger [" + text + "].");
			}
			else
			{
				LogLog.Debug("XmlHierarchyConfigurator: Logger [" + text + "] level set to [name=\"" + log.Level.Name + "\",value=" + log.Level.Value + "].");
			}
		}

		protected void SetParameter(XmlElement element, object target)
		{
			string text = element.GetAttribute("name");
			if (element.LocalName != "param" || text == null || text.Length == 0)
			{
				text = element.LocalName;
			}
			Type type = target.GetType();
			Type type2 = null;
			PropertyInfo propertyInfo = null;
			MethodInfo methodInfo = null;
			propertyInfo = type.GetProperty(text, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if ((object)propertyInfo != null && propertyInfo.CanWrite)
			{
				type2 = propertyInfo.PropertyType;
			}
			else
			{
				propertyInfo = null;
				methodInfo = FindMethodInfo(type, text);
				if ((object)methodInfo != null)
				{
					type2 = methodInfo.GetParameters()[0].ParameterType;
				}
			}
			if ((object)type2 == null)
			{
				LogLog.Error("XmlHierarchyConfigurator: Cannot find Property [" + text + "] to set object on [" + target.ToString() + "]");
				return;
			}
			string text2 = null;
			if (element.GetAttributeNode("value") != null)
			{
				text2 = element.GetAttribute("value");
			}
			else if (element.HasChildNodes)
			{
				foreach (XmlNode childNode in element.ChildNodes)
				{
					if (childNode.NodeType == XmlNodeType.CDATA || childNode.NodeType == XmlNodeType.Text)
					{
						text2 = ((text2 != null) ? (text2 + childNode.InnerText) : childNode.InnerText);
					}
				}
			}
			if (text2 != null)
			{
				try
				{
					text2 = OptionConverter.SubstituteVariables(text2, Environment.GetEnvironmentVariables());
				}
				catch (SecurityException)
				{
					LogLog.Debug("XmlHierarchyConfigurator: Security exception while trying to expand environment variables. Error Ignored. No Expansion.");
				}
				Type type3 = null;
				string attribute = element.GetAttribute("type");
				if (attribute != null && attribute.Length > 0)
				{
					try
					{
						Type typeFromString = SystemInfo.GetTypeFromString(attribute, true, true);
						LogLog.Debug("XmlHierarchyConfigurator: Parameter [" + text + "] specified subtype [" + typeFromString.FullName + "]");
						if (!type2.IsAssignableFrom(typeFromString))
						{
							if (OptionConverter.CanConvertTypeTo(typeFromString, type2))
							{
								type3 = type2;
								type2 = typeFromString;
							}
							else
							{
								LogLog.Error("XmlHierarchyConfigurator: Subtype [" + typeFromString.FullName + "] set on [" + text + "] is not a subclass of property type [" + type2.FullName + "] and there are no acceptable type conversions.");
							}
						}
						else
						{
							type2 = typeFromString;
						}
					}
					catch (Exception exception)
					{
						LogLog.Error("XmlHierarchyConfigurator: Failed to find type [" + attribute + "] set on [" + text + "]", exception);
					}
				}
				object obj = ConvertStringTo(type2, text2);
				if (obj != null && (object)type3 != null)
				{
					LogLog.Debug("XmlHierarchyConfigurator: Performing additional conversion of value from [" + obj.GetType().Name + "] to [" + type3.Name + "]");
					obj = OptionConverter.ConvertTypeTo(obj, type3);
				}
				if (obj != null)
				{
					if ((object)propertyInfo != null)
					{
						LogLog.Debug("XmlHierarchyConfigurator: Setting Property [" + propertyInfo.Name + "] to " + obj.GetType().Name + " value [" + obj.ToString() + "]");
						try
						{
							propertyInfo.SetValue(target, obj, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
							return;
						}
						catch (TargetInvocationException ex2)
						{
							LogLog.Error("XmlHierarchyConfigurator: Failed to set parameter [" + propertyInfo.Name + "] on object [" + ((target != null) ? target.ToString() : null) + "] using value [" + ((obj != null) ? obj.ToString() : null) + "]", ex2.InnerException);
							return;
						}
					}
					if ((object)methodInfo != null)
					{
						LogLog.Debug("XmlHierarchyConfigurator: Setting Collection Property [" + methodInfo.Name + "] to " + obj.GetType().Name + " value [" + obj.ToString() + "]");
						try
						{
							methodInfo.Invoke(target, BindingFlags.InvokeMethod, null, new object[1] { obj }, CultureInfo.InvariantCulture);
							return;
						}
						catch (TargetInvocationException ex3)
						{
							LogLog.Error("XmlHierarchyConfigurator: Failed to set parameter [" + text + "] on object [" + ((target != null) ? target.ToString() : null) + "] using value [" + ((obj != null) ? obj.ToString() : null) + "]", ex3.InnerException);
							return;
						}
					}
				}
				else
				{
					LogLog.Warn("XmlHierarchyConfigurator: Unable to set property [" + text + "] on object [" + ((target != null) ? target.ToString() : null) + "] using value [" + text2 + "] (with acceptable conversion types)");
				}
				return;
			}
			object obj2 = null;
			if ((object)type2 == typeof(string) && !HasAttributesOrElements(element))
			{
				obj2 = "";
			}
			else
			{
				Type defaultTargetType = null;
				if (IsTypeConstructible(type2))
				{
					defaultTargetType = type2;
				}
				obj2 = CreateObjectFromXml(element, defaultTargetType, type2);
			}
			if (obj2 == null)
			{
				LogLog.Error("XmlHierarchyConfigurator: Failed to create object to set param: " + text);
				return;
			}
			if ((object)propertyInfo != null)
			{
				LogLog.Debug("XmlHierarchyConfigurator: Setting Property [" + propertyInfo.Name + "] to object [" + ((obj2 != null) ? obj2.ToString() : null) + "]");
				try
				{
					propertyInfo.SetValue(target, obj2, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
					return;
				}
				catch (TargetInvocationException ex4)
				{
					LogLog.Error("XmlHierarchyConfigurator: Failed to set parameter [" + propertyInfo.Name + "] on object [" + ((target != null) ? target.ToString() : null) + "] using value [" + ((obj2 != null) ? obj2.ToString() : null) + "]", ex4.InnerException);
					return;
				}
			}
			if ((object)methodInfo == null)
			{
				return;
			}
			LogLog.Debug("XmlHierarchyConfigurator: Setting Collection Property [" + methodInfo.Name + "] to object [" + ((obj2 != null) ? obj2.ToString() : null) + "]");
			try
			{
				methodInfo.Invoke(target, BindingFlags.InvokeMethod, null, new object[1] { obj2 }, CultureInfo.InvariantCulture);
			}
			catch (TargetInvocationException ex5)
			{
				LogLog.Error("XmlHierarchyConfigurator: Failed to set parameter [" + methodInfo.Name + "] on object [" + ((target != null) ? target.ToString() : null) + "] using value [" + ((obj2 != null) ? obj2.ToString() : null) + "]", ex5.InnerException);
			}
		}

		private bool HasAttributesOrElements(XmlElement element)
		{
			foreach (XmlNode childNode in element.ChildNodes)
			{
				if (childNode.NodeType == XmlNodeType.Attribute || childNode.NodeType == XmlNodeType.Element)
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsTypeConstructible(Type type)
		{
			if (type.IsClass && !type.IsAbstract)
			{
				ConstructorInfo constructor = type.GetConstructor(new Type[0]);
				if ((object)constructor != null && !constructor.IsAbstract && !constructor.IsPrivate)
				{
					return true;
				}
			}
			return false;
		}

		private MethodInfo FindMethodInfo(Type targetType, string name)
		{
			string strB = "Add" + name;
			MethodInfo[] methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (MethodInfo methodInfo in methods)
			{
				if (!methodInfo.IsStatic && (string.Compare(methodInfo.Name, name, true, CultureInfo.InvariantCulture) == 0 || string.Compare(methodInfo.Name, strB, true, CultureInfo.InvariantCulture) == 0) && methodInfo.GetParameters().Length == 1)
				{
					return methodInfo;
				}
			}
			return null;
		}

		protected object ConvertStringTo(Type type, string value)
		{
			if ((object)typeof(Level) == type)
			{
				Level level = m_hierarchy.LevelMap[value];
				if (level == null)
				{
					LogLog.Error("XmlHierarchyConfigurator: Unknown Level Specified [" + value + "]");
				}
				return level;
			}
			return OptionConverter.ConvertStringTo(type, value);
		}

		protected object CreateObjectFromXml(XmlElement element, Type defaultTargetType, Type typeConstraint)
		{
			Type type = null;
			string attribute = element.GetAttribute("type");
			if (attribute == null || attribute.Length == 0)
			{
				if ((object)defaultTargetType == null)
				{
					LogLog.Error("XmlHierarchyConfigurator: Object type not specified. Cannot create object of type [" + typeConstraint.FullName + "]. Missing Value or Type.");
					return null;
				}
				type = defaultTargetType;
			}
			else
			{
				try
				{
					type = SystemInfo.GetTypeFromString(attribute, true, true);
				}
				catch (Exception exception)
				{
					LogLog.Error("XmlHierarchyConfigurator: Failed to find type [" + attribute + "]", exception);
					return null;
				}
			}
			bool flag = false;
			if ((object)typeConstraint != null && !typeConstraint.IsAssignableFrom(type))
			{
				if (!OptionConverter.CanConvertTypeTo(type, typeConstraint))
				{
					LogLog.Error("XmlHierarchyConfigurator: Object type [" + type.FullName + "] is not assignable to type [" + typeConstraint.FullName + "]. There are no acceptable type conversions.");
					return null;
				}
				flag = true;
			}
			object obj = null;
			try
			{
				obj = Activator.CreateInstance(type);
			}
			catch (Exception ex)
			{
				LogLog.Error("XmlHierarchyConfigurator: Failed to construct object of type [" + type.FullName + "] Exception: " + ex.ToString());
			}
			foreach (XmlNode childNode in element.ChildNodes)
			{
				if (childNode.NodeType == XmlNodeType.Element)
				{
					SetParameter((XmlElement)childNode, obj);
				}
			}
			IOptionHandler optionHandler = obj as IOptionHandler;
			if (optionHandler != null)
			{
				optionHandler.ActivateOptions();
			}
			if (flag)
			{
				return OptionConverter.ConvertTypeTo(obj, typeConstraint);
			}
			return obj;
		}
	}
}
