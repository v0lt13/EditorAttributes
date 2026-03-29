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
        private Dictionary<int, string> animatorParametersHash = new();

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (!IsSupportedPropertyType(property))
                return new HelpBox("The AnimatorParamDropdown Attribute can only be attached to a string or int", HelpBoxMessageType.Error);

            var animatorParamAttribute = attribute as AnimatorParamDropdownAttribute;

            HelpBox errorBox = new();
            List<string> animatorParameters = GetAnimatorParameters(animatorParamAttribute, property, errorBox, out animatorParametersHash);
            DropdownField dropdownField = CreateDropdownField(animatorParameters, property);

            UpdateVisualElement(dropdownField, () =>
            {
                List<string> animatorParams = GetAnimatorParameters(animatorParamAttribute, property, errorBox, out animatorParametersHash);

                if (IsCollectionValid(animatorParams))
                    dropdownField.choices = animatorParams;
            });

            DisplayErrorBox(dropdownField, errorBox);
            return dropdownField;
        }

        protected override void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue)
        {
            var dropdown = element as DropdownField;

            string parameterName = clipboardValue;

            if (int.TryParse(clipboardValue, out int parameterHash) && animatorParametersHash.ContainsKey(parameterHash))
                parameterName = animatorParametersHash[parameterHash];

            if (dropdown.choices.Contains(parameterName))
            {
                dropdown.value = parameterName;
            }
            else
            {
                Debug.LogWarning($"Could not paste value <b>{clipboardValue}</b> since is not availiable as an option in the dropdown");
            }
        }

        protected override string SetDropdownDefaultValue(List<string> collectionValues, SerializedProperty property)
        {
            string propertyStringValue;

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                animatorParametersHash.TryGetValue(property.intValue, out propertyStringValue);
            }
            else
            {
                propertyStringValue = property.stringValue;
            }

            return collectionValues.Contains(propertyStringValue) ? propertyStringValue : collectionValues[0];
        }

        protected override void SetPropertyValueFromDropdown(SerializedProperty property, DropdownField dropdown)
        {
            if (property.hasMultipleDifferentValues)
                return;

            if (property.propertyType == SerializedPropertyType.String)
            {
                property.stringValue = dropdown.value;
            }
            else
            {
                property.intValue = Animator.StringToHash(dropdown.value);
            }

            property.serializedObject.ApplyModifiedProperties();
        }

        protected override void SetDropdownValueFromProperty(SerializedProperty trackedProperty, DropdownField dropdownField)
        {
            string parameterName;

            if (trackedProperty.propertyType == SerializedPropertyType.Integer)
            {
                animatorParametersHash.TryGetValue(trackedProperty.intValue, out parameterName);
            }
            else
            {
                parameterName = trackedProperty.stringValue;
            }

            if (dropdownField.choices.Contains(parameterName))
            {
                dropdownField.SetValueWithoutNotify(parameterName);
            }
            else
            {
                Debug.LogWarning($"The value <b>{GetPropertyValueAsString(trackedProperty)}</b> set to the <b>{trackedProperty.name}</b> variable is not a valid animator parameter", trackedProperty.serializedObject.targetObject);
            }
        }

        protected override bool IsSupportedPropertyType(SerializedProperty property) => property.propertyType is SerializedPropertyType.String or SerializedPropertyType.Integer;

        private List<string> GetAnimatorParameters(AnimatorParamDropdownAttribute animatorParamAttribute, SerializedProperty property, HelpBox errorBox, out Dictionary<int, string> paramterHashTable)
        {
            List<string> paramList = new();
            paramterHashTable = new Dictionary<int, string>();

            MemberInfo memberInfo = ReflectionUtils.GetValidMemberInfo(animatorParamAttribute.AnimatorFieldName, property);
            Type memberInfoType = ReflectionUtils.GetMemberInfoType(memberInfo);

            if (memberInfoType != typeof(Animator))
            {
                errorBox.text = $"The provided field <b>{animatorParamAttribute.AnimatorFieldName}</b> is not of type <b>Animator</b>";

                paramterHashTable = null;
                return null;
            }

            var memberInfoValue = ReflectionUtils.GetMemberInfoValue(memberInfo, property) as Animator;

            if (memberInfoValue != null && memberInfoValue.runtimeAnimatorController != null)
            {
                // Hack for having the animator refesh its parameters when editing them in edit mode otherwise the parameters array will be empty
                var editorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GetAssetPath(memberInfoValue.runtimeAnimatorController));

                foreach (var parameter in editorController.parameters)
                {
                    paramList.Add(parameter.name);
                    paramterHashTable.Add(parameter.nameHash, parameter.name);
                }
            }
            else
            {
                errorBox.text = "The <b>Animator</b> or <b>Animator Controller</b> is null, make sure they are assigned";

                paramterHashTable = null;
                return null;
            }

            return paramList;
        }
    }
}
