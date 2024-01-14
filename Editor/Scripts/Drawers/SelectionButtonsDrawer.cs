using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

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
					property.enumValueFlag = DrawEnumButtons(property.enumDisplayNames, label, property.enumValueFlag, true);
				}
				else
				{
					property.enumValueIndex = DrawEnumButtons(property.enumDisplayNames, label, property.enumValueIndex, false);
				}
			}
			else if (property.propertyType != SerializedPropertyType.Enum && !string.IsNullOrEmpty(selectionButtonsAttribute.CollectionName))
			{
				var memberInfo = ReflectionUtility.GetValidMemberInfo(selectionButtonsAttribute.CollectionName, property);

				var stringArray = GetArrayValues(property, memberInfo);
				int selectedIndex = 0;

				for (int i = 0; i < stringArray.Length; i++)
				{
					if (stringArray[i] == GetPropertyValueAsString(property)) selectedIndex = i;
				}

				selectedIndex = DrawSelectionButtons(stringArray, selectedIndex, label);

				if (selectedIndex >= 0 && selectedIndex < stringArray.Length) SetProperyValueFromString(stringArray[selectedIndex], ref property);
			}
			else
			{
				EditorGUILayout.HelpBox("If the attached field is not an enum, a collection name must be provided", MessageType.Error);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -EditorGUIUtility.standardVerticalSpacing;

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

		private T DrawEnumButtons<T>(string[] valueLabels, GUIContent label, T fieldValue, bool allowMultiselection)
		{
			if (valueLabels == null || valueLabels.Length == 0)
			{
				EditorGUILayout.HelpBox("No values provided for selection buttons", MessageType.Error);
				return fieldValue;
			}

			var selectionButtonsAttribute = attribute as SelectionButtonsAttribute;

			var selectionButtonStyle = new GUIStyle(EditorStyles.miniButtonMid)
			{
				fixedHeight = selectionButtonsAttribute.ButtonsHeight
			};

			EditorGUILayout.BeginHorizontal();

			if (selectionButtonsAttribute.ShowLabel) EditorGUILayout.LabelField(label);

			for (int i = 0; i < valueLabels.Length; i++)
			{
				bool isSelected;

				if (allowMultiselection)
				{
					int intValue = Convert.ToInt32(fieldValue);
					int enumValue = 1 << i;

					isSelected = (intValue & enumValue) != 0;
				}
				else
				{
					isSelected = EqualityComparer<T>.Default.Equals(fieldValue, (T)Convert.ChangeType(i, typeof(T)));
				}

				bool toggleValue = GUILayout.Toggle(isSelected, valueLabels[i], selectionButtonStyle);

				if (toggleValue != isSelected)
				{
					if (allowMultiselection)
					{
						int intValue = Convert.ToInt32(fieldValue);
						int enumValue = 1 << i;

						if (toggleValue)
						{
							intValue |= enumValue;
						}
						else
						{
							intValue &= ~enumValue;
						}

						fieldValue = (T)Convert.ChangeType(intValue, typeof(T));
					}
					else
					{
						fieldValue = (T)Convert.ChangeType(i, typeof(T));
					}
				}
			}

			EditorGUILayout.EndHorizontal();

			return fieldValue;
		}

		private int DrawSelectionButtons(string[] valueLabels, int selectedIndex, GUIContent label)
		{
			if (valueLabels == null || valueLabels.Length == 0)
			{
				EditorGUILayout.HelpBox("No values provided for selection buttons", MessageType.Error);
				return -1;
			}

			var selectionButtonsAttribute = attribute as SelectionButtonsAttribute;

			var selectionButtonStyle = new GUIStyle(EditorStyles.miniButtonMid)
			{
				fixedHeight = selectionButtonsAttribute.ButtonsHeight
			};

			EditorGUILayout.BeginHorizontal();

			if (selectionButtonsAttribute.ShowLabel) EditorGUILayout.LabelField(label);

			for (int i = 0; i < valueLabels.Length; i++)
			{
				bool isSelected = i == selectedIndex;

				bool toggleValue = GUILayout.Toggle(isSelected, valueLabels[i], selectionButtonStyle);

				if (toggleValue && !isSelected)
				{
					selectedIndex = i;
				}
			}

			EditorGUILayout.EndHorizontal();

			return selectedIndex;
		}
	}
}
