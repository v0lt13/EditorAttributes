using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ValueButtonsAttribute))]
    public class ValueButtonsDrawer : CollectionDisplayDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var valueButtonsAttribute = attribute as ValueButtonsAttribute;

            HelpBox errorBox = new();
            MemberInfo collectionInfo = ReflectionUtils.GetValidMemberInfo(valueButtonsAttribute.CollectionName, property);

            List<string> propertyValues = ConvertCollectionValuesToStrings(valueButtonsAttribute.CollectionName, property, collectionInfo, errorBox);
            List<string> displayValues = GetDisplayValues(collectionInfo, valueButtonsAttribute, property, propertyValues);

            if (!IsCollectionValid(displayValues))
                return new HelpBox("The provided collection is empty", HelpBoxMessageType.Error);

            int buttonsValueIndex = propertyValues.IndexOf(GetPropertyValueAsString(property));

            ToggleButtonGroup valueButtons = DrawButtons(buttonsValueIndex, displayValues, valueButtonsAttribute, (value) =>
            {
                if (valueButtonsAttribute.DisplayNames != null || IsCollectionDictionary(collectionInfo, property, out _))
                {
                    if (value >= 0 && value < propertyValues.Count)
                        SetPropertyValueFromString(propertyValues[value], property);
                }
                else
                {
                    if (value >= 0 && value < propertyValues.Count)
                        SetPropertyValueFromString(propertyValues[value], property);
                }
            });

            valueButtons.TrackPropertyValue(property, (trackedProperty) =>
            {
                string propertyStringValue = GetPropertyValueAsString(trackedProperty);

                if (propertyValues.Contains(propertyStringValue))
                {
                    int propertyValueIndex = propertyValues.IndexOf(propertyStringValue);
                    bool[] selectionValues = new bool[propertyValues.Count];

                    selectionValues[propertyValueIndex] = true;

                    valueButtons.SetValueWithoutNotify(ToggleButtonGroupState.CreateFromOptions(selectionValues));
                }
                else
                {
                    Debug.LogWarning($"The value <b>{propertyStringValue}</b> set to the <b>{trackedProperty.name}</b> variable is not a value available in the button selection", trackedProperty.serializedObject.targetObject);
                }
            });

            AddPropertyContextMenu(valueButtons, property);
            DisplayErrorBox(valueButtons, errorBox);

            return valueButtons;
        }

        private ToggleButtonGroup DrawButtons(int buttonsValue, List<string> valueLabels, ValueButtonsAttribute selectionButtonsAttribute, Action<int> onValueChanged)
        {
            List<bool> activeButtonList = new();
            ToggleButtonGroup buttonGroup = new(selectionButtonsAttribute.ShowLabel ? preferredLabel : string.Empty);

            foreach (string label in valueLabels)
            {
                Button toggle = new()
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
    }
}
