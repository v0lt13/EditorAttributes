using EditorAttributes.Editor.Utility;
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
				var errorBox = new HelpBox
				{
					text = $"No conversion found for <b>{unitFieldAttribute.DisplayUnit}</b> to <b>{unitFieldAttribute.ConversionUnit}</b>. You can add custom conversions in the <b>ProjectSettings/EditorAttributes</b> window"
				};

				DisplayErrorBox(root, errorBox);
				return root;
			}

			// Wait for Unity fields to be setup.
			root.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

			void OnGeometryChanged(GeometryChangedEvent changeEvent)
			{
				root.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);

				// This will query into all types that implement these numerics.
				// For example, the Unit attribute will also work with the three
				// float inputs in a Vector3 property drawer.
				root.Query<BindableElement>(className: TextInputBaseField<Void>.ussClassName).ForEach((element) =>
				{
					// We create the field copy after 25ms because otherwise the built
					// in property field automatically cleans manually created elements.
					ExecuteLater(root, () =>
					{
						var fieldCopy = CreateFieldCopy(element, property, unitFieldAttribute);

						if (fieldCopy == null)
							return;


						element.style.display = DisplayStyle.None;
						element.parent.Add(fieldCopy);

						if (CanApplyGlobalColor)
							ColorUtils.ApplyColor(fieldCopy, EditorExtension.GLOBAL_COLOR);

						fieldCopy.PlaceInFront(element);
						fieldCopy.AddToClassList(BaseField<Void>.alignedFieldUssClassName);

						var children = element.Query().Build();
						var childrenCopies = fieldCopy.Query().Build();

						for (int i = 0; i < children.Count(); i++)
							CopyStyle(childrenCopies.AtIndex(i), children.AtIndex(i));

						var textCopy = fieldCopy.Q(className: TextInputBaseField<Void>.inputUssClassName).Q<TextElement>();

						textCopy.style.flexGrow = 0;
						textCopy.style.flexShrink = 1;

						var suffix = new Label(convertedUnit.unitLabel)
						{
							focusable = false,
							tooltip = unitFieldAttribute.DisplayUnit,
							style =
							{
								color = CanApplyGlobalColor ? EditorExtension.GLOBAL_COLOR : Color.gray,
								unityFontDefinition = textCopy.style.unityFontDefinition,
								unityTextAlign = TextAnchor.MiddleRight,
								flexGrow = 1f,
								flexShrink = 1f,
								paddingLeft = 1f
							},
						};

						textCopy.parent.Add(suffix);
					}, 25L);
				});
			}

			return root;
		}

		private VisualElement CreateFieldCopy(BindableElement element, SerializedProperty property, UnitFieldAttribute unitFieldAttribute)
		{
			var convertedUnit = UnitConverter.GetConversion(unitFieldAttribute.DisplayUnit, unitFieldAttribute.ConversionUnit);

			var propertyInfo = ReflectionUtility.GetValidMemberInfo(property.name, property);
			var propertyType = ReflectionUtility.GetMemberInfoType(propertyInfo);

			if (propertyType == null)
				return null;

			var elementCopy = CreateFieldForType(propertyType, GetFieldLabel(element), Convert.ChangeType(Convert.ToDouble(GetFieldValue(element)) / convertedUnit.conversion, propertyType), property.hasMultipleDifferentValues);

			RegisterValueChangedCallbackByType(propertyType, elementCopy, (changeEvent) => SetFieldValue(element, Convert.ChangeType(Convert.ToDouble(GetFieldValue(elementCopy)) * convertedUnit.conversion, propertyType), true));

			element.TrackPropertyValue(property, (trackedProperty) => SetFieldValue(elementCopy, Convert.ChangeType(Convert.ToDouble(GetFieldValue(element)) / convertedUnit.conversion, propertyType)));

			return elementCopy;
		}
	}
}
