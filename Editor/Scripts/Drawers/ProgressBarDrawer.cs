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
				var propertyValue = GetPropertyValue(property);

				EditorGUI.ProgressBar(position, propertyValue / progressBarAttribute.MaxValue, $"{label}: {propertyValue}/{progressBarAttribute.MaxValue}");
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
