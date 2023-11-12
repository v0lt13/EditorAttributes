using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ShowFieldAttribute))]
    public class ShowFieldDrawer : PropertyDrawerBase
    {
		private MemberInfo conditionalProperty;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var showAttribute = attribute as ShowFieldAttribute;

			conditionalProperty = GetValidMemberInfo(showAttribute.ConditionName, property);

			if (GetConditionValue<ShowFieldAttribute>(conditionalProperty, attribute, property.serializedObject.targetObject, true)) DrawProperty(position, property, label);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (conditionalProperty != null && GetConditionValue<ShowFieldAttribute>(conditionalProperty, attribute, property.serializedObject.targetObject))
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
