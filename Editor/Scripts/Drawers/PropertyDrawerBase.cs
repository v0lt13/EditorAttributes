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

		protected static void SetProperyValueFromString(string value, ref SerializedProperty property)
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

		protected static string GetPropertyValueAsString(SerializedProperty property)
		{
			return property.propertyType switch
			{
				SerializedPropertyType.Integer => property.intValue.ToString(),
				SerializedPropertyType.Float => property.floatValue.ToString(),
				SerializedPropertyType.String => property.stringValue,
				_ => string.Empty,
			};
		}

		protected static SerializedProperty FindNestedProperty(SerializedProperty property, string propertyName)
		{
			var propertyPath = property.propertyPath;
			var cutPathIndex = propertyPath.LastIndexOf('.');

			if (cutPathIndex == -1)
			{
				return property.serializedObject.FindProperty(propertyName);
			}
			else
			{
				propertyPath = propertyPath[..cutPathIndex];

				return property.serializedObject.FindProperty(propertyPath).FindPropertyRelative(propertyName);
			}
		}

		public static bool GetConditionValue(MemberInfo memberInfo, IConditionalAttribute conditionalAttribute, SerializedProperty serializedProperty)
		{
			var memberInfoType = ReflectionUtility.GetMemberInfoType(memberInfo);

			if (memberInfoType == null)
			{
				EditorGUILayout.HelpBox($"The provided condition \"{conditionalAttribute.ConditionName}\" could not be found", MessageType.Error);
				return false;
			}

			if (memberInfoType == typeof(bool))
			{
				return (bool)ReflectionUtility.GetMemberInfoValue(memberInfo, serializedProperty);
			}
			else if (memberInfoType.IsEnum)
			{
				return (int)ReflectionUtility.GetMemberInfoValue(memberInfo, serializedProperty) == conditionalAttribute.EnumValue;
			}

			EditorGUILayout.HelpBox($"The provided condition \"{conditionalAttribute.ConditionName}\" is not a valid boolean or an enum", MessageType.Error);

			return false;
		}

		internal static bool GetConditionValue(MemberInfo memberInfo, IConditionalAttribute conditionalAttribute, object targetObject) // Internal function used for the button drawer
		{			
			var memberInfoType = ReflectionUtility.GetMemberInfoType(memberInfo);

			if (memberInfoType == null)
			{
				EditorGUILayout.HelpBox($"The provided condition \"{conditionalAttribute.ConditionName}\" could not be found", MessageType.Error);
				return false;
			}

			if (memberInfoType == typeof(bool))
			{
				return (bool)ReflectionUtility.GetMemberInfoValue(memberInfo, targetObject);
			}
			else if (memberInfoType.IsEnum)
			{
				return (int)ReflectionUtility.GetMemberInfoValue(memberInfo, targetObject) == conditionalAttribute.EnumValue;
			}	

			EditorGUILayout.HelpBox($"The provided condition \"{conditionalAttribute.ConditionName}\" is not a valid boolean or an enum", MessageType.Error);

			return false;
		}

		public static object DrawField(Type fieldType, string fieldName, object fieldValue)
		{
			fieldName = char.ToUpper(fieldName[0]) + fieldName[1..]; // Uppercase the first character of the name

			bool isDBNull = Convert.IsDBNull(fieldValue);

			if (fieldType == typeof(string))
			{
				return EditorGUILayout.TextField(fieldName, fieldValue.ToString());
			}
			else if (fieldType == typeof(int))
			{
				return EditorGUILayout.IntField(fieldName, isDBNull ? 0 : Convert.ToInt32(fieldValue));
			}
			else if (fieldType == typeof(float))
			{
				return EditorGUILayout.FloatField(fieldName, isDBNull ? 0.0f : Convert.ToSingle(fieldValue));
			}
			else if (fieldType == typeof(double))
			{
				return EditorGUILayout.DoubleField(fieldName, isDBNull ? 0.0 : (double)fieldValue);
			}
			else if (fieldType == typeof(bool))
			{
				return EditorGUILayout.Toggle(fieldName, !isDBNull && (bool)fieldValue);
			}
			else if (fieldType.IsEnum)
			{
				return EditorGUILayout.EnumPopup(fieldName, isDBNull ? Enum.ToObject(fieldType, 0) as Enum : Enum.ToObject(fieldType, fieldValue) as Enum);
			}
			else if (fieldType == typeof(GameObject))
			{
				return EditorGUILayout.ObjectField(fieldName, isDBNull ? null : (GameObject)fieldValue, typeof(GameObject), true);
			}
			else if (fieldType == typeof(Vector2))
			{
				return EditorGUILayout.Vector2Field(fieldName, isDBNull ? Vector2.zero : (Vector2)fieldValue);
			}
			else if (fieldType == typeof(Vector2Int))
			{
				return EditorGUILayout.Vector2IntField(fieldName, isDBNull ? Vector2Int.zero : (Vector2Int)fieldValue);
			}
			else if (fieldType == typeof(Vector3))
			{
				return EditorGUILayout.Vector3Field(fieldName, isDBNull ? Vector3.zero : (Vector3)fieldValue);
			}
			else if (fieldType == typeof(Vector3Int))
			{
				return EditorGUILayout.Vector3IntField(fieldName, isDBNull ? Vector3Int.zero : (Vector3Int)fieldValue);
			}
			else if (fieldType == typeof(Vector4))
			{
				return EditorGUILayout.Vector4Field(fieldName, isDBNull ? Vector4.zero : (Vector4)fieldValue);
			}
			else if (fieldType == typeof(Color))
			{
				return EditorGUILayout.ColorField(fieldName, isDBNull ? Color.white : (Color)fieldValue);
			}
			else if (fieldType == typeof(Gradient))
			{
				return EditorGUILayout.GradientField(fieldName, isDBNull ? new Gradient() : (Gradient)fieldValue);
			}
			else if (fieldType == typeof(AnimationCurve))
			{
				return EditorGUILayout.CurveField(fieldName, isDBNull ? AnimationCurve.Linear(0f, 0f, 1f, 1f) : (AnimationCurve)fieldValue);
			}
			else if (fieldType == typeof(LayerMask))
			{
				return EditorGUILayout.LayerField(fieldName, isDBNull ? 0 : Convert.ToInt32(fieldValue));
			}
			else if (fieldType == typeof(Rect))
			{
				return EditorGUILayout.RectField(fieldName, isDBNull ? new Rect(0f, 0f, 0f, 0f) : (Rect)fieldValue);
			}
			else if (fieldType == typeof(RectInt))
			{
				return EditorGUILayout.RectIntField(fieldName, isDBNull ? new RectInt(0, 0, 0, 0) : (RectInt)fieldValue);
			}
			else
			{
				EditorGUILayout.HelpBox($"The type {fieldType} is not supported", MessageType.Warning);
				return null;
			}
		}

		public static string GetDynamicString(string inputText, SerializedProperty property, IDynamicStringAttribute dynamicStringAttribute)
		{
			switch (dynamicStringAttribute.StringInputMode)
			{
				default:
				case StringInputMode.Constant:
					return inputText;

				case StringInputMode.Dynamic:
					var memberInfo = ReflectionUtility.GetValidMemberInfo(inputText, property);

					if (memberInfo == null)
					{
						EditorGUILayout.HelpBox($"The member {inputText} could not be found", MessageType.Error);
						return inputText;
					}

					var memberValue = ReflectionUtility.GetMemberInfoValue(memberInfo, property);
					var memberType = ReflectionUtility.GetMemberInfoType(memberInfo);

					if (memberValue == null)
						return inputText;

					if (memberType == typeof(string))
						return memberValue.ToString();

					EditorGUILayout.HelpBox($"The member {inputText} needs to be a string", MessageType.Error);
					return inputText;
			}
		}

		public static Vector2Int Vector3IntToVector2Int(Vector3Int vector3Int) => new(vector3Int.x, vector3Int.y);

		public static Color GUIColorToColor(IColorAttribute colorAttribute)
		{
			if (colorAttribute.UseRGB) return new(colorAttribute.R / 255f, colorAttribute.G / 255f, colorAttribute.B / 255f);

			return colorAttribute.Color switch
			{
				GUIColor.Black => Color.black,
				GUIColor.Gray => Color.gray,
				GUIColor.Red => Color.red,
				GUIColor.Green => Color.green,
				GUIColor.Blue => Color.blue,
				GUIColor.Cyan => Color.cyan,
				GUIColor.Magenta => Color.magenta,
				GUIColor.Yellow => Color.yellow,
				GUIColor.Orange => new(1f, 149f / 255f, 0f),
				GUIColor.Brown => new(161f / 255f, 62f / 255f, 0f),
				GUIColor.Purple => new(158f / 255f, 5f / 255f, 247f / 255f),
				GUIColor.Pink => new(247f / 255f, 5f / 255f, 171f / 255f),
				GUIColor.Lime => new(145f / 255f, 1f, 0f),
				_ => Color.white
			};
		}

		public static Color GUIColorToColor(IColorAttribute colorAttribute, float alpha)
		{
			if (colorAttribute.UseRGB) return new(colorAttribute.R / 255f, colorAttribute.G / 255f, colorAttribute.B / 255f, alpha);

			return colorAttribute.Color switch
			{
				GUIColor.Black => new(Color.black.r, Color.black.g, Color.black.b, alpha),
				GUIColor.Gray => new(Color.gray.r, Color.gray.g, Color.gray.b, alpha),
				GUIColor.Red => new(Color.red.r, Color.red.g, Color.red.b, alpha),
				GUIColor.Green => new(Color.green.r, Color.green.g, Color.green.b, alpha),
				GUIColor.Blue => new(Color.blue.r, Color.blue.g, Color.blue.b, alpha),
				GUIColor.Cyan => new(Color.cyan.r, Color.cyan.g, Color.cyan.b, alpha),
				GUIColor.Magenta => new(Color.magenta.r, Color.magenta.g, Color.magenta.b, alpha),
				GUIColor.Yellow => new(Color.yellow.r, Color.yellow.g, Color.yellow.b, alpha),
				GUIColor.Orange => new(1f, 149f / 255f, 0f, alpha),
				GUIColor.Brown => new(161f / 255f, 62f / 255f, 0f, alpha),
				GUIColor.Purple => new(158f / 255f, 5f / 255f, 247f / 255f, alpha),
				GUIColor.Pink => new(247f / 255f, 5f / 255f, 171f / 255f, alpha),
				GUIColor.Lime => new(145f / 255f, 1f, 0f, alpha),
				_ => new(Color.white.r, Color.white.g, Color.white.b, alpha)
			};
		}
	}
}
