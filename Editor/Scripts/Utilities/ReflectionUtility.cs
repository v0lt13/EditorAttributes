using System;
using UnityEditor;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;

namespace EditorAttributes.Editor.Utility
{
	public static class ReflectionUtility
	{
		public const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy;

		/// <summary>
		/// Finds a field inside a serialized object
		/// </summary>
		/// <param name="fieldName">The name of the field to search</param>
		/// <param name="property">The serialized property</param>
		/// <returns>The field info of the desired field</returns>
		public static FieldInfo FindField(string fieldName, SerializedProperty property)
		{
			if (fieldName.Contains('.'))
				return GetStaticMemberInfoFromPath(fieldName, MemberTypes.Field) as FieldInfo;

			var fieldInfo = FindField(fieldName, property.serializedObject.targetObject);

			// If the field null we try to see if its inside a serialized object
			if (fieldInfo == null)
			{
				var serializedObjectType = GetNestedObjectType(property, out _);

				if (serializedObjectType != null)
					fieldInfo = serializedObjectType.GetField(fieldName, BINDING_FLAGS);
			}

			return fieldInfo;
		}

		internal static FieldInfo FindField(string fieldName, object targetObject) => FindMember(fieldName, targetObject?.GetType(), BINDING_FLAGS, MemberTypes.Field) as FieldInfo;

		/// <summary>
		/// Finds a property inside a serialized object
		/// </summary>
		/// <param name="propertyName">The name of the property to search</param>
		/// <param name="property">The serialized property</param>
		/// <returns>The property info of the desired property</returns>
		public static PropertyInfo FindProperty(string propertyName, SerializedProperty property)
		{
			if (propertyName.Contains('.'))
				return GetStaticMemberInfoFromPath(propertyName, MemberTypes.Property) as PropertyInfo;

			var propertyInfo = FindProperty(propertyName, property.serializedObject.targetObject);

			// If the property null we try to see if its inside a serialized object
			if (propertyInfo == null)
			{
				var serializedObjectType = GetNestedObjectType(property, out _);

				if (serializedObjectType != null)
					propertyInfo = serializedObjectType.GetProperty(propertyName, BINDING_FLAGS);
			}

			return propertyInfo;
		}

		internal static PropertyInfo FindProperty(string propertyName, object targetObject) => FindMember(propertyName, targetObject?.GetType(), BINDING_FLAGS, MemberTypes.Property) as PropertyInfo;

		/// <summary>
		/// Finds a funciton inside a serialized object
		/// </summary>
		/// <param name="functionName">The name of the function to search</param>
		/// <param name="property">The serialized property</param>
		/// <returns>The method info of the desired function</returns>
		public static MethodInfo FindFunction(string functionName, SerializedProperty property)
		{
			if (functionName.Contains('.'))
				return GetStaticMemberInfoFromPath(functionName, MemberTypes.Method) as MethodInfo;

			MethodInfo methodInfo;

			methodInfo = FindFunction(functionName, property.serializedObject.targetObject);

			// If the method is null we try to see if its inside a serialized object
			if (methodInfo == null)
			{
				var serializedObjectType = GetNestedObjectType(property, out _);

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
				return FindMember(functionName, targetObject?.GetType(), BINDING_FLAGS, MemberTypes.Method) as MethodInfo;
			}
			catch (AmbiguousMatchException)
			{
				var functions = targetObject?.GetType().GetMethods();

				foreach (var function in functions)
				{
					if (function.Name == functionName)
						return function;
				}

				return null;
			}
		}

		/// <summary>
		/// Finds a member from the target and it's inherited types
		/// </summary>
		/// <param name="memberName">The name of the member to look for</param>
		/// <param name="targetType">The type to get the member from</param>
		/// <param name="bindingFlags">The binding flags</param>
		/// <param name="memberType">The type of the member to look for. Only Field, Property and Method types are supported</param>
		/// <returns>The member info of the specified member type</returns>
		public static MemberInfo FindMember(string memberName, Type targetType, BindingFlags bindingFlags, MemberTypes memberType)
		{
			MemberInfo memberInfo = null;

			while (targetType != null)
			{
				switch (memberType)
				{
					case MemberTypes.Field:
						memberInfo = targetType.GetField(memberName, bindingFlags);
						break;

					case MemberTypes.Property:
						memberInfo = targetType.GetProperty(memberName, bindingFlags);
						break;

					case MemberTypes.Method:
						memberInfo = targetType.GetMethod(memberName, bindingFlags);
						break;
				}

				if (memberInfo != null)
					return memberInfo;

				targetType = targetType.BaseType;
			}

			return null;
		}

