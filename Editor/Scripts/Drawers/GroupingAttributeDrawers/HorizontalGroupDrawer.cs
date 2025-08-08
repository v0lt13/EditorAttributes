using UnityEngine;
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
			root.style.alignItems = Align.FlexStart;

			foreach (string variableName in horizontalGroup.FieldsToGroup)
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

				// Add space between properties excluding the last property
				if (ArrayUtility.LastIndexOf(horizontalGroup.FieldsToGroup, variableName) != horizontalGroup.FieldsToGroup.Length - 1)
					groupBox.style.marginRight = horizontalGroup.PropertySpace;

				var variableProperty = FindNestedProperty(property, GetSerializedPropertyName(variableName, property));
				var memberInfo = ReflectionUtility.GetValidMemberInfo(variableName, property);

				var propertyField = CreateField(variableName, variableProperty);

				if (variableProperty == null)
				{
					groupBox.Add(propertyField);
					root.Add(groupBox);

					continue;
				}

				var hideLabelAttribute = memberInfo?.GetCustomAttribute<HideLabelAttribute>();

				propertyField.style.flexGrow = 1f;
				propertyField.style.flexBasis = 0.1f;
				groupBox.style.paddingLeft = 20f;

				if (hideLabelAttribute == null)
				{
					var renameAttribute = memberInfo?.GetCustomAttribute<RenameAttribute>();
					var tooltipAttribute = memberInfo?.GetCustomAttribute<TooltipAttribute>();

					string labelText = renameAttribute == null ? ObjectNames.NicifyVariableName(variableName) : RenameDrawer.GetNewName(renameAttribute, property, errorBox);

					var label = new Label(labelText)
					{
						tooltip = tooltipAttribute?.tooltip,
						style = {
							flexGrow = 1f,
							flexBasis = 0.1f,
							marginRight = horizontalGroup.WidthOffset
						}
					};

					// Serialized objects and Vector 4 are displayed with foldouts and don't need the custom label
					if (variableProperty.propertyType is not SerializedPropertyType.Generic and not SerializedPropertyType.Vector4)
						groupBox.Add(label);

					if (renameAttribute != null)
					{
						UpdateVisualElement(label, () =>
						{
							label.text = RenameDrawer.GetNewName(renameAttribute, property, errorBox);
							DisplayErrorBox(propertyField, errorBox);
						});
					}
				}

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

					if (variableProperty.propertyType is not SerializedPropertyType.Generic and not SerializedPropertyType.Vector4)
					{
						var propertyLabel = field.Q<Label>();

						if (propertyLabel != null)
						{
							if (!propertyLabel.parent.ClassListContains(BaseCompositeField<Void, IntegerField, int>.fieldUssClassName)) // Do not remove the label from composite fields
								propertyLabel.RemoveFromHierarchy();
						}
					}
					else
					{
						var alignedFields = field.Query<VisualElement>(className: BaseField<Void>.alignedFieldUssClassName).ToList();

						// Fix alignment issues with fields inside foldouts
						foreach (var alignedField in alignedFields)
						{
							alignedField.RemoveFromClassList(BaseField<Void>.alignedFieldUssClassName);

							var alignedFieldLabel = alignedField.Q<Label>();

							alignedFieldLabel.style.width = 0f;
							alignedFieldLabel.style.minWidth = 80f;
							alignedFieldLabel.style.marginRight = (attribute as HorizontalGroupAttribute).WidthOffset;
						}
					}
				}, 100L).ForDuration(400L);

				field.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
			}

			return field;
		}
	}
}
