using UnityEditor;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HideFieldAttribute))]
    public class HideFieldDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var hideAttribute = attribute as HideFieldAttribute;
            MemberInfo conditionalProperty = ReflectionUtils.GetValidMemberInfo(hideAttribute.ConditionName, property);

            HelpBox errorBox = new();
            PropertyField propertyField = CreatePropertyField(property);

            UpdateVisualElement(propertyField, () =>
            {
                propertyField.style.display = !GetConditionValue(conditionalProperty, hideAttribute, property, errorBox) ? DisplayStyle.Flex : DisplayStyle.None;
                DisplayErrorBox(propertyField, errorBox);
            });

            return propertyField;
        }
    }
}
