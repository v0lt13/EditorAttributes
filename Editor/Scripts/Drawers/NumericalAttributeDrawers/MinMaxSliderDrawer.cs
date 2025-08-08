using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace EditorAttributes.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
	public class MinMaxSliderDrawer : PropertyDrawerBase
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var minMaxSliderAttribute = attribute as MinMaxSliderAttribute;
			var root = new VisualElement();

			if (property.propertyType == SerializedPropertyType.Vector2 || property.propertyType == SerializedPropertyType.Vector2Int)
			{
				bool isIntVector = property.propertyType == SerializedPropertyType.Vector2Int;

				float minValue = isIntVector ? property.vector2IntValue.x : property.vector2Value.x;
				float maxValue = isIntVector ? property.vector2IntValue.y : property.vector2Value.y;

				var sliderHolder = new VisualElement
				{
					style = {
						flexDirection = FlexDirection.Row,
						flexGrow = 1f
					}
				};

				var label = new Label(property.displayName)
				{
					tooltip = property.tooltip,
					style = {
						overflow = Overflow.Hidden,
						alignSelf = Align.Center,
						paddingLeft = 3f,
						minWidth = 120f,
						maxWidth = 200f,
						flexGrow = 1f
					}
				};

				var minMaxSlider = new MinMaxSlider(minValue, maxValue, minMaxSliderAttribute.MinRange, minMaxSliderAttribute.MaxRange)
				{
					style = {
						flexGrow = 1f,
						paddingLeft = 5f,
						paddingRight = 5f
					}
				};

				root.style.flexDirection = FlexDirection.Row;

				AddPropertyContextMenu(root, property);

				root.TrackPropertyValue(property, (trackedProperty) => minMaxSlider.value = isIntVector ? trackedProperty.vector2IntValue : trackedProperty.vector2Value);

				if (minMaxSliderAttribute.ShowValues)
				{
					var minField = new FloatField(5) { showMixedValue = property.hasMultipleDifferentValues, style = { maxWidth = 50f, minWidth = 50f } };
					var maxField = new FloatField(5) { showMixedValue = property.hasMultipleDifferentValues, style = { maxWidth = 50f, minWidth = 50f } };

					// Initialize the fields
					minField.SetValueWithoutNotify(minValue);
					maxField.SetValueWithoutNotify(maxValue);

					minMaxSlider.RegisterValueChangedCallback((callback) =>
					{
						minField.SetValueWithoutNotify(isIntVector ? Mathf.RoundToInt(callback.newValue.x) : callback.newValue.x);
						maxField.SetValueWithoutNotify(isIntVector ? Mathf.RoundToInt(callback.newValue.y) : callback.newValue.y);

						if (isIntVector) // This will snap the handles when changing the values
						{
							minMaxSlider.minValue = Mathf.RoundToInt(callback.newValue.x);
							minMaxSlider.maxValue = Mathf.RoundToInt(callback.newValue.y);
						}

						ApplyPropertyValues(property, isIntVector, minMaxSlider.minValue, minMaxSlider.maxValue);
					});

					minField.RegisterValueChangedCallback((callback) =>
					{
						if (isIntVector)
							minField.value = (int)callback.newValue;

						minMaxSlider.value = isIntVector ? new Vector2Int(Mathf.RoundToInt(callback.newValue), Mathf.RoundToInt(minMaxSlider.value.y)) : new Vector2(callback.newValue, minMaxSlider.value.y);

						ApplyPropertyValues(property, isIntVector, minMaxSlider.minValue, minMaxSlider.maxValue);
					});

					maxField.RegisterValueChangedCallback((callback) =>
					{
						if (isIntVector)
							maxField.value = (int)callback.newValue;

						minMaxSlider.value = isIntVector ? new Vector2Int(Mathf.RoundToInt(minMaxSlider.value.x), Mathf.RoundToInt(callback.newValue)) : new Vector2(minMaxSlider.value.x, callback.newValue);

						ApplyPropertyValues(property, isIntVector, minMaxSlider.minValue, minMaxSlider.maxValue);
					});

					sliderHolder.Add(minField);
					sliderHolder.Add(minMaxSlider);
					sliderHolder.Add(maxField);

					root.Add(label);
					root.Add(sliderHolder);

					if (CanApplyGlobalColor)
					{
						ExecuteLater(root, () =>
						{
							minField.Q("unity-text-input").style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
							maxField.Q("unity-text-input").style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
						});
					}
				}
				else
				{
					minMaxSlider.RegisterValueChangedCallback((callback) =>
					{
						if (isIntVector) // This will snap the handles when changing the values
						{
							minMaxSlider.minValue = Mathf.RoundToInt(callback.newValue.x);
							minMaxSlider.maxValue = Mathf.RoundToInt(callback.newValue.y);
						}

						ApplyPropertyValues(property, isIntVector, minMaxSlider.minValue, minMaxSlider.maxValue);
					});

					sliderHolder.Add(minMaxSlider);

					root.Add(label);
					root.Add(sliderHolder);
				}

				if (CanApplyGlobalColor)
				{
					ExecuteLater(minMaxSlider, () =>
					{
						minMaxSlider.Q("unity-dragger").style.unityBackgroundImageTintColor = EditorExtension.GLOBAL_COLOR;
						minMaxSlider.Q("unity-tracker").style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
					});
				}
			}
			else
			{
				root.Add(new HelpBox("MinMaxSlider Attribute can only be attached to a Vector2 and Vector2Int", HelpBoxMessageType.Warning));
			}

			return root;
		}

		protected override void PasteValue(VisualElement element, SerializedProperty property, string clipboardValue)
		{
			var minMaxSlider = element.Q<MinMaxSlider>();

			base.PasteValue(element, property, clipboardValue);
			minMaxSlider.value = property.propertyType == SerializedPropertyType.Vector2 ? property.vector2Value : property.vector2IntValue;
		}

		private void ApplyPropertyValues(SerializedProperty property, bool isIntVector, float minValue, float maxValue)
		{
			if (isIntVector)
			{
				property.vector2IntValue = new Vector2Int(Mathf.RoundToInt(minValue), Mathf.RoundToInt(maxValue));
			}
			else
			{
				property.vector2Value = new Vector2(minValue, maxValue);
			}

			property.serializedObject.ApplyModifiedProperties();
		}
	}
}
