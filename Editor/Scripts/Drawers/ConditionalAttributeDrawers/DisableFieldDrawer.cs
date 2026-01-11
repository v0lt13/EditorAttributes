using UnityEditor;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(DisableFieldAttribute))]
    public class DisableFieldDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var disableAttribute = attribute as DisableFieldAttribute;
            MemberInfo conditionalProperty = ReflectionUtils.GetValidMemberInfo(disableAttribute.ConditionName, property);

            HelpBox errorBox = new();
            PropertyField propertyField = CreatePropertyField(property);

            UpdateVisualElement(propertyField, () =>
            {
                propertyField.SetEnabled(!GetConditionValue(conditionalProperty, disableAttribute, property, errorBox));
                DisplayErrorBox(propertyField, errorBox);
            });

            return propertyField;
        }
    }
}
