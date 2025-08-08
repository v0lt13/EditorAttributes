using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(TypeDropdownAttribute))]
	public class TypeDropdownDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();

			if (property.propertyType == SerializedPropertyType.String)
			{
				var dropdownValues = GetAvailableTypes();

				var typeDropdown = new DropdownField(property.displayName, dropdownValues, GetDropdownDefaultValue(dropdownValues, property))
				{
					showMixedValue = property.hasMultipleDifferentValues,
					tooltip = property.tooltip
				};

				typeDropdown.AddToClassList(BaseField<Void>.alignedFieldUssClassName);
				AddPropertyContextMenu(typeDropdown, property);

				typeDropdown.RegisterValueChangedCallback(callback =>
				{
					if (callback.newValue == "Null")
					{
						property.stringValue = string.Empty;
					}
					else if (callback.newValue.StartsWith("Global/"))
					{
						property.stringValue = callback.newValue[7..].Replace('/', '.');
					}
					else
					{
						property.stringValue = callback.newValue.Replace('/', '.');
					}

					property.serializedObject.ApplyModifiedProperties();
				});

				typeDropdown.TrackPropertyValue(property, (trackedProperty) =>
				{
					string dropdownValue = ConvertPropertyValueToDropdownValue(trackedProperty.stringValue);

					if (typeDropdown.choices.Contains(dropdownValue))
					{
						typeDropdown.SetValueWithoutNotify(dropdownValue);
					}
					else
					{
						Debug.LogWarning($"The value <b>{trackedProperty.stringValue}</b> set to the <b>{trackedProperty.name}</b> variable is not a valid type string.", trackedProperty.serializedObject.targetObject);
					}
				});

				root.Add(typeDropdown);

				ExecuteLater(typeDropdown, () => typeDropdown.Q(className: DropdownField.inputUssClassName).style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f);
			}
			else
			{
				root.Add(new HelpBox("The TypeDropdown attribute can only be attached to string fields", HelpBoxMessageType.Error));
			}

			return root;
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
				Debug.LogWarning($"Could not paste value \"{dropdownValue}\" since is not availiable as an option in the dropdown");
			}
		}

		private List<string> GetAvailableTypes()
		{
			var typeDropdownAttribute = attribute as TypeDropdownAttribute;

			var typeNameList = new List<string>();
			var typeCollection = typeDropdownAttribute.AssemblyName == string.Empty ? TypeCache.GetTypesDerivedFrom<object>() : TypeCache.GetTypesDerivedFrom<object>(typeDropdownAttribute.AssemblyName);

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

		private string GetDropdownDefaultValue(List<string> collectionValues, SerializedProperty property)
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
