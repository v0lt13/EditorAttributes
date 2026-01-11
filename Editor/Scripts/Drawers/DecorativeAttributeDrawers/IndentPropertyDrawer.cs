using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(IndentPropertyAttribute))]
    public class IndentPropertyDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var indentPropertyAttribute = attribute as IndentPropertyAttribute;

            PropertyField propertyField = CreatePropertyField(property);
            propertyField.style.marginLeft = indentPropertyAttribute.IndentLevel;

            return propertyField;
        }
    }
}
