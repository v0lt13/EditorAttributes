using System;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

namespace EditorAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxDrawer : PropertyDrawer
    {
		private UnityEventDrawer eventDrawer;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var messageBox = attribute as HelpBoxAttribute;

			eventDrawer ??= new UnityEventDrawer();

			EditorGUILayout.HelpBox(messageBox.message, (MessageType)messageBox.messageType);

			if (messageBox.drawProperty) DrawDefaultProperty(position, property, label);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var messageBox = attribute as HelpBoxAttribute;

			eventDrawer ??= new UnityEventDrawer();

			if (messageBox.drawProperty)
			{
				try
				{
					return eventDrawer.GetPropertyHeight(property, label);
				}
				catch (NullReferenceException)
				{
					return EditorGUI.GetPropertyHeight(property, label);
				}
			}
			else
			{
				return -EditorGUIUtility.standardVerticalSpacing; // Remove the space for the hidden field
			}
		}

		private void DrawDefaultProperty(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginChangeCheck();

			try
			{
				eventDrawer.OnGUI(position, property, label);
			}
			catch (NullReferenceException)
			{
				EditorGUI.PropertyField(position, property, label, true);
			}

			if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
		}
	}
}
