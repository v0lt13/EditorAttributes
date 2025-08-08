using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
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

			var collectionInfo = ReflectionUtility.GetValidMemberInfo(dropdownAttribute.CollectionName, property);
			var propertyValues = ConvertCollectionValuesToStrings(dropdownAttribute.CollectionName, property, collectionInfo, errorBox);

			var displayValues = GetDisplayValues(collectionInfo, dropdownAttribute, property, propertyValues);

			var dropdownField = IsCollectionValid(displayValues) ? new DropdownField(property.displayName, displayValues, GetDropdownDefaultValueIndex(propertyValues, property))
				: new DropdownField(property.displayName, new List<string>() { "NULL" }, 0);

			dropdownField.tooltip = property.tooltip;
			dropdownField.AddToClassList(BaseField<Void>.alignedFieldUssClassName);

			AddPropertyContextMenu(dropdownField, property);

			dropdownField.RegisterValueChangedCallback((callback) =>
			{
				if (!property.hasMultipleDifferentValues)
					SetPropertyValue(property, callback.newValue, dropdownAttribute, propertyValues, dropdownField, collectionInfo);
			});

			dropdownField.TrackPropertyValue(property, (trackedProperty) =>
			{
				if (propertyValues.Contains(trackedProperty.boxedValue.ToString()))
				{
					dropdownField.SetValueWithoutNotify(displayValues[propertyValues.IndexOf(trackedProperty.boxedValue.ToString())]);
				}
				else
				{
					Debug.LogWarning($"The value <b>{trackedProperty.boxedValue}</b> set to the <b>{trackedProperty.name}</b> variable is not a value available in the dropdown", trackedProperty.serializedObject.targetObject);
				}
			});

			if (dropdownField.value != "NULL" && !HasMismatchedDisplayCollectionCounts(dropdownAttribute, propertyValues, displayValues))
			{
				dropdownField.showMixedValue = property.hasMultipleDifferentValues;

				if (!property.hasMultipleDifferentValues)
					SetPropertyValue(property, dropdownField.value, dropdownAttribute, propertyValues, dropdownField, collectionInfo);
			}

			root.Add(dropdownField);

			ExecuteLater(dropdownField, () => dropdownField.Q(className: DropdownField.inputUssClassName).style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f);

			UpdateVisualElement(dropdownField, () =>
			{
				var currentPropertyValues = ConvertCollectionValuesToStrings(dropdownAttribute.CollectionName, property, collectionInfo, errorBox);
				var currentDisplayValues = GetDisplayValues(collectionInfo, dropdownAttribute, property, currentPropertyValues);

				if (IsCollectionValid(currentPropertyValues))
				{
					errorBox.text = string.Empty;
					dropdownField.choices = currentDisplayValues;

					propertyValues = currentPropertyValues;
				}

				if (HasMismatchedDisplayCollectionCounts(dropdownAttribute, propertyValues, displayValues))
				{
					errorBox.text = "The value collection item count and display names count do not match";
					DisplayErrorBox(root, errorBox);

					return;
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

		private bool HasMismatchedDisplayCollectionCounts(DropdownAttribute dropdownAttribute, List<string> propertyValues, List<string> collectionValues) => dropdownAttribute.DisplayNames != null && propertyValues.Count != collectionValues.Count;

		private void SetPropertyValue(SerializedProperty property, string value, DropdownAttribute dropdownAttribute, List<string> propertyValues, DropdownField dropdownField, MemberInfo collectionInfo)
		{
			if (dropdownAttribute.DisplayNames != null || IsDictionary(collectionInfo, property, out _))
			{
				SetPropertyValueFromString(propertyValues[dropdownField.index], property);
			}
			else
			{
				SetPropertyValueFromString(value, property);
			}
		}

		private List<string> GetDisplayValues(MemberInfo collectionInfo, DropdownAttribute dropdownAttribute, SerializedProperty serializedProperty, List<string> propertyValues)
		{
			var displayStrings = new List<string>();

			if (dropdownAttribute.DisplayNames == null)
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
				displayStrings = dropdownAttribute.DisplayNames.ToList();
			}

			return displayStrings;
		}

		private int GetDropdownDefaultValueIndex(List<string> collectionValues, SerializedProperty property)
		{
			var propertyStringValue = GetPropertyValueAsString(property);

			return collectionValues.Contains(propertyStringValue) ? collectionValues.IndexOf(propertyStringValue) : 0;
		}

		private bool IsDictionary(MemberInfo collectionInfo, SerializedProperty serializedProperty, out IDictionary dictionary)
		{
			var collectionValue = ReflectionUtility.GetMemberInfoValue(collectionInfo, serializedProperty);

			dictionary = collectionValue as IDictionary;

			return collectionValue is IDictionary;
		}
	}
}
