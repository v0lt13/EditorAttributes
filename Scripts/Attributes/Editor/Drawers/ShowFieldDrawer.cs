using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ShowFieldAttribute))]
    public class ShowFieldDrawer : PropertyDrawerBase
    {
		private bool showField;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var showAttribute = attribute as ShowFieldAttribute;
			var conditionalProperty = ReflectionUtility.GetValidMemberInfo(showAttribute.ConditionName, property);

			showField = GetConditionValue(conditionalProperty, showAttribute, property);

			if (showField) DrawProperty(position, property, label);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (showField)
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
