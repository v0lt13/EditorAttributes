using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(PrefixAttribute))]
    public class PrefixDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var prefixAttribute = attribute as PrefixAttribute;

            HelpBox errorBox = new();
            PropertyField propertyField = CreatePropertyField(property);

            Label prefixLabel = new()
            {
                style = {
                    fontSize = 12,
                    maxWidth = 200f,
                    marginRight = prefixAttribute.Offset,
                    unityTextAlign = TextAnchor.MiddleRight,
                    alignSelf = Align.FlexEnd,
                    overflow = Overflow.Hidden
                }
            };

            prefixLabel.style.color = CanApplyGlobalColor ? EditorExtension.GLOBAL_COLOR : Color.gray;

            propertyField.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
            {
                var field = propertyField.Q<Label>();
                field.Add(prefixLabel);
            });

            UpdateVisualElement(prefixLabel, () =>
            {
                prefixLabel.text = GetDynamicString(prefixAttribute.Prefix, property, prefixAttribute, errorBox);
                DisplayErrorBox(propertyField, errorBox);
            });

            return propertyField;
        }
    }
}
