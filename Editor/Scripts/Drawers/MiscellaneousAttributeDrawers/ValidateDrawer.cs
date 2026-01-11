using System;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ValidateAttribute))]
    public class ValidateDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var validateAttribute = attribute as ValidateAttribute;

            VisualElement root = new();
            PropertyField propertyField = CreatePropertyField(property);

            HelpBox errorBox = new();
            HelpBox helpBox = new(validateAttribute.ValidationMessage, (HelpBoxMessageType)validateAttribute.Severety);

            if (CanApplyGlobalColor)
            {
                helpBox.style.color = EditorExtension.GLOBAL_COLOR;
                helpBox.style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
            }

            root.Add(propertyField);
            root.Add(helpBox);

            MemberInfo conditionalProperty = ReflectionUtils.GetValidMemberInfo(validateAttribute.ConditionName, property);

            UpdateVisualElement(root, () =>
            {
                helpBox.style.display = GetConditionValue(conditionalProperty, validateAttribute, property, helpBox, errorBox) ? DisplayStyle.Flex : DisplayStyle.None;
                DisplayErrorBox(root, errorBox);
            });

            return root;
        }

        private bool GetConditionValue(MemberInfo memberInfo, ValidateAttribute validateAttribute, SerializedProperty serializedProperty, HelpBox helpBox, HelpBox errorBox)
        {
            Type memberInfoType = ReflectionUtils.GetMemberInfoType(memberInfo);

            if (memberInfoType == null)
            {
                errorBox.text = $"The provided condition <b>{validateAttribute.ConditionName}</b> could not be found";
                return false;
            }

            if (memberInfoType == typeof(bool))
            {
                object memberInfoValue = ReflectionUtils.GetMemberInfoValue(memberInfo, serializedProperty);

                if (memberInfoValue == null)
                    return false;

                return (bool)memberInfoValue;
            }
            else if (memberInfoType == typeof(ValidationCheck))
            {
                if (ReflectionUtils.GetMemberInfoValue(memberInfo, serializedProperty) is not ValidationCheck memberInfoValue)
                    return false;

                if (validateAttribute.ValidationMessage != null)
                {
                    errorBox.text = "The condition uses <b>ValidationCheck</b> but the attribute still uses the constructor with the <b>validationMessage</b> parameter which will be overriden";
                    errorBox.messageType = HelpBoxMessageType.Info;
                }

                helpBox.text = memberInfoValue.ValidationMessage;
                helpBox.messageType = (HelpBoxMessageType)memberInfoValue.Severety;

                return !memberInfoValue.PassedCheck;
            }

            errorBox.text = $"The provided condition <b>{validateAttribute.ConditionName}</b> is not a valid <b>bool</b> or <b>ValidationCheck</b> type";

            return false;
        }
    }
}
