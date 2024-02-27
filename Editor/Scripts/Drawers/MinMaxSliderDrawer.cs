using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            var minMaxAttribute = attribute as MinMaxSliderAttribute;

			if (property.propertyType == SerializedPropertyType.Vector2 || property.propertyType == SerializedPropertyType.Vector2Int)
			{
				bool isIntVector = property.propertyType == SerializedPropertyType.Vector2Int;

				float minValue = isIntVector ? property.vector2IntValue.x : property.vector2Value.x;
				float maxValue = isIntVector ? property.vector2IntValue.y : property.vector2Value.y;

				float oldMinValue = minValue;
				float oldMaxValue = maxValue;

				EditorGUI.BeginChangeCheck();

				if (minMaxAttribute.ShowValues)
				{
					float labelWidth = EditorGUIUtility.labelWidth;

					var prefixLabelRect = new Rect(position.x, position.y, labelWidth, EditorGUIUtility.singleLineHeight);
					EditorGUI.PrefixLabel(prefixLabelRect, label);

					float fieldWidth = (position.width - labelWidth - 200f) / 3f;
					const float fieldsOffset = 10f;

					var minValueRect = new Rect(prefixLabelRect.xMax, position.y, fieldWidth - fieldsOffset, EditorGUIUtility.singleLineHeight);
					var sliderRect = new Rect(minValueRect.xMax + fieldsOffset, position.y, position.width - labelWidth - fieldWidth * 2f, EditorGUIUtility.singleLineHeight);
					var maxValueRect = new Rect(sliderRect.xMax + fieldsOffset, position.y, fieldWidth - fieldsOffset, EditorGUIUtility.singleLineHeight);

					EditorGUI.MinMaxSlider(sliderRect, ref minValue, ref maxValue, minMaxAttribute.MinRange, minMaxAttribute.MaxRange);

					minValue = isIntVector ? EditorGUI.IntField(minValueRect, (int)minValue) : EditorGUI.FloatField(minValueRect, minValue);
					maxValue = isIntVector ? EditorGUI.IntField(maxValueRect, (int)maxValue) : EditorGUI.FloatField(maxValueRect, maxValue);
				}
				else
				{
					EditorGUI.MinMaxSlider(position, label, ref minValue, ref maxValue, minMaxAttribute.MinRange, minMaxAttribute.MaxRange);
				}

				if (EditorGUI.EndChangeCheck())
				{
					minValue = Mathf.Clamp(minValue, minMaxAttribute.MinRange, oldMaxValue);
					maxValue = Mathf.Clamp(maxValue, oldMinValue, minMaxAttribute.MaxRange);

					if (isIntVector)
					{
						property.vector2IntValue = new Vector2Int((int)minValue, (int)maxValue);
					}
					else
					{
						property.vector2Value = new Vector2(minValue, maxValue);
					}
				}
			}
			else
			{
				EditorGUILayout.HelpBox("MinMaxSlider Attribute can only be attached to a Vector2 and Vector2Int", MessageType.Warning);
			}
		}
	}
}
