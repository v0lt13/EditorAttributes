using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(SortingLayerDropdownAttribute))]
	public class SortingLayerDropdownDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var root = new VisualElement();

			if (property.propertyType == SerializedPropertyType.Integer)
			{
				var maskField = new MaskField(property.displayName, GetSortingLayerNames(), property.intValue)
				{
					showMixedValue = property.hasMultipleDifferentValues,
					tooltip = property.tooltip
				};

				maskField.AddToClassList(BaseField<Void>.alignedFieldUssClassName);
				AddPropertyContextMenu(maskField, property);

				maskField.RegisterValueChangedCallback(callback =>
				{
					property.intValue = maskField.value;
					property.serializedObject.ApplyModifiedProperties();
				});

				maskField.TrackPropertyValue(property, (trackedProperty) => maskField.SetValueWithoutNotify(trackedProperty.intValue));

				root.Add(maskField);

				ExecuteLater(maskField, () => maskField.Q(className: MaskField.inputUssClassName).style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f);
			}
			else
			{
				root.Add(new HelpBox("The SortingLayerDropdown attribute can only be attached to int fields", HelpBoxMessageType.Error));
			}

			return root;
		}

		protected override void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue)
		{
			var dropdown = element as MaskField;

			base.PasteValue(element, property, clipboardValue);

			try
			{
				dropdown.SetValueWithoutNotify(int.Parse(clipboardValue));
			}
			catch (FormatException)
			{
				// Ignore, error will already be thrown by the base function
			}
		}

		private List<string> GetSortingLayerNames()
		{
			var layerList = new List<string>();

			foreach (var layer in SortingLayer.layers)
				layerList.Add(layer.name);

			return layerList;
		}
	}
}
