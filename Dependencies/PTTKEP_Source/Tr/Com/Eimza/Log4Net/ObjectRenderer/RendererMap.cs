using System;
using System.Collections;
using System.Globalization;
using System.IO;
using Tr.Com.Eimza.Log4Net.Util;

namespace Tr.Com.Eimza.Log4Net.ObjectRenderer
{
	public class RendererMap
	{
		private Hashtable m_map;

		private Hashtable m_cache = new Hashtable();

		private static IObjectRenderer s_defaultRenderer = new DefaultRenderer();

		public IObjectRenderer DefaultRenderer
		{
			get
			{
				return s_defaultRenderer;
			}
		}

		public RendererMap()
		{
			m_map = Hashtable.Synchronized(new Hashtable());
		}

		public string FindAndRender(object obj)
		{
			string text = obj as string;
			if (text != null)
			{
				return text;
			}
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			FindAndRender(obj, stringWriter);
			return stringWriter.ToString();
		}

		public void FindAndRender(object obj, TextWriter writer)
		{
			if (obj == null)
			{
				writer.Write(SystemInfo.NullText);
				return;
			}
			string text = obj as string;
			if (text != null)
			{
				writer.Write(text);
				return;
			}
			try
			{
				Get(obj.GetType()).RenderObject(this, obj, writer);
			}
			catch (Exception ex)
			{
				LogLog.Error("RendererMap: Exception while rendering object of type [" + obj.GetType().FullName + "]", ex);
				string text2 = "";
				if (obj != null && (object)obj.GetType() != null)
				{
					text2 = obj.GetType().FullName;
				}
				writer.Write("<log4net.Error>Exception rendering object type [" + text2 + "]");
				if (ex != null)
				{
					string text3 = null;
					try
					{
						text3 = ex.ToString();
					}
					catch
					{
					}
					writer.Write("<stackTrace>" + text3 + "</stackTrace>");
				}
				writer.Write("</log4net.Error>");
			}
		}

		public IObjectRenderer Get(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			return Get(obj.GetType());
		}

		public IObjectRenderer Get(Type type)
		{
			if ((object)type == null)
			{
				throw new ArgumentNullException("type");
			}
			IObjectRenderer objectRenderer = null;
			objectRenderer = (IObjectRenderer)m_cache[type];
			if (objectRenderer == null)
			{
				Type type2 = type;
				while ((object)type2 != null)
				{
					objectRenderer = SearchTypeAndInterfaces(type2);
					if (objectRenderer != null)
					{
						break;
					}
					type2 = type2.BaseType;
				}
				if (objectRenderer == null)
				{
					objectRenderer = s_defaultRenderer;
				}
				m_cache[type] = objectRenderer;
			}
			return objectRenderer;
		}

		private IObjectRenderer SearchTypeAndInterfaces(Type type)
		{
			IObjectRenderer objectRenderer = (IObjectRenderer)m_map[type];
			if (objectRenderer != null)
			{
				return objectRenderer;
			}
			Type[] interfaces = type.GetInterfaces();
			foreach (Type type2 in interfaces)
			{
				objectRenderer = SearchTypeAndInterfaces(type2);
				if (objectRenderer != null)
				{
					return objectRenderer;
				}
			}
			return null;
		}

		public void Clear()
		{
			m_map.Clear();
			m_cache.Clear();
		}

		public void Put(Type typeToRender, IObjectRenderer renderer)
		{
			m_cache.Clear();
			if ((object)typeToRender == null)
			{
				throw new ArgumentNullException("typeToRender");
			}
			if (renderer == null)
			{
				throw new ArgumentNullException("renderer");
			}
			m_map[typeToRender] = renderer;
		}
	}
}
