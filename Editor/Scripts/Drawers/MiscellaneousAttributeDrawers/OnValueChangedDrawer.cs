using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;
using System.Reflection;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(OnValueChangedAttribute))]
    public class OnValueChangedDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var onValueChangedAttribute = attribute as OnValueChangedAttribute;

            ReflectionUtils.GetNestedObjectType(property, out object target);
            PropertyField propertyField = CreatePropertyField(property);

            MethodInfo function = ReflectionUtils.FindFunction(onValueChangedAttribute.FunctionName, property);

            if (function.GetParameters().Length != 0)
            {
                propertyField.Add(new HelpBox("The function cannot have parameters", HelpBoxMessageType.Error));
                return propertyField;
            }

            propertyField.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
            {
                var field = propertyField.Q(className: PropertyField.ussClassName) as PropertyField;
                field.RegisterValueChangeCallback((callback) => function.Invoke(target, null));
            });

            return propertyField;
        }
    }
}
