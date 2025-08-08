using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(TagDropdownAttribute))]
	public class TagDropdownDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();

			if (property.propertyType == SerializedPropertyType.String)
			{
				var tagField = new TagField(property.displayName, DoesStringValueContainTag(property.stringValue) ? property.stringValue : "Untagged")
				{
					showMixedValue = property.hasMultipleDifferentValues,
					tooltip = property.tooltip
				};

				tagField.AddToClassList(BaseField<Void>.alignedFieldUssClassName);
				AddPropertyContextMenu(tagField, property);

				tagField.RegisterValueChangedCallback(callback =>
				{
					property.stringValue = tagField.value;
					property.serializedObject.ApplyModifiedProperties();
				});

				tagField.TrackPropertyValue(property, (trackedProperty) =>
				{
					if (DoesStringValueContainTag(trackedProperty.stringValue))
					{
						tagField.SetValueWithoutNotify(trackedProperty.stringValue);
					}
					else
					{
						Debug.LogWarning($"The value <b>{trackedProperty.stringValue}</b> set to the <b>{trackedProperty.name}</b> variable is not a valid tag.", trackedProperty.serializedObject.targetObject);
					}
				});

				root.Add(tagField);

				ExecuteLater(tagField, () => tagField.Q(className: TagField.inputUssClassName).style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f);
			}
			else
			{
				root.Add(new HelpBox("The TagDropdown attribute can only be attached to string fields", HelpBoxMessageType.Error));
			}

			return root;
		}

		protected override void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue)
		{
			var dropdown = element as TagField;

			if (dropdown.choices.Contains(clipboardValue))
			{
				base.PasteValue(element, property, clipboardValue);
				dropdown.SetValueWithoutNotify(clipboardValue);
			}
			else
			{
				Debug.LogWarning($"Could not paste value \"{clipboardValue}\" since is not availiable as an option in the dropdown");
			}
		}

		private bool DoesStringValueContainTag(string stringValue)
		{
			foreach (var tag in InternalEditorUtility.tags)
			{
				if (stringValue == tag)
					return true;
			}

			return false;
		}
	}
}
