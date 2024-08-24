using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

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

				var label = new Label(property.displayName);
				var minMaxSlider = new MinMaxSlider(minValue, maxValue, minMaxSliderAttribute.MinRange, minMaxSliderAttribute.MaxRange) 
				{
					style = {
						flexGrow = 1f,
						paddingLeft = 5f,
						paddingRight = 5f
					}
				};

				root.style.flexDirection = FlexDirection.Row;
				label.style.minWidth = 150f;

				if (CanApplyGlobalColor)
				{
					root.schedule.Execute(() =>
					{
						minMaxSlider.Q("unity-dragger").style.unityBackgroundImageTintColor = EditorExtension.GLOBAL_COLOR;
						minMaxSlider.Q("unity-tracker").style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
					}).ExecuteLater(1);
				}

				if (minMaxSliderAttribute.ShowValues)
				{
					var minField = new FloatField(5) { showMixedValue = property.hasMultipleDifferentValues, style = { maxWidth = 50f, minWidth = 50f } };
					var maxField = new FloatField(5) { showMixedValue = property.hasMultipleDifferentValues, style = { maxWidth = 50f, minWidth = 50f } };

					if (CanApplyGlobalColor)
					{
						root.schedule.Execute(() =>
						{
							minField.Q("unity-text-input").style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
							maxField.Q("unity-text-input").style.backgroundColor = EditorExtension.GLOBAL_COLOR / 2f;
						}).ExecuteLater(1);
					}

					// Initialize the fields
					minField.SetValueWithoutNotify(minValue);
					maxField.SetValueWithoutNotify(maxValue);

					minMaxSlider.RegisterValueChangedCallback((callback) => 
					{
						minField.SetValueWithoutNotify(isIntVector ? (int)callback.newValue.x : callback.newValue.x);
						maxField.SetValueWithoutNotify(isIntVector ? (int)callback.newValue.y : callback.newValue.y);

						ApplyPropertyValues(property, isIntVector, minMaxSlider.minValue, minMaxSlider.maxValue); 
					});

					minField.RegisterValueChangedCallback((callback) => 
					{
						if (isIntVector) 
							minField.value = (int)callback.newValue;
						
						minMaxSlider.value = isIntVector ? new Vector2Int((int)callback.newValue, (int)minMaxSlider.value.y) : new Vector2(callback.newValue, minMaxSlider.value.y);

						ApplyPropertyValues(property, isIntVector, minMaxSlider.minValue, minMaxSlider.maxValue);
					});

					maxField.RegisterValueChangedCallback((callback) =>
					{
						if (isIntVector)
							maxField.value = (int)callback.newValue;

						minMaxSlider.value = isIntVector ? new Vector2Int((int)minMaxSlider.value.x, (int)callback.newValue) : new Vector2(minMaxSlider.value.x, callback.newValue);

						ApplyPropertyValues(property, isIntVector, minMaxSlider.minValue, minMaxSlider.maxValue);
					});

					root.Add(label);
					root.Add(minField);
					root.Add(minMaxSlider);
					root.Add(maxField);
				}
				else
				{
					minMaxSlider.RegisterValueChangedCallback((callback) => ApplyPropertyValues(property, isIntVector, minMaxSlider.minValue, minMaxSlider.maxValue));

					root.Add(label);
					root.Add(minMaxSlider);
				}
			}
			else
			{
				root.Add(new HelpBox("MinMaxSlider Attribute can only be attached to a Vector2 and Vector2Int", HelpBoxMessageType.Warning));
			}

			return root;
		}

		private void ApplyPropertyValues(SerializedProperty property, bool isIntVector, float minValue, float maxValue)
		{
			if (isIntVector)
			{
				property.vector2IntValue = new Vector2Int((int)minValue, (int)maxValue);
			}
			else
			{
				property.vector2Value = new Vector2(minValue, maxValue);
			}

			property.serializedObject.ApplyModifiedProperties();
		}
	}
}
