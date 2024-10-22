using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using EditorAttributes.Editor.Utility;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(DataTableAttribute))]
    public class DataTableDrawer : PropertyDrawerBase
    {
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var dataTableAttribute = attribute as DataTableAttribute;
			var root = new VisualElement();

			if (property.propertyType != SerializedPropertyType.Generic)
			{
				var errorBox = new HelpBox("The DataTableAttribute can only be attached to serialized structs or classes and collections containing them", HelpBoxMessageType.Error);
				root.Add(errorBox);
				return root;
			}

			property.isExpanded = true;

			root.style.flexDirection = FlexDirection.Row;

			if (dataTableAttribute.DrawInBox)
				ApplyBoxStyle(root);

			var label = new Label(property.displayName) 
			{
				style = {
					unityFontStyleAndWeight = FontStyle.Bold,
					marginRight = 50f,
					maxWidth = 50f,
					alignSelf = Align.Center,
					color = EditorExtension.GLOBAL_COLOR
				}
			};			

			root.Add(label);

			var serializedProperty = property.Copy();
			int initialDepth = serializedProperty.depth;

			while (serializedProperty.NextVisible(true) && serializedProperty.depth > initialDepth)
			{
				if (serializedProperty.propertyType is SerializedPropertyType.Generic or SerializedPropertyType.Vector4 or SerializedPropertyType.ArraySize)
				{
					var errorBox = new HelpBox("Collection, UnityEvent and Serialized object types are not supported", HelpBoxMessageType.Error);
					root.Add(errorBox);
					break;
				}
				 
				if (serializedProperty.depth >= initialDepth + 2) // Skip the X Y Z properties that are inside Vectors since we draw the vector field ourself
					continue;

				var tableColumn = new VisualElement();
				tableColumn.style.flexGrow = 1f;
				tableColumn.style.flexBasis = 0.1f;

				if (dataTableAttribute.ShowLabels && IsNotFirstArrayElement(property))
				{
					var propertyLabel = new Label(serializedProperty.displayName);
					propertyLabel.style.color = EditorExtension.GLOBAL_COLOR;

					tableColumn.Add(propertyLabel);
				}

				var propertyField = DrawProperty(serializedProperty, new Label());

				propertyField.style.flexGrow = 1f;
				propertyField.style.marginRight = 10f;

				if (EditorExtension.GLOBAL_COLOR != EditorExtension.DEFAULT_GLOBAL_COLOR)
					ColorUtils.ApplyColor(propertyField, EditorExtension.GLOBAL_COLOR, 100);

				tableColumn.Add(propertyField);
				root.Add(tableColumn);
			}

			// When there are other attributes on the dataTable field they would recreate the label of the property field so we make sure it will never be there
			UpdateVisualElement(root, () =>
			{
				var labels = root.Query<Label>(className: "unity-base-field__label").ToList();

				foreach (var label in labels)
					label.RemoveFromHierarchy();
			});

			return root;
		}

		private bool IsNotFirstArrayElement(SerializedProperty property)
		{
			if (ReflectionUtility.IsPropertyCollection(property))
			{
				var splitName = property.displayName.Split(" ");

				return splitName[^1] == "0";
			}

			return true;
		}
	}
}
