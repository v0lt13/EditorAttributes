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
			var target = property.serializedObject.targetObject;

			var function = ReflectionUtility.FindFunction(buttonFieldAttribute.FunctionName, target);
			var functionParameters = function.GetParameters();

			var root = new VisualElement();

			if (functionParameters.Length == 0)
			{
				var button = new Button(() => function.Invoke(target, null)) { text = string.IsNullOrWhiteSpace(buttonFieldAttribute.ButtonLabel) ? function.Name : buttonFieldAttribute.ButtonLabel };

				button.style.height = buttonFieldAttribute.ButtonHeight;

				root.Add(button);
			}
			else
			{
				root.Add(new HelpBox("Function cannot have parameters", HelpBoxMessageType.Error));
			}

			return root;
		}
	}
}
