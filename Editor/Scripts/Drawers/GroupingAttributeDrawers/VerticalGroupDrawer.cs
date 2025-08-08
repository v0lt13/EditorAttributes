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
				var groupBox = new VisualElement()
				{
					style = {
						flexDirection = FlexDirection.Row,
						alignItems = Align.Center
					}
				};

				var variableProperty = FindNestedProperty(property, GetSerializedPropertyName(variableName, property));

				var propertyField = CreateField(variableName, variableProperty);

				propertyField.style.flexGrow = 1f;
				propertyField.style.flexBasis = 0.1f;

				if (variableProperty == null)
				{
					groupBox.Add(propertyField);
					root.Add(groupBox);

					continue;
				}

				if (variableProperty.propertyType == SerializedPropertyType.Generic && verticalGroup.DrawInBox) // Add an offset to serialized objects drawn in a box
					propertyField.style.marginLeft = 10f;

				groupBox.Add(propertyField);
				root.Add(groupBox);
			}

			return root;
		}

		private VisualElement CreateField(string variableName, SerializedProperty variableProperty)
		{
			VisualElement field;

			if (variableProperty == null)
				return new HelpBox($"<b>{variableName}</b> is not a valid field or property", HelpBoxMessageType.Error);

			field = CreatePropertyField(variableProperty);

			field.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

			void OnGeometryChanged(GeometryChangedEvent changeEvent)
			{
				// Force update this logic to make sure fields are visible
				UpdateVisualElement(field, () =>
				{
					var hiddenField = field.Q<VisualElement>(HidePropertyDrawer.HIDDEN_PROPERTY_ID);

					if (hiddenField != null)
					{
						hiddenField.name = GROUPED_PROPERTY_ID;
						hiddenField.style.display = DisplayStyle.Flex;
					}

					var alignedFields = field.Query<VisualElement>(className: BaseField<Void>.alignedFieldUssClassName).ToList();

					// Fix alignment issues with fields
					foreach (var alignedField in alignedFields)
					{
						alignedField.RemoveFromClassList(BaseField<Void>.alignedFieldUssClassName);

						var alignedFieldLabel = alignedField.Q<Label>();

						alignedFieldLabel.style.width = 0f;
						alignedFieldLabel.style.minWidth = 100f;
					}
				}, 100L).ForDuration(400L);

				field.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
			}

			return field;
		}
	}
}
