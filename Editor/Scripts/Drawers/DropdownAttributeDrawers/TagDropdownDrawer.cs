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
            if (property.propertyType != SerializedPropertyType.String)
                return new HelpBox("The TagDropdown Attribute can only be attached to string fields", HelpBoxMessageType.Error);

            TagField tagField = new(property.displayName, DoesStringValueContainTag(property.stringValue) ? property.stringValue : "Untagged")
            {
                showMixedValue = property.hasMultipleDifferentValues,
                tooltip = property.tooltip
            };

            tagField.AddToClassList(BaseField<Void>.alignedFieldUssClassName);
            AddPropertyContextMenu(tagField, property);

            tagField.RegisterValueChangedCallback((callback) =>
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

            tagField.RegisterCallbackOnce<GeometryChangedEvent>((callback) => tagField.Q(className: TagField.inputUssClassName).style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f);

            return tagField;
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
                Debug.LogWarning($"Could not paste value <b>{clipboardValue}</b> since is not availiable as an option in the dropdown");
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
