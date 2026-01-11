using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HideLabelAttribute))]
    public class HideLabelDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            PropertyField propertyField = CreatePropertyField(property);

            propertyField.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
            {
                var label = propertyField.Q<Label>(className: "unity-label");

                if (label != null)
                    label.style.display = DisplayStyle.None;
            });

            return propertyField;
        }
    }
}
