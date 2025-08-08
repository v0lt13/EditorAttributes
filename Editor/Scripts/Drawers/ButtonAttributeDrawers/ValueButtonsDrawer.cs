using UnityEditor;
using UnityEngine.UIElements;

#if UNITY_6000_0_OR_NEWER
using System;
using System.Linq;
using UnityEngine;
using System.Reflection;
using System.Collections;
using UnityEditor.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;
#endif

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ValueButtonsAttribute))]
	public class ValueButtonsDrawer : PropertyDrawerBase
	{
#if !UNITY_6000_0_OR_NEWER
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();

			root.Add(new HelpBox("The <b>ValueButtons Attribute</b> is only available in <b>Unity 6 and above</b>, use the <b>SelectionButtons Attribute</b> for the same functionality", HelpBoxMessageType.Warning));

			return root;
		}
#else
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var valueButtonsAttribute = attribute as ValueButtonsAttribute;

			var root = new VisualElement();
			var errorBox = new HelpBox();

			var collectionInfo = ReflectionUtility.GetValidMemberInfo(valueButtonsAttribute.CollectionName, property);

			string[] propertyValues = ConvertCollectionValuesToStrings(valueButtonsAttribute.CollectionName, property, collectionInfo, errorBox).ToArray();
			string[] displayValues = GetDisplayValues(collectionInfo, valueButtonsAttribute, property, propertyValues.ToList());

			if (!IsCollectionValid(displayValues))
			{
				errorBox.text = "The provided collection is empty";
				DisplayErrorBox(root, errorBox);

				return root;
			}

			int buttonsValueIndex = Array.IndexOf(propertyValues, GetPropertyValueAsString(property));

			var valueButtons = DrawButtons(buttonsValueIndex, displayValues, valueButtonsAttribute, (value) =>
			{
				if (valueButtonsAttribute.DisplayNames != null || IsDictionary(collectionInfo, property, out _))
				{
					if (value >= 0 && value < propertyValues.Length)
						SetPropertyValueFromString(propertyValues[value], property);
				}
				else
				{
					if (value >= 0 && value < propertyValues.Length)
						SetPropertyValueFromString(propertyValues[value], property);
				}
			});

			valueButtons.TrackPropertyValue(property, (trackedProperty) =>
			{
				if (propertyValues.Contains(trackedProperty.boxedValue.ToString()))
				{
					int propertyValueIndex = Array.IndexOf(propertyValues, GetPropertyValueAsString(trackedProperty));
					bool[] selectionValues = new bool[propertyValues.Length];

					selectionValues[propertyValueIndex] = true;

					valueButtons.SetValueWithoutNotify(ToggleButtonGroupState.CreateFromOptions(selectionValues));
				}
				else
				{
					Debug.LogWarning($"The value <b>{trackedProperty.boxedValue}</b> set to the <b>{trackedProperty.name}</b> variable is not a value available in the button selection", trackedProperty.serializedObject.targetObject);
				}
			});

			AddPropertyContextMenu(valueButtons, property);

			root.Add(valueButtons);

			DisplayErrorBox(root, errorBox);

			return root;
		}

		private ToggleButtonGroup DrawButtons(int buttonsValue, string[] valueLabels, ValueButtonsAttribute selectionButtonsAttribute, Action<int> onValueChanged)
		{
			var activeButtonList = new List<bool>();
			var buttonGroup = new ToggleButtonGroup(selectionButtonsAttribute.ShowLabel ? preferredLabel : string.Empty);

			foreach (string label in valueLabels)
			{
				var toggle = new Button
				{
					text = label,
					style = { height = selectionButtonsAttribute.ButtonsHeight }
				};

				activeButtonList.Add(false);
				buttonGroup.Add(toggle);
			}

			activeButtonList[buttonsValue == -1 ? 0 : buttonsValue] = true;

			buttonGroup.SetValueWithoutNotify(ToggleButtonGroupState.CreateFromOptions(activeButtonList));
			buttonGroup.RegisterValueChangedCallback((value) => onValueChanged.Invoke(value.newValue.GetActiveOptions(ConvertBoolsToSpan(activeButtonList))[0]));

			return buttonGroup;
		}

		private static Span<int> ConvertBoolsToSpan(List<bool> boolList)
		{
			var intArray = new int[boolList.Count];

			for (int i = 0; i < boolList.Count; i++)
				intArray[i] = boolList[i] ? 1 : 0;

			return new Span<int>(intArray);
		}

		private string[] GetDisplayValues(MemberInfo collectionInfo, ValueButtonsAttribute valueButtonsAttribute, SerializedProperty serializedProperty, List<string> propertyValues)
		{
			var displayStrings = new List<string>();

			if (valueButtonsAttribute.DisplayNames == null)
			{
				if (IsDictionary(collectionInfo, serializedProperty, out IDictionary dictionary))
				{
					foreach (DictionaryEntry item in dictionary)
						displayStrings.Add(item.Key == null ? "NULL" : item.Key.ToString());
				}
				else
				{
					displayStrings = propertyValues;
				}
			}
			else
			{
				displayStrings = valueButtonsAttribute.DisplayNames.ToList();
			}

			return displayStrings.ToArray();
		}

		private bool IsDictionary(MemberInfo collectionInfo, SerializedProperty serializedProperty, out IDictionary dictionary)
		{
			var collectionValue = ReflectionUtility.GetMemberInfoValue(collectionInfo, serializedProperty);

			dictionary = collectionValue as IDictionary;

			return collectionValue is IDictionary;
		}
#endif
	}
}
