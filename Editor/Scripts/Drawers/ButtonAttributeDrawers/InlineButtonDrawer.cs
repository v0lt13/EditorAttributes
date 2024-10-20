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

			if (methodInfo.GetParameters().Length > 0)
			{
				root.Add(new HelpBox("The function cannot have parameters", HelpBoxMessageType.Error));
				return root;
			}

			root.style.flexDirection = FlexDirection.Row;
			propertyField.style.flexGrow = 1f;

			root.Add(propertyField);

			if (inlineButtonAttribute.IsRepetable)
			{
				var repeatButton = new RepeatButton(() => methodInfo.Invoke(property.serializedObject.targetObject, null), inlineButtonAttribute.PressDelay, inlineButtonAttribute.RepetitionInterval) { text = buttonLabel };

				repeatButton.style.width = inlineButtonAttribute.ButtonWidth;
				repeatButton.AddToClassList("unity-button");

				root.Add(repeatButton);
			}
			else
			{
				var button = new Button(() => methodInfo.Invoke(property.serializedObject.targetObject, null)) { text = buttonLabel };

				button.style.width = inlineButtonAttribute.ButtonWidth;

				root.Add(button);
			}

            return root;
		}
	}
}
