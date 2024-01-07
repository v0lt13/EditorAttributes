using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HideFieldAttribute))]
    public class HideFieldDrawer : PropertyDrawerBase
    {
		private bool hideField;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var hideAttribute = attribute as HideFieldAttribute;
			var conditionalProperty = ReflectionUtility.GetValidMemberInfo(hideAttribute.ConditionName, property);

			hideField = !GetConditionValue(conditionalProperty, hideAttribute, property);

			if (hideField) DrawProperty(position, property, label);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (hideField)
			{
				return GetCorrectPropertyHeight(property, label);
			}
			else
			{
				return -EditorGUIUtility.standardVerticalSpacing; // Remove the space for the hidden field
			}
		}
	}
}
