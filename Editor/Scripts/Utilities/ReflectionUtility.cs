using System;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System.Collections;

namespace EditorAttributes.Editor.Utility
{
    public static class ReflectionUtility
    {
		public const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

		public static FieldInfo FindField(string fieldName, SerializedProperty property)
		{
			var fieldInfo = FindField(fieldName, property.serializedObject.targetObject);

			// If the field null we try to see if its inside a serialized object
			if (fieldInfo == null)
			{
				var serializedObjectType = GetNestedFieldType(property, out _);

				if (serializedObjectType != null)
					fieldInfo = serializedObjectType.GetField(fieldName, BINDING_FLAGS);
			}

			return fieldInfo;
		}

		internal static FieldInfo FindField(string fieldName, object targetObject) => FindMember(fieldName, targetObject.GetType(), BINDING_FLAGS, MemberTypes.Field) as FieldInfo;

		public static PropertyInfo FindProperty(string propertyName, SerializedProperty property)
		{
			var propertyInfo = FindProperty(propertyName, property.serializedObject.targetObject);

			// If the property null we try to see if its inside a serialized object
			if (propertyInfo == null)
			{
				var serializedObjectType = GetNestedFieldType(property, out _);

				if (serializedObjectType != null) 
					propertyInfo = serializedObjectType.GetProperty(propertyName, BINDING_FLAGS);
			}

			return propertyInfo;
		}

		internal static PropertyInfo FindProperty(string propertyName, object targetObject) => FindMember(propertyName, targetObject.GetType(), BINDING_FLAGS, MemberTypes.Property) as PropertyInfo;

		public static MethodInfo FindFunction(string functionName, SerializedProperty property)
		{
			MethodInfo methodInfo;

			methodInfo = FindFunction(functionName, property.serializedObject.targetObject);

			// If the method is null we try to see if its inside a serialized object
			if (methodInfo == null)
			{
				var serializedObjectType = GetNestedFieldType(property, out _);

				if (serializedObjectType == null)
					return methodInfo;

				try
				{
					methodInfo = serializedObjectType.GetMethod(functionName, BINDING_FLAGS);
				}
				catch (AmbiguousMatchException)
				{
					var functions = serializedObjectType.GetMethods();

					foreach (var function in functions)
					{
						if (function.Name == functionName) 
							methodInfo = function;
					}
				}
			}

			return methodInfo;
		}

		internal static MethodInfo FindFunction(string functionName, object targetObject)
		{
			try
			{
				return FindMember(functionName, targetObject.GetType(), BINDING_FLAGS, MemberTypes.Method) as MethodInfo;
			}
			catch (AmbiguousMatchException)
			{
				var functions = targetObject.GetType().GetMethods();

				foreach (var function in functions)
				{
					if (function.Name == functionName) 
						return function;
				}

				return null;
			}
		}

		public static MemberInfo FindMember(string memberName, Type targetType, BindingFlags bindingFlags, MemberTypes memberType)
		{
			switch (memberType)
			{
				case MemberTypes.Field:

					FieldInfo fieldInfo = null;

					while (targetType != null && !TryGetField(memberName, targetType, bindingFlags, out fieldInfo)) 
						targetType = targetType.BaseType;

					return fieldInfo;

				case MemberTypes.Property:

					PropertyInfo propertyInfo = null;

					while (targetType != null && !TryGetProperty(memberName, targetType, bindingFlags, out propertyInfo)) 
						targetType = targetType.BaseType;

					return propertyInfo;

				case MemberTypes.Method:

					MethodInfo methodInfo = null;

					while (targetType != null && !TryGetMethod(memberName, targetType, bindingFlags, out methodInfo)) 
						targetType = targetType.BaseType;

					return methodInfo;
			}

			return null;
		}

		public static bool TryGetField(string name, Type type, BindingFlags bindingFlags, out FieldInfo fieldInfo)
		{
			fieldInfo = type.GetField(name, bindingFlags);

			return fieldInfo != null;
		}

		public static bool TryGetProperty(string name, Type type, BindingFlags bindingFlags, out PropertyInfo propertyInfo)
		{
			propertyInfo = type.GetProperty(name, bindingFlags);

			return propertyInfo != null;
		}

		public static bool TryGetMethod(string name, Type type, BindingFlags bindingFlags, out MethodInfo methodInfo)
		{
			methodInfo = type.GetMethod(name, bindingFlags);

			return methodInfo != null;
		}

		public static bool IsPropertyCollection(SerializedProperty property)
		{
			var arrayField = FindField(property.propertyPath.Split(".")[0], property);
			var memberInfoType = GetMemberInfoType(arrayField);

			return memberInfoType.IsArray || memberInfoType.GetInterfaces().Contains(typeof(IList));
		}

