using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(LayerDropdownAttribute))]
    public class LayerDropdownDrawer : CollectionDisplayDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.String)
                return new HelpBox("The LayerDropdown Attribute can only be attached to a string", HelpBoxMessageType.Error);

            List<string> layerNames = InternalEditorUtility.layers.ToList();
            DropdownField dropdownField = CreateDropdownField(layerNames, property);

            UpdateVisualElement(dropdownField, () =>
            {
                List<string> sceneNames = InternalEditorUtility.layers.ToList();

                if (IsCollectionValid(sceneNames))
                    dropdownField.choices = sceneNames;
            });

            return dropdownField;
        }

        protected override void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue)
        {
            var dropdown = element as DropdownField;

            if (dropdown.choices.Contains(clipboardValue))
            {
                base.PasteValue(element, property, clipboardValue);
                dropdown.SetValueWithoutNotify(clipboardValue);
            }
            else
            {
                Debug.LogWarning($"Could not paste value <b>{clipboardValue}</b> since is not availiable as an option in the dropdown");
            }
        }
    }
}
