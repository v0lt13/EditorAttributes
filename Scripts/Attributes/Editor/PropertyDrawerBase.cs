using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEditorInternal;

namespace EditorAttributes.Editor
{
    public class PropertyDrawerBase : PropertyDrawer
    {
		protected UnityEventDrawer eventDrawer;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => GetCorrectPropertyHeight(property, label);

		protected virtual void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
		{
			eventDrawer ??= new UnityEventDrawer();

			try
			{
				eventDrawer.OnGUI(position, property, label);
			}
			catch (NullReferenceException)
			{
				EditorGUI.PropertyField(position, property, label, true);
			}
		}

		protected virtual float GetCorrectPropertyHeight(SerializedProperty property, GUIContent label)
		{
			eventDrawer ??= new UnityEventDrawer();
			
			try
			{
				return eventDrawer.GetPropertyHeight(property, label);
			}
			catch (NullReferenceException)
			{
				return EditorGUI.GetPropertyHeight(property, label);
			}
		}

		protected void SetProperyValueAsString(string value, ref SerializedProperty property)
		{
			try
			{
				switch (property.propertyType)
				{
					case SerializedPropertyType.Integer:
						property.intValue = Convert.ToInt32(value);
						break;

					case SerializedPropertyType.Float:
						property.floatValue = Convert.ToSingle(value);
						break;

					case SerializedPropertyType.String:
						property.stringValue = value;
						break;

					default:
						EditorGUILayout.HelpBox($"The type {property.propertyType} is not supported", MessageType.Warning);
						break;
				}
			}
			catch (FormatException)
			{
				EditorGUILayout.HelpBox($"Could not convert the value \"{value}\" to {property.propertyType}", MessageType.Error);
			}
		}

		protected string GetPropertyValueAsString(SerializedProperty property)
		{
			return property.propertyType switch
			{
				SerializedPropertyType.Integer => property.intValue.ToString(),
				SerializedPropertyType.Float => property.floatValue.ToString(),
				SerializedPropertyType.String => property.stringValue,
				_ => string.Empty,
			};
		}

		protected bool GetConditionValue<T>(MemberInfo memberInfo, SerializedProperty property, bool drawErrorBox = false) where T : PropertyAttribute, IConditionalAttribute
		{
			var conditionAttribute = attribute as T;

			try
			{
				var memberInfoType = GetMemberInfoType(memberInfo);

				if (memberInfoType == typeof(bool))
				{
					return (bool)GetMemberInfoValue(memberInfo, property.serializedObject.targetObject);
				}
				else if (memberInfoType.IsEnum)
				{
					return (int)GetMemberInfoValue(memberInfo, property.serializedObject.targetObject) == conditionAttribute.EnumValue;
				}
			}
			catch (NullReferenceException)
			{
				var unityProperty = property.serializedObject.FindProperty(conditionAttribute.ConditionName);

				if (unityProperty != null)
				{
					switch (unityProperty.propertyType)
					{
						case SerializedPropertyType.Boolean:
							return unityProperty.boolValue;

						case SerializedPropertyType.Enum:
							return unityProperty.intValue == conditionAttribute.EnumValue;

						default:
							break;
					}					
				}
			}

			if (drawErrorBox) EditorGUILayout.HelpBox($"The provided condition \"{conditionAttribute.ConditionName}\" is not a valid boolean or an enum", MessageType.Error);

			return false;
		}

		protected FieldInfo FindField(string fieldName, SerializedProperty property) => property.serializedObject.targetObject.GetType().GetField(fieldName);

		protected PropertyInfo FindProperty(string propertyName, SerializedProperty property) => property.serializedObject.targetObject.GetType().GetProperty(propertyName);

		protected MethodInfo FindFunction(string functionName, SerializedProperty property)
		{
			try
			{
				return property.serializedObject.targetObject.GetType().GetMethod(functionName);
			}
			catch (AmbiguousMatchException)
			{
				var functions = property.serializedObject.targetObject.GetType().GetMethods();

				foreach (var function in functions)
				{
					if (function.Name == functionName) return function;
				}

				return null;
			}
		}

		protected virtual MemberInfo GetValidMemberInfo(string parameterName, SerializedProperty serializedProperty)
		{
			MemberInfo memberInfo;

			memberInfo = FindField(parameterName, serializedProperty);

			memberInfo ??= FindProperty(parameterName, serializedProperty);
			memberInfo ??= FindFunction(parameterName, serializedProperty);

			return memberInfo;
		}

