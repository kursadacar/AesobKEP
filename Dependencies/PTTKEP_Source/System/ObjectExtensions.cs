using System.Collections.Generic;
using System.Reflection;

namespace System
{
	internal static class ObjectExtensions
	{
		private static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);

		public static bool IsPrimitive(this Type type)
		{
			if ((object)type == typeof(string))
			{
				return true;
			}
			return type.IsValueType & type.IsPrimitive;
		}

		public static object Copy(this object originalObject)
		{
			return InternalCopy(originalObject, new Dictionary<object, object>(new ReferenceEqualityComparer()));
		}

		private static object InternalCopy(object originalObject, IDictionary<object, object> visited)
		{
			if (originalObject == null)
			{
				return null;
			}
			Type type = originalObject.GetType();
			if (type.IsPrimitive())
			{
				return originalObject;
			}
			if (visited.ContainsKey(originalObject))
			{
				return visited[originalObject];
			}
			if (typeof(Delegate).IsAssignableFrom(type))
			{
				return null;
			}
			object obj = CloneMethod.Invoke(originalObject, null);
			if (type.IsArray && !type.GetElementType().IsPrimitive())
			{
				Array clonedArray = (Array)obj;
				clonedArray.ForEach(delegate(Array array, int[] indices)
				{
					array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices);
				});
			}
			visited.Add(originalObject, obj);
			CopyFields(originalObject, visited, obj, type);
			RecursiveCopyBaseTypePrivateFields(originalObject, visited, obj, type);
			return obj;
		}

		private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
		{
			if ((object)typeToReflect.BaseType != null)
			{
				RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
				CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, (FieldInfo info) => info.IsPrivate);
			}
		}

		private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
		{
			FieldInfo[] fields = typeToReflect.GetFields(bindingFlags);
			foreach (FieldInfo fieldInfo in fields)
			{
				if ((filter == null || filter(fieldInfo)) && !fieldInfo.FieldType.IsPrimitive())
				{
					object value = InternalCopy(fieldInfo.GetValue(originalObject), visited);
					fieldInfo.SetValue(cloneObject, value);
				}
			}
		}

		private static T Copy<T>(this T original)
		{
			return (T)((object)original).Copy();
		}
	}
}
