using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(UnitFieldAttribute))]
	public class UnitFieldDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var unitFieldAttribute = attribute as UnitFieldAttribute;

            var root = CreatePropertyField(property);

            var convertedUnit = UnitConverter.GetConversion(unitFieldAttribute.DisplayUnit, unitFieldAttribute.ConversionUnit);

            if (convertedUnit == null)
            {
                var errorBox = new HelpBox();
                errorBox.text = $"No conversion found for <b>{unitFieldAttribute.DisplayUnit}</b> to <b>{unitFieldAttribute.ConversionUnit}</b>. You can add custom conversions in the <b>ProjectSettings/EditorAttributes</b> window";

                DisplayErrorBox(root, errorBox);
                return root;
            }

            // Wait for Unity fields to be setup.
            root.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

            void OnGeometryChanged(GeometryChangedEvent changeEvent)
            {
                root.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);

                root.Query<BindableElement>(className: TextInputBaseField<Void>.ussClassName).ForEach((element) =>
                {
                    // This will query into all types that implement these numerics.
                    // For example, the Unit attribute will also work with the three
                    // float inputs in a Vector3 property drawer.
                    //
                    // We create the field copy after 25ms because otherwise the built
                    // in property field automatically cleans manually created elements.
                    ExecuteLater(root, () =>
                    {
                        var fieldCopy = CreateFieldCopy(element, property, unitFieldAttribute);

                        if (fieldCopy == null)
                            return;

                        element.style.display = DisplayStyle.None;
                        element.parent.Add(fieldCopy);
                        fieldCopy.PlaceInFront(element);

                        var children = element.Query().Build();
                        var childrenCopies = fieldCopy.Query().Build();

                        for (int i = 0; i < children.Count(); i++)
                            CopyStyle(childrenCopies.AtIndex(i), children.AtIndex(i));

                        var textCopy = fieldCopy.Q<TextElement>(className: TextElement.selectableUssClassName);

                        textCopy.style.flexGrow = 0;
                        textCopy.style.flexShrink = 1;

                        var suffix = new Label(convertedUnit.unitLabel);
                        suffix.tooltip = unitFieldAttribute.DisplayUnit;
                        suffix.style.color = textCopy.style.color;
                        suffix.style.unityFontDefinition = textCopy.style.unityFontDefinition;
                        suffix.style.opacity = 0.5f;
                        suffix.style.unityTextAlign = TextAnchor.MiddleRight;
                        suffix.style.flexGrow = 1;
                        suffix.style.flexShrink = 1;
                        suffix.style.paddingLeft = 1;
                        suffix.focusable = false;

                        textCopy.parent.Add(suffix);
                    }, 25);
                });
            }

            return root;
        }

        // NOTE: TrackPropertyValue is used to sync value changes from
        // non-inspector sources back to the inspector (like a script
        // that changes values in OnValidate).
        private VisualElement CreateFieldCopy(BindableElement element, SerializedProperty property, UnitFieldAttribute unitFieldAttribute)
        {
            UnitConverter GetConversion() => UnitConverter.GetConversion(unitFieldAttribute.DisplayUnit, unitFieldAttribute.ConversionUnit);
            UnitConverter convertedUnit = GetConversion();

            Type fieldType = GetFieldType(element, FieldMask.Numeric);

            var elementCopy = CreateFieldForType(fieldType, GetFieldLabel(element), Convert.ChangeType(Convert.ToDouble(GetFieldValue(element)) / convertedUnit.conversion, fieldType), property.hasMultipleDifferentValues);
            RegisterValueChangedCallbackByType(fieldType, elementCopy, (changeEvent) => SetFieldValue(element, Convert.ChangeType(Convert.ToDouble(GetFieldValue(elementCopy)) * GetConversion().conversion, fieldType), true));
            element.TrackPropertyValue(property, (property) => SetFieldValue(elementCopy, Convert.ChangeType(Convert.ToDouble(GetFieldValue(element)) / GetConversion().conversion, fieldType)));

            return elementCopy;
        }
    }
}
