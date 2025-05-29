using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(OnValueChangedAttribute))]
	public class OnValueChangedDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var onValueChangedAttribute = attribute as OnValueChangedAttribute;
			ReflectionUtility.GetNestedObjectType(property, out object target);

			var root = new VisualElement();
			var propertyField = CreatePropertyField(property);

			var function = ReflectionUtility.FindFunction(onValueChangedAttribute.FunctionName, property);
			var functionParameters = function.GetParameters();

			if (functionParameters.Length == 0)
			{
				root.Add(propertyField);

				ExecuteLater(propertyField, () =>
				{
					var field = propertyField.Q(className: PropertyField.ussClassName) as PropertyField;

					field.RegisterValueChangeCallback((callback) => function.Invoke(target, null));
				});
			}
			else
			{
				root.Add(propertyField);
				root.Add(new HelpBox("The function cannot have parameters", HelpBoxMessageType.Error));
			}

			return root;
		}
	}
}
