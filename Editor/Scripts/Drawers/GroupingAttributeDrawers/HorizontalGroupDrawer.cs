using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HorizontalGroupAttribute))]
    public class HorizontalGroupDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var horizontalGroup = attribute as HorizontalGroupAttribute;
			var root = new VisualElement();

			if (horizontalGroup.DrawInBox)
				ApplyBoxStyle(root);

			root.style.flexDirection = FlexDirection.Row;

			foreach (string variableName in horizontalGroup.FieldsToGroup)
			{
				var variableProperty = FindNestedProperty(property, variableName);

				// Check for serialized properties since they have a weird naming when serialized and they cannot be found by the normal name
				variableProperty ??= FindNestedProperty(property, $"<{variableName}>k__BackingField");

				if (variableProperty != null)
				{
					var groupBox = new VisualElement() 
					{
						style = {
							flexDirection = FlexDirection.Row,
							flexGrow = 1f,
							flexBasis = 0.1f,
							alignItems = Align.Center							
						}
					};

					var label = new Label(variableProperty.displayName)
					{
						style = {
							flexGrow = 1f,
							flexBasis = 0.1f,
							marginRight = horizontalGroup.WidthOffset
						}
					};

					var propertyField = DrawProperty(variableProperty, new Label());

					propertyField.style.flexGrow = 1f;
					propertyField.style.flexBasis = 0.1f;

					if (variableProperty.type != "Void") // Do not add labels to Void holders
					{
						groupBox.style.paddingLeft = 10f;
						groupBox.Add(label);
					}
					
					groupBox.Add(propertyField);
					root.Add(groupBox);
				}
				else
				{
					root.Add(new HelpBox($"{variableName} is not a valid field", HelpBoxMessageType.Error));
					break;
				}
			}

			return root;
		}
	}
}
