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
            root.RegisterCallbackOnce<GeometryChangedEvent>((changeEvent) =>
            {
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
                        suffix.style.fontSize = 10;
                        suffix.style.unityFontDefinition = textCopy.style.unityFontDefinition;
                        suffix.style.opacity = 0.5f;
                        suffix.style.unityTextAlign = TextAnchor.MiddleLeft;
                        suffix.style.flexGrow = 1;
                        suffix.style.flexShrink = 1;
                        suffix.style.paddingLeft = 1;
                        suffix.focusable = false;

                        textCopy.parent.Add(suffix);
                    }, 25);
                });
            });

            return root;
        }

        private VisualElement CreateFieldCopy(BindableElement element, SerializedProperty property, UnitFieldAttribute unitFieldAttribute)
        {
            UnitConverter convertedUnit = GetConversion();

            if (element is FloatField)
            {
                FloatField floatField = element as FloatField;

                var floatFieldCopy = CreateFieldForType<float>(floatField.label, (float)(floatField.value / convertedUnit.conversion), property.hasMultipleDifferentValues) as FloatField;
                floatFieldCopy.RegisterValueChangedCallback((changeEvent) => floatField.value = (float)(floatFieldCopy.value * GetConversion().conversion));
                floatField.TrackPropertyValue(property, (property) => floatFieldCopy.SetValueWithoutNotify((float)(floatField.value / GetConversion().conversion)));
                return floatFieldCopy;
            }
            else if (element is DoubleField)
            {
                DoubleField doubleField = element as DoubleField;

                var doubleFieldCopy = CreateFieldForType<double>(doubleField.label, doubleField.value / convertedUnit.conversion, property.hasMultipleDifferentValues) as DoubleField;
                doubleFieldCopy.RegisterValueChangedCallback((changeEvent) => doubleField.value = doubleFieldCopy.value * GetConversion().conversion);
                doubleField.TrackPropertyValue(property, (property) => doubleFieldCopy.SetValueWithoutNotify(doubleField.value / GetConversion().conversion));
                return doubleFieldCopy;
            }
            else if (element is IntegerField)
            {
                IntegerField integerField = element as IntegerField;

                var integerFieldCopy = CreateFieldForType<int>(integerField.label, (int)(integerField.value / convertedUnit.conversion), property.hasMultipleDifferentValues) as IntegerField;
                integerFieldCopy.RegisterValueChangedCallback((changeEvent) => integerField.value = (int)(integerFieldCopy.value * GetConversion().conversion));
                integerField.TrackPropertyValue(property, (property) => integerFieldCopy.SetValueWithoutNotify((int)(integerField.value / GetConversion().conversion)));
                return integerFieldCopy;
            }
            else if (element is UnsignedIntegerField)
            {
                UnsignedIntegerField unsignedIntegerField = element as UnsignedIntegerField;

                var unsignedIntegerFieldCopy = CreateFieldForType<uint>(unsignedIntegerField.label, (uint)(unsignedIntegerField.value / convertedUnit.conversion), property.hasMultipleDifferentValues) as UnsignedIntegerField;
                unsignedIntegerFieldCopy.RegisterValueChangedCallback((changeEvent) => unsignedIntegerField.value = (uint)(unsignedIntegerFieldCopy.value * GetConversion().conversion));
                unsignedIntegerField.TrackPropertyValue(property, (property) => unsignedIntegerFieldCopy.SetValueWithoutNotify((uint)(unsignedIntegerField.value / GetConversion().conversion)));
                return unsignedIntegerFieldCopy;
            }
            else if (element is LongField)
            {
                LongField longField = element as LongField;

                var longFieldCopy = CreateFieldForType<long>(longField.label, (long)(longField.value / convertedUnit.conversion), property.hasMultipleDifferentValues) as LongField;
                longFieldCopy.RegisterValueChangedCallback((changeEvent) => longField.value = (long)(longFieldCopy.value * GetConversion().conversion));
                longField.TrackPropertyValue(property, (property) => longFieldCopy.SetValueWithoutNotify((long)(longField.value / GetConversion().conversion)));
                return longFieldCopy;
            }
            else if (element is UnsignedLongField)
            {
                UnsignedLongField unsignedLongField = element as UnsignedLongField;

                var unsignedLongFieldCopy = CreateFieldForType<ulong>(unsignedLongField.label, (ulong)(unsignedLongField.value / convertedUnit.conversion), property.hasMultipleDifferentValues) as UnsignedLongField;
                unsignedLongFieldCopy.RegisterValueChangedCallback((changeEvent) => unsignedLongField.value = (ulong)(unsignedLongFieldCopy.value * GetConversion().conversion));
                unsignedLongField.TrackPropertyValue(property, (property) => unsignedLongFieldCopy.SetValueWithoutNotify((ulong)(unsignedLongField.value / GetConversion().conversion)));
                return unsignedLongFieldCopy;
            }
            else
                return null;

            UnitConverter GetConversion() => UnitConverter.GetConversion(unitFieldAttribute.DisplayUnit, unitFieldAttribute.ConversionUnit);
        }

        private void CopyStyle(VisualElement copyTo, VisualElement copyFrom)
        {
            copyTo.style.position = copyFrom.style.position;
            copyTo.style.top = copyFrom.style.top;
            copyTo.style.bottom = copyFrom.style.bottom;
            copyTo.style.left = copyFrom.style.left;
            copyTo.style.right = copyFrom.style.right;
            copyTo.style.paddingTop = copyFrom.style.paddingTop;
            copyTo.style.paddingBottom = copyFrom.style.paddingBottom;
            copyTo.style.paddingLeft = copyFrom.style.paddingLeft;
            copyTo.style.paddingRight = copyFrom.style.paddingRight;
            copyTo.style.alignContent = copyFrom.style.alignContent;
            copyTo.style.alignItems = copyFrom.style.alignItems;
            copyTo.style.alignSelf = copyFrom.style.alignSelf;
            copyTo.style.flexBasis = copyFrom.style.flexBasis;
            copyTo.style.flexDirection = copyFrom.style.flexDirection;
            copyTo.style.flexWrap = copyFrom.style.flexWrap;
            copyTo.style.width = copyFrom.style.width;
            copyTo.style.height = copyFrom.style.height;
            copyTo.style.justifyContent = copyFrom.style.justifyContent;
            copyTo.style.marginTop = copyFrom.style.marginTop;
            copyTo.style.marginBottom = copyFrom.style.marginBottom;
            copyTo.style.marginLeft = copyFrom.style.marginLeft;
            copyTo.style.marginRight = copyFrom.style.marginRight;
            copyTo.style.transformOrigin = copyFrom.style.transformOrigin;
            copyTo.style.translate = copyFrom.style.translate;
            copyTo.style.rotate = copyFrom.style.rotate;
            copyTo.style.scale = copyFrom.style.scale;
            copyTo.style.transitionDelay = copyFrom.style.transitionDelay;
            copyTo.style.transitionDuration = copyFrom.style.transitionDuration;
            copyTo.style.transitionProperty = copyFrom.style.transitionProperty;
            copyTo.style.transitionTimingFunction = copyFrom.style.transitionTimingFunction;
            copyTo.style.color = copyFrom.style.color;
            copyTo.style.backgroundColor = copyFrom.style.backgroundColor;
            copyTo.style.unityBackgroundImageTintColor = copyFrom.style.unityBackgroundImageTintColor;
            copyTo.style.backgroundImage = copyFrom.style.backgroundImage;
            copyTo.style.backgroundPositionX = copyFrom.style.backgroundPositionX;
            copyTo.style.backgroundPositionY = copyFrom.style.backgroundPositionY;
            copyTo.style.backgroundRepeat = copyFrom.style.backgroundRepeat;
            copyTo.style.backgroundSize = copyFrom.style.backgroundSize;
            copyTo.style.opacity = copyFrom.style.opacity;
            copyTo.style.unityOverflowClipBox = copyFrom.style.unityOverflowClipBox;
            copyTo.style.minWidth = copyFrom.style.minWidth;
            copyTo.style.maxWidth = copyFrom.style.maxWidth;
            copyTo.style.minHeight = copyFrom.style.minHeight;
            copyTo.style.maxHeight = copyFrom.style.maxHeight;
            copyTo.style.borderTopColor = copyFrom.style.borderTopColor;
            copyTo.style.borderBottomColor = copyFrom.style.borderBottomColor;
            copyTo.style.borderLeftColor = copyFrom.style.borderLeftColor;
            copyTo.style.borderRightColor = copyFrom.style.borderRightColor;
            copyTo.style.fontSize = copyFrom.style.fontSize;
            copyTo.style.unityFont = copyFrom.style.unityFont;
            copyTo.style.unityFontStyleAndWeight = copyFrom.style.unityFontStyleAndWeight;
            copyTo.style.unityFontDefinition = copyFrom.style.unityFontDefinition;
            copyTo.style.unityTextAlign = copyFrom.style.unityTextAlign;
            copyTo.style.textShadow = copyFrom.style.textShadow;
            copyTo.style.unityTextOutlineColor = copyFrom.style.unityTextOutlineColor;
            copyTo.style.unityEditorTextRenderingMode = copyFrom.style.unityEditorTextRenderingMode;
            copyTo.style.unityTextOverflowPosition = copyFrom.style.unityTextOverflowPosition;
            copyTo.style.textOverflow = copyFrom.style.textOverflow;
            copyTo.style.unityTextGenerator = copyFrom.style.unityTextGenerator;
            copyTo.style.unityTextOutlineWidth = copyFrom.style.unityTextOutlineWidth;
            copyTo.style.wordSpacing = copyFrom.style.wordSpacing;
            copyTo.style.unityParagraphSpacing = copyFrom.style.unityParagraphSpacing;
            copyTo.style.whiteSpace = copyFrom.style.whiteSpace;
            copyTo.style.cursor = copyFrom.style.cursor;
            copyTo.style.overflow = copyFrom.style.overflow;
            foreach (var c in copyFrom.GetClasses())
                copyTo.AddToClassList(c);
        }
    }
}
