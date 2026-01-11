using UnityEditor;
using UnityEngine.UIElements;

#if UNITY_6000_3_OR_NEWER
using System;
using UnityEngine;
using System.Reflection;
using UnityEditor.UIElements;
using EditorAttributes.Editor.Utility;
#endif

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ApplyMaterialAttribute))]
    public class ApplyMaterialDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
#if UNITY_6000_3_OR_NEWER
            var applyMaterialAttribute = attribute as ApplyMaterialAttribute;

            MemberInfo materialMemberInfo = ReflectionUtils.GetValidMemberInfo(applyMaterialAttribute.MaterialMemberName, property);
            Type materialMemberType = ReflectionUtils.GetMemberInfoType(materialMemberInfo);

            if (materialMemberType == null)
                return new HelpBox($"The provided member <b>{applyMaterialAttribute.MaterialMemberName}</b> could not be found", HelpBoxMessageType.Error);

            if (materialMemberType != typeof(Material))
                return new HelpBox($"<b>{applyMaterialAttribute.MaterialMemberName}</b> is not a valid Material", HelpBoxMessageType.Error);

            PropertyField propertyField = CreatePropertyField(property);

            UpdateVisualElement(propertyField, () =>
            {
                var materialMemberValue = ReflectionUtils.GetMemberInfoValue(materialMemberInfo, property) as Material;

                propertyField.style.unityMaterial = materialMemberValue == null ? new StyleMaterialDefinition(StyleKeyword.Initial) : materialMemberValue;
            });

            return propertyField;
#else
            return new HelpBox("ApplyMaterial Attribute requires <b>Unity 6.3</b> or newer", HelpBoxMessageType.Warning);
#endif
        }
    }
}
