using UnityEngine;
using UnityEditor;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(PropertyWidthAttribute))]
    public class PropertyWidthDrawer : PropertyDrawerBase
    {
    	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    	{
			var propertyWidthAttribute = attribute as PropertyWidthAttribute;

            EditorGUIUtility.labelWidth = propertyWidthAttribute.LabelWidth;
            EditorGUIUtility.fieldWidth = propertyWidthAttribute.FieldWidth;

		    DrawProperty(position, property, label);
    	}
    }
}
