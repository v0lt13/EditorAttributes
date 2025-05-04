using UnityEditor;
using System.Reflection;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

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
					var errorBox = new HelpBox();
					var groupBox = new VisualElement() 
					{
						style = {
							flexDirection = FlexDirection.Row,
							flexGrow = 1f,
							flexBasis = 0.1f,
							alignItems = Align.Center							
						}
					};

					var fieldInfo = ReflectionUtility.GetValidMemberInfo(variableProperty.name, property) as FieldInfo;
					var renameAttribute = fieldInfo?.GetCustomAttribute<RenameAttribute>();

					string labelText = renameAttribute == null ? variableProperty.displayName : RenameDrawer.GetNewName(renameAttribute, variableProperty, errorBox);

					var label = new Label(labelText)
					{
						tooltip = variableProperty.tooltip,
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

					if (variableProperty.propertyType != SerializedPropertyType.Generic) // Do not add labels to serialized objects else it will show twice
						groupBox.Add(label);

					groupBox.Add(propertyField);
					root.Add(groupBox);

					if (renameAttribute != null)
					{
						UpdateVisualElement(label, () =>
						{
							label.text = RenameDrawer.GetNewName(renameAttribute, property, errorBox);
							DisplayErrorBox(propertyField, errorBox);
						});
					}
				}
				else
				{
					root.Add(new HelpBox($"{variableName} is not a valid field", HelpBoxMessageType.Error));
					break;
				}
			}

			return root;
		}

		private VisualElement DrawProperty(SerializedProperty property)
		{
			var propertyField = CreatePropertyField(property);

			if (property.propertyType != SerializedPropertyType.Generic)
			{
				ExecuteLater(propertyField, () =>
				{
					var propertyLabel = propertyField.Q<Label>();

					propertyLabel?.RemoveFromHierarchy();
				});
			}

			return propertyField;
		}
	}
}
