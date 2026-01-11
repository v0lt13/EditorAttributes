using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    public abstract class GroupDrawer : PropertyDrawerBase
    {
        /// <summary>
        /// Creates a property setup to display in a group
        /// </summary>
        /// <param name="memberName">The name of the member to create the property for</param>
        /// <param name="property">The target serialized property</param>
        /// <returns>A visual element contaning the property field</returns>
        protected virtual VisualElement CreateGroupProperty(string memberName, SerializedProperty property)
        {
            SerializedProperty variableProperty = FindNestedProperty(property, GetSerializedPropertyName(memberName, property));

            if (variableProperty == null)
                return new HelpBox($"<b>{memberName}</b> is not a valid field or property", HelpBoxMessageType.Error);

            PropertyField propertyField = CreatePropertyField(variableProperty);

            // Slightly move foldouts for certain types so they don't go out of bounds
            if (variableProperty.propertyType == SerializedPropertyType.Generic && variableProperty.type != "UnityEvent" && !variableProperty.isArray)
                propertyField.style.marginLeft = 10f;

            // Replace the hidden field ID with the grouped field ID so the EditorExtension class doesn't remove it when drawing the editor
            propertyField.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
            {
                // Force update this logic to make sure fields are visible
                UpdateVisualElement(propertyField, () =>
                {
                    var hiddenField = propertyField.Q<VisualElement>(HidePropertyDrawer.HIDDEN_PROPERTY_ID);

                    if (hiddenField != null)
                    {
                        hiddenField.name = GROUPED_PROPERTY_ID;
                        hiddenField.style.display = DisplayStyle.Flex;
                    }
                }, 100L);
            });

            return propertyField;
        }
    }
}
