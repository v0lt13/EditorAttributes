using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(PropertyWidthAttribute))]
    public class PropertyWidthDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var propertyWidthAttribute = attribute as PropertyWidthAttribute;

            PropertyField propertyField = CreatePropertyField(property);

            propertyField.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
            {
                var fieldLabel = propertyField.Q<Label>();

                if (fieldLabel != null)
                    fieldLabel.style.marginRight = propertyWidthAttribute.WidthOffset;
            });

            return propertyField;
        }
    }
}
