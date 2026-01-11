using System;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System.Collections;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    public abstract class CollectionDisplayDrawer : PropertyDrawerBase
    {
        protected readonly List<string> nullList = new() { "NULL" };

        /// <summary>
        /// Creates a dropdown field with all the basic setup for displaying collections
        /// </summary>
        /// <param name="choices">The choices for the dropdown</param>
        /// <param name="property">The serialized property this dropdown attaches to</param>
        /// <returns>The dropdown field created</returns>
        protected virtual DropdownField CreateDropdownField(List<string> choices, SerializedProperty property)
        {
            DropdownField dropdownField = IsCollectionValid(choices) ? new(property.displayName, choices, SetDropdownDefaultValue(choices, property)) : new(property.displayName, nullList, 0);

            dropdownField.tooltip = property.tooltip;
            dropdownField.showMixedValue = property.hasMultipleDifferentValues;
            dropdownField.AddToClassList(BaseField<Void>.alignedFieldUssClassName);

            AddPropertyContextMenu(dropdownField, property);

            if (dropdownField.value != "NULL")
                SetPropertyValueFromDropdown(property, dropdownField);

            dropdownField.TrackPropertyValue(property, (trackedProperty) => SetDropdownValueFromProperty(trackedProperty, dropdownField));
            dropdownField.RegisterValueChangedCallback((callback) => SetPropertyValueFromDropdown(property, dropdownField));
            dropdownField.RegisterCallbackOnce<GeometryChangedEvent>((callback) => dropdownField.Q(className: DropdownField.inputUssClassName).style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f);

            return dropdownField;
        }

        /// <summary>
        /// Gets the initial value for when the dropdown is created in the inspector
        /// </summary>
        /// <param name="collectionValues">The collection linked to the dropdown</param>
        /// <param name="property">The serialized property attached to the dropdown</param>
        /// <returns>The string value set to the dropdown</returns>
        protected virtual string SetDropdownDefaultValue(List<string> collectionValues, SerializedProperty property) => collectionValues.Contains(property.stringValue) ? property.stringValue : collectionValues[0];

        /// <summary>
        /// Sets the value of the property from the dropdown selection
        /// </summary>
        /// <param name="property">The property attached to the dropdown</param>
        /// <param name="dropdownField">The dropdown field</param>
        protected virtual void SetPropertyValueFromDropdown(SerializedProperty property, DropdownField dropdownField)
        {
            if (property.hasMultipleDifferentValues)
                return;

            SetPropertyValueFromString(dropdownField.value, property);
        }

        /// <summary>
        /// Sets the value of the dropdown from the property value
        /// </summary>
        /// <param name="trackedProperty">The property attached to the dropdown</param>
        /// <param name="dropdownField">The dropdown field</param>
        protected virtual void SetDropdownValueFromProperty(SerializedProperty trackedProperty, DropdownField dropdownField)
        {
            if (dropdownField.choices.Contains(trackedProperty.stringValue))
            {
                dropdownField.SetValueWithoutNotify(trackedProperty.stringValue);
            }
            else
            {
                Debug.LogWarning($"The value <b>{trackedProperty.stringValue}</b> set to the <b>{trackedProperty.name}</b> variable is not a value available in the dropdown", trackedProperty.serializedObject.targetObject);
            }
        }

        /// <summary>
        /// Gets the display names from the list or dictionary
        /// </summary>
        /// <param name="collectionInfo">The member info of the collection</param>
        /// <param name="displayNamesAttribute">The attribute containing the display names</param>
        /// <param name="serializedProperty">The target property</param>
        /// <param name="propertyValues">A collection of all property values as a string</param>
        /// <returns>A list with the display names</returns>
        protected List<string> GetDisplayValues(MemberInfo collectionInfo, IDisplayNamesAttribute displayNamesAttribute, SerializedProperty serializedProperty, List<string> propertyValues)
        {
            List<string> displayStrings = new();

            if (displayNamesAttribute.DisplayNames == null)
            {
                if (IsCollectionDictionary(collectionInfo, serializedProperty, out IDictionary dictionary))
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
                displayStrings = displayNamesAttribute.DisplayNames.ToList();
            }

            return displayStrings;
        }

        /// <summary>
        /// Checks if a collection is a dictionary
        /// </summary>
        /// <param name="collectionInfo">The member info of the collection</param>
        /// <param name="serializedProperty">The target property</param>
        /// <param name="dictionary">The collection as a dictionary</param>
        /// <returns>True if the collection is an IDictionary, false otherwise</returns>
        protected bool IsCollectionDictionary(MemberInfo collectionInfo, SerializedProperty serializedProperty, out IDictionary dictionary)
        {
            object collectionValue = ReflectionUtils.GetMemberInfoValue(collectionInfo, serializedProperty);

            dictionary = collectionValue as IDictionary;
            return collectionValue is IDictionary;
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
            List<string> stringList = new();
            object memberInfoValue = ReflectionUtils.GetMemberInfoValue(memberInfo, serializedProperty);

            if (memberInfoValue is Array array)
            {
                foreach (var item in array)
                    stringList.Add(item == null ? "NULL" : item.ToString());
            }
            else if (memberInfoValue is IList list)
            {
                foreach (var item in list)
                    stringList.Add(item == null ? "NULL" : item.ToString());
            }
            else if (memberInfoValue is IDictionary dictionary)
            {
                foreach (DictionaryEntry item in dictionary)
                    stringList.Add(item.Value == null ? "NULL" : item.Value.ToString());
            }
            else
            {
                errorBox.text = $"Could not find the collection <b>{collectionName}</b>";
            }

            return stringList;
        }
    }
}
