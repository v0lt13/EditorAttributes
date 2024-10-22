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
		private protected UnityEventDrawer eventDrawer;

		protected bool CanApplyGlobalColor => EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR;				

		public override VisualElement CreatePropertyGUI(SerializedProperty property) => DrawProperty(property);	

		/// <summary>
		/// Draws a property field also accounting for Unity Events
		/// </summary>
		/// <param name="property">The serialized property to draw</param>
		/// <param name="label">Add a custom label to the property</param>
		/// <returns>The property visual element</returns>
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

		/// <summary>
		/// Sets the value of a property from a string
		/// </summary>
		/// <param name="value">The string value to convert</param>
		/// <param name="property">The serialized property reference to assign the value to</param>
		/// <param name="errorBox">The error box to display any errors to</param>
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

		/// <summary>
		/// Gets the value of a serialzied property and returns it as a string
		/// </summary>
		/// <param name="property">The serialized property to get the value from</param>
		/// <returns>The serialized property value as a string</returns>
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

		/// <summary>
		/// Converts the values of a collection into strings
		/// </summary>
		/// <param name="collectionName">The name of the collection to convert</param>
		/// <param name="serializedProperty">The serialized property</param>
		/// <param name="memberInfo">The member info of the collection</param>
		/// <param name="errorBox">The error box to display any errors to</param>
		/// <returns>The values of the collection in a list of strings</returns>
		protected static List<string> ConvertCollectionValuesToStrings(string collectionName, SerializedProperty serializedProperty, MemberInfo memberInfo, HelpBox errorBox)
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

		/// <summary>
		/// Finds a nested serialized property
		/// </summary>
		/// <param name="property">The serialized property</param>
		/// <param name="propertyName">The name of the property to find</param>
		/// <returns>The nested serialized property</returns>
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

		/// <summary>
		/// Gets the name of a serialized property accounting for C# properties
		/// </summary>
		/// <param name="propertyName">The name of the property to look for</param>
		/// <param name="property">The serialized property</param>
		/// <returns>The name of the serialized property</returns>
		public static string GetSerializedPropertyName(string propertyName, SerializedProperty property)
		{
			var memberInfo = ReflectionUtility.GetValidMemberInfo(propertyName, property);

			return memberInfo is PropertyInfo ? $"<{propertyName}>k_BackingField" : propertyName;
		}

		/// <summary>
		/// Displays an error box in the inspector
		/// </summary>
		/// <param name="root">The root visual element</param>
		/// <param name="errorBox">The help box to displaying the errors</param>
		public static void DisplayErrorBox(VisualElement root, HelpBox errorBox)
		{
			errorBox.messageType = HelpBoxMessageType.Error;

			if (!string.IsNullOrEmpty(errorBox.text))
			{
				root.Add(errorBox);
			}
			else
			{
				RemoveElement(root, errorBox);
			}
		}

		/// <summary>
		/// Update logic for a visual element
		/// </summary>
		/// <param name="visualElement">The element to update</param>
		/// <param name="logicToUpdate">The logic to update</param>
		protected void UpdateVisualElement(VisualElement visualElement, Action logicToUpdate) => visualElement.schedule.Execute(() => EditorExtension.AddToUpdateLoop(logicToUpdate)).ExecuteLater(1);

		/// <summary>
		/// Updates a visual element at a set interval
		/// </summary>
		/// <param name="visualElement">The visual element to update</param>
		/// <param name="logicToUpdate">The logic to execute on the specified element</param>
		/// <param name="intervalMs">The update interval in milliseconds</param>
		public static void UpdateVisualElementAtInterval(VisualElement visualElement, Action logicToUpdate, long intervalMs = 60) => visualElement.schedule.Execute(logicToUpdate).Every(intervalMs);

		/// <summary>
		/// Add an element from another visual element if it doesn't exist
		/// </summary>
		/// <param name="root">The root to add the element on</param>
		/// <param name="element">The element to add</param>
		public static void AddElement(VisualElement root, VisualElement element)
		{
			if (!root.Contains(element))
				root.Add(element);
		}

		/// <summary>
		/// Removes an element from another visual element if it exists
		/// </summary>
		/// <param name="owner">The owner containing the element</param>
		/// <param name="element">The element to remove</param>
		public static void RemoveElement(VisualElement owner, VisualElement element)
		{
			if (owner.Contains(element))
				owner.Remove(element);
		}

		/// <summary>
		/// Gets the value of a condition for a conditional attribute
		/// </summary>
		/// <param name="memberInfo">The member info of the condition</param>
		/// <param name="conditionalAttribute">The conditional attribute</param>
		/// <param name="serializedProperty">The serialized property</param>
		/// <param name="errorBox">The error box to display any errors to</param>
		/// <returns>True if the condition is satisfied</returns>
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

		/// <summary>
		/// Gets the string value from a member if the input mode is set to Dynamic
		/// </summary>
		/// <param name="inputText">The string input that may contain the member name</param>
		/// <param name="property">The serialized property</param>
		/// <param name="dynamicStringAttribute">The dynamic string attribute</param>
		/// <param name="errorBox">The error box to display any errors to</param>
		/// <returns>If the input mode is Constant will return the base input string, if is Dynamic will return the string value of the member</returns>
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

					errorBox.text = $"The member {inputText} needs to return a string";
					return inputText;
			}
		}

		/// <summary>
		/// Applies the help box style to a visual element
		/// </summary>
		/// <param name="visualElement">The element to apply the style to</param>
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

		#region NON_GUI_RELATED_UTILITY_FUNCITONS

		/// <summary>
		/// A short handy version of Debug.Log
		/// </summary>
		/// <param name="message">The message to print</param>
		protected void Print(object message) => Debug.Log(message);

		/// <summary>
		/// Checks if a collection is null or has no members
		/// </summary>
		/// <param name="collection">The collection to check</param>
		/// <returns>False is the collection is null or has no members, true otherwise</returns>
		public static bool IsCollectionValid(ICollection collection) => collection != null && collection.Count != 0;

		/// <summary>
		/// Gets the size of a 2D texture
		/// </summary>
		/// <param name="texture">The texture to get the size from</param>
		/// <returns>The width and height of the texture as a Vector2</returns>
		public static Vector2 GetTextureSize(Texture2D texture) => new(texture.width, texture.height);

		#endregion
	}
}
