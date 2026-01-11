using System;
using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ButtonFieldAttribute))]
    public class ButtonFieldDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var buttonFieldAttribute = attribute as ButtonFieldAttribute;

            string[] path = property.propertyPath.Split('.');
            object ownerObject = null;

            if (path.Length == 1)
            {
                ownerObject = property.serializedObject.targetObject;
            }
            else
            {
                // Get the object that the property is a member of
                Type type = ReflectionUtils.GetNestedObjectType(property, out ownerObject);

                if (type == null)
                    return new HelpBox("Field must be a member of a class", HelpBoxMessageType.Error);
            }

            MethodInfo function = ReflectionUtils.FindFunction(buttonFieldAttribute.FunctionName, ownerObject);

            if (function == null)
                return new HelpBox($"Could not find function <b>{buttonFieldAttribute.FunctionName}</b>", HelpBoxMessageType.Error);

            ParameterInfo[] functionParameters = function.GetParameters();

            if (functionParameters.Length != 0)
                return new HelpBox("The function cannot have parameters", HelpBoxMessageType.Error);

            string buttonLabel = string.IsNullOrWhiteSpace(buttonFieldAttribute.ButtonLabel) ? function.Name : buttonFieldAttribute.ButtonLabel;

            if (buttonFieldAttribute.IsRepetable)
            {
                RepeatButton repeatButton = new(() => InvokeFunctionOnAllTargets(property.serializedObject.targetObjects, function.Name), buttonFieldAttribute.PressDelay, buttonFieldAttribute.RepetitionInterval)
                {
                    text = buttonLabel,
                    tooltip = property.tooltip,
                    style = { height = buttonFieldAttribute.ButtonHeight }
                };

                repeatButton.AddToClassList(Button.ussClassName);

                return repeatButton;
            }
            else
            {
                return new Button(() => InvokeFunctionOnAllTargets(property.serializedObject.targetObjects, function.Name))
                {
                    text = buttonLabel,
                    tooltip = property.tooltip,
                    style = { height = buttonFieldAttribute.ButtonHeight }
                };
            }
        }
    }
}
