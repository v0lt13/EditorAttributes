using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using UnityEditorInternal;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    public class PropertyDrawerBase : PropertyDrawer
    {
		protected readonly bool canApplyGlobalColor = EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR;
		protected UnityEventDrawer eventDrawer;

		public override VisualElement CreatePropertyGUI(SerializedProperty property) => DrawProperty(property);	

		protected virtual VisualElement DrawProperty(SerializedProperty property, Label label = null)
		{
			eventDrawer ??= new UnityEventDrawer();

			try
			{
				var eventContainer = eventDrawer.CreatePropertyGUI(property);
				var eventLabel = eventContainer.Q<Label>();

				eventLabel.text = label == null ? eventLabel.text : "";

				return eventContainer;
			}
			catch (NullReferenceException)
			{
				label ??= new Label(property.displayName);

				var propertyField = new PropertyField(property, label.text);
				
				propertyField.BindProperty(property);

				return propertyField;
			}
		}

		protected static void SetProperyValueFromString(string value, ref SerializedProperty property, HelpBox errorBox)
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

					case SerializedPropertyType.Boolean:
						property.boolValue = Convert.ToBoolean(value);
						break;

					case SerializedPropertyType.String:
						property.stringValue = value;
						break;

					default:
						errorBox.text = $"The type {property.propertyType} is not supported";
						errorBox.messageType = HelpBoxMessageType.Warning;
						break;
				}
			}
			catch (FormatException)
			{
				errorBox.text = $"Could not convert the value \"{value}\" to {property.propertyType}";
			}
		}

		protected static string GetPropertyValueAsString(SerializedProperty property)
		{
			return property.propertyType switch
			{
				SerializedPropertyType.Integer => property.intValue.ToString(),
				SerializedPropertyType.Float => property.floatValue.ToString(),
				SerializedPropertyType.Boolean => property.boolValue.ToString(),
				SerializedPropertyType.String => property.stringValue,
				_ => string.Empty
			};
		}

		internal static bool IsCollectionValid(ICollection collection) => collection != null && collection.Count != 0;

		protected static List<string> GetCollectionValuesAsString(string collectionName, SerializedProperty serializedProperty, MemberInfo memberInfo, HelpBox errorBox)
		{
			var stringList = new List<string>();
			var memberInfoValue = ReflectionUtility.GetMemberInfoValue(memberInfo, serializedProperty);

			if (memberInfoValue is Array array)
			{
				foreach (var item in array) 
					stringList.Add(item.ToString());
			}
			else if (memberInfoValue is IList list)
			{
				foreach (var item in list) 
					stringList.Add(item.ToString());
			}
			else
			{
				errorBox.text = $"Could not find the collection {collectionName}";
			}

			return stringList;
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

		protected void Print(object message) => Debug.Log(message);

		public static void DisplayErrorBox(VisualElement root, HelpBox errorBox)
		{
			errorBox.messageType = HelpBoxMessageType.Error;

			if (!string.IsNullOrEmpty(errorBox.text))
				root.Add(errorBox);
		}

		public static void UpdateVisualElement(VisualElement visualElement, Action logicToUpdate, long intervalMs = 60) => visualElement.schedule.Execute(logicToUpdate).Every(intervalMs);

		public static void RemoveElement(VisualElement root, VisualElement element)
		{
			if (root.Contains(element))
				root.Remove(element);
		}

		public static bool GetConditionValue(MemberInfo memberInfo, IConditionalAttribute conditionalAttribute, SerializedProperty serializedProperty, HelpBox errorBox)
		{
			var memberInfoType = ReflectionUtility.GetMemberInfoType(memberInfo);

			if (memberInfoType == null)
			{
				errorBox.text = $"The provided condition \"{conditionalAttribute.ConditionName}\" could not be found";
				return false;
			}

			if (memberInfoType == typeof(bool))
			{
				var memberInfoValue = ReflectionUtility.GetMemberInfoValue(memberInfo, serializedProperty);

				if (memberInfoValue == null)
					return false;

				return (bool)memberInfoValue;
			}
			else if (memberInfoType.IsEnum)
			{
				var memberInfoValue = ReflectionUtility.GetMemberInfoValue(memberInfo, serializedProperty);

				if (memberInfoValue == null)
					return false;

				return (int)memberInfoValue == conditionalAttribute.EnumValue;
			}

			errorBox.text = $"The provided condition \"{conditionalAttribute.ConditionName}\" is not a valid boolean or an enum";

			return false;
		}

		internal static bool GetConditionValue(MemberInfo memberInfo, IConditionalAttribute conditionalAttribute, object targetObject, HelpBox errorBox) // Internal function used for the button drawer
		{			
			var memberInfoType = ReflectionUtility.GetMemberInfoType(memberInfo);

			if (memberInfoType == null)
			{
				errorBox.text = $"The provided condition \"{conditionalAttribute.ConditionName}\" could not be found";
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

			errorBox.text = $"The provided condition \"{conditionalAttribute.ConditionName}\" is not a valid boolean or an enum";
			return false;
		}

		public static string GetDynamicString(string inputText, SerializedProperty property, IDynamicStringAttribute dynamicStringAttribute, HelpBox errorBox)
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
						errorBox.text = $"The member {inputText} could not be found";
						return inputText;
					}

					var memberValue = ReflectionUtility.GetMemberInfoValue(memberInfo, property);
					var memberType = ReflectionUtility.GetMemberInfoType(memberInfo);

					if (memberValue == null)
						return inputText;

					if (memberType == typeof(string))
						return memberValue.ToString();

					errorBox.text = $"The member {inputText} needs to be a string";
					return inputText;
			}
		}

		public static Vector2Int Vector3IntToVector2Int(Vector3Int vector3Int) => new(vector3Int.x, vector3Int.y);

		public static void ApplyBoxStyle(VisualElement visualElement)
		{
			visualElement.style.borderTopLeftRadius = 3f;
			visualElement.style.borderTopRightRadius = 3f;
			visualElement.style.borderBottomLeftRadius = 3f;
			visualElement.style.borderBottomRightRadius = 3f;

			visualElement.style.borderBottomWidth = 1f;
			visualElement.style.borderTopWidth = 1f;
			visualElement.style.borderLeftWidth = 1f;
			visualElement.style.borderRightWidth = 1f;

			visualElement.style.borderTopColor = new Color(26f / 255f, 26f / 255f, 26f / 255f);
			visualElement.style.borderBottomColor = new Color(26f / 255f, 26f / 255f, 26f / 255f);
			visualElement.style.borderLeftColor = new Color(26f / 255f, 26f / 255f, 26f / 255f);
			visualElement.style.borderRightColor = new Color(26f / 255f, 26f / 255f, 26f / 255f);

			visualElement.style.backgroundColor = EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR ? EditorExtension.GLOBAL_COLOR / 2f : new Color(63f / 255f, 63f / 255f, 63f / 255f);

			visualElement.style.paddingTop = 3f;
			visualElement.style.paddingBottom = 3f;
			visualElement.style.paddingLeft = 3f;
			visualElement.style.paddingRight = 3f;

			visualElement.style.marginTop = 1f;
			visualElement.style.marginBottom = 1f;
			visualElement.style.marginRight = 3f;
			visualElement.style.marginLeft = 3f;
		}
	}
}
