using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownDrawer : CollectionDisplayDrawer
    {
        private MemberInfo collectionInfo;
        private List<string> propertyValues = new();
        private List<string> displayValues = new();

        private DropdownAttribute DropdownAttribute => attribute as DropdownAttribute;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            HelpBox errorBox = new();

            collectionInfo = ReflectionUtils.GetValidMemberInfo(DropdownAttribute.CollectionName, property);
            propertyValues = ConvertCollectionValuesToStrings(DropdownAttribute.CollectionName, property, collectionInfo, errorBox);
            displayValues = GetDisplayValues(collectionInfo, DropdownAttribute, property, propertyValues);

            DropdownField dropdownField = CreateDropdownField(displayValues, property);

            UpdateVisualElement(dropdownField, () =>
            {
                List<string> currentPropertyValues = ConvertCollectionValuesToStrings(DropdownAttribute.CollectionName, property, collectionInfo, errorBox);
                List<string> currentDisplayValues = GetDisplayValues(collectionInfo, DropdownAttribute, property, currentPropertyValues);

                if (IsCollectionValid(currentPropertyValues))
                {
                    errorBox.text = string.Empty;
                    dropdownField.choices = currentDisplayValues;

                    displayValues = currentDisplayValues;
                    propertyValues = currentPropertyValues;
                }
                else
                {
                    dropdownField.choices = nullList;
                    propertyValues = nullList;
                    displayValues = nullList;
                }

                if (HasMismatchedDisplayCollectionCounts(currentPropertyValues, currentDisplayValues))
                    errorBox.text = "The value collection item count and display names count do not match";

                DisplayErrorBox(dropdownField, errorBox);
            });

            return dropdownField;
        }

        protected override string CopyValue(VisualElement element, SerializedProperty property)
        {
            var dropdown = element as DropdownField;

            return DropdownAttribute.DisplayNames != null ? dropdown.value : base.CopyValue(element, property);
        }

        protected override void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue)
        {
            var dropdown = element as DropdownField;

            if (dropdown.choices.Contains(clipboardValue))
            {
                if (DropdownAttribute.DisplayNames != null)
                {
                    dropdown.value = clipboardValue;
                }
                else
                {
                    base.PasteValue(element, property, clipboardValue);
                    dropdown.SetValueWithoutNotify(clipboardValue);
                }
            }
            else
            {
                Debug.LogWarning($"Could not paste value <b>{clipboardValue}</b> since is not availiable as an option in the dropdown");
            }
        }

        protected override DropdownField CreateDropdownField(List<string> choices, SerializedProperty property)
        {
            DropdownField dropdownField = IsCollectionValid(choices) ? new(property.displayName, choices, GetDefaultValueIndex(choices, property)) : new(property.displayName, nullList, 0);

            dropdownField.tooltip = property.tooltip;
            dropdownField.showMixedValue = property.hasMultipleDifferentValues;
            dropdownField.AddToClassList(BaseField<Void>.alignedFieldUssClassName);

            AddPropertyContextMenu(dropdownField, property);

            if (dropdownField.value != "NULL" && !HasMismatchedDisplayCollectionCounts(propertyValues, displayValues))
                SetPropertyValueFromDropdown(property, dropdownField);

            dropdownField.TrackPropertyValue(property, (trackedProperty) => SetDropdownValueFromProperty(trackedProperty, dropdownField));
            dropdownField.RegisterValueChangedCallback((callback) => SetPropertyValueFromDropdown(property, dropdownField));
            dropdownField.RegisterCallbackOnce<GeometryChangedEvent>((callback) => dropdownField.Q(className: DropdownField.inputUssClassName).style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f);

            return dropdownField;
        }

        protected override void SetPropertyValueFromDropdown(SerializedProperty property, DropdownField dropdown)
        {
            if (property.hasMultipleDifferentValues)
                return;

            if (DropdownAttribute.DisplayNames != null || IsCollectionDictionary(collectionInfo, property, out _))
            {
                SetPropertyValueFromString(propertyValues[dropdown.index], property);
            }
            else
            {
                SetPropertyValueFromString(dropdown.value, property);
            }
        }

        protected override void SetDropdownValueFromProperty(SerializedProperty trackedProperty, DropdownField dropdownField)
        {
            string propertyStringValue = GetPropertyValueAsString(trackedProperty);

            if (propertyValues.Contains(propertyStringValue))
            {
                dropdownField.SetValueWithoutNotify(displayValues[propertyValues.IndexOf(propertyStringValue)]);
            }
            else
            {
                Debug.LogWarning($"The value <b>{propertyStringValue}</b> set to the <b>{trackedProperty.name}</b> variable is not a value available in the dropdown", trackedProperty.serializedObject.targetObject);
            }
        }

        private int GetDefaultValueIndex(List<string> collectionValues, SerializedProperty property)
        {
            string propertyStringValue = GetPropertyValueAsString(property);
            return collectionValues.Contains(propertyStringValue) ? collectionValues.IndexOf(propertyStringValue) : 0;
        }

        private bool HasMismatchedDisplayCollectionCounts(List<string> propertyValues, List<string> collectionValues) => DropdownAttribute.DisplayNames != null && propertyValues.Count != collectionValues.Count;
    }
}
