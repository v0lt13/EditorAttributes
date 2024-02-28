using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(SelectionButtonsAttribute))]
    public class SelectionButtonsDrawer : PropertyDrawerBase
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var selectionButtonsAttribute = attribute as SelectionButtonsAttribute;

			if (property.propertyType == SerializedPropertyType.Enum)
			{
				Type enumType = fieldInfo.FieldType;

				bool isFlagsEnum = enumType.IsDefined(typeof(FlagsAttribute), false);

				if (isFlagsEnum)
				{
					property.enumValueFlag = DrawEnumFlagButtons(position, property.enumDisplayNames, label, property.enumValueFlag);
				}
				else
				{
					property.enumValueIndex = DrawButtons(position, label, property.enumValueIndex, property.enumDisplayNames, selectionButtonsAttribute);
				}
			}
			else if (property.propertyType != SerializedPropertyType.Enum && !string.IsNullOrEmpty(selectionButtonsAttribute.CollectionName))
			{
				var memberInfo = ReflectionUtility.GetValidMemberInfo(selectionButtonsAttribute.CollectionName, property);

				var displayNames = GetArrayValues(property, memberInfo);
				int selectedIndex = 0;

				for (int i = 0; i < displayNames.Length; i++)
				{
					if (displayNames[i] == GetPropertyValueAsString(property)) selectedIndex = i;
				}

				selectedIndex = DrawButtons(position, label, selectedIndex, displayNames, selectionButtonsAttribute);

				if (selectedIndex >= 0 && selectedIndex < displayNames.Length) SetProperyValueFromString(displayNames[selectedIndex], ref property);
			}
			else
			{
				EditorGUILayout.HelpBox("If the attached field is not an enum, a collection name must be provided", MessageType.Error);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var selectionButtonsAttribute = attribute as SelectionButtonsAttribute;

			return selectionButtonsAttribute.ButtonsHeight;
		}

		private string[] GetArrayValues(SerializedProperty serializedProperty, MemberInfo memberInfo)
		{
			var selectionButtonsAttribute = attribute as SelectionButtonsAttribute;

			var stringList = new List<string>();
			var memberInfoValue = ReflectionUtility.GetMemberInfoValue(memberInfo, serializedProperty);

			if (memberInfoValue is Array array)
			{
				foreach (var item in array) stringList.Add(item.ToString());
			}
			else if (memberInfoValue is IList list)
			{
				foreach (var item in list) stringList.Add(item.ToString());
			}
			else
			{
				EditorGUILayout.HelpBox($"Could not find the collection {selectionButtonsAttribute.CollectionName}", MessageType.Error);
			}

			return stringList.ToArray();
		}

		private int DrawButtons(Rect position, GUIContent label, int selectedButton, string[] valueLabels, SelectionButtonsAttribute selectionButtonsAttribute)
		{
			if (selectionButtonsAttribute.ShowLabel)
			{
				EditorGUI.LabelField(position, label);

				return GUI.Toolbar(new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, position.height), selectedButton, valueLabels);
			}
			else
			{
				return GUI.Toolbar(position, selectedButton, valueLabels);
			}
		}

		private int DrawEnumFlagButtons(Rect position, string[] valueLabels, GUIContent label, int fieldValue)
		{
			if (valueLabels == null || valueLabels.Length == 0)
			{
				EditorGUI.HelpBox(position, "No values provided for selection buttons", MessageType.Error);
				return fieldValue;
			}
			var selectionButtonsAttribute = attribute as SelectionButtonsAttribute;
			var labelPosition = position;
			var buttonsPosition = position;

			if (selectionButtonsAttribute.ShowLabel)
			{
				EditorGUI.LabelField(labelPosition, label);

				labelPosition.x += EditorGUIUtility.labelWidth;
				labelPosition.width -= EditorGUIUtility.labelWidth;

				buttonsPosition.x += EditorGUIUtility.labelWidth;
				buttonsPosition.width -= EditorGUIUtility.labelWidth;
			}

			for (int i = 0; i < valueLabels.Length; i++)
			{
				int enumValue = 1 << i;
				bool isSelected = (fieldValue & enumValue) != 0;

				var buttonRect = new Rect(buttonsPosition.x + (i * (buttonsPosition.width / valueLabels.Length)), buttonsPosition.y, buttonsPosition.width / valueLabels.Length, buttonsPosition.height);

				GUIStyle buttonStyle;

				if (i == 0)
				{
					buttonStyle = EditorStyles.miniButtonLeft;
				}
				else if (i == valueLabels.Length - 1)
				{
					buttonStyle = EditorStyles.miniButtonRight;
				}
				else
				{
					buttonStyle = EditorStyles.miniButtonMid;
				}

				bool toggleValue = GUI.Toggle(buttonRect, isSelected, valueLabels[i], buttonStyle);

				if (toggleValue != isSelected)
				{
					if (toggleValue)
					{
						fieldValue |= enumValue;
					}
					else
					{
						fieldValue &= ~enumValue;
					}
				}
			}

			return fieldValue;
		}

	}
}
