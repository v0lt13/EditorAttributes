using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
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
				var variableProperty = FindNestedProperty(property, GetSerializedPropertyName(variableName, property));

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

					var propertyField = DrawProperty(variableProperty);

					propertyField.style.flexGrow = 1f;
					propertyField.style.flexBasis = 0.1f;
					groupBox.style.paddingLeft = 20f;

					if (variableProperty.propertyType != SerializedPropertyType.Generic) 
						groupBox.Add(label); // Do not add labels to serialized objects else it will show twice

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

		// Had to override this function to remove the label from property fields since they are drawn manualy
		protected override VisualElement DrawProperty(SerializedProperty property, Label label = null)
		{
			eventDrawer ??= new UnityEventDrawer();

			try
			{
				var eventContainer = eventDrawer.CreatePropertyGUI(property);
				var eventLabel = eventContainer.Q<Label>();

				eventLabel.text = label == null ? eventLabel.text : "";

				return eventContainer;
			}
			catch (NullReferenceException)
			{
				var propertyField = new PropertyField(property, "");

				propertyField.BindProperty(property);

				return propertyField;
			}
		}
	}
}
