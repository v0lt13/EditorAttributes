using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var helpBoxAttribute = attribute as HelpBoxAttribute;
            PropertyField propertyField = CreatePropertyField(property);

            VisualElement root = new();
            HelpBox errorBox = new();
            HelpBox helpBox = new(string.Empty, (HelpBoxMessageType)helpBoxAttribute.MessageType);

            if (EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR)
                helpBox.style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;

            root.Add(propertyField);
            root.Add(helpBox);

            if (helpBoxAttribute.DrawAbove)
                helpBox.PlaceBehind(propertyField);

            UpdateVisualElement(helpBox, () =>
            {
                helpBox.text = GetDynamicString(helpBoxAttribute.Message, property, helpBoxAttribute, errorBox);
                DisplayErrorBox(root, errorBox);
            });

            return root;
        }
    }
}
