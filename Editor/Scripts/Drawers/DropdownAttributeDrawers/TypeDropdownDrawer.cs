using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(TypeDropdownAttribute))]
    public class TypeDropdownDrawer : CollectionDisplayDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.String)
                return new HelpBox("The TypeDropdown Attribute can only be attached to string fields", HelpBoxMessageType.Error);

            List<string> dropdownValues = GetTypeList();
            DropdownField typeDropdown = CreateDropdownField(dropdownValues, property);

            return typeDropdown;
        }

        protected override void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue)
        {
            var dropdown = element as DropdownField;
            string dropdownValue = ConvertPropertyValueToDropdownValue(clipboardValue);

            if (dropdown.choices.Contains(dropdownValue))
            {
                base.PasteValue(element, property, clipboardValue);
                dropdown.SetValueWithoutNotify(dropdownValue);
            }
            else
            {
                Debug.LogWarning($"Could not paste value <b>{dropdownValue}</b> since is not availiable as an option in the dropdown");
            }
        }

        protected override void SetPropertyValueFromDropdown(SerializedProperty property, DropdownField dropdownField)
        {
            if (property.hasMultipleDifferentValues)
                return;

            if (dropdownField.value == "Null")
            {
                property.stringValue = string.Empty;
            }
            else if (dropdownField.value.StartsWith("Global/"))
            {
                property.stringValue = dropdownField.value[7..].Replace('/', '.');
            }
            else
            {
                property.stringValue = dropdownField.value.Replace('/', '.');
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        protected override void SetDropdownValueFromProperty(SerializedProperty property, DropdownField dropdownField)
        {
            string dropdownValue = ConvertPropertyValueToDropdownValue(property.stringValue);

            if (dropdownField.choices.Contains(dropdownValue))
            {
                dropdownField.SetValueWithoutNotify(dropdownValue);
            }
            else
            {
                Debug.LogWarning($"The value <b>{property.stringValue}</b> set to the <b>{property.name}</b> variable is not a valid type string.", property.serializedObject.targetObject);
            }
        }

        private List<string> GetTypeList()
        {
            var typeDropdownAttribute = attribute as TypeDropdownAttribute;

            List<string> typeNameList = new();
            TypeCache.TypeCollection typeCollection = typeDropdownAttribute.AssemblyName == string.Empty ? TypeCache.GetTypesDerivedFrom<object>() : TypeCache.GetTypesDerivedFrom<object>(typeDropdownAttribute.AssemblyName);

            foreach (var item in typeCollection)
            {
                if (typeDropdownAttribute.AssemblyName == string.Empty && !item.IsVisible)
                    continue;

                string assemblyName = item.Assembly.ToString().Split(',')[0];

                if (!item.FullName.Contains('.'))
                {
                    typeNameList.Add($"Global/{item.FullName}, {assemblyName}");
                }
                else
                {
                    typeNameList.Add($"{item.FullName.Replace('.', '/')}, {assemblyName}");
                }
            }

            typeNameList.Sort();
            typeNameList.Insert(0, "Null");

            return typeNameList;
        }

        protected override string SetDropdownDefaultValue(List<string> collectionValues, SerializedProperty property)
        {
            string propertyStringValue = ConvertPropertyValueToDropdownValue(property.stringValue);
            return collectionValues.Contains(propertyStringValue) ? propertyStringValue : collectionValues[0];
        }

        private string ConvertPropertyValueToDropdownValue(string propertyValue)
        {
            if (propertyValue == string.Empty)
                return "Null";

            int commaIndex = propertyValue.IndexOf(',');

            if (commaIndex == -1)
                return propertyValue;

            string typeName = propertyValue[..commaIndex].Replace('.', '/');
            string assemblyName = propertyValue[(commaIndex + 1)..];

            return !typeName.Contains("/") ? $"Global/{propertyValue}" : $"{typeName},{assemblyName}";
        }
    }
}
