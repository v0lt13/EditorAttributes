using UnityEditor;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ShowFieldAttribute))]
    public class ShowFieldDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var showAttribute = attribute as ShowFieldAttribute;
            MemberInfo conditionalProperty = ReflectionUtils.GetValidMemberInfo(showAttribute.ConditionName, property);

            HelpBox errorBox = new();
            PropertyField propertyField = CreatePropertyField(property);

            UpdateVisualElement(propertyField, () =>
            {
                propertyField.style.display = GetConditionValue(conditionalProperty, showAttribute, property, errorBox) ? DisplayStyle.Flex : DisplayStyle.None;
                DisplayErrorBox(propertyField, errorBox);
            });

            return propertyField;
        }
    }
}
