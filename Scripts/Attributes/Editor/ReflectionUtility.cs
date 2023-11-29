using System;
using UnityEditor;
using System.Reflection;

namespace EditorAttributes.Editor
{
    public static class ReflectionUtility
    {
		public const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

		public static FieldInfo FindField(string fieldName, SerializedProperty property)
		{
			FieldInfo fieldInfo;

			fieldInfo = property.serializedObject.targetObject.GetType().GetField(fieldName, BINDING_FLAGS);

			// If we cannot find the field in the target object we try to see if its inside a serialized object
			if (fieldInfo == null)
			{
				var serializedObjectType = GetSerializedObjectFieldType(property, out _);

				if (serializedObjectType != null) fieldInfo = serializedObjectType.GetField(fieldName, BINDING_FLAGS);
			}

			return fieldInfo;
		}

		public static PropertyInfo FindProperty(string propertyName, SerializedProperty property)
		{
			PropertyInfo propertyInfo;

			propertyInfo = property.serializedObject.targetObject.GetType().GetProperty(propertyName, BINDING_FLAGS);

			// If we cannot find the field in the target object we try to see if its inside a serialized object
			if (propertyInfo == null)
			{
				var serializedObjectType = GetSerializedObjectFieldType(property, out _);

				if (serializedObjectType != null) propertyInfo = serializedObjectType.GetProperty(propertyName, BINDING_FLAGS);
			}

			return propertyInfo;
		}

		public static MethodInfo FindFunction(string functionName, SerializedProperty property)
		{
			MethodInfo methodInfo;

			methodInfo = FindFunction(functionName, property.serializedObject.targetObject);

			// If we cannot find the field in the target object we try to see if its inside a serialized object
			if (methodInfo == null)
			{
				var serializedObjectType = GetSerializedObjectFieldType(property, out _);

				try
				{
					methodInfo = serializedObjectType.GetMethod(functionName, BINDING_FLAGS);
				}
				catch (AmbiguousMatchException)
				{
					var functions = serializedObjectType.GetMethods();

					foreach (var function in functions)
					{
						if (function.Name == functionName) methodInfo = function;
					}
				}
			}

			return methodInfo;
		}

		internal static MethodInfo FindFunction(string functionName, object targetObject)
		{
			try
			{
				return targetObject.GetType().GetMethod(functionName, BINDING_FLAGS);
			}
			catch (AmbiguousMatchException)
			{
				var functions = targetObject.GetType().GetMethods();

				foreach (var function in functions)
				{
					if (function.Name == functionName) return function;
				}

				return null;
			}
		}

		internal static MemberInfo GetValidMemberInfo(string parameterName, object targetObject)
		{
			MemberInfo memberInfo;

			memberInfo = targetObject.GetType().GetField(parameterName, BINDING_FLAGS);

			memberInfo ??= targetObject.GetType().GetProperty(parameterName, BINDING_FLAGS);
			memberInfo ??= FindFunction(parameterName, targetObject);

			return memberInfo;
		}

		public static MemberInfo GetValidMemberInfo(string parameterName, SerializedProperty serializedProperty)
		{
			MemberInfo memberInfo;

			memberInfo = FindField(parameterName, serializedProperty);

			memberInfo ??= FindProperty(parameterName, serializedProperty);
			memberInfo ??= FindFunction(parameterName, serializedProperty);

			return memberInfo;
		}

		public static Type GetSerializedObjectFieldType(SerializedProperty property, out object serializedObject)
		{
			var targetObject = property.serializedObject.targetObject;
			var pathComponents = property.propertyPath.Split('.'); // Split the property path to get individual components
			var serializedObjectField = targetObject.GetType().GetField(pathComponents[0], BINDING_FLAGS);

			serializedObject = serializedObjectField.GetValue(targetObject);

			return serializedObject.GetType();
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
				GetSerializedObjectFieldType(property, out object serializedObjectTarget);

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

			return null;
		}

		internal static object GetMemberInfoValue(MemberInfo memberInfo, object targetObject)
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