		protected object GetMemberInfoValue(MemberInfo memberInfo, object targetObject)
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

		protected Type GetMemberInfoType(MemberInfo memberInfo)
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

		public static object DrawField(Type fieldType, string fieldName, object fieldValue)
		{
			fieldName = char.ToUpper(fieldName[0]) + fieldName[1..]; // Uppercase the first character of the name

			if (fieldType == typeof(string))
			{
				return EditorGUILayout.TextField(fieldName, fieldValue.ToString());
			}
			else if (fieldType == typeof(int))
			{
				return EditorGUILayout.IntField(fieldName, Convert.IsDBNull(fieldValue) ? 0 : Convert.ToInt32(fieldValue));
			}
			else if (fieldType == typeof(float))
			{
				return EditorGUILayout.FloatField(fieldName, Convert.IsDBNull(fieldValue) ? 0.0f : Convert.ToSingle(fieldValue));
			}
			else if (fieldType == typeof(double))
			{
				return EditorGUILayout.DoubleField(fieldName, Convert.IsDBNull(fieldValue) ? 0.0 : (double)fieldValue);
			}
			else if (fieldType == typeof(bool))
			{
				return EditorGUILayout.Toggle(fieldName, !Convert.IsDBNull(fieldValue) && (bool)fieldValue);
			}
			else if (fieldType == typeof(GameObject))
			{
				return EditorGUILayout.ObjectField(fieldName, Convert.IsDBNull(fieldValue) ? null : (GameObject)fieldValue, typeof(GameObject), true);
			}
			else if (fieldType == typeof(Vector2))
			{
				return EditorGUILayout.Vector2Field(fieldName, Convert.IsDBNull(fieldValue) ? Vector2.zero : (Vector2)fieldValue);
			}
			else if (fieldType == typeof(Vector2Int))
			{
				return EditorGUILayout.Vector2IntField(fieldName, Convert.IsDBNull(fieldValue) ? Vector2Int.zero : (Vector2Int)fieldValue);
			}
			else if (fieldType == typeof(Vector3))
			{
				return EditorGUILayout.Vector3Field(fieldName, Convert.IsDBNull(fieldValue) ? Vector3.zero : (Vector3)fieldValue);
			}
			else if (fieldType == typeof(Vector3Int))
			{
				return EditorGUILayout.Vector3IntField(fieldName, Convert.IsDBNull(fieldValue) ? Vector3Int.zero : (Vector3Int)fieldValue);
			}
			else if (fieldType == typeof(Vector4))
			{
				return EditorGUILayout.Vector4Field(fieldName, Convert.IsDBNull(fieldValue) ? Vector4.zero : (Vector4)fieldValue);
			}
			else if (fieldType == typeof(Color))
			{
				return EditorGUILayout.ColorField(fieldName, Convert.IsDBNull(fieldValue) ? Color.white : (Color)fieldValue);
			}
			else if (fieldType == typeof(Gradient))
			{
				return EditorGUILayout.GradientField(fieldName, Convert.IsDBNull(fieldValue) ? new Gradient() : (Gradient)fieldValue);
			}
			else if (fieldType == typeof(AnimationCurve))
			{
				return EditorGUILayout.CurveField(fieldName, Convert.IsDBNull(fieldValue) ? AnimationCurve.Linear(0f, 0f, 1f, 1f) : (AnimationCurve)fieldValue);
			}
			else if (fieldType == typeof(LayerMask))
			{
				return EditorGUILayout.LayerField(fieldName, Convert.IsDBNull(fieldValue) ? 0 : Convert.ToInt32(fieldValue));
			}
			else if (fieldType == typeof(Rect))
			{
				return EditorGUILayout.RectField(fieldName, Convert.IsDBNull(fieldValue) ? new Rect(0f, 0f, 0f, 0f) : (Rect)fieldValue);
			}
			else if (fieldType == typeof(RectInt))
			{
				return EditorGUILayout.RectIntField(fieldName, Convert.IsDBNull(fieldValue) ? new RectInt(0, 0, 0, 0) : (RectInt)fieldValue);
			}
			else
			{
				EditorGUILayout.HelpBox($"The type {fieldType} is not supported", MessageType.Warning);
				return null;
			}
		}
	}
}
