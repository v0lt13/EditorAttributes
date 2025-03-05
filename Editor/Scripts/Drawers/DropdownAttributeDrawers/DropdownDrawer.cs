using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var dropdownAttribute = attribute as DropdownAttribute;
			var root = new VisualElement();
			var errorBox = new HelpBox();

			var memberInfo = ReflectionUtility.GetValidMemberInfo(dropdownAttribute.ValueCollectionName, property);
			var propertyValues = ConvertCollectionValuesToStrings(dropdownAttribute.ValueCollectionName, property, memberInfo, errorBox);

			var collectionValues = dropdownAttribute.DisplayNames == null ? propertyValues : dropdownAttribute.DisplayNames.ToList();

			var dropdownField = IsCollectionValid(collectionValues) ? new DropdownField(property.displayName, collectionValues, GetDropdownDefaultValue(collectionValues, property)) 
				: new DropdownField(property.displayName, new List<string>() { "NULL" }, 0);

			dropdownField.tooltip = property.tooltip;
			dropdownField.AddToClassList(BaseField<Void>.alignedFieldUssClassName);

			AddPropertyContextMenu(dropdownField, property);

			if (propertyValues.Count != collectionValues.Count)
			{
				errorBox.text = "The value collection item count and display names count do not match";
				DisplayErrorBox(root, errorBox);

				return root;
			}

			dropdownField.RegisterValueChangedCallback((callback) =>
			{
				if (!property.hasMultipleDifferentValues)
					SetPropertyValue(property, callback.newValue, dropdownAttribute, propertyValues, dropdownField);
			});

			if (dropdownField.value != "NULL")
			{
				dropdownField.showMixedValue = property.hasMultipleDifferentValues;

				if (!property.hasMultipleDifferentValues)
					SetPropertyValue(property, dropdownField.value, dropdownAttribute, propertyValues, dropdownField);
			}

			root.Add(dropdownField);

			ExecuteLater(dropdownField, () => dropdownField.Q(className: DropdownField.inputUssClassName).style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f);

			UpdateVisualElement(dropdownField, () =>
			{
				var currentPropertyValues = ConvertCollectionValuesToStrings(dropdownAttribute.ValueCollectionName, property, memberInfo, errorBox);
				var currentCollectionValues = dropdownAttribute.DisplayNames == null ? propertyValues : dropdownAttribute.DisplayNames.ToList();

				if (IsCollectionValid(currentPropertyValues))
				{
					errorBox.text = string.Empty;
					dropdownField.choices = currentCollectionValues;

					propertyValues = currentPropertyValues;
				}

				DisplayErrorBox(root, errorBox);
			});
			
			return root;
		}

		protected override string CopyValue(VisualElement element, SerializedProperty property)
		{
			var dropdown = element as DropdownField;
			var dropdownAttribute = attribute as DropdownAttribute;

			return dropdownAttribute.DisplayNames != null ? dropdown.value : base.CopyValue(element, property);
		}

		protected override void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue)
		{
			var dropdown = element as DropdownField;
			var dropdownAttribute = attribute as DropdownAttribute;

			if (dropdown.choices.Contains(clipboardValue))
			{
				if (dropdownAttribute.DisplayNames != null)
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
				Debug.LogWarning($"Could not paste value \"{clipboardValue}\" since is not availiable as an option in the dropdown");
			}
		}

		private void SetPropertyValue(SerializedProperty property, string value, DropdownAttribute dropdownAttribute, List<string> propertyValues, DropdownField dropdownField)
		{
			if (dropdownAttribute.DisplayNames != null)
			{
				SetPropertyValueFromString(propertyValues[dropdownField.index], property);
			}
			else
			{
				SetPropertyValueFromString(value, property);
			}
		}

		private string GetDropdownDefaultValue(List<string> collectionValues, SerializedProperty property)
		{
			var propertyStringValue = GetPropertyValueAsString(property);

			return collectionValues.Contains(propertyStringValue) ? propertyStringValue : collectionValues[0];
		}
	}
}
