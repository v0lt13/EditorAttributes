using UnityEditor;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(InlineButtonAttribute))]
    public class InlineButtonDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var inlineButtonAttribute = attribute as InlineButtonAttribute;
			var buttonLabel = inlineButtonAttribute.ButtonLabel == string.Empty ? inlineButtonAttribute.FunctionName : inlineButtonAttribute.ButtonLabel;

			var methodInfo = ReflectionUtility.FindFunction(inlineButtonAttribute.FunctionName, property.serializedObject.targetObject);

            var root = new VisualElement();
            var propertyField = DrawProperty(property);
            var button = new Button(() => methodInfo.Invoke(property.serializedObject.targetObject, null)) { text = buttonLabel };

			if (methodInfo.GetParameters().Length > 0)
			{
				root.Add(new HelpBox("The function cannot have parameters", HelpBoxMessageType.Error));
				return root;
			}

			button.style.width = inlineButtonAttribute.ButtonWidth;
			root.style.flexDirection = FlexDirection.Row;
			propertyField.style.flexGrow = 1f;

			root.Add(propertyField);
			root.Add(button);

            return root;
		}
	}
}
