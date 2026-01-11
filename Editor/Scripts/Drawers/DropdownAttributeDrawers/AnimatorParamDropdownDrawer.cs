using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEditor.Animations;
using UnityEngine.UIElements;
using System.Collections.Generic;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(AnimatorParamDropdownAttribute))]
    public class AnimatorParamDropdownDrawer : CollectionDisplayDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.String)
                return new HelpBox("The AnimatorParamDropdown Attribute can only be attached to string fields", HelpBoxMessageType.Error);

            var animatorParamAttribute = attribute as AnimatorParamDropdownAttribute;

            HelpBox errorBox = new();
            List<string> animatorParameters = GetAnimatorParameterList(animatorParamAttribute, property, errorBox);
            DropdownField dropdownField = CreateDropdownField(animatorParameters, property);

            UpdateVisualElement(dropdownField, () =>
            {
                List<string> animatorParams = GetAnimatorParameterList(animatorParamAttribute, property, errorBox);

                if (IsCollectionValid(animatorParams))
                    dropdownField.choices = animatorParams;
            });

            DisplayErrorBox(dropdownField, errorBox);
            return dropdownField;
        }

        protected override void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue)
        {
            var dropdown = element as DropdownField;

            if (dropdown.choices.Contains(clipboardValue))
            {
                base.PasteValue(element, property, clipboardValue);
                dropdown.SetValueWithoutNotify(clipboardValue);
            }
            else
            {
                Debug.LogWarning($"Could not paste value <b>{clipboardValue}</b> since is not availiable as an option in the dropdown");
            }
        }

        private List<string> GetAnimatorParameterList(AnimatorParamDropdownAttribute animatorParamAttribute, SerializedProperty property, HelpBox errorBox)
        {
            List<string> paramList = new();

            MemberInfo memberInfo = ReflectionUtils.GetValidMemberInfo(animatorParamAttribute.AnimatorFieldName, property);
            Type memberInfoType = ReflectionUtils.GetMemberInfoType(memberInfo);

            if (memberInfoType != typeof(Animator))
            {
                errorBox.text = $"The provided field <b>{animatorParamAttribute.AnimatorFieldName}</b> is not of type <b>Animator</b>";
                return null;
            }

            var memberInfoValue = ReflectionUtils.GetMemberInfoValue(memberInfo, property) as Animator;

            if (memberInfoValue != null && memberInfoValue.runtimeAnimatorController != null)
            {
                // Hack for having the animator refesh its parameters when editing them in edit mode otherwise the parameters array will be empty
                var editorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GetAssetPath(memberInfoValue.runtimeAnimatorController));

                foreach (var parameter in editorController.parameters)
                    paramList.Add(parameter.name);
            }
            else
            {
                errorBox.text = "The <b>Animator</b> or <b>Animator Controller</b> is null, make sure they are assigned";
                return null;
            }

            return paramList;
        }
    }
}