		/// <summary>
		/// Gets the info of a const or static member from the type specified in the path
		/// </summary>
		/// <param name="memberPath">The path on which to locate the member</param>
		/// <param name="memberTypes">The type of the member to look for. Only Field, Property and Method types are supported</param>
		/// <returns>The member info of the specified member type</returns>
		public static MemberInfo GetStaticMemberInfoFromPath(string memberPath, MemberTypes memberTypes)
		{
			MemberInfo memberInfo = null;

			string[] splitPath = memberPath.Split('.');

			string typeNamespace = GetNamespaceString(splitPath);
			string typeName = splitPath[^2];
			string actualFieldName = splitPath[^1];

			var matchingTypes = TypeCache.GetTypesDerivedFrom<object>().Where((type) => type.Name == typeName && type.Namespace == typeNamespace);

			foreach (var type in matchingTypes)
			{
				memberInfo = FindMember(actualFieldName, type, BINDING_FLAGS ^ BindingFlags.Instance, memberTypes);

				if (memberInfo == null)
				{
					continue;
				}
				else
				{
					break;
				}
			}

			return memberInfo;
		}

		private static string GetNamespaceString(string[] splitMemberPath)
		{
			var stringBuilder = new StringBuilder();

			string[] namespacePath = splitMemberPath[..^2];

			for (int i = 0; i < namespacePath.Length; i++)
			{
				stringBuilder.Append(namespacePath[i]);

				if (i != namespacePath.Length - 1)
					stringBuilder.Append('.');
			}

			return stringBuilder.Length == 0 ? null : stringBuilder.ToString();
		}

		/// <summary>
		/// Tries to get a field from the target type
		/// </summary>
		/// <param name="name">The name of the field to search for</param>
		/// <param name="targetType">The type to get the field from</param>
		/// <param name="bindingFlags">The binding flags</param>
		/// <param name="fieldInfo">The field info of the desired field</param>
		/// <returns>True if the field was succesfully found, false otherwise</returns>
		public static bool TryGetField(string name, Type targetType, BindingFlags bindingFlags, out FieldInfo fieldInfo)
		{
			fieldInfo = targetType.GetField(name, bindingFlags);

			return fieldInfo != null;
		}

		/// <summary>
		/// Tries to get a property from the target type
		/// </summary>
		/// <param name="name">The name of the property to search for</param>
		/// <param name="targetType">The type to get the property from</param>
		/// <param name="bindingFlags">The binding flags</param>
		/// <param name="propertyInfo">The property info of the desired property</param>
		/// <returns>True if the property was succesfully found, false otherwise</returns>
		public static bool TryGetProperty(string name, Type targetType, BindingFlags bindingFlags, out PropertyInfo propertyInfo)
		{
			propertyInfo = targetType.GetProperty(name, bindingFlags);

			return propertyInfo != null;
		}

		/// <summary>
		/// Tries to get a function from the target type
		/// </summary>
		/// <param name="name">The name of the function to search for</param>
		/// <param name="targetType">The type to get the function from</param>
		/// <param name="bindingFlags">The binding flags</param>
		/// <param name="methodInfo">The method info of the desired function</param>
		/// <returns>True if the function was succesfully found, false otherwise</returns>
		public static bool TryGetMethod(string name, Type targetType, BindingFlags bindingFlags, out MethodInfo methodInfo)
		{
			methodInfo = targetType.GetMethod(name, bindingFlags);

			return methodInfo != null;
		}

		/// <summary>
		/// Checks to see if a seralized property is a list or array
		/// </summary>
		/// <param name="property">The serialized property to check</param>
		/// <returns>True if the property is a list or array, false otherwise</returns>
		public static bool IsPropertyCollection(SerializedProperty property)
		{
			var arrayField = FindField(property.propertyPath.Split(".")[0], property);
			var memberInfoType = GetMemberInfoType(arrayField);

			return IsTypeCollection(memberInfoType);
		}