		public static MemberInfo GetValidMemberInfo(string memberName, SerializedProperty serializedProperty)
		{
			MemberInfo memberInfo;

			memberInfo = FindField(memberName, serializedProperty);

			memberInfo ??= FindProperty(memberName, serializedProperty);
			memberInfo ??= FindFunction(memberName, serializedProperty);

			return memberInfo;
		}

		internal static MemberInfo GetValidMemberInfo(string memberName, object targetObject) // Internal function used for the button drawer
		{
			MemberInfo memberInfo;

			memberInfo = FindField(memberName, targetObject);

			memberInfo ??= FindProperty(memberName, targetObject);
			memberInfo ??= FindFunction(memberName, targetObject);

			return memberInfo;
		}

		// Code "borrowed" from https://forum.unity.com/threads/casting-serializedproperty-to-the-desired-type.1169618/
		public static Type GetNestedFieldType(SerializedProperty property, out object targetObject)
		{
			try
			{
				targetObject = property.serializedObject.targetObject;
				var cutPathIndex = property.propertyPath.LastIndexOf('.');

				if (cutPathIndex == -1) // If the cutPathIndex is -1 it means that the property is not nested and we return null
					return null;

				var path = property.propertyPath[..cutPathIndex].Replace(".Array.data[", "[");
				var elements = path.Split('.');

				foreach (var element in elements)
				{
					if (element.Contains("["))
					{
						var elementName = element[..element.IndexOf("[")];
						var index = Convert.ToInt32(element[element.IndexOf("[")..].Replace("[", "").Replace("]", ""));
					
						targetObject = GetValue(targetObject, elementName, index);
					}
					else
					{
						targetObject = GetValue(targetObject, element);
					}
				}

				return targetObject?.GetType();
			}
			catch (ObjectDisposedException)
			{
				targetObject = null;
				return null;
			}
		}

		private static object GetValue(object source, string name, int index)
		{
			if (GetValue(source, name) is not IEnumerable enumerable) 
				return null;

			var enumerator = enumerable.GetEnumerator();

			for (int i = 0; i <= index; i++)
			{
				if (!enumerator.MoveNext()) 
					return null;
			}

			return enumerator.Current;
		}

		private static object GetValue(object source, string name)
		{
			if (source == null)
				return null;

			var type = source.GetType();

			while (type != null)
			{
				var field = FindMember(name, type, BINDING_FLAGS, MemberTypes.Field) as FieldInfo;

				if (field != null)
					return field.GetValue(source);

				type = type.BaseType;
			}

			return null;
		}

		public static Type GetMemberInfoType(MemberInfo memberInfo)
		{
			if (memberInfo is FieldInfo fieldInfo)
			{
				return fieldInfo.FieldType;
			}
			else if (memberInfo is PropertyInfo propertyInfo)
			{
				return propertyInfo.PropertyType;
			}
			else if (memberInfo is MethodInfo methodInfo)
			{
				return methodInfo.ReturnType;
			}

			return null;
		}

		public static object GetMemberInfoValue(MemberInfo memberInfo, SerializedProperty property)
		{
			var targetObject = property.serializedObject.targetObject;

			try
			{
				if (memberInfo is FieldInfo fieldInfo)
				{
					return fieldInfo.GetValue(targetObject);
				}
				else if (memberInfo is PropertyInfo propertyInfo)
				{
					return propertyInfo.GetValue(targetObject);
				}
				else if (memberInfo is MethodInfo methodInfo)
				{
					return methodInfo.Invoke(targetObject, null);
				}
			}
			catch (ArgumentException) // If this expection is thrown it means that the member we try to get the value from is inside a different target
			{
				GetNestedFieldType(property, out object serializedObjectTarget);

				if (serializedObjectTarget != null)
				{
					if (memberInfo is FieldInfo fieldInfo)
					{
						return fieldInfo.GetValue(serializedObjectTarget);
					}
					else if (memberInfo is PropertyInfo propertyInfo)
					{
						return propertyInfo.GetValue(serializedObjectTarget);
					}
					else if (memberInfo is MethodInfo methodInfo)
					{
						return methodInfo.Invoke(serializedObjectTarget, null);
					}
				}
			}

			return null;
		}

		internal static object GetMemberInfoValue(MemberInfo memberInfo, object targetObject) // Internal function used for the button drawer
		{
			if (memberInfo is FieldInfo fieldInfo)
			{
				return fieldInfo.GetValue(targetObject);
			}
			else if (memberInfo is PropertyInfo propertyInfo)
			{
				return propertyInfo.GetValue(targetObject);
			}
			else if (memberInfo is MethodInfo methodInfo)
			{
				return methodInfo.Invoke(targetObject, null);
			}

			return null;
		}
	}
}
