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
			var conditionalProperty = GetValidMemberInfo(showAttribute.ConditionName, property);

			showField = GetConditionValue<ShowFieldAttribute>(conditionalProperty, attribute, property.serializedObject.targetObject);

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