		/// <summary>
		/// Checks to see if a type is a list or array
		/// </summary>
		/// <param name="type">The type to check</param>
		/// <returns>True if the type is a list or array</returns>
		public static bool IsTypeCollection(Type type) => type.IsArray || type.GetInterfaces().Contains(typeof(IList));

		/// <summary>
		/// Checks to see if a member has one of the specified attributes
		/// </summary>
		/// <param name="memberInfo">The member to check</param>
		/// <param name="attributeTypes">The attribute types</param>
		/// <returns>True if the member has at least one of specified attributes</returns>
		public static bool HasAnyAttributes(MemberInfo memberInfo, params Type[] attributeTypes)
		{
			if (memberInfo == null)
				return false;

			foreach (var attribute in attributeTypes)
			{
				if (memberInfo.GetCustomAttribute(attribute) != null)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Finds a member inside a serialzied object
		/// </summary>
		/// <param name="memberName">The name of the member to look for</param>
		/// <param name="serializedProperty">The serialized property</param>
		/// <returns>The member info of the member</returns>
		public static MemberInfo GetValidMemberInfo(string memberName, SerializedProperty serializedProperty)
		{
			MemberInfo memberInfo;

			memberInfo = FindField(memberName, serializedProperty);

			memberInfo ??= FindProperty(memberName, serializedProperty);
			memberInfo ??= FindFunction(memberName, serializedProperty);

			return memberInfo;
		}

		internal static MemberInfo GetValidMemberInfo(string memberName, object targetObject)
		{
			MemberInfo memberInfo;

			memberInfo = FindField(memberName, targetObject);

			memberInfo ??= FindProperty(memberName, targetObject);
			memberInfo ??= FindFunction(memberName, targetObject);

			return memberInfo;
		}

		/// <summary>
		/// Gets the type of a nested serialized object
		/// </summary>
		/// <param name="property">The serialized property</param>
		/// <param name="nestedObject">Outputs the serialized nested object</param>
		/// <returns>The nested object type</returns>
		public static Type GetNestedObjectType(SerializedProperty property, out object nestedObject)
		{
			try
			{
				nestedObject = property.serializedObject.targetObject;
				int cutPathIndex = property.propertyPath.LastIndexOf('.');

				if (cutPathIndex == -1) // If the cutPathIndex is -1 it means that the member is not nested and we return null
					return null;

				string path = property.propertyPath[..cutPathIndex].Replace(".Array.data[", "[");
				string[] elements = path.Split('.');

				foreach (var element in elements)
				{
					if (element.Contains("["))
					{
						var elementName = element[..element.IndexOf("[")];
						var index = Convert.ToInt32(element[element.IndexOf("[")..].Replace("[", "").Replace("]", ""));

						nestedObject = GetValue(nestedObject, elementName, index);
					}
					else
					{
						nestedObject = GetValue(nestedObject, element);
					}
				}

				return nestedObject?.GetType();
			}
			catch (ObjectDisposedException)
			{
				nestedObject = null;
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

		/// <summary>
		/// Gets the type of a member
		/// </summary>
		/// <param name="memberInfo">The member to get the type from</param>
		/// <returns>The type of the member</returns>
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

		/// <summary>
		/// Gets the value of a member
		/// </summary>
		/// <param name="memberInfo">The member to get the value from</param>
		/// <param name="property">The serialized property</param>
		/// <returns>The value of the member</returns>
		public static object GetMemberInfoValue(MemberInfo memberInfo, SerializedProperty property)
		{
			var targetObject = property.serializedObject.targetObject;

			if (targetObject == null)
				return null;

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
			catch (Exception exception)
			{
				if (exception is ArgumentException or TargetException) // If these expections are thrown it means that the member we try to get the value from is inside a different target
				{
					GetNestedObjectType(property, out object serializedObjectTarget);

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
				else
				{
					throw;
				}
			}

			return null;
		}

		internal static object GetMemberInfoValue(MemberInfo memberInfo, object targetObject)
		{
			if (targetObject == null)
				return null;

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
