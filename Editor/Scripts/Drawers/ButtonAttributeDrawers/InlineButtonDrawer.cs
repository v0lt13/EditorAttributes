using System.Linq;
using UnityEditor;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(InlineButtonAttribute))]
    public class InlineButtonDrawer : PropertyDrawerBase
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new() { style = { flexDirection = FlexDirection.Row } };
            PropertyField propertyField = CreatePropertyField(property);

            propertyField.name = "CustomPropertyField"; // This is used to identify the our property field from the one automatically created by unity for the drawer
            propertyField.style.flexGrow = 1f;

            root.Add(propertyField);
            root.RegisterCallbackOnce<GeometryChangedEvent>((callback) =>
            {
                // Adding multiple InlineButton attributes on the same property causes a root and propertyField to be continously added to the hierarchy with each property drawer call
                // So we move children recursively from the bottom of the hierarchy to the root and remove their original parents so there is only one root and property field no matter how many times this attribute is used
                MoveChildren(root, propertyField.name);
                MoveChildren(propertyField, propertyField.name);

                MemberInfo propertyInfo = ReflectionUtils.GetValidMemberInfo(property.name, property);
                var inlineButtonAttributes = propertyInfo.GetCustomAttributes(typeof(InlineButtonAttribute), true) as InlineButtonAttribute[];

                foreach (var inlineButtonAttribute in inlineButtonAttributes)
                    root.Add(CreateInlineButton(inlineButtonAttribute, property));
            });

            return root;
        }

        private VisualElement CreateInlineButton(InlineButtonAttribute inlineButtonAttribute, SerializedProperty property)
        {
            MethodInfo methodInfo = ReflectionUtils.FindFunction(inlineButtonAttribute.FunctionName, property);

            if (methodInfo.GetParameters().Length > 0)
                return new HelpBox("The function cannot have parameters", HelpBoxMessageType.Error);

            string buttonLabel = inlineButtonAttribute.ButtonLabel == string.Empty ? inlineButtonAttribute.FunctionName : inlineButtonAttribute.ButtonLabel;

            if (inlineButtonAttribute.IsRepetable)
            {
                RepeatButton repeatButton = new(() => InvokeFunctionOnAllTargets(property.serializedObject.targetObjects, methodInfo.Name, makeTargetsDirty: inlineButtonAttribute.MakeDirty), inlineButtonAttribute.PressDelay, inlineButtonAttribute.RepetitionInterval)
                {
                    text = buttonLabel
                };

                repeatButton.style.width = inlineButtonAttribute.ButtonWidth;
                repeatButton.AddToClassList(Button.ussClassName);

                return repeatButton;
            }
            else
            {
                Button button = new(() => InvokeFunctionOnAllTargets(property.serializedObject.targetObjects, methodInfo.Name, makeTargetsDirty: inlineButtonAttribute.MakeDirty))
                {
                    text = buttonLabel
                };

                button.style.width = inlineButtonAttribute.ButtonWidth;

                return button;
            }
        }

        private void MoveChildren(VisualElement element, string parentName)
        {
            if (element.parent.name == parentName)
            {
                foreach (var child in element.Children().ToList())
                    element.parent.Add(child);

                element.RemoveFromHierarchy();
            }
        }
    }
}
