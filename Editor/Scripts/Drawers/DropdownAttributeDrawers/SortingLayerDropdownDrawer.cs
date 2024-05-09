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
				var maskField = new MaskField(property.displayName, GetSortingLayerNames(), property.intValue) { showMixedValue = property.hasMultipleDifferentValues };

				root.schedule.Execute(() => maskField.Q(className: "unity-base-popup-field__input").style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f).ExecuteLater(1);

				maskField.AddToClassList("unity-base-field__aligned");

				maskField.RegisterValueChangedCallback(callback =>
				{
					property.intValue = maskField.value;
					property.serializedObject.ApplyModifiedProperties();
				});

				root.Add(maskField);
			}
			else
			{
				root.Add(new HelpBox("The SortingLayerDropdown attribute can only be attached to int fields", HelpBoxMessageType.Error));
			}

			return root;
		}

        private List<string> GetSortingLayerNames()
        {
            var layerList = new List<string>();

            foreach (var layer in SortingLayer.layers) layerList.Add(layer.name);

            return layerList;
        }
    }
}
