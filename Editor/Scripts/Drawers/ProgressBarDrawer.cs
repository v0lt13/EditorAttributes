using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
            if (property.propertyType == SerializedPropertyType.Integer || property.propertyType == SerializedPropertyType.Float)
            {
                var progressBarAttribute = attribute as ProgressBarAttribute;

				if (ColorUtility.TryParseHtmlString(progressBarAttribute.HexColor, out Color color))
				{
					DrawProgressBar(position, property, label, color);
					return;
				}
				else if (!string.IsNullOrEmpty(progressBarAttribute.HexColor))
				{
					EditorGUILayout.HelpBox($"The provided value {progressBarAttribute.HexColor} is not a valid Hex color", MessageType.Error);
				}

				DrawProgressBar(position, property, label, GUIColorToColor(progressBarAttribute));
			}
			else
			{
				DrawProperty(position, property, label);
                EditorGUILayout.HelpBox("The ProgressBar Attribute can only be attached to an int or float", MessageType.Error);
            }
    	}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var progressBarAttribute = attribute as ProgressBarAttribute;

			return progressBarAttribute.BarHeight;
		}

		private void DrawProgressBar(Rect position, SerializedProperty property, GUIContent label, Color barColor)
		{
			var progressBarAttribute = attribute as ProgressBarAttribute;

			var propertyValue = GetPropertyValue(property);
			var progressBarLabel = $"{label}: {propertyValue}/{progressBarAttribute.MaxValue}";

			var padding = 5f;
			EditorGUI.DrawRect(position, Color.black);

			var progressBarWidth = Mathf.Clamp01(propertyValue / progressBarAttribute.MaxValue) * (position.width - padding);
			EditorGUI.DrawRect(new Rect(position.x + padding / 2f, position.y + padding / 2f, progressBarWidth, position.height - padding), barColor);

			var labelHeight = EditorStyles.label.CalcHeight(new GUIContent(progressBarLabel), position.width);

			GUI.skin.label.alignment = TextAnchor.UpperCenter;
			EditorGUI.DropShadowLabel(new Rect(position.x, position.y - 6f + (position.height - labelHeight) / 2f, position.width, position.height), progressBarLabel);
			GUI.skin.label.alignment = default;
		}

		private float GetPropertyValue(SerializedProperty property)
		{
			return property.propertyType switch
			{
				SerializedPropertyType.Integer => property.intValue,
				SerializedPropertyType.Float => property.floatValue,
				_ => 0f
			};
		}
	}
}
