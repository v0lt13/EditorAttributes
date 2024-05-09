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
			var target = property.serializedObject.targetObject;

    		var root = new VisualElement();
		    var propertyField = DrawProperty(property);

			var function = ReflectionUtility.FindFunction(onValueChangedAttribute.FunctionName, property);
			var functionParameters = function.GetParameters();

			if (functionParameters.Length == 0)
			{
				root.schedule.Execute(() =>
				{
					var field = propertyField.Q(className: "unity-property-field") as PropertyField;

					field.RegisterValueChangeCallback((callback) => function.Invoke(target, null));
				}).ExecuteLater(1);

				root.Add(propertyField);
			}
			else
			{
				root.Add(propertyField);
				root.Add(new HelpBox("Function cannot have parameters", HelpBoxMessageType.Error));
			}

    		return root;
    	}
    }
}
