using System;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;
using UnityEditor.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HideInChildrenAttribute))]
    public class HideInChildrenDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var hideInChildrenAttribute = attribute as HideInChildrenAttribute;

            PropertyField propertyField = CreatePropertyField(property);
            propertyField.style.display = IsPropertyInherited(property, hideInChildrenAttribute) ? DisplayStyle.None : DisplayStyle.Flex;

            return propertyField;
        }

        private bool IsPropertyInherited(SerializedProperty property, HideInChildrenAttribute attribute)
        {
            Type targetObjectType = property.serializedObject.targetObject.GetType();
            FieldInfo fieldInfo = ReflectionUtils.FindField(property.name, property);

            foreach (var type in attribute.ChildTypes)
            {
                if (targetObjectType != type)
                    return false;
            }

            return targetObjectType != fieldInfo.DeclaringType;
        }
    }
}
