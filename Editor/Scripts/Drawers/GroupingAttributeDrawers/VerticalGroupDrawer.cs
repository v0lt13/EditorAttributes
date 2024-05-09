using UnityEditor;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(VerticalGroupAttribute))]
    public class VerticalGroupDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var verticalGroup = attribute as VerticalGroupAttribute;
			var root = new VisualElement();

			if (verticalGroup.DrawInBox)
				ApplyBoxStyle(root);

			foreach (string variableName in verticalGroup.FieldsToGroup)
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
							alignItems = Align.Center
						}
					};

					var label = new Label(variableProperty.displayName) 
					{
						style = {
							flexGrow = 1f,
							flexBasis = 0.1f,
							marginRight = verticalGroup.WidthOffset
						}
					};

					var propertyField = DrawProperty(variableProperty, new Label());

					propertyField.style.flexGrow = 1f;
					propertyField.style.flexBasis = 0.1f;

					if (variableProperty.type != "Void") // Do not add labels to Void holders
						groupBox.Add(label);

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
