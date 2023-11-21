using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(MessageBoxAttribute))]
    public class MessageBoxDrawer : PropertyDrawerBase
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var messageBox = attribute as MessageBoxAttribute;
			var conditionalProperty = GetValidMemberInfo(messageBox.ConditionName, property);

			if (GetConditionValue<MessageBoxAttribute>(conditionalProperty, attribute, property.serializedObject.targetObject)) EditorGUILayout.HelpBox(messageBox.Message, (MessageType)messageBox.MessageType);

			if (messageBox.DrawProperty) DrawProperty(position, property, label);
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
