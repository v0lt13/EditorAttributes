using UnityEditor;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(EnableFieldAttribute))]
    public class EnableFieldDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var enableAttribute = attribute as EnableFieldAttribute;
            MemberInfo conditionalProperty = ReflectionUtils.GetValidMemberInfo(enableAttribute.ConditionName, property);

            HelpBox errorBox = new();
            PropertyField propertyField = CreatePropertyField(property);

            UpdateVisualElement(propertyField, () =>
            {
                propertyField.SetEnabled(GetConditionValue(conditionalProperty, enableAttribute, property, errorBox));
                DisplayErrorBox(propertyField, errorBox);
            });

            return propertyField;
        }
    }
}
