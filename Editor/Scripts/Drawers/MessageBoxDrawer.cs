using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(MessageBoxAttribute))]
    public class MessageBoxDrawer : PropertyDrawerBase
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var messageBoxAttribute = attribute as MessageBoxAttribute;
			var conditionalProperty = ReflectionUtility.GetValidMemberInfo(messageBoxAttribute.ConditionName, property);

			var helpBoxStyle = GUI.skin.GetStyle("HelpBox");
			helpBoxStyle.richText = true;

			if (GetConditionValue(conditionalProperty, messageBoxAttribute, property)) 
				EditorGUILayout.HelpBox(GetDynamicString(messageBoxAttribute.Message, property, messageBoxAttribute), (MessageType)messageBoxAttribute.MessageType);

			if (messageBoxAttribute.DrawProperty) 
				DrawProperty(position, property, label);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var messageBox = attribute as MessageBoxAttribute;

			if (messageBox.DrawProperty)
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
