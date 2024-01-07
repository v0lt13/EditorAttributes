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
					EditorGUILayout.BeginHorizontal();

					EditorGUILayout.PrefixLabel(label);

					minValue = isIntVector ? EditorGUILayout.IntField((int)minValue, GUILayout.Width(50f)) : EditorGUILayout.FloatField(minValue, GUILayout.Width(50f));

					EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, minMaxAttribute.MinRange, minMaxAttribute.MaxRange);

					maxValue = isIntVector ? EditorGUILayout.IntField((int)maxValue, GUILayout.Width(50f)) : EditorGUILayout.FloatField(maxValue, GUILayout.Width(50f));

					EditorGUILayout.EndHorizontal();
				}
				else
				{
					EditorGUILayout.MinMaxSlider(label, ref minValue, ref maxValue, minMaxAttribute.MinRange, minMaxAttribute.MaxRange);
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

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => -EditorGUIUtility.standardVerticalSpacing;
	}
}
