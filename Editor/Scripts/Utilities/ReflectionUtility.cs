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

		/// <summary>
		/// Finds a field inside a serialized object
		/// </summary>
		/// <param name="fieldName">The name of the field to search</param>
		/// <param name="property">The serialized property</param>
		/// <returns>The field info of the desired field</returns>
		public static FieldInfo FindField(string fieldName, SerializedProperty property)
		{
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

		internal static FieldInfo FindField(string fieldName, object targetObject) => FindMember(fieldName, targetObject.GetType(), BINDING_FLAGS, MemberTypes.Field) as FieldInfo;

		/// <summary>
		/// Finds a property inside a serialized object
		/// </summary>
		/// <param name="propertyName">The name of the property to search</param>
		/// <param name="property">The serialized property</param>
		/// <returns>The property info of the desired property</returns>
		public static PropertyInfo FindProperty(string propertyName, SerializedProperty property)
		{
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

		internal static PropertyInfo FindProperty(string propertyName, object targetObject) => FindMember(propertyName, targetObject.GetType(), BINDING_FLAGS, MemberTypes.Property) as PropertyInfo;

		/// <summary>
		/// Finds a funciton inside a serialized object
		/// </summary>
		/// <param name="functionName">The name of the function to search</param>
		/// <param name="property">The serialized property</param>
		/// <returns>The method info of the desired function</returns>
		public static MethodInfo FindFunction(string functionName, SerializedProperty property)
		{
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

		/// <summary>
		/// Finds a member from the target type
		/// </summary>
		/// <param name="memberName">The name of the member to look for</param>
		/// <param name="targetType">The type to get the member from</param>
		/// <param name="bindingFlags">The binding flags</param>
		/// <param name="memberType">The type of the member to look for. Only Field, Property and Method are supported</param>
		/// <returns>The member info of the desired member</returns>
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

			return memberInfoType.IsArray || memberInfoType.GetInterfaces().Contains(typeof(IList));
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

		internal static MemberInfo GetValidMemberInfo(string memberName, object targetObject) // Internal function used for the button drawer
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
				var cutPathIndex = property.propertyPath.LastIndexOf('.');

				if (cutPathIndex == -1) // If the cutPathIndex is -1 it means that the member is not nested and we return null
					return null;

				var path = property.propertyPath[..cutPathIndex].Replace(".Array.data[", "[");
				var elements = path.Split('.');

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

		internal static object GetMemberInfoValue(MemberInfo memberInfo, object targetObject) // Internal function used for the button drawer
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
