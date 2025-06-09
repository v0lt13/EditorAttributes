using UnityEditor;
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

			var path = property.propertyPath.Split('.');
			object ownerObject = null;

			if (path.Length == 1)
			{
				ownerObject = property.serializedObject.targetObject;
			}
			else
			{
				// Get the object that the property is a member of
				var type = ReflectionUtility.GetNestedObjectType(property, out ownerObject);

				if (type == null)
					return new HelpBox("Field must be a member of a class", HelpBoxMessageType.Error);
			}

			var function = ReflectionUtility.FindFunction(buttonFieldAttribute.FunctionName, ownerObject);

			if (function == null)
				return new HelpBox($"Could not find function <b>{buttonFieldAttribute.FunctionName}</b>. If this function is inherited make sure is marked at protected.", HelpBoxMessageType.Error);

			var functionParameters = function.GetParameters();
			var buttonLabel = string.IsNullOrWhiteSpace(buttonFieldAttribute.ButtonLabel) ? function.Name : buttonFieldAttribute.ButtonLabel;

			var root = new VisualElement();

			if (functionParameters.Length == 0)
			{
				if (buttonFieldAttribute.IsRepetable)
				{
					var repeatButton = new RepeatButton(() => InvokeFunctionOnAllTargets(property.serializedObject.targetObjects, function.Name), buttonFieldAttribute.PressDelay, buttonFieldAttribute.RepetitionInterval)
					{
						text = buttonLabel,
						tooltip = property.tooltip
					};

					repeatButton.style.height = buttonFieldAttribute.ButtonHeight;
					repeatButton.AddToClassList(Button.ussClassName);

					root.Add(repeatButton);
				}
				else
				{
					var button = new Button(() => InvokeFunctionOnAllTargets(property.serializedObject.targetObjects, function.Name))
					{
						text = buttonLabel,
						tooltip = property.tooltip
					};

					button.style.height = buttonFieldAttribute.ButtonHeight;

					root.Add(button);
				}
			}
			else
			{
				root.Add(new HelpBox("The function cannot have parameters", HelpBoxMessageType.Error));
			}

			return root;
		}
	}
}
