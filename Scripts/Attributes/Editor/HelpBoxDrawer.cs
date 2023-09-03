using UnityEditor;
using UnityEngine;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxDrawer : PropertyDrawer
    {
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var messageBox = attribute as HelpBoxAttribute;

			EditorGUILayout.HelpBox(messageBox.message, (MessageType)messageBox.messageType);

			if (messageBox.drawProperty) DrawDefaultProperty(position, property, label);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var messageBox = attribute as HelpBoxAttribute;

			if (messageBox.drawProperty)
			{
				return EditorGUI.GetPropertyHeight(property, label);
			}
			else
			{
				return -EditorGUIUtility.standardVerticalSpacing; // Remove the space for the hidden field
			}
		}

		private void DrawDefaultProperty(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(position, property, label, true);

			if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
		}
	}
}
